namespace BlurryRoots.Asteroid.Events {

	using System;

	/// <summary>
	/// Description of ResizeEventArgs.
	/// </summary>
	public
	class PlayerHitEvent : EventArgs {

		public
		PlayerHitEvent (float mass) {
			this.Mass = mass;
		}

		public
		float Mass { get; private set; }

	}

}
