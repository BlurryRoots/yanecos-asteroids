namespace BlurryRoots.Asteroid.Processors {

	using System;
	using System.Collections.Generic;

	using BlurryRoots.Yanecos.Core;
	using BlurryRoots.Asteroid.Data;

	using OpenTK;

	/// <summary>
	/// Description of CollisionProcessor.
	/// </summary>
	public
	class CollisionProcessor : DataProcessor {

		public
		CollisionProcessor () {
		}

		protected override
		void OnInitialize () {
		}

		protected override
		void OnProcessing (double someDeltaTime) {
			List<IEntity> asteroids =
					this.DataCenter.GetEntitiesWithTag ("Asteroid");
			List<IEntity> projectiles =
					this.DataCenter.GetEntitiesWithTag ("Projectile");

			List<ulong> removeList = new List<ulong> ();

			foreach (IEntity asteroid in asteroids) {
				SpatialData asteroidSpatial = asteroid.GetData<SpatialData> ();
				ShapeData asteroidShape = asteroid.GetData<ShapeData> ();

				foreach (IEntity projectile in projectiles) {
					SpatialData projectileSpatial = projectile.GetData<SpatialData> ();
					ShapeData projectileShape = projectile.GetData<ShapeData> ();

					foreach (Vector2 point in projectileShape.Points) {
						Vector2 actualPoint =
								(point * projectileShape.Size) + new Vector2 (projectileSpatial.X, projectileSpatial.Y);
						/*TODO: blow up the shape before check for containing ???!!!*/
						if (asteroidShape.Contains (asteroidSpatial, actualPoint)) {
							if (!removeList.Contains (asteroid.ID)) {
								removeList.Add (asteroid.ID);
							}

							if (!removeList.Contains (projectile.ID)) {
								removeList.Add (projectile.ID);
							}
						}
					}
				}
			}

			foreach (ulong id in removeList) {
				this.DataCenter.EntityManager.RemoveEntity (id);
			}
		}

	}

}
