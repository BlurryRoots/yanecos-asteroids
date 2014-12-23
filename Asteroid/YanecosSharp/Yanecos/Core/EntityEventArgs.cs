/*
 * Descrition: ...
 * 
 * Author: sven freiberg
 *
 */
namespace BlurryRoots.Yanecos.Core
{
    using System;
    
    /// <summary>
    /// Event data class for entity events.
    /// </summary>
    public class EntityEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of EntityEventArgs.
        /// </summary>
        /// <param name="someEntityId">Entity id.</param>
        public EntityEventArgs( ulong someEntityId )
        {
            this.EntityId = someEntityId;
        }
        
        /// <summary>
        /// Gets id of entity which has been manipulated.
        /// </summary>
        public ulong EntityId { get; private set; }
    }
}
