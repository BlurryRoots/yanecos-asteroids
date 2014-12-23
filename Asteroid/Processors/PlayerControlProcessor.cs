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
    using BlurryRoots.Asteroid.Events;
    
    using OpenTK;
    using OpenTK.Input;        
    
    /// <summary>
    /// Description of PlayerControlProcessor.
    /// </summary>
    public 
        class PlayerControlProcessor : DataProcessor
    {
        private
            float Speed = 333.0f;
        
        private
            EventManager EventManager { get; set; }
        
        private
            KeyboardDevice Keyboard { get; set; }
        
        private
            List<Key> keysDown;
        
        private
            List<Type> interestList;
        
        
        private
            List<Key> AvailableKeys;
        
        public 
            PlayerControlProcessor(
                EventManager someEventManager,
                KeyboardDevice someKeyboard )
        {
            this.EventManager = someEventManager;      
            this.Keyboard = someKeyboard;
            
            this.keysDown =
                new List<Key>();
            
            this.interestList = new List<Type>()
            {
                typeof(PlayerData),
                typeof(SpatialData)
            };
            
            Type enumType = typeof( Key );
            this.AvailableKeys = new List<Key>();
            string[] k = Enum.GetNames( enumType );
            foreach( string s in k )
            {
                this.AvailableKeys.Add( (Key)Enum.Parse( enumType, s ) );
            }
        }
        
        ~PlayerControlProcessor()
        {
        }
            
        protected override 
            void OnInitialize()
        {     
            this.keysDown.Clear();
        }
        
        protected override 
            void OnProcessing(double someDeltaTime)
        {
            this.CheckKeys();
            
            foreach( IEntity entity in this.DataCenter.GetEntities( this.interestList ) )
            {
                SpatialData spatial = entity.GetData<SpatialData>();
                VelocityData sd = entity.GetData<VelocityData>();     
                
                float radiants = (float)(Math.PI/180.0) * (spatial.Rotation);
                Vector2 dir = new Vector2(
                    (float)Math.Cos(radiants),
                    (float)Math.Sin(radiants)
                );                
                
                //        
                Vector2 movement = new Vector2();        
                if( this.keysDown.Contains( Key.W ) )
                {
                    movement += dir * this.Speed;
                }
                
                if( this.keysDown.Contains( Key.A ) )
                {
                    movement += dir.PerpendicularRight * this.Speed;
                }
                
                if( this.keysDown.Contains( Key.S ) )
                {
                    movement += -1 * dir * this.Speed;
                }
                
                if( this.keysDown.Contains( Key.D ) )
                {
                    movement += dir.PerpendicularLeft * this.Speed;
                }
                
                //
                float rotationDirection = 0;
                if( this.keysDown.Contains( Key.Left ) )
                {
                    rotationDirection -= 1;
                }                
                
                if( this.keysDown.Contains( Key.Right ) )
                {
                    rotationDirection += 1;
                }
                
                spatial.Rotation += rotationDirection * this.Speed * (float)someDeltaTime;
                                
                sd.X += movement.X * (float)someDeltaTime;
                sd.Y += movement.Y * (float)someDeltaTime;
            }
        }
        
        private
            void OnKeyDown( Key someKey )
        {
            
        }
        
        private
            void OnKeyUp( Key someKey )
        {
            if( someKey == Key.Space )
            {
                this.EventManager.PublishEvent(
                    this,
                    new TriggerPulledEventArgs()
                );
            }
        }
        
        private
            void CheckKeys()
        {          
            foreach( Key k in this.AvailableKeys )
            {
                if( this.Keyboard[k] )
                {
                    if( !this.keysDown.Contains( k ) )
                    {
                        this.keysDown.Add( k ); 
                        this.OnKeyDown( k );
                    }
                }
                else
                {
                    bool keyup = false;
                    while( this.keysDown.Contains( k ) )
                    {
                        this.keysDown.Remove( k );
                        keyup = true;
                    }
                    
                    if( keyup )
                    {
                        this.OnKeyUp( k );
                    }
                }
            }
        }
    }
}
