namespace BlurryRoots.Asteroid.Processors {

	using System;
	using System.Collections.Generic;

	using BlurryRoots.Yanecos.Core;

	using BlurryRoots.Asteroid.Data;

	/// <summary>
	/// Description of MovementProcessor.
	/// </summary>
	public
	class MovementProcessor : DataProcessor {

		private
		EventManager EventManager { get; set; }

		private
		List<Type> interestList;

		public
		MovementProcessor (EventManager someEventManager) {
			this.EventManager = someEventManager;

			this.interestList = new List<Type> ()
            {
                typeof(SpatialData),
                typeof(VelocityData)
            };
		}

		protected override
		void OnInitialize () {
		}

		protected override
		void OnProcessing (double someDeltaTime) {
			foreach (IEntity entity in this.DataCenter.GetEntities (this.interestList)) {
				SpatialData sd = entity.GetData<SpatialData> ();
				VelocityData vd = entity.GetData<VelocityData> ();

				sd.X += vd.X * (float)someDeltaTime;
				sd.Y += vd.Y * (float)someDeltaTime;
			}
		}

	}

}
