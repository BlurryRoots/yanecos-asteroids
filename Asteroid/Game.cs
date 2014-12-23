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
namespace BlurryRoots.Asteroid
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;    
    using System.Linq;
    
    using OpenTK;
    using OpenTK.Input;
    using OpenTK.Graphics;
    using OpenTK.Graphics.OpenGL;
    
    using BlurryRoots.Yanecos.Core;
    
    using BlurryRoots.Asteroid.Data;
    using BlurryRoots.Asteroid.Events;
    using BlurryRoots.Asteroid.Processors;
    
    /// <summary>
    /// Description of EntryPoint.
    /// </summary>
    public
        class Game : GameWindow
    {
        private   
            IDataCenter DataCenter { get; set; }
        
        private
            EventManager eventManager;
        
        public
            Game()
        {
            this.DataCenter = new DataCenter(
                new EntityManager(),
                new DataProcessorManager(),
                new DataCore() 
            );
            
            this.eventManager = new EventManager();   

            this.TargetRenderFrequency = 10000;
            this.TargetUpdateFrequency = 10000;            
        }
        
        private
            void SetupProcessors()
        {            
            this.DataCenter.AddProcessor(
                new ClearProcessor()
            );
            
            this.DataCenter.AddProcessor(
                new RenderTextProcessor(
                    this.eventManager,
                    this.Width,
                    this.Height
                )
            );
            
            this.DataCenter.AddProcessor(
                new RenderShapeProcessor(
                    this.eventManager
                )
            );
            
            this.DataCenter.AddProcessor(
                new PlayerControlProcessor(
                    this.eventManager,
                    this.Keyboard
                )
            ); 

            this.DataCenter.AddProcessor(
                new MovementProcessor(
                    this.eventManager
                )
            );    

            this.DataCenter.AddProcessor(
                new AsteroidSpawnProcessor(
                    this.eventManager,
                    this.Width,
                    this.Height
                )
            );  

            this.DataCenter.AddProcessor(
                new WeaponProcessor(
                    this.eventManager,
                    this.Width,
                    this.Height
                )
            );  

            this.DataCenter.AddProcessor(
                new CollisionProcessor()
            );            
        }
        
        private
            void SetupEntities()
        {
            this.DataCenter.CreateEntity(
                new IData[]
                {
                    new SpatialData( 42, 84 ),
                    new TextData()
                    {
                        Text = "0.0 FPS",
                        Font = new Font( "Arial", 12, FontStyle.Regular ),
                        Brush = Brushes.Red
                    }
                }
            ).Tag = "FPS";
            
            this.DataCenter.CreateEntity(
                new IData[]
                {
                    new ClearOptionData()
                    {
                        Color = Color4.Black
                    }
                }
            );
            
            this.DataCenter.CreateEntity(
                new IData[]
                {
                    new SpatialData( 42, 42 ),
                    new TextData()
                    {
                        Text = "Hy there",
                        Font = new Font( "Arial", 12, FontStyle.Regular ),
                        Brush = Brushes.Red
                    }
                }
            );
            
            this.CreatePlayer();            
        }
        
        protected override 
            void OnLoad(EventArgs e)
        {                
            base.OnLoad( e );
            
            foreach( JoystickDevice js in this.Joysticks )
            {
                Debug.WriteLine( js.Axis.Count );
                Debug.WriteLine( js.Button.Count );
                Debug.WriteLine( js.Description );                
                Debug.WriteLine( js.DeviceType.ToString() );
                Debug.WriteLine( "---" );
            }
            
            Debug.WriteLine( this.Keyboard.Description );            
            Debug.WriteLine( this.Keyboard.DeviceType.ToString() );
            Debug.WriteLine( this.Keyboard.NumberOfLeds );
            
            this.Keyboard.KeyDown += this.PublishKeyDown;
            this.Keyboard.KeyUp += this.PublishKeyUp;
            
            this.SetupProcessors();
            this.SetupEntities();
        }
        
        protected override
            void OnUpdateFrame(FrameEventArgs e)
        {            
            base.OnUpdateFrame( e );            
            
            this.ProcessEvents();
            
            //
            this.eventManager.ProcessEvents();
            
            this.DataCenter.Update( e.Time );
            
            this.DataCenter.GetEntitiesWithTag( "FPS" ).Single().GetData<TextData>().Text = (1.0/e.Time).ToString() + " FPS";
            
            this.DataCenter.GetProcessor<PlayerControlProcessor>().Process();
            
            this.DataCenter.GetProcessor<MovementProcessor>().Process();
            
            this.DataCenter.GetProcessor<AsteroidSpawnProcessor>().Process();
            
            this.DataCenter.GetProcessor<WeaponProcessor>().Process();
            
            this.DataCenter.GetProcessor<CollisionProcessor>().Process();
        }
        
        protected override 
            void OnRenderFrame( FrameEventArgs e )
        {            
            base.OnRenderFrame( e );
            
            this.DataCenter.GetProcessor<ClearProcessor>().Process();
            
            this.DataCenter.GetProcessor<RenderShapeProcessor>().Process();
            
            //this.DataCenter.GetProcessor<RenderTextProcessor>().Process();
            
            this.SwapBuffers();
        }
        
        private
            void PublishKeyDown( object sender, KeyboardKeyEventArgs e )
        {   
            this.eventManager.PublishEvent(
                this,
                new KeyDownEventArgs( e )
            );
        }
        
        
        
        private
            void PublishKeyUp( object sender, KeyboardKeyEventArgs e )
        {            
            this.eventManager.PublishEvent(
                this,
                new KeyUpEventArgs( e )
            );
        }
        
        protected override 
            void OnResize(EventArgs e)
        {
            base.OnResize(e); 
            
            int someWidth = this.Width;
            int someHeight = this.Height;
            
            //  Create an orthographic projection.
            GL.MatrixMode( MatrixMode.Projection );
            
            GL.LoadIdentity();
            GL.Ortho( 0, someWidth, someHeight, 0, -10, 10 );
            
            //  Back to the modelview.           
            GL.MatrixMode( MatrixMode.Modelview );  
            
            GL.Viewport( 0, 0, someWidth, someHeight );
            
            this.eventManager.PublishEvent(
                this,
                new ResizeEventArgs(this.Width, this.Height)
            );
        }
        
        private
            void CreatePlayer()
        {
            IEntity e = this.DataCenter.CreateEntity();
            e.Tag = "Player";
            e.AddData( new PlayerData( "Sfehn" ) );
            e.AddData( new SpatialData( 256, 128 ) );
            e.AddData( 
                new ShapeData( 
                    32, 
                    new List<Vector2>()
                    {
                        new Vector2( 0, 0 ),
                        new Vector2( -0.25f, -0.25f ),
                        new Vector2( 0.75f, 0 ),
                        new Vector2( -0.25f, 0.25f )
                    }
               )
            );
            e.AddData( new MassData( 100 ) );
            e.AddData( new VelocityData( 0, 0 ) );  
        }
        
        public static
            void Main( string[] parameters )
        {
            using( Game g = new Game() )
            {
                g.Run();
            }
        }
    }
}
