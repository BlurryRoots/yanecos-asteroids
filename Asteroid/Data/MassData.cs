namespace BlurryRoots.Asteroid.Data {

	using System;

	using BlurryRoots.Yanecos.Core;

	/// <summary>
	/// Description of MassData.
	/// </summary>
	[Serializable]
	public
	class MassData : IData {

		public
		MassData () {
			this.Mass = 1;
		}

		public
		MassData (float someMass) {
			this.Mass = someMass;
		}

		public
		float Mass { get; set; }

	}

}
