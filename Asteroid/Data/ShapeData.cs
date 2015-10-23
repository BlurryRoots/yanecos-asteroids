namespace BlurryRoots.Asteroid.Data {

	using System;
	using System.Collections.Generic;

	using OpenTK;

	using BlurryRoots.Yanecos.Core;

	/// <summary>
	/// Description of RenderData.
	/// </summary>
	[Serializable]
	public
	class ShapeData : IData {

		public
		ShapeData (float someSize, List<Vector2> somePoints) {
			this.Size = someSize;

			this.Points = somePoints;
		}

		public
		float Size { get; private set; }

		public
		List<Vector2> Points { get; private set; }

	}

}
