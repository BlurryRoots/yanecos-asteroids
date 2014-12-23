/*
 * Descrition: ...
 * 
 * Author: sven freiberg
 *
 */
namespace BlurryRoots.Yanecos.Core
{    
    /// <summary>
    /// Event data for entity data events.
    /// </summary>
    public class EntityDataEventArgs : EntityEventArgs
    {
        /// <summary>
        /// Creates a new instance of EntityDataEventArgs.
        /// </summary>
        /// <param name="someEntityId">Entity id.</param>
        /// <param name="someData">Data being manipulated.</param>
        public EntityDataEventArgs( ulong someEntityId, IData someData )
            : base( someEntityId )
        {
            this.Data = someData;
        }
        
        /// <summary>
        /// Gets data which has been manipulated.
        /// </summary>
        public IData Data { get; private set; }
    }
}
