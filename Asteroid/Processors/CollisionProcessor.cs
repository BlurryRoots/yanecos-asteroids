namespace BlurryRoots.Asteroid.Processors {

	using System;
	using System.Collections.Generic;

	using BlurryRoots.Yanecos.Core;
	using BlurryRoots.Asteroid.Data;
	using BlurryRoots.Asteroid.Events;

	using OpenTK;

	/// <summary>
	/// Description of CollisionProcessor.
	/// </summary>
	public
	class CollisionProcessor : DataProcessor {

		private
		EventManager eventManager;

		public
		CollisionProcessor (EventManager someEventManager) {
			this.eventManager = someEventManager;
		}

		protected override
		void OnInitialize () {
		}

		protected override
		void OnProcessing (double someDeltaTime) {
			this.RemoveAsteroidsIfHit ();
			this.CheckIfPlayerWasHit ();
		}

		private
		void RemoveAsteroidsIfHit () {
			var asteroids =
					this.DataCenter.GetEntitiesWithTag ("Asteroid");
			var projectiles =
					this.DataCenter.GetEntitiesWithTag ("Projectile");

			var removeList = new List<ulong> ();

			foreach (var asteroid in asteroids) {
				var asteroidSpatial = asteroid.GetData<SpatialData> ();
				var asteroidShape = asteroid.GetData<ShapeData> ();

				foreach (IEntity projectile in projectiles) {
					var projectileSpatial = projectile.GetData<SpatialData> ();
					var projectileShape = projectile.GetData<ShapeData> ();

					foreach (var point in projectileShape.Points) {
						var actualPoint =
								(point * projectileShape.Size)
								+ new Vector2 (projectileSpatial.X, projectileSpatial.Y);
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

			foreach (var id in removeList) {
				this.DataCenter.EntityManager.RemoveEntity (id);
			}
		}

		private
		void CheckIfPlayerWasHit () {
			var asteroids =
					this.DataCenter.GetEntitiesWithTag ("Asteroid");
			var player =
					this.DataCenter.GetEntitiesWithTag ("Player")[0];

			var playerShape = player.GetData<ShapeData> ();
			var playerSpatial = player.GetData<SpatialData> ();

			var hit = false;
			foreach (var asteroid in asteroids) {
				if (hit) break;

				var asteroidSpatial = asteroid.GetData<SpatialData> ();
				var asteroidShape = asteroid.GetData<ShapeData> ();

				foreach (var point in asteroidShape.Points) {
					var actualPoint =
							(point * asteroidShape.Size)
							+ new Vector2 (asteroidSpatial.X, asteroidSpatial.Y);

					if (playerShape.Contains (playerSpatial, actualPoint)) {
						var asteroidMass = asteroid.GetData<MassData> ().Mass;
						var e = new PlayerHitEvent (asteroidMass);

						this.eventManager.PublishEvent (this, e);
						hit = true;
						break;
					}
				}
			}
		}
	}

}
