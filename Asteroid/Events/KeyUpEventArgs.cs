namespace BlurryRoots.Asteroid.Events {

	using System;

	/// <summary>
	/// Description of KeyUpEventArgs.
	/// </summary>
	public class KeyUpEventArgs : EventArgs {

		public
		KeyUpEventArgs (OpenTK.Input.KeyboardKeyEventArgs someKeyArgs) {
			this.KeyArgs = someKeyArgs;
		}

		public
		OpenTK.Input.KeyboardKeyEventArgs KeyArgs { get; private set; }

	}

}
