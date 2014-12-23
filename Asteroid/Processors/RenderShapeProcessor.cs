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
    using OpenTK.Graphics.OpenGL;
    
    /// <summary>
    /// Description of ShapeRenderProcessor.
    /// </summary>
    public
        class RenderShapeProcessor : DataProcessor
    {      
        private
            EventManager EventManager { get; set; }
        
        private
            List<Type> interestList;
        
        public 
            RenderShapeProcessor( 
                EventManager someEventManager )
        {
            this.EventManager = someEventManager;
            
            this.interestList = new List<Type>()
            {
                typeof(ShapeData),
                typeof(SpatialData)
            };
        }
        
        
        
        protected override 
            void OnInitialize()
        {
        }
        
        protected override
            void OnProcessing( double someDeltaTime )
        {
            foreach( IEntity entity in this.DataCenter.GetEntities( this.interestList )  )
            {
                if( entity.Tag == "Projectile" )
                {
                    int i;
                }
                
                this.Render(
                    entity.GetData<SpatialData>(),
                    entity.GetData<ShapeData>()
                );
            }
        }
        
        private 
            void Render( SpatialData somePos, ShapeData someShape )
        {              
            GL.PointSize( 4.0f );
            
            GL.MatrixMode( MatrixMode.Modelview );
            GL.PushMatrix();
            {    
                RotateCenter( somePos, someShape );
                  
                GL.Enable( EnableCap.Texture2D );
                GL.Enable( EnableCap.Blend );
                GL.BlendFunc(
                    BlendingFactorSrc.SrcAlpha, 
                    BlendingFactorDest.OneMinusSrcAlpha
                );
                GL.Begin( BeginMode.LineLoop );            
                foreach( Vector2 point in someShape.Points )
                {
                    GL.Vertex2( 
                        (point * someShape.Size) + new Vector2( somePos.X, somePos.Y ) 
                    );
                }            
                GL.End();
            }
            GL.PopMatrix();  
        }
        
        private 
            void RotateCenter( SpatialData somePos, ShapeData someShape )
        {
            Vector2 centroid = 
                someShape.GetCentroid() + new Vector2( somePos.X, somePos.Y );
            
            GL.Translate( centroid.X, centroid.Y, 0 ); // move back to focus of GLuLookAt
            GL.Rotate( somePos.Rotation, 0.0f, 0.0f, 1.0f );
            GL.Translate( -centroid.X, -centroid.Y, 0 ); //move object to center
        }
    }
}
