/*
 * Descrition: ...
 * 
 * Author: sven freiberg
 *
 */
namespace BlurryRoots.Yanecos.Core
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Interface describing an entity.
    /// </summary>
    public 
        interface IEntity
    {
        /// <summary>
        /// Gets the id of an entity.
        /// </summary>
        ulong ID { get; }
        
        /// <summary>
        /// Gets or sets the tag of this entity.
        /// </summary>
        string Tag { get; set; }
    
        /// <summary>
        /// Adds new data to this entity.
        /// </summary>
        /// <param name="someData">Some data.</param>
        void AddData( IData someData );
        
        /// <summary>
        /// Checks if entity holds given type of data.
        /// </summary>
        /// <typeparam name="TData">Type of data.</typeparam>
        /// <returns>True if data of given type is present, false otherwise.</returns>
        bool HasData<TData>() where TData : IData;
        
        /// <summary>
        /// Gets a particular type of data.
        /// </summary>
        /// <returns>Some data.</returns>
        TData GetData<TData>() 
            where TData : IData;
        
        /// <summary>
        /// Removes a particular type of data from this entity.
        /// </summary>
        /// <param name="someData"></param>
        void RemoveData<TData>()
            where TData : IData;
        
        /// <summary>
        /// Returns all data contained in this entity.
        /// </summary>
        /// <returns>List of data.</returns>
        List<IData> GetAllData();
    }
}
