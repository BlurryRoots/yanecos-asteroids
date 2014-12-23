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
    
    using BlurryRoots.Yanecos.Core;
    using BlurryRoots.Asteroid.Data;
    
    using OpenTK;
    
    /// <summary>
    /// Description of CollisionProcessor.
    /// </summary>
    public 
        class CollisionProcessor : DataProcessor
    {
        public 
            CollisionProcessor()
        {
        }
        
        protected override
            void OnInitialize()
        {
        }
            
        protected override
            void OnProcessing( double someDeltaTime )
        {
            List<IEntity> asteroids =
                this.DataCenter.GetEntitiesWithTag( "Asteroid" );
            List<IEntity> projectiles =
                this.DataCenter.GetEntitiesWithTag( "Projectile" );
            
            List<ulong> removeList = new List<ulong>();
            
            foreach( IEntity asteroid in asteroids )
            {
                SpatialData asteroidSpatial = asteroid.GetData<SpatialData>();
                ShapeData asteroidShape = asteroid.GetData<ShapeData>();
                
                foreach( IEntity projectile in projectiles )
                {
                    SpatialData projectileSpatial = projectile.GetData<SpatialData>();
                    ShapeData projectileShape = projectile.GetData<ShapeData>();
                    
                    foreach( Vector2 point in projectileShape.Points )
                    {
                        Vector2 actualPoint = 
                            (point * projectileShape.Size) + new Vector2( projectileSpatial.X, projectileSpatial.Y );
                        /*TODO: blow up the shape before check for containing ???!!!*/
                        if( asteroidShape.Contains( asteroidSpatial, actualPoint ) )
                        {
                            if( ! removeList.Contains( asteroid.ID ) )
                            {
                                removeList.Add( asteroid.ID );
                            }
                            
                            if( ! removeList.Contains( projectile.ID ) )
                            {
                                removeList.Add( projectile.ID );
                            }
                        }
                    }
                }
            }
            
            foreach( ulong id in removeList )
            {
                this.DataCenter.EntityManager.RemoveEntity( id );
            }
        }
    }
}
