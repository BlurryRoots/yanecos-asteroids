namespace BlurryRoots.Asteroid.Data {

	using System;

	using BlurryRoots.Yanecos.Core;

	/// <summary>
	/// Description of ShipData.
	/// </summary>
	[Serializable]
	public
	class ShipData : IData {

		public
		ShipData () {
		}

		public
		float Health { get; set; }

		public
		float Damage { get; set; }

	}

}
