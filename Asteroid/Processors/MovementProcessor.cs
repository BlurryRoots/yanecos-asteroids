namespace BlurryRoots.Asteroid.Processors {

	using System;
	using System.Collections.Generic;

	using BlurryRoots.Yanecos.Core;

	using BlurryRoots.Asteroid.Data;
	using BlurryRoots.Asteroid.Events;

	/// <summary>
	/// Description of MovementProcessor.
	/// </summary>
	public
	class MovementProcessor : DataProcessor {

		private
		EventManager eventManager { get; set; }

		private
		List<Type> interestList;

		private
		int width;
		private
		int height;

		public
		MovementProcessor (EventManager someEventManager, int someWidth, int someHeight) {
			this.eventManager = someEventManager;
			this.eventManager.AddHandler<ResizeEventArgs> (this.OnResize);

			this.interestList = new List<Type> () {
        typeof (SpatialData),
        typeof (VelocityData)
      };

			this.width = someWidth;
			this.height = someHeight;
		}

		protected override
		void OnInitialize () {

		}

		void OnResize (object sender, ResizeEventArgs e) {
			this.width = e.Width;
			this.height = e.Height;
		}

		protected override
		void OnProcessing (double someDeltaTime) {
			var entities = this.DataCenter.GetEntities (this.interestList);
			foreach (var entity in entities) {
				var sd = entity.GetData<SpatialData> ();
				var vd = entity.GetData<VelocityData> ();

				sd.X += vd.X * (float)someDeltaTime;
				sd.Y += vd.Y * (float)someDeltaTime;

				if (sd.X > this.width) {
					sd.X = sd.X - this.width;
				}
				else if (sd.X < 0) {
					sd.X = this.width - sd.X;
				}

				if (sd.Y > this.height) {
					sd.Y = sd.Y - this.height;
				}
				else if (sd.Y < 0) {
					sd.Y = this.height - sd.Y;
				}
			}
		}

	}

}
