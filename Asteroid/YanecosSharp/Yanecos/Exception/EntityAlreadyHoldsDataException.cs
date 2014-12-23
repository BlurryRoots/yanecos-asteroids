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
    /// Exception throw when tried to add data to an entity already containing
    /// an instance of this data.
    /// </summary>
    public class EntityAlreadyHoldsDataException : Exception
    {
        /// <summary>
        /// Creates a new instance of EntityAlreadyHoldsDataException.
        /// </summary>
        /// <param name="someId">Entity id.</param>
        /// <param name="someDataType">Type of data.</param>
        public EntityAlreadyHoldsDataException( ulong someId, Type someDataType )
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
