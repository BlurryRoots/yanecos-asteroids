namespace BlurryRoots.Asteroid.Processors {

	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Drawing.Imaging;

	using BlurryRoots.Yanecos.Core;

	using BlurryRoots.Asteroid;
	using BlurryRoots.Asteroid.Data;
	using BlurryRoots.Asteroid.Events;

	using OpenTK.Graphics.OpenGL;

	/// <summary>
	/// Description of FpsRenderProcessor.
	/// </summary>
	public
	class RenderTextProcessor : DataProcessor {

		private
		EventManager EventManager { get; set; }

		private
		Bitmap textBitmap;

		private
		int textTexture;

		private
		int width;
		private
		int height;

		private
		List<Type> interestList;

		public
		RenderTextProcessor (EventManager someEventManager, int someWidth, int someHeight) {
			this.EventManager = someEventManager;

			this.EventManager.AddHandler<ResizeEventArgs> (
					this.OnResize
			);

			this.width = someWidth;
			this.height = someHeight;
		}

		~RenderTextProcessor () {
			this.EventManager.RemoveHandler<ResizeEventArgs> (
					this.OnResize
			);
		}

		protected override
		void OnInitialize () {
			// Create Bitmap and OpenGL texture
			this.textBitmap = new Bitmap (this.width, this.height); // match window size

			this.textTexture = GL.GenTexture ();

			/*GL.BindTexture(
					TextureTarget.Texture2D, 
					this.textTexture 
			);           
			GL.TexParameter(
					TextureTarget.Texture2D, 
					TextureParameterName.TextureMagFilter, 
					(int)All.Linear);
			GL.TexParameter(
					TextureTarget.Texture2D, 
					TextureParameterName.TextureMinFilter, 
					(int)All.Linear);
            
			// just allocate memory, so we can update efficiently using TexSubImage2D
			GL.TexImage2D(
					TextureTarget.Texture2D, 
					0, 
					PixelInternalFormat.Rgba, 
					this.textBitmap.Width, 
					this.textBitmap.Height,
					0,
					OpenTK.Graphics.OpenGL.PixelFormat.Bgra, 
					PixelType.UnsignedByte, 
					IntPtr.Zero
			); */

			this.interestList = new List<Type> () {
          typeof(TextData),
          typeof(SpatialData)
      };
		}

		protected override
		void OnProcessing (double someDeltaTime) {
			foreach (IEntity entity in this.DataCenter.GetEntities (this.interestList)) {
				this.RenderText (
						entity.GetData<TextData> (),
						entity.GetData<SpatialData> ()
				);
			}
		}

		private
		void RenderText (TextData someTextData, SpatialData someSpatial) {
			// Render text using System.Drawing.
			// Do this only when text changes.
			using (Graphics gfx = Graphics.FromImage (this.textBitmap)) {
				gfx.Clear (Color.Transparent);
				gfx.DrawString (
						someTextData.Text,
						someTextData.Font,      //new Font( "Arial", 12, FontStyle.Regular ), 
						someTextData.Brush,     //Brushes.BlueViolet, 
						someSpatial.X,
						someSpatial.Y
				);
			}

			GL.Enable (EnableCap.Texture2D);
			GL.BindTexture (
					TextureTarget.Texture2D,
					this.textTexture
			);
			GL.TexParameter (
					TextureTarget.Texture2D,
					TextureParameterName.TextureMagFilter,
					(int)All.Linear);
			GL.TexParameter (
					TextureTarget.Texture2D,
					TextureParameterName.TextureMinFilter,
					(int)All.Linear);
			GL.Enable (EnableCap.Blend);
			GL.BlendFunc (
					BlendingFactorSrc.SrcAlpha,
					BlendingFactorDest.OneMinusSrcAlpha
			);

			BitmapData data = this.textBitmap.LockBits (
					new Rectangle (0, 0, this.textBitmap.Width, this.textBitmap.Height),
					ImageLockMode.ReadOnly,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb
			);
			GL.TexImage2D (
					TextureTarget.Texture2D,
					0,
					PixelInternalFormat.Rgba,
					this.width,
					this.height,
					0,
					OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
					PixelType.UnsignedByte,
					data.Scan0
			);
			this.textBitmap.UnlockBits (data);

			GL.Begin (BeginMode.Quads);
			GL.TexCoord2 (0f, 0f); GL.Vertex2 (0f, 0f);
			GL.TexCoord2 (1f, 0f); GL.Vertex2 (this.width, 0f);
			GL.TexCoord2 (1f, 1f); GL.Vertex2 (this.width, this.height);
			GL.TexCoord2 (0f, 1f); GL.Vertex2 (0f, this.height);
			GL.End ();

			GL.Disable (EnableCap.Texture2D);
		}

		private
		void OnResize (object someSender, ResizeEventArgs someArgument) {
			this.width = someArgument.Width;
			this.height = someArgument.Height;

			// Ensure Bitmap and texture match window size
			if (this.textBitmap != null) {
				this.textBitmap.Dispose ();
			}

			this.textBitmap = new Bitmap (
					this.width,
					this.height
			);
		}

	}

}
