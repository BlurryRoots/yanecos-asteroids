namespace BlurryRoots.Asteroid.Events {

	using System;

	/// <summary>
	/// Description of KeyDownEventArgs.
	/// </summary>
	public
	class KeyDownEventArgs : EventArgs {

		public
		KeyDownEventArgs (OpenTK.Input.KeyboardKeyEventArgs someKeyArgs) {
			this.KeyArgs = someKeyArgs;
		}

		public
		OpenTK.Input.KeyboardKeyEventArgs KeyArgs { get; private set; }

	}

}
