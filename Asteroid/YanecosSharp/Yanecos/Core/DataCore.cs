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
    /// DataCore holds all entity and data information.
    /// </summary>
    public 
        class DataCore
            : IDataCore
    {        
        public
            DataCore()
        {            
            this.EntityIDCounter = 0;
            
            //create table
            this.StatusEntityIDListTable = 
                new Dictionary<bool, List<ulong>>();
            //setup structure
            this.StatusEntityIDListTable.Add( true, new List<ulong>() );
            this.StatusEntityIDListTable.Add( false, new List<ulong>() );
            //create first free id = 1
            this.StatusEntityIDListTable[false].Add( ++EntityIDCounter );
            
            this.EntityIDDataTable = 
                new Dictionary<ulong, Dictionary<Type, IData>>();
            
            this.DataTypeEntityIDListTable = 
                new Dictionary<Type, List<ulong>>();
            
            this.EntityIDTagTable = 
                new Dictionary<ulong, string>();
            
            this.EntityTagEntityIDListTable = 
                new Dictionary<string, List<ulong>>();
        }
        
        /// <summary>
        /// Gets the current id count.
        /// </summary>
        public ulong EntityIDCounter
        {
            get; 
            set;
        }
        
        /// <summary>
        /// Gets the current number of registered entities.
        /// </summary>
        public int EntityCount 
        {
            get { return EntityIDDataTable.Count; }
        }
        
        /// <summary>
        /// Relational-Map of an status -> list of ids.
        /// </summary>
        public Dictionary<bool, List<ulong>> StatusEntityIDListTable 
        {
            get;            
            private set;
        }
        
        /// <summary>
        /// Relational-Map of an entity(id) -> its components.
        /// </summary>
        public Dictionary<ulong, Dictionary<Type, IData>> EntityIDDataTable 
        {
            get;            
            private set;
        }
        
        /// <summary>
        /// Relational-Map of a type of component -> entities owning it.
        /// </summary>
        public Dictionary<Type, List<ulong>> DataTypeEntityIDListTable 
        {
            get;            
            private set;
        }
        
        /// <summary>
        /// Relational-Map of an entity(id) -> its tag. 
        /// </summary>
        public Dictionary<ulong, string> EntityIDTagTable 
        {
            get;            
            private set;
        }
        
        /// <summary>
        /// Relational-Map of a tag -> its entities.
        /// </summary>
        public Dictionary<string, List<ulong>> EntityTagEntityIDListTable 
        {
            get;            
            private set;
        }
    }
}
