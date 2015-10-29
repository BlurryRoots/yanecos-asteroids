namespace BlurryRoots.Asteroid.Processors {
	using System;
	using System.Collections.Generic;

	using BlurryRoots.Yanecos.Core;

	using BlurryRoots.Asteroid.Data;
	using BlurryRoots.Asteroid.Events;

	using OpenTK;
	using OpenTK.Input;

	/// <summary>
	/// Description of PlayerControlProcessor.
	/// </summary>
	public
	class PlayerControlProcessor : DataProcessor {

		private
		float Speed = 266.0f;

		private
		EventManager EventManager { get; set; }

		private
		KeyboardDevice Keyboard { get; set; }

		private
		List<Key> keysDown;

		private
		List<Type> interestList;


		private
		List<Key> AvailableKeys;

		public
		PlayerControlProcessor (
			EventManager someEventManager,
			KeyboardDevice someKeyboard) {
			this.EventManager = someEventManager;
			this.Keyboard = someKeyboard;

			this.keysDown =
					new List<Key> ();

			this.interestList = new List<Type> ()
            {
                typeof(PlayerData),
                typeof(SpatialData)
            };

			Type enumType = typeof (Key);
			this.AvailableKeys = new List<Key> ();
			string[] k = Enum.GetNames (enumType);
			foreach (string s in k) {
				this.AvailableKeys.Add ((Key)Enum.Parse (enumType, s));
			}
		}

		~PlayerControlProcessor () {
		}

		protected override
		void OnInitialize () {
			this.keysDown.Clear ();
		}

		protected override
		void OnProcessing (double someDeltaTime) {
			this.CheckKeys ();

			var entities = this.DataCenter.GetEntities (this.interestList);
			foreach (var e in entities) {
				var spatial = e.GetData<SpatialData> ();
				var sd = e.GetData<VelocityData> ();

				var radiants = (float)(Math.PI / 180.0) * (spatial.Rotation);
				var dir = new Vector2 (
						(float)Math.Cos (radiants),
						(float)Math.Sin (radiants)
				);

				//        
				var movement = new Vector2 ();
				if (this.keysDown.Contains (Key.W)) {
					movement += dir * this.Speed;
				}

				//if (this.keysDown.Contains (Key.A)) {
				//	movement += dir.PerpendicularRight * this.Speed;
				//}

				if (this.keysDown.Contains (Key.S)) {
					movement += -1 * dir * this.Speed;
				}

				//if (this.keysDown.Contains (Key.D)) {
				//	movement += dir.PerpendicularLeft * this.Speed;
				//}

				//
				var rotationDirection = 0f;
				if (this.keysDown.Contains (Key.A)) {
					rotationDirection -= 1;
				}

				if (this.keysDown.Contains (Key.D)) {
					rotationDirection += 1;
				}

				spatial.Rotation += rotationDirection * this.Speed * (float)someDeltaTime;

				sd.X += movement.X * (float)someDeltaTime;
				sd.Y += movement.Y * (float)someDeltaTime;
			}
		}

		private
		void OnKeyDown (Key someKey) {
			//
		}

		private
		void OnKeyUp (Key someKey) {
			if (someKey == Key.Space) {
				this.EventManager.PublishEvent (
						this,
						new TriggerPulledEventArgs ()
				);
			}
		}

		private
		void CheckKeys () {
			foreach (var k in this.AvailableKeys) {
				if (this.Keyboard[k]) {
					if (!this.keysDown.Contains (k)) {
						this.keysDown.Add (k);
						this.OnKeyDown (k);
					}
				}
				else {
					bool keyup = false;

					while (this.keysDown.Contains (k)) {
						this.keysDown.Remove (k);
						keyup = true;
					}

					if (keyup) {
						this.OnKeyUp (k);
					}
				}
			}

		}

	}
}
