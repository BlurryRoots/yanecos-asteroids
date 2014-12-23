/*
 * Descrition: ...
 * 
 * Author: sven freiberg
 *
 */
namespace BlurryRoots.Yanecos.Exception
{
    using System;
    
    /// <summary>
    /// Exception class throw when tried to access data on an entity which does
    /// not contains data of that type.
    /// </summary>
    public class EntityDoesNotHoldDataException : Exception
    {
        /// <summary>
        /// Creates a new instance of EntityDoesNotHoldDataException.
        /// </summary>
        /// <param name="someId">Entity id.</param>
        /// <param name="someDataType">Type of data.</param>
        public EntityDoesNotHoldDataException( ulong someId, Type someDataType )
        {
            this.EntityID = someId;
            this.DataType = someDataType;
        }
        
        /// <summary>
        /// Gets entity id.
        /// </summary>
        public ulong EntityID
        { 
            get; 
            private set; 
        }
        
        /// <summary>
        /// Gets type of data.
        /// </summary>
        public Type DataType
        { 
            get; 
            private set; 
        }
    }
}
