namespace BlurryRoots.Asteroid.Data {

	using System;

	using BlurryRoots.Yanecos.Core;

	/// <summary>
	/// Description of RotationData.
	/// </summary>
	public
	class RotationData : IData {

		public
		RotationData (float someRotation) {
			this.Rotation = someRotation;
		}

		public
		float Rotation { get; set; }

	}

}
