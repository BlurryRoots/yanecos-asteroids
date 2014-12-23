/*
 * Descrition: ...
 * 
 * Author: sven freiberg
 *
 */
namespace BlurryRoots.Yanecos.Core
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Manager responsible for creating, manipulating and deleting entities.
    /// </summary>
    public 
        interface IEntityManager
    {        
        /// <summary>
        /// Gets the current number of entities in the system.
        /// </summary>
        ulong EntityCount { get; }
        
        /// <summary>
        /// Gets called when entity manager has been attached to data center.
        /// </summary>
        /// <param name="someDataCenter">Data center.</param>
        void OnAttach( IDataCenter someDataCenter );
        
        /// <summary>
        /// Creates a new entity id.
        /// </summary>
        /// <returns>New id.</returns>
        ulong CreateEntity();
        
        /// <summary>
        /// Creates a new entity id.
        /// </summary>
        /// <param name="someTag">A tag for entity sorting.</param>
        /// <returns>New id.</returns>
        ulong CreateEntity( string someTag );
        
        /// <summary>
        /// Gets called when new entity has been created.
        /// </summary>
        event EventHandler<EntityEventArgs> EntityCreated;
        
        /// <summary>
        /// Removes an entity from the system.
        /// </summary>
        /// <param name="someId">Id for entity to be removed.</param>
        /// <returns>True if successfuly removed, else false.</returns>
        void RemoveEntity( ulong someId );
        
        /// <summary>
        /// Gets called when entity has been removed.
        /// </summary>
        event EventHandler<EntityEventArgs> EntityRemoved;
        
        /// <summary>
        /// Sets the tag on an entity.
        /// </summary>
        /// <param name="someEntityId">An entity id.</param>
        /// <param name="someTag">Some tag.</param>
        void SetTagForEntity( ulong someEntityId, string someTag );
        
        /// <summary>
        /// Gets the tag for an entity.
        /// </summary>
        /// <param name="someEntityId">An entity id.</param>
        /// <returns>A tag.</returns>
        string GetTagForEntity( ulong someEntityId );
        
        /// <summary>
        /// Returns a list of entities with a particular tag.
        /// </summary>
        /// <param name="someTag">Some tag.</param>
        /// <returns>List of entity ids.</returns>
        List<ulong> GetEntitiesWithTag( string someTag );
        
        /// <summary>
        /// Adds a certain data to a certain entity.
        /// </summary>
        /// <param name="someEntityId">An entity id.</param>
        /// <param name="someData">Data to be added.</param>
        void AddDataToEntity( ulong someEntityId, IData someData );
        
        /// <summary>
        /// Gets called when data has been added to entity.
        /// </summary>
        event EventHandler<EntityDataEventArgs> DataAdded;
        
        /// <summary>
        /// Checks if entity holds given type of data.
        /// </summary>
        /// <typeparam name="TData">Type of data to check.</typeparam>
        /// <param name="someEntityId">Some id.</param>
        /// <returns></returns>
        bool HasEntityData<TData>( ulong someEntityId )
            where TData : IData;
        
        /// <summary>
        /// Returns data of given type for given entity id.
        /// </summary>
        /// <param name="someEntityId">Entity id.</param>
        /// <returns>Data</returns>
        TData GetDataFromEntity<TData>( ulong someEntityId ) 
            where TData : IData;
        
        /// <summary>
        /// Removes a certain data from an entity.
        /// </summary>
        /// <param name="someEntityId">An entity.</param>
        void RemoveDataFromEntity<TData>( ulong someEntityId ) 
            where TData : IData;
        
        /// <summary>
        /// Gets called when data has been removed from entity.
        /// </summary>
        event EventHandler<EntityDataEventArgs> DataRemoved;
        
        /// <summary>
        /// Returns a list of entities holding a particular type of data.
        /// </summary>
        /// <typeparam name="TData">Type of data.</typeparam>
        /// <returns>List of entities.</returns>
        List<ulong> GetEntitiesWithData<TData>() 
            where TData : IData;
        
        /// <summary>
        /// Returns a list of entities holding a particular set of data.
        /// </summary>
        /// <param name="someDataTypeList">Set of data</param>
        /// <returns>List of entities.</returns>
        List<ulong> GetEntitiesWithData( List<Type> someDataTypeList );
        
        /// <summary>
        /// Returns all data contained in an entity.
        /// </summary>
        /// <param name="someEntity">An entity.</param>
        /// <returns>List of data.</returns>
        List<IData> GetAllDataFromEntity( ulong someEntity );
    }
}
