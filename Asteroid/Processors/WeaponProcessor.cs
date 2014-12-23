//
//    Copyright (C) 2013 sven freiberg
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
namespace BlurryRoots.Asteroid.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using BlurryRoots.Yanecos.Core;
    
    using BlurryRoots.Asteroid.Data;
    using BlurryRoots.Asteroid.Events;
    
    using OpenTK;
    
    /// <summary>
    /// Description of WeaponProcessor.
    /// </summary>
    public 
        class WeaponProcessor : DataProcessor
    {
        private
            EventManager eventManager;
        
        private
            Random random;
        
        private
            readonly float ProjectileSpeed = 400;
        
        private
            int width;
        private
            int height;
        
        public 
            WeaponProcessor( EventManager someEventManager, int someWidth, int someHeight )
        {
            this.eventManager = someEventManager;
            
            this.width = someWidth;            
            this.height = someHeight;
            
            this.random = new Random();
            
            this.eventManager.AddHandler<TriggerPulledEventArgs>(
                this.OnTriggerPulled
            );
            
            this.eventManager.AddHandler<ResizeEventArgs>(
                this.OnResize
            );
        }
        
        ~WeaponProcessor()
        {
            this.eventManager.RemoveHandler<TriggerPulledEventArgs>(
                this.OnTriggerPulled
            );
            
            this.eventManager.RemoveHandler<ResizeEventArgs>(
                this.OnResize
            );
        }
        
        protected override 
            void OnInitialize()
        {
        }
            
        protected override 
            void OnProcessing( double someDeltaTime )
        {
            List<ulong> removeList = new List<ulong>();
            List<IEntity> projectiles = this.DataCenter.GetEntitiesWithTag( "Projectile" );
            foreach( IEntity projectile in projectiles )
            {
                SpatialData pos = projectile.GetData<SpatialData>();
                
                if( pos.X > this.width || pos.X < 0 || pos.Y > this.height || pos.Y < 0 )
                {
                    removeList.Add( projectile.ID );
                }
            }
            
            foreach( ulong id in removeList )
            {
                this.DataCenter.EntityManager.RemoveEntity( id );
            }
        }
        
        private
            void OnTriggerPulled( object someSender, TriggerPulledEventArgs e )
        {
            IEntity player = this.DataCenter.GetEntitiesWithTag( "Player" ).Single();
            SpatialData playerSpatial = player.GetData<SpatialData>();
            VelocityData playerVelocity = player.GetData<VelocityData>();            
            
            
            IEntity projectile = this.DataCenter.CreateEntity();
            projectile.Tag = "Projectile";            
            
            Vector2 pos = new Vector2( playerSpatial.X, playerSpatial.Y );
            projectile.AddData(
                new SpatialData()
                {
                    X = pos.X,
                    Y = pos.Y,
                    Rotation = playerSpatial.Rotation
                }
            );            
            
            float rotationRads = (float)(Math.PI/180.0) * playerSpatial.Rotation;
            Vector2 vPlayer = new Vector2( playerVelocity.X, playerVelocity.Y );
            Vector2 v = new Vector2(
                (float)Math.Cos(rotationRads) * this.ProjectileSpeed,
                (float)Math.Sin(rotationRads) * this.ProjectileSpeed
            );
            /*float angle = 
                Vector2.Dot( v, vPlayer )
                / ( v.LengthFast * vPlayer.LengthFast );
            float arcX = angle;
            float arcY = (float)Math.Sin( Math.Acos( angle ) );*/
            projectile.AddData(
                new VelocityData()
                {
                    X = v.X, //+ ( arcX * playerVelocity.X ),
                    Y = v.Y //+ ( arcY * playerVelocity.Y )
                }
            );
            
            projectile.AddData(
                new ShapeData(
                    10,
                    new List<Vector2>()
                    {
                        new Vector2( 0, 0 ),
                        new Vector2( 0, -0.25f ),
                        new Vector2( 0.75f, -0.5f ),
                        new Vector2( 1, 0 ),
                        new Vector2( 0.75f, 0.5f ),
                        new Vector2( 0, 0.25f ),
                    }
                )
            );
        }
        
        private
            void OnResize( object someSender, ResizeEventArgs e )
        {
            this.width = e.Width;
            this.height = e.Height;            
        }
    }
}
