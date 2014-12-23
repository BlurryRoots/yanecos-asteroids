//
//    Copyright (C) 2013 sven freiberg
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
namespace BlurryRoots.Asteroid
{
    using System;    
    using System.Collections.Generic;
    
    /// <summary>
    /// Description of EventManager.
    /// </summary>
    public 
        class EventManager
    {
        private
            class Event
        {
            public
                Event( object someSender, EventArgs someArguments )
            {
                this.Sender = someSender;
                
                this.Arguments = someArguments;
            }
            
            public
                object Sender { get; private set; }
            
            public
                EventArgs Arguments { get; private set; }
        }
        
        private
            Dictionary<Type, List<Delegate>> eventInvokationDictionary;
        
        private
            Queue<Event> eventQueue;
        
        public 
            EventManager()
        {
            this.eventInvokationDictionary = 
                new Dictionary<Type, List<Delegate>>();
            
            this.eventQueue = 
                new Queue<Event>();
        }
        
        public
            void AddHandler<TEventArgsType>( EventHandler<TEventArgsType> someHandler )
            where TEventArgsType : EventArgs
        {
            Type t = typeof(TEventArgsType);
            
            if( !this.eventInvokationDictionary.ContainsKey( t ) )
            {
                this.eventInvokationDictionary.Add(
                    t,
                    new List<Delegate>()
                );
            }
            
            this.eventInvokationDictionary[t].Add( someHandler );
        }
        
        public
            void RemoveHandler<TEventArgsType>( EventHandler<TEventArgsType> someHandler )
            where TEventArgsType : EventArgs
        {
            Type t = typeof(TEventArgsType);
            
            this.eventInvokationDictionary.Remove( t );
        }
        
        public
            void PublishEvent( object someSender, EventArgs someEventArguments )
        {
            Type t = someEventArguments.GetType();
            
            this.eventQueue.Enqueue( 
                new Event( someSender, someEventArguments )
            );
        }
        
        public
            void ProcessEvents()
        {
            while( this.eventQueue.Count > 0 )
            {
                Event e = this.eventQueue.Dequeue();
                
                this.FireEvent( e );
            }
        }
        
        private
            void FireEvent( Event someEvent )
        {
            Type t = someEvent.Arguments.GetType();
            
            if( this.eventInvokationDictionary.ContainsKey( t ) )
            {
                foreach( Delegate d in this.eventInvokationDictionary[t] )
                {
                    d.Method.Invoke(d.Target, 
                    //d.DynamicInvoke( 
                        new object[] 
                        { 
                            someEvent.Sender, 
                            someEvent.Arguments
                        }
                    );
                }
            }
        }
    }
}
