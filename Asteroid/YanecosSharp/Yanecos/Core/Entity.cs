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
    /// Description of Entity.
    /// </summary>
    internal 
        class Entity 
            : IEntity
    {        
        private
            ulong id;
        
        private
            IEntityManager entityManager;
        
        public Entity( ulong someId, IEntityManager someEntityManager )
        {
            this.id = someId;
            
            this.entityManager = someEntityManager;
        }
        
        /// <summary>
        /// Gets the id of an entity.
        /// </summary>
        public ulong ID 
        {
            get { return this.id; }
        }
        
        /// <summary>
        /// Gets or sets the tag of this entity.
        /// </summary>
        public string Tag 
        {
            get { return this.entityManager.GetTagForEntity( this.id ); }
            set 
            {
                this.entityManager.SetTagForEntity( 
                    this.id, 
                    ( value != null ) ? value : ""
                );
            }
        }
        
        /// <summary>
        /// Adds new data to this entity.
        /// </summary>
        /// <param name="someData">Some data.</param>
        public 
            void AddData(IData someData)
        {
            if( someData == null )
            {
                throw new ArgumentNullException( "someData" );
            }
            
            this.entityManager.AddDataToEntity( this.id, someData );
        }
        
        /// <summary>
        /// Checks if entity holds given type of data.
        /// </summary>
        /// <typeparam name="TData">Type of data.</typeparam>
        /// <returns>True if data of given type is present, false otherwise.</returns>
        public
            bool HasData<TData>()
                where TData : IData
        {
            return this.entityManager.HasEntityData<TData>( this.id );
        }
        
        /// <summary>
        /// Gets a particular type of data.
        /// </summary>
        /// <returns>Some data.</returns>
        public 
            TData GetData<TData>()
                where TData : IData
        {
            return this.entityManager.GetDataFromEntity<TData>( this.id );
        }
        
        /// <summary>
        /// Removes a particular type of data from this entity.
        /// </summary>
        /// <param name="someData"></param>
        public 
            void RemoveData<TData>()
                where TData : IData
        {
            this.entityManager.RemoveDataFromEntity<TData>( this.id );
        }
        
        /// <summary>
        /// Returns all data contained in this entity.
        /// </summary>
        /// <returns>List of data.</returns>
        public 
            List<IData> GetAllData()
        {
            return this.entityManager.GetAllDataFromEntity( this.id );
        }
    }
}
