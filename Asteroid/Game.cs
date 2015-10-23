namespace BlurryRoots.Asteroid {

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
	/// Entry point for the game.
	/// </summary>
	public
			class Game : GameWindow {

		/// <summary>
		/// Starting point of application.
		/// </summary>
		/// <param name="parameters">Command line paramters.</param>
		public static
				void Main (string[] parameters) {
			using (Game g = new Game ()) {
				g.Run ();
			}
		}

		public
				Game () {
			this.DataCenter = new DataCenter (
					new EntityManager (),
					new DataProcessorManager (),
					new DataCore ()
			);

			this.eventManager = new EventManager ();

			this.TargetRenderFrequency = 10000;
			this.TargetUpdateFrequency = 10000;
		}

		/// <summary>
		/// Hook exectued on loading the game.
		/// </summary>
		/// <param name="e">Event data.</param>
		protected override
				void OnLoad (EventArgs e) {
			base.OnLoad (e);

			foreach (JoystickDevice js in this.Joysticks) {
				Debug.WriteLine (js.Axis.Count);
				Debug.WriteLine (js.Button.Count);
				Debug.WriteLine (js.Description);
				Debug.WriteLine (js.DeviceType.ToString ());
				Debug.WriteLine ("---");
			}

			Debug.WriteLine (this.Keyboard.Description);
			Debug.WriteLine (this.Keyboard.DeviceType.ToString ());
			Debug.WriteLine (this.Keyboard.NumberOfLeds);

			this.Keyboard.KeyDown += this.PublishKeyDown;
			this.Keyboard.KeyUp += this.PublishKeyUp;

			this.SetupProcessors ();
			this.SetupEntities ();
		}

		/// <summary>
		/// Hook executed when frames is about to be updated.
		/// </summary>
		/// <param name="e">Event data.</param>
		protected override
				void OnUpdateFrame (FrameEventArgs e) {
			base.OnUpdateFrame (e);

			this.ProcessEvents ();

			//
			this.eventManager.ProcessEvents ();

			this.DataCenter.Update (e.Time);

			this.DataCenter.GetEntitiesWithTag ("FPS").Single ().GetData<TextData> ().Text = (1.0 / e.Time).ToString () + " FPS";

			this.DataCenter.GetProcessor<PlayerControlProcessor> ().Process ();

			this.DataCenter.GetProcessor<MovementProcessor> ().Process ();

			this.DataCenter.GetProcessor<AsteroidSpawnProcessor> ().Process ();

			this.DataCenter.GetProcessor<WeaponProcessor> ().Process ();

			this.DataCenter.GetProcessor<CollisionProcessor> ().Process ();
		}

		/// <summary>
		/// Hook executed when frames is about to be rendered.
		/// </summary>
		/// <param name="e">Event data.</param>
		protected override
				void OnRenderFrame (FrameEventArgs e) {
			base.OnRenderFrame (e);

			this.DataCenter.GetProcessor<ClearProcessor> ().Process ();

			this.DataCenter.GetProcessor<RenderShapeProcessor> ().Process ();

			//this.DataCenter.GetProcessor<RenderTextProcessor>().Process();

			this.SwapBuffers ();
		}

		/// <summary>
		/// Hook executed when buffer has been resized.
		/// </summary>
		/// <param name="e">Event data.</param>
		protected override
				void OnResize (EventArgs e) {
			base.OnResize (e);

			int someWidth = this.Width;
			int someHeight = this.Height;

			//  Create an orthographic projection.
			GL.MatrixMode (MatrixMode.Projection);

			GL.LoadIdentity ();
			GL.Ortho (0, someWidth, someHeight, 0, -10, 10);

			//  Back to the modelview.           
			GL.MatrixMode (MatrixMode.Modelview);

			GL.Viewport (0, 0, someWidth, someHeight);

			this.eventManager.PublishEvent (
					this,
					new ResizeEventArgs (this.Width, this.Height)
			);
		}

		/// <summary>
		/// Prepare all processors.
		/// </summary>
		private
				void SetupProcessors () {
			this.DataCenter.AddProcessor (
					new ClearProcessor ()
			);

			this.DataCenter.AddProcessor (
					new RenderTextProcessor (
							this.eventManager,
							this.Width,
							this.Height
					)
			);

			this.DataCenter.AddProcessor (
					new RenderShapeProcessor (
							this.eventManager
					)
			);

			this.DataCenter.AddProcessor (
					new PlayerControlProcessor (
							this.eventManager,
							this.Keyboard
					)
			);

			this.DataCenter.AddProcessor (
					new MovementProcessor (
							this.eventManager
					)
			);

			this.DataCenter.AddProcessor (
					new AsteroidSpawnProcessor (
							this.eventManager,
							this.Width,
							this.Height
					)
			);

			this.DataCenter.AddProcessor (
					new WeaponProcessor (
							this.eventManager,
							this.Width,
							this.Height
					)
			);

			this.DataCenter.AddProcessor (
					new CollisionProcessor ()
			);
		}

		/// <summary>
		/// Setup any entities needed on startup.
		/// </summary>
		private
				void SetupEntities () {
			this.DataCenter.CreateEntity (
				new IData[] {
          new SpatialData( 42, 84 ),
          new TextData() {
            Text = "0.0 FPS",
            Font = new Font ("Arial", 12, FontStyle.Regular),
            Brush = Brushes.Red
          }
        }
			).Tag = "FPS";

			this.DataCenter.CreateEntity (
					new IData[] {
            new ClearOptionData () {
                Color = Color4.Black
            }
          }
			);

			this.DataCenter.CreateEntity (
				new IData[] {
          new SpatialData (42, 42),
          new TextData () {
            Text = "Hy there",
            Font = new Font ("Arial", 12, FontStyle.Regular),
            Brush = Brushes.Red
          }
        }
			);

			this.CreatePlayer ();
		}

		/// <summary>
		/// Factory methdo to create player entity.
		/// </summary>
		private
				void CreatePlayer () {
			IEntity e = this.DataCenter.CreateEntity ();
			e.Tag = "Player";
			e.AddData (new PlayerData ("Sfehn"));
			e.AddData (new SpatialData (256, 128));
			e.AddData (
				new ShapeData (
					32,
					new List<Vector2> () {
            new Vector2 (0, 0),
            new Vector2 (-0.25f, -0.25f),
            new Vector2 (0.75f, 0),
            new Vector2 (-0.25f, 0.25f)
          }
				)
			);
			e.AddData (new MassData (100));
			e.AddData (new VelocityData (0, 0));
		}

		/// <summary>
		/// Callback used by the input manager to publish keyboard up events.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event data.</param>
		private
				void PublishKeyUp (object sender, KeyboardKeyEventArgs e) {
			this.eventManager.PublishEvent (
					this,
					new KeyUpEventArgs (e)
			);
		}

		/// <summary>
		/// Callback used by the input manager to publish keyboard down events.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event data.</param>
		private
				void PublishKeyDown (object sender, KeyboardKeyEventArgs e) {
			this.eventManager.PublishEvent (
					this,
					new KeyDownEventArgs (e)
			);
		}


		/// <summary>
		/// Central control point for yanecos.
		/// </summary>
		private
				IDataCenter DataCenter { get; set; }

		/// <summary>
		/// Event queue.
		/// </summary>
		private
				EventManager eventManager;

	}

}
