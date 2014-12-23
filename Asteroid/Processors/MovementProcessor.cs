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
    
    /// <summary>
    /// Description of MovementProcessor.
    /// </summary>
    public 
        class MovementProcessor : DataProcessor
    {        
        private
            EventManager EventManager { get; set; }
        
        private
            List<Type> interestList;
        
        public 
            MovementProcessor( EventManager someEventManager )
        {
            this.EventManager = someEventManager;
            
            this.interestList = new List<Type>()
            {
                typeof(SpatialData),
                typeof(VelocityData)
            };
        }
        
        protected override
            void OnInitialize()
        {
        }
            
        protected override 
            void OnProcessing( double someDeltaTime )
        {
            foreach( IEntity entity in this.DataCenter.GetEntities( this.interestList ) )
            {
                SpatialData sd = entity.GetData<SpatialData>();
                VelocityData vd = entity.GetData<VelocityData>();
                
                sd.X += vd.X * (float)someDeltaTime;
                sd.Y += vd.Y * (float)someDeltaTime;
            }    
        }
    }
}
