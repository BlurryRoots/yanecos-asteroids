namespace BlurryRoots.Asteroid.Data {
	using System;

	using OpenTK;

	/// <summary>
	/// Description of ShapeDataExtension.
	/// </summary>
	public static
	class ShapeDataExtension {

		private static
		float AreaSumPart (float xi, float yip1, float xip1, float yi) {
			return xi * yip1 - xip1 * yi;
		}

		public static
		float GetArea (this ShapeData someShape) {
			float A = 0;

			for (int i = 0; i < someShape.Points.Count - 1; ++i) {
				A += ShapeDataExtension.AreaSumPart (
					someShape.Points[i].X * someShape.Size,
					someShape.Points[i + 1].Y * someShape.Size,
					someShape.Points[i + 1].X * someShape.Size,
					someShape.Points[i].Y * someShape.Size
				);
			}

			return A / 2.0f;
		}

		public static
		Vector2 GetCentroid (this ShapeData someShape) {
			float AFactor = 1.0f / (6 * someShape.GetArea ());

			float xSum = 0;
			float ySum = 0;

			for (int i = 0; i < someShape.Points.Count - 1; ++i) {
				float part = ShapeDataExtension.AreaSumPart (
					someShape.Points[i].X * someShape.Size,
					someShape.Points[i + 1].Y * someShape.Size,
					someShape.Points[i + 1].X * someShape.Size,
					someShape.Points[i].Y * someShape.Size
				);

				xSum +=
					(someShape.Points[i].X * someShape.Size
						+ someShape.Points[i + 1].X * someShape.Size)
					* part;

				ySum +=
					(someShape.Points[i].Y * someShape.Size
						+ someShape.Points[i + 1].Y * someShape.Size)
					* part;
			}

			return
				new Vector2 (xSum, ySum) * AFactor;
		}

		public static
		bool Contains (this ShapeData someShape, SpatialData somePosition, Vector2 somePoint) {
			/*take from
			 * http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
			 */
			int j = someShape.Points.Count - 1;
			bool c = false;
			for (int i = 0; i < someShape.Points.Count; j = i++) {
				bool unequal =
					((someShape.Points[i].Y * someShape.Size + somePosition.Y) > somePoint.Y)
					!=
					((someShape.Points[j].Y * someShape.Size + somePosition.Y) > somePoint.Y);

				float thingy =
					((someShape.Points[j].X * someShape.Size + somePosition.X)
						- (someShape.Points[i].X * someShape.Size + somePosition.X))
					* (somePoint.Y - (someShape.Points[i].Y * someShape.Size + somePosition.Y))
					/ ((someShape.Points[j].Y * someShape.Size + somePosition.Y)
							- (someShape.Points[i].Y * someShape.Size + somePosition.Y))
					+ (someShape.Points[i].X * someShape.Size + somePosition.X);

				if (unequal && somePoint.X < thingy) {
					c = !c;
				}
			}

			return c;
		}

	}

}
