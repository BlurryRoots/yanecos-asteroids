namespace BlurryRoots.Asteroid {

	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Responsible for queuing events for deferred execution.
	/// </summary>
	public
			class EventManager {

		/// <summary>
		/// Publish event.
		/// </summary>
		/// <param name="someSender">Event sender.</param>
		/// <param name="someEventArguments">Arguments of event.</param>
		public
				void PublishEvent (object someSender, EventArgs someEventArguments) {
			Type t = someEventArguments.GetType ();

			this.eventQueue.Enqueue (
					new Event (someSender, someEventArguments)
			);
		}

		/// <summary>
		/// Adds handler for specific type of event.
		/// </summary>
		/// <typeparam name="TEventArgsType">Type of event to handle.</typeparam>
		/// <param name="someHandler">Handler callback.</param>
		public
				void AddHandler<TEventArgsType> (EventHandler<TEventArgsType> someHandler)
				where TEventArgsType : EventArgs {
			Type t = typeof (TEventArgsType);

			if (!this.eventInvokationDictionary.ContainsKey (t)) {
				this.eventInvokationDictionary.Add (
						t,
						new List<Delegate> ()
				);
			}

			this.eventInvokationDictionary[t].Add (someHandler);
		}

		/// <summary>
		/// Removes handler for specific type of event.
		/// </summary>
		/// <typeparam name="TEventArgsType">Type of event to handle.</typeparam>
		/// <param name="someHandler">Handler callback.</param>
		public
				void RemoveHandler<TEventArgsType> (EventHandler<TEventArgsType> someHandler)
				where TEventArgsType : EventArgs {
			Type t = typeof (TEventArgsType);

			this.eventInvokationDictionary.Remove (t);
		}

		/// <summary>
		/// Executes all queued events.
		/// </summary>
		public
				void ProcessEvents () {
			while (this.eventQueue.Count > 0) {
				Event e = this.eventQueue.Dequeue ();

				this.FireEvent (e);
			}
		}

		/// <summary>
		/// Creates a new EventManager.
		/// </summary>
		public
				EventManager () {
			this.eventInvokationDictionary =
					new Dictionary<Type, List<Delegate>> ();

			this.eventQueue =
					new Queue<Event> ();
		}

		/// <summary>
		/// Invokes all handlers for given event.
		/// </summary>
		/// <param name="someEvent">Event to fire.</param>
		private
				void FireEvent (Event someEvent) {
			Type t = someEvent.Arguments.GetType ();

			if (this.eventInvokationDictionary.ContainsKey (t)) {
				foreach (Delegate d in this.eventInvokationDictionary[t]) {
					d.Method.Invoke (d.Target,
						new object[]  { 
                someEvent.Sender, 
                someEvent.Arguments
            }
					);
				}
			}
		}

		/// <summary>
		/// Holds all handlers for specific types of events.
		/// </summary>
		private
				Dictionary<Type, List<Delegate>> eventInvokationDictionary;

		/// <summary>
		/// Queue of previously published events, not yet processed.
		/// </summary>
		private
				Queue<Event> eventQueue;

		/// <summary>
		/// Internal structure for an event.
		/// </summary>
		private
				class Event {

			public
					Event (object someSender, EventArgs someArguments) {
				this.Sender = someSender;

				this.Arguments = someArguments;
			}

			public
					object Sender { get; private set; }

			public
					EventArgs Arguments { get; private set; }
		}

	}
}
