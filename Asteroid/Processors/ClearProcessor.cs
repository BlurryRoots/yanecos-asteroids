namespace BlurryRoots.Asteroid.Processors {

	using System;
	using System.Drawing;
	using System.Linq;

	using BlurryRoots.Yanecos.Core;

	using BlurryRoots.Asteroid.Data;

	using OpenTK.Graphics.OpenGL;

	/// <summary>
	/// Description of ClearProcessor.
	/// </summary>
	public
	class ClearProcessor : DataProcessor {

		public
		ClearProcessor () {
		}

		protected override
		void OnInitialize () {
		}

		protected override
		void OnProcessing (double someDeltaTime) {
			ClearOptionData data =
					this.DataCenter
							.GetEntities<ClearOptionData> ()
							.Single ()
							.GetData<ClearOptionData> ();

			GL.ClearColor (data.Color);
			GL.Clear (data.BufferMask);
		}

	}

}
