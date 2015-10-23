namespace BlurryRoots.Asteroid.Data {

	using System;

	using BlurryRoots.Yanecos.Core;

	using OpenTK.Graphics;
	using OpenTK.Graphics.OpenGL;

	/// <summary>
	/// Description of ClearOption.
	/// </summary>
	public class ClearOptionData : IData {

		public
		ClearOptionData () {
			this.BufferMask =
					OpenTK.Graphics.OpenGL.ClearBufferMask.ColorBufferBit
					| OpenTK.Graphics.OpenGL.ClearBufferMask.DepthBufferBit;

			this.Color = Color4.Black;
		}

		public
		OpenTK.Graphics.OpenGL.ClearBufferMask BufferMask { get; set; }

		public
		Color4 Color { get; set; }

	}

}
