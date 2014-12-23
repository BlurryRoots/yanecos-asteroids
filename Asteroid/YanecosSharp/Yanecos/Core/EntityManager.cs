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
    using System.Linq;
    
    using BlurryRoots.Yanecos.Exception;
    
    /// <summary>
    /// Manager class responsible for creating, manipulating and deleting entities.
    /// </summary>
    public
        class EntityManager 
            : IEntityManager
    {
        /// <summary>
        /// Reference to containing data center.
        /// </summary>
        private
            IDataCenter dataCenter;
        
        /// <summary>
        /// Creates a new instance of EntityManager.
        /// </summary>
        public
            EntityManager()
        {            
        }
        
        /// <summary>
        /// Gets called when entity manager has been attached to data center.
        /// </summary>
        /// <param name="someDataCenter">Data center.</param>
        public
            void OnAttach( IDataCenter someDataCenter )
        {
            if( someDataCenter == null )
                throw new ArgumentNullException( "someDataCenter" );
            dataCenter = someDataCenter;            
        }
        
        /// <summary>
        /// Gets called when new entity has been created.
        /// </summary>
        public event EventHandler<EntityEventArgs> EntityCreated;
        
        /// <summary>
        /// Gets called when entity has been removed.
        /// </summary>
        public event EventHandler<EntityEventArgs> EntityRemoved;
        
        /// <summary>
        /// Gets called when data has been added to entity.
        /// </summary>
        public event EventHandler<EntityDataEventArgs> DataAdded;
        
        /// <summary>
        /// Gets called when data has been removed from entity.
        /// </summary>
        public event EventHandler<EntityDataEventArgs> DataRemoved;
        
        /// <summary>
        /// Gets the current number of entities in the system.
        /// </summary>
        public
            ulong EntityCount
        {
            get 
            { 
                return 
                   (ulong)this.dataCenter
                              .DataCore
                              .StatusEntityIDListTable[true]
                              .Count; 
            }
        }
        
        /// <summary>
        /// Creates a new entity id.
        /// </summary>
        /// <returns>New id.</returns>
        public
            ulong CreateEntity()
        {     
            return CreateEntity( "" );
        }
        
        /// <summary>
        /// Creates a new entity id.
        /// </summary>
        /// <param name="someTag">A tag for entity sorting.</param>
        /// <returns>New id.</returns>
        public
            ulong CreateEntity( string someTag )
        {
            ulong id;
            IDataCore core;
            
            //if there is no id available create a new one.
            if( !this.HasFreeID() )
            {
                this.CreateNewID();
            }
            
            //pop the next available id
            id = this.PopNextID();
            
            //fetch data core
            core = this.dataCenter.DataCore;
            
            //create new data storage for id
            core.EntityIDDataTable.Add( 
                id, 
                new Dictionary<Type, IData>() 
            );
            
            //create tag entry
            core.EntityIDTagTable.Add( id, someTag );
            
            //create tag -> entity relation
            if( !core.EntityTagEntityIDListTable.ContainsKey( someTag ) )
            {
                core.EntityTagEntityIDListTable.Add( 
                    someTag, 
                    new List<ulong>() 
                );
            }
            
            core.EntityTagEntityIDListTable[someTag].Add( id );
            
            // Fire entity created event
            if( this.EntityCreated != null )
            {
                this.EntityCreated( this, new EntityEventArgs( id ) );
            }
            
            //return id
            return id;
        }
        
        /// <summary>
        /// Removes an entity from the system.
        /// </summary>
        /// <param name="someId">Id for entity to be removed.</param>
        /// <returns>True if successfuly removed, else false.</returns>
        public
            void RemoveEntity( ulong someId )
        {
            if( ! this.ContainsEntity( someId ) )
            {
                throw new ArgumentException( 
                    "No entity with given id registered!" 
                );
            }
            
            IDataCore core = this.dataCenter.DataCore;
            
            string tag = core.EntityIDTagTable[someId];            
            List<IData> entityData = this.GetAllDataFromEntity( someId );
            
            core.EntityTagEntityIDListTable[tag].Remove( someId );
            core.EntityIDTagTable.Remove( someId );
            
            foreach( IData data in entityData )
            {
                Type t = data.GetType();
                core.DataTypeEntityIDListTable[t].Remove( someId );
            }
            core.EntityIDDataTable.Remove( someId );
            
            this.FreeID( someId );
            
            // Fire entity created event
            if( this.EntityRemoved != null )
            {
                this.EntityRemoved( this, new EntityEventArgs( someId ) );
            }
        }
        
        /// <summary>
        /// Sets the tag on an entity.
        /// </summary>
        /// <param name="someEntityId">An entity id.</param>
        /// <param name="someTag">Some tag.</param>
        public 
            void SetTagForEntity( ulong someEntityId, string someTag )
        {
            if( ! this.ContainsEntity( someEntityId ) )
            {
                throw new ArgumentException( 
                    "No entity with given id registered!" 
                );
            }
            
            if( someTag == null )
            {
                throw new ArgumentNullException( "someTag" );
            }
            
            IDataCore core = this.dataCenter.DataCore;
            
            string previousTag = core.EntityIDTagTable[someEntityId];
            core.EntityTagEntityIDListTable[previousTag].Remove( someEntityId );
            
            //create tag -> entity relation
            if( ! core.EntityTagEntityIDListTable.ContainsKey( someTag ) )
            {
                core.EntityTagEntityIDListTable.Add( 
                    someTag, 
                    new List<ulong>() 
                );
            }
            core.EntityTagEntityIDListTable[someTag].Add( someEntityId );
            
            core.EntityIDTagTable[someEntityId] = someTag;
        }
        
        /// <summary>
        /// Gets the tag for an entity.
        /// </summary>
        /// <param name="someEntityId">An entity id.</param>
        /// <returns>A tag.</returns>
        public 
            string GetTagForEntity( ulong someEntityId )
        {
            if( ! this.ContainsEntity( someEntityId ) )
            {
                throw new ArgumentException( 
                    "No entity with given id registered!" 
                );
            }
            
            IDataCore core = this.dataCenter.DataCore;
            
            return core.EntityIDTagTable[someEntityId];
        }
        
        /// <summary>
        /// Returns a list of entities with a particular tag.
        /// </summary>
        /// <param name="someTag">Some tag.</param>
        /// <returns>List of entity ids.</returns>
        public 
            List<ulong> GetEntitiesWithTag( string someTag )
        {
            IDataCore core = this.dataCenter.DataCore;
            
            if( ! core.EntityTagEntityIDListTable.ContainsKey( someTag ) )
            {
                return new List<ulong>();
            }
            
            return core.EntityTagEntityIDListTable[someTag];
        }
        
        /// <summary>
        /// Adds a certain data to a certain entity.
        /// </summary>
        /// <param name="someEntityId">An entity id.</param>
        /// <param name="someData">Data to be added.</param>
        public 
            void AddDataToEntity( ulong someEntityId, IData someData )
        {
            if( ! this.ContainsEntity( someEntityId ) )
            {
                throw new ArgumentException( 
                    "No entity with given id registered!" 
                );
            }
            
            if( someData == null )
            {
                throw new ArgumentNullException( "someData" );
            }
            
            IDataCore core = this.dataCenter.DataCore;
            Type dataType = someData.GetType();
            
            if( core.EntityIDDataTable[someEntityId].ContainsKey( dataType ) )
            {
                throw new EntityAlreadyHoldsDataException( someEntityId, dataType );
            }
            
            core.EntityIDDataTable[someEntityId].Add( 
                dataType, 
                someData 
            );
            
            if( ! core.DataTypeEntityIDListTable.ContainsKey( dataType ) )
            {
                core.DataTypeEntityIDListTable.Add( 
                    dataType, 
                    new List<ulong>() 
                );
            }
            
            core.DataTypeEntityIDListTable[dataType].Add( someEntityId );
            
            // Fire data added event
            if( this.DataAdded != null )
            {
                this.DataAdded( 
                   this, 
                   new EntityDataEventArgs( someEntityId, someData ) );
            }            
        }
        
        /// <summary>
        /// Checks if entity holds given type of data.
        /// </summary>
        /// <typeparam name="TData">Type of data to check.</typeparam>
        /// <param name="someEntityId">Some id.</param>
        /// <returns></returns>
        public
            bool HasEntityData<TData>( ulong someEntityId )
                where TData : IData
        {
            if( ! this.ContainsEntity( someEntityId ) )
            {
                throw new ArgumentException( 
                    "No entity with given id registered!" 
                );
            }
            
            IDataCore core = this.dataCenter.DataCore;
            Type dataType = typeof( TData );
            
            return core.EntityIDDataTable[someEntityId].ContainsKey( dataType );            
        }
        
        /// <summary>
        /// Returns data of given type for given entity id.
        /// </summary>
        /// <param name="someEntityId">Entity id.</param>
        /// <returns>Data</returns>
        public 
            TData GetDataFromEntity<TData>( ulong someEntityId )
                where TData : IData
        {
            if( ! this.ContainsEntity( someEntityId ) )
            {
                throw new ArgumentException( 
                    "No entity with given id registered!" 
                );
            }
            
            IDataCore core = this.dataCenter.DataCore;
            Type dataType = typeof( TData );
            
            if( ! core.EntityIDDataTable[someEntityId].ContainsKey( dataType ) )
            {
                throw new EntityDoesNotHoldDataException( someEntityId, dataType );
            }
            
            return (TData)core.EntityIDDataTable[someEntityId][dataType];
        }
        
        /// <summary>
        /// Removes a certain data from an entity.
        /// </summary>
        /// <param name="someEntityId">An entity.</param>
        public
            void RemoveDataFromEntity<TData>( ulong someEntityId ) 
                where TData : IData
        {
            IDataCore core = this.dataCenter.DataCore;
            Type dataType = typeof( TData );
            
            if( ! core.DataTypeEntityIDListTable.ContainsKey( dataType ) )
            {
                throw new ArgumentException( 
                    "Entity is not holding data of given type" 
                );
            }
            
            core.DataTypeEntityIDListTable[dataType].Remove( someEntityId );
            
            // fetch data then remove it
            IData removedData = core.EntityIDDataTable[someEntityId][dataType];
            core.EntityIDDataTable[someEntityId].Remove( dataType );           
            
            // Fire data removed event
            if( this.DataRemoved != null )
            {
                this.DataRemoved( 
                   this, 
                   new EntityDataEventArgs( someEntityId, removedData ) );
            }      
        }
        
        /// <summary>
        /// Returns a list of entities holding a particular type of data.
        /// </summary>
        /// <typeparam name="TData">Type of data.</typeparam>
        /// <returns>List of entities.</returns>
        public 
            List<ulong> GetEntitiesWithData<TData>()
                where TData : IData
        {            
            IDataCore core = this.dataCenter.DataCore;
            Type dataType = typeof( TData );
            List<ulong> entities;
         
            if( ! core.DataTypeEntityIDListTable.ContainsKey( dataType ) )
            {
                entities = new List<ulong>();
            }
            else
            {
                entities = core.DataTypeEntityIDListTable[dataType];
            }
            
            return entities;
        }        
        
        /// <summary>
        /// Returns a list of entities holding a particular set of data.
        /// </summary>
        /// <param name="someDataTypeList">Set of data</param>
        /// <returns>List of entities.</returns>
        public
            List<ulong> GetEntitiesWithData( List<Type> someDataTypeList )
        {
            if( someDataTypeList == null )
            {
                throw new ArgumentNullException( "someDataTypeList" );
            }
            
            IDataCore core = 
                this.dataCenter.DataCore;
            
            Dictionary<Type, List<ulong>> canditates = 
                new Dictionary<Type, List<ulong>>();
            
            List<ulong> entities = 
                new List<ulong>();
            
            if( someDataTypeList.Count == 0 )
            {
                return entities;
            }
            
            IEnumerable<ulong> intersectionList = null;
            foreach( Type dataType in someDataTypeList )
            {
                if( ! core.DataTypeEntityIDListTable.ContainsKey( dataType ) )
                {
                    entities.Clear();
                    break;
                }
                
                if( intersectionList == null )
                {
                    intersectionList = core.DataTypeEntityIDListTable[dataType];
                }
                
                intersectionList = 
                    intersectionList.Intersect( 
                        core.DataTypeEntityIDListTable[dataType] );
            }
            
            if( intersectionList != null )
            {
                entities.AddRange( intersectionList );
            }
            
            return entities;
        }
        
        /// <summary>
        /// Returns all data contained in an entity.
        /// </summary>
        /// <param name="someEntity">An entity.</param>
        /// <returns>List of data.</returns>
        public
            List<IData> GetAllDataFromEntity( ulong someEntityId )
        {
            if( ! this.ContainsEntity( someEntityId ) )
            {
                throw new ArgumentException( 
                    "No entity with given id registered!" 
                );
            }
            
            IDataCore core = this.dataCenter.DataCore;
            List<IData> entityDataList = new List<IData>();
            
            foreach( Dictionary<Type, IData> dict in core.EntityIDDataTable.Values )
            {
                foreach( IData data in dict.Values )
                {
                    entityDataList.Add( data );
                }
            }
            
            return entityDataList;
        }
        
        /// <summary>
        /// Checks if entity with given id exists.
        /// </summary>
        /// <param name="someId">Entity id.</param>
        /// <returns>True if registered, false otherwise.</returns>
        private
            bool ContainsEntity( ulong someId )
        {
            return this.dataCenter
                       .DataCore
                       .EntityIDDataTable
                       .ContainsKey( someId );
        }
        
        /// <summary>
        /// Frees given id for reuse.
        /// </summary>
        /// <param name="someId">Entity id.</param>
        private
            void FreeID( ulong someId )
        {
            IDataCore core = this.dataCenter.DataCore;
            
            core.StatusEntityIDListTable[true].Remove( someId );
            core.StatusEntityIDListTable[false].Add( someId );
        }
        
        /// <summary>
        /// Checks if there is an unused id.
        /// </summary>
        /// <returns>True if there is, false otherwise.</returns>
        private
            bool HasFreeID()
        {            
            return 
                this.dataCenter.DataCore
                               .StatusEntityIDListTable[false]
                               .Count > 0;
        }
        
        /// <summary>
        /// Creates a new id.
        /// </summary>
        private 
            void CreateNewID()
        {
            IDataCore core = this.dataCenter.DataCore;
            
            if( core.EntityIDCounter == ulong.MaxValue )
            {
                throw new ArithmeticException( 
                    "Can not create new entity. ulong limit reached!" 
                );
            }
            
            core.StatusEntityIDListTable[false].Add( ++core.EntityIDCounter );
        }
        
        /// <summary>
        /// Pops a new free id and marks it as used.
        /// </summary>
        /// <returns>An id.</returns>
        private
            ulong PopNextID()
        {
            ulong id = 0;
            int lastIndex = 0;            
            IDataCore core = this.dataCenter.DataCore;
            
            lastIndex = core.StatusEntityIDListTable[false].Count - 1;
            
            id = core.StatusEntityIDListTable[false][lastIndex];
            
            core.StatusEntityIDListTable[false].RemoveAt( lastIndex );
            core.StatusEntityIDListTable[true].Add( id );
            
            return id;
        }
    }
}
