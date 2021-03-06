﻿namespace BlurryRoots.Asteroid.Processors {

	using System;
	using System.Collections.Generic;
	using System.Linq;

	using BlurryRoots.Yanecos.Core;

	using BlurryRoots.Asteroid.Data;
	using BlurryRoots.Asteroid.Events;

	using OpenTK;

	/// <summary>
	/// Description of AsteroidSpawnProcess.
	/// </summary>
	public
	class AsteroidSpawnProcessor : DataProcessor {

		private
		readonly string AsteroidTag = "Asteroid";

		private
		readonly int MaxAsteroids = 23;

		private
		EventManager eventManager;

		private
		Random random;

		private
		int screenWidth;
		private
		int screenHeight;

		private
		double spawnTimeToWait;
		private
		double spawnTimeAccu;

		public
		AsteroidSpawnProcessor (
			EventManager someEventManager,
			int someWidth,
			int someHeight,
			double someSpawnTimeToWait) {
			this.eventManager = someEventManager;

			this.random = new Random ();

			this.screenWidth = someWidth;
			this.screenHeight = someHeight;

			this.spawnTimeToWait = someSpawnTimeToWait;
			this.spawnTimeAccu = 0;

			this.eventManager.AddHandler<ResizeEventArgs> (
					this.OnResize
			);
		}

		~AsteroidSpawnProcessor () {
			this.eventManager.RemoveHandler<ResizeEventArgs> (
					this.OnResize
			);
		}

		protected override
		void OnInitialize () {
		}

		protected override
		void OnProcessing (double someDeltaTime) {
			var asteroids = this.GetAsteroids ();

			if (asteroids.Count > 0) {
				var removeList = new List<ulong> ();

				foreach (var asteroid in asteroids) {
					var sd = asteroid.GetData<SpatialData> ();

					if (sd.X >= this.screenWidth || sd.X < 0 || sd.Y >= this.screenHeight || sd.Y < 0) {
						removeList.Add (asteroid.ID);
					}
				}

				foreach (ulong id in removeList) {
					this.DataCenter.EntityManager.RemoveEntity (id);
				}
			}

			if (this.spawnTimeAccu >= this.spawnTimeToWait) {
				this.spawnTimeAccu = 0;

				if (asteroids.Count < this.MaxAsteroids) {
					this.CreateAsteroid ();
				}
			}
			else {
				this.spawnTimeAccu += someDeltaTime;
			}
		}

		private
		List<IEntity> GetAsteroids () {
			return this.DataCenter.GetEntitiesWithTag (AsteroidTag);
		}

		private
		bool GetNextBool () {
			return this.random.Next (0, 100) % 2 == 0;
		}

		private
		object[] CreatePositionAndVelocity () {
			int x, y;
			int vx, vy;
			x = this.random.Next (0, this.screenWidth);
			if (x <= 42) {
				// spawn left
				y = this.random.Next (0, this.screenHeight);

				vx = this.random.Next (50, 200);
				vy = this.random.Next (50, 200) * (this.GetNextBool () ? 1 : -1);
			}
			else if (x >= this.screenWidth - 42) {
				// spawn right
				y = this.random.Next (0, this.screenHeight);

				vx = this.random.Next (50, 200) * -1;
				vy = this.random.Next (50, 200) * (this.GetNextBool () ? 1 : -1);
			}
			else //if( x > 42 && x < someAppData.Width - 42 )
            {
				if (this.GetNextBool ()) {
					// spawn top
					y = this.random.Next (0, 42);

					vx = this.random.Next (50, 200) * (this.GetNextBool () ? 1 : -1);
					vy = this.random.Next (50, 200);
				}
				else {
					// spawn bottom                    
					y = this.random.Next (this.screenHeight - 42, this.screenHeight);

					vx = this.random.Next (100, 400) * (this.GetNextBool () ? 1 : -1);
					vy = this.random.Next (100, 400) * -1;
				}
			}

			return new object[]
            {
                new SpatialData( x, y )
                {
                    Rotation = this.random.Next(0, 100) * (this.GetNextBool() ? 1 : -1 )
                },
                new VelocityData( vx, vy )
            };
		}

		private
		ShapeData CreateShape (int someMinSize, int someMaxSize, int someMinVertecies, int someMaxVerticies) {
			if (someMaxSize < someMinSize) {
				throw new Exception ("Nope!");
			}

			if (someMinSize < 0 || someMaxSize < 0) {
				throw new Exception ("Nope!");
			}

			if (someMinVertecies < 3 || someMaxVerticies < someMinVertecies || someMinVertecies < 0 || someMaxVerticies < 0) {
				throw new Exception ("Nope!");
			}

			List<Vector2> vertexList = new List<Vector2> ()
            {
                new Vector2( 0, 0 ),
                new Vector2( 1, 0 ),
                new Vector2( 1, 1 )
            };

			int addintional = this.random.Next (0, someMaxVerticies - someMinVertecies);

			if (addintional >= 1) {
				vertexList.Insert (
						1,
						new Vector2 (0.5f, -0.5f)
				);
			}

			if (addintional >= 2) {
				vertexList.Insert (
						3,
						new Vector2 (1.5f, 0.5f)
				);
			}

			if (addintional >= 2) {
				vertexList.Add (
						new Vector2 (0.5f, 1.0f)
				);
			}

			return new ShapeData (
					this.random.Next (someMinSize, someMaxSize),
					vertexList
			);
		}

		private
		void CreateAsteroid () {
		object[] posAndVel = this.CreatePositionAndVelocity ();

			IEntity e = this.DataCenter.CreateEntity ();
			e.Tag = "Asteroid";
			e.AddData (
					posAndVel[0] as SpatialData
			);
			e.AddData (
					posAndVel[1] as VelocityData
			);
			e.AddData (
					this.CreateShape (24, 42, 3, 7)
			);
			e.AddData (
					new MassData (this.random.Next (100, 10000))
			);
		}

		private
			void OnResize (object someSender, ResizeEventArgs someArgument) {
			this.screenWidth = someArgument.Width;
			this.screenHeight = someArgument.Height;
		}

	}

}
