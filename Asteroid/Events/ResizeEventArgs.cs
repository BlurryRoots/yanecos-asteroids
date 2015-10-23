namespace BlurryRoots.Asteroid.Events {

	using System;

	/// <summary>
	/// Description of ResizeEventArgs.
	/// </summary>
	public
	class ResizeEventArgs : EventArgs {

		public
		ResizeEventArgs (int someWidth, int someHeight) {
			this.Width = someWidth;

			this.Height = someHeight;
		}

		public
		int Width { get; private set; }

		public
		int Height { get; private set; }

	}

}
