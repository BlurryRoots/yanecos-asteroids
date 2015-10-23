namespace BlurryRoots.Asteroid.Data {

	using System;

	using BlurryRoots.Yanecos.Core;

	/// <summary>
	/// Description of PositionData.
	/// </summary>
	[Serializable]
	public
	class VelocityData : IData {

		public
		VelocityData () {
			this.X = 0;
			this.Y = 0;
		}

		public
		VelocityData (float someX, float someY) {
			this.X = someX;
			this.Y = someY;
		}

		public
		float X { get; set; }

		public
		float Y { get; set; }

	}

}
