namespace BlurryRoots.Asteroid.Data {

	using System;

	using BlurryRoots.Yanecos.Core;

	/// <summary>
	/// Description of PositionData.
	/// </summary>
	[Serializable]
	public
	class SpatialData : IData {

		public
		SpatialData ()
			: this (0, 0) {
		}

		public
		SpatialData (float someX, float someY) {
			this.X = someX;
			this.Y = someY;

			this.Rotation = 0;
		}

		public
		float X { get; set; }

		public
		float Y { get; set; }

		public
		float Rotation { get; set; }

	}

}
