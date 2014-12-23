/*
 * Descrition: ...
 * 
 * Author: sven freiberg
 *
 */
namespace BlurryRoots.Yanecos.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    
    /// <summary>
    /// Data center.
    /// </summary>
    public 
        class DataCenter 
            : IDataCenter
    {
        private 
            IEntityManager entityManager;
        
        private 
            IDataProcessorManager dataProcessorManager;
        
        private
            double deltaTime;
        
        private
            IDataCore dataCore;
        
        public 
            DataCenter( 
                IEntityManager someEntityManager, 
                IDataProcessorManager someDataProcessorManager,
                IDataCore someDataCore )
        {
            // someEntityManager
            if( someEntityManager == null )
            {
                throw new ArgumentNullException( "someEntityManager" );
            }
            
            this.entityManager = someEntityManager;
            this.entityManager.OnAttach( this );
            
            // someDataProcessorManager
            if( someDataProcessorManager == null )
            {
                throw new ArgumentNullException( "someDataProcessorManager" );
            }
            
            this.dataProcessorManager = someDataProcessorManager;
            this.dataProcessorManager.OnAttach( this );
            
            // someDataCore
            if( someDataCore == null )
            {
                throw new ArgumentNullException( "someDataCore" );
            }
            
            dataCore = someDataCore;
            
            deltaTime = 0.0;
        }
        
        /// <summary>
        /// Gets the entity manager.
        /// </summary>
        public 
            IEntityManager EntityManager
        {
            get { return entityManager; }
        }
        
        /// <summary>
        /// Gets the processor manager.
        /// </summary>
        public 
            IDataProcessorManager DataProcessorManager
        {
            get { return dataProcessorManager; }
        }
        
        /// <summary>
        /// Gets or sets the data core for this data center.
        /// </summary>
        public 
            IDataCore DataCore
        {
            get { return this.dataCore; }
            set 
            {
                this.dataCore = value;                
            }
        }
        
        /// <summary>
        /// Gets the time passed since last update.
        /// </summary>
        public 
            double DeltaTime
        {
            get { return this.deltaTime; }
        }
        
        /// <summary>
        /// Updates the data center.
        /// </summary>
        /// <param name="someDeltaTime">Time passed, since the last update.</param>
        public
            void Update( double someDeltaTime )
        {
            this.deltaTime = someDeltaTime;
        }
        
        /// <summary>
        /// Creates a new Entity.
        /// </summary>
        /// <returns>An entity.</returns>
        public 
            IEntity CreateEntity()
        {
            ulong id = this.entityManager.CreateEntity();
            
            return new Entity( id, this.entityManager );
        }
        
        /// <summary>
        /// Creates a new Entity.
        /// </summary>
        /// <param name="someData">Data to be added on newly created entity.</param>
        /// <returns>An entity.</returns>
        public 
            IEntity CreateEntity( IData[] someData )
        {
            IEntity entity = this.CreateEntity();
            
            if( someData != null && someData.Length > 0 )
            {
                foreach( IData data in someData )
                {
                    entity.AddData( data );
                }
            }
            
            return entity;
        }
        
        /// <summary>
        /// Removes a given entity.
        /// </summary>
        /// <param name="someEntity">Some entity.</param>
        public 
            void RemoveEntity( IEntity someEntity )
        {
            this.entityManager.RemoveEntity( someEntity.ID );
        }
        
        /// <summary>
        /// Gets a list of entities containing all given types of data.
        /// </summary>
        /// <param name="someDataTypeList">List of data types.</param>
        /// <returns>List of entities.</returns>
        public
            List<IEntity> GetEntities( List<Type> someDataTypeList )
        {
            List<ulong> entityIds = 
                this.entityManager.GetEntitiesWithData( someDataTypeList );
            
            List<IEntity> entities = new List<IEntity>();
            
            foreach( ulong id in entityIds )
            {
                entities.Add( new Entity( id, this.entityManager ) );
            }
            
            return entities;
        }
        
         /// <summary>
        /// Gets a list of entities containing all given types of data.
        /// </summary>
        /// <typeparam name="TDataType">Type of data to query for.</typeparam>
        /// <returns>List of entities.</returns>
        public 
            List<IEntity> GetEntities<TDataType>()
            where TDataType : IData
        {
            List<ulong> entityIds = 
                this.entityManager.GetEntitiesWithData<TDataType>();
            
            List<IEntity> entities = new List<IEntity>();
            
            foreach( ulong id in entityIds )
            {
                entities.Add( new Entity( id, this.entityManager ) );
            }
            
            return entities;            
        }
        
        /// <summary>
        /// Returns a list of entities with a particular tag.
        /// </summary>
        /// <param name="someTag">Some tag.</param>
        /// <returns>List of entities.</returns>
        public 
            List<IEntity> GetEntitiesWithTag( string someTag )
        {
            List<ulong> entityIds = 
                this.entityManager.GetEntitiesWithTag( someTag );
            
            List<IEntity> entities = new List<IEntity>();
            
            foreach( ulong id in entityIds )
            {
                entities.Add( new Entity( id, this.entityManager ) );
            }
            
            return entities;            
        }
        
        /// <summary>
        /// Adds a new processor.
        /// </summary>
        /// <param name="someProcessor">Some processor.</param>
        public 
            void AddProcessor( IDataProcessor someProcessor )
        {
            this.dataProcessorManager.AddProcessor( someProcessor );
            
        }
        
        /// <summary>
        /// Returns a given type of processor.
        /// </summary>
        /// <typeparam name="TProcessorType">Type of processor</typeparam>
        /// <returns>A processor.</returns>
        public 
            TProcessorType GetProcessor<TProcessorType>()
                where TProcessorType : IDataProcessor
        {
            return this.dataProcessorManager.GetProcessor<TProcessorType>();
        }
        
        /// <summary>
        /// Removes processor of given type.
        /// </summary>
        /// <typeparam name="TProcessorType">Type of processor</typeparam>
        public 
            void RemoveProcessor<TProcessorType>()
                where TProcessorType : IDataProcessor
        {
            this.dataProcessorManager.RemoveProcessor<TProcessorType>();
        }
    }
}
