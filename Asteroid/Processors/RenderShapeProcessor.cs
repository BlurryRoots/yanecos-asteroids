namespace BlurryRoots.Asteroid.Processors {

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
	class RenderShapeProcessor : DataProcessor {

		private
		EventManager EventManager { get; set; }

		private
		List<Type> interestList;

		public
		RenderShapeProcessor (
		EventManager someEventManager) {
			this.EventManager = someEventManager;

			this.interestList = new List<Type> () {
        typeof(ShapeData),
        typeof(SpatialData)
      };
		}



		protected override
		void OnInitialize () {
		}

		protected override
		void OnProcessing (double someDeltaTime) {
			foreach (IEntity entity in this.DataCenter.GetEntities (this.interestList)) {
				if (entity.Tag == "Projectile") {
					int i;
				}

				this.Render (
						entity.GetData<SpatialData> (),
						entity.GetData<ShapeData> ()
				);
			}
		}

		private
		void Render (SpatialData somePos, ShapeData someShape) {
			GL.PointSize (4.0f);

			GL.MatrixMode (MatrixMode.Modelview);
			GL.PushMatrix ();
			{
				RotateCenter (somePos, someShape);

				GL.Enable (EnableCap.Texture2D);
				GL.Enable (EnableCap.Blend);
				GL.BlendFunc (
						BlendingFactorSrc.SrcAlpha,
						BlendingFactorDest.OneMinusSrcAlpha
				);
				GL.Begin (BeginMode.LineLoop);
				foreach (Vector2 point in someShape.Points) {
					GL.Vertex2 (
							(point * someShape.Size) + new Vector2 (somePos.X, somePos.Y)
					);
				}
				GL.End ();
			}
			GL.PopMatrix ();
		}

		private
		void RotateCenter (SpatialData somePos, ShapeData someShape) {
			Vector2 centroid =
					someShape.GetCentroid () + new Vector2 (somePos.X, somePos.Y);

			GL.Translate (centroid.X, centroid.Y, 0); // move back to focus of GLuLookAt
			GL.Rotate (somePos.Rotation, 0.0f, 0.0f, 1.0f);
			GL.Translate (-centroid.X, -centroid.Y, 0); //move object to center
		}

	}

}
