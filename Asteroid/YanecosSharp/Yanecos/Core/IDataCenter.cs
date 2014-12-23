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
    /// Interface for data center.
    /// </summary>
    public 
        interface IDataCenter
    {
        /// <summary>
        /// Gets the entity manager.
        /// </summary>
        IEntityManager EntityManager { get; }
        
        /// <summary>
        /// Gets the processor manager.
        /// </summary>
        IDataProcessorManager DataProcessorManager { get; }
        
        /// <summary>
        /// Gets or sets the data core for this data center.
        /// </summary>
        IDataCore DataCore { get; set; }
        
        /// <summary>
        /// Gets the time passed since last update.
        /// </summary>
        double DeltaTime { get; }

        /// <summary>
        /// Updates the data center.
        /// </summary>
        /// <param name="someDeltaTime">Time passed, since the last update.</param>
        void Update( double someDeltaTime );
        
        /// <summary>
        /// Creates a new Entity.
        /// </summary>
        /// <returns>An entity.</returns>
        IEntity CreateEntity();  

        /// <summary>
        /// Creates a new Entity.
        /// </summary>
        /// <param name="someData">Data to be added on newly created entity.</param>
        /// <returns>An entity.</returns>
        IEntity CreateEntity( IData[] someData );

        /// <summary>
        /// Removes a given entity.
        /// </summary>
        /// <param name="someEntity">Some entity.</param>
        void RemoveEntity( IEntity someEntity );      

        /// <summary>
        /// Gets a list of entities containing all given types of data.
        /// </summary>
        /// <param name="someDataTypeList">List of data types.</param>
        /// <returns>List of entities.</returns>
        List<IEntity> GetEntities( List<Type> someDataTypeList );
        
        /// <summary>
        /// Gets a list of entities containing all given types of data.
        /// </summary>
        /// <typeparam name="TDataType">Type of data to query for.</typeparam>
        /// <returns>List of entities.</returns>
        List<IEntity> GetEntities<TDataType>()
            where TDataType : IData;
        
        /// <summary>
        /// Returns a list of entities with a particular tag.
        /// </summary>
        /// <param name="someTag">Some tag.</param>
        /// <returns>List of entities.</returns>
        List<IEntity> GetEntitiesWithTag( string someTag );
        
        /// <summary>
        /// Adds a new processor.
        /// </summary>
        /// <param name="someProcessor">Some processor.</param>
        void AddProcessor( IDataProcessor someProcessor );
        
        /// <summary>
        /// Returns a given type of processor.
        /// </summary>
        /// <typeparam name="TProcessorType">Type of processor</typeparam>
        /// <returns>A processor.</returns>
        TProcessorType GetProcessor<TProcessorType>()
                where TProcessorType : IDataProcessor;
        
        /// <summary>
        /// Removes processor of given type.
        /// </summary>
        /// <typeparam name="TProcessorType">Type of processor</typeparam>
        void RemoveProcessor<TProcessorType>()
            where TProcessorType : IDataProcessor;
    }
}
