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
    /// Interface describing a data core.
    /// </summary>
    public 
        interface IDataCore
    {
        /// <summary>
        /// Gets the current id count.
        /// </summary>
        ulong EntityIDCounter
        { get; set; }
        
        /// <summary>
        /// Gets the current number of registered entities.
        /// </summary>
        int EntityCount 
        { get; }
        
        /// <summary>
        /// Relational-Map of an status -> list of ids.
        /// </summary>
        Dictionary<bool,List<ulong>> StatusEntityIDListTable 
        { get; }
        
        /// <summary>
        /// Relational-Map of an entity(id) -> its components.
        /// </summary>
        Dictionary<ulong, Dictionary<Type, IData>> EntityIDDataTable
        { get; }
        
        /// <summary>
        /// Relational-Map of a type of component -> entities owning it.
        /// </summary>
        Dictionary<Type, List<ulong>> DataTypeEntityIDListTable
        { get; }
        
        /// <summary>
        /// Relational-Map of an entity(id) -> its tag. 
        /// </summary>
        Dictionary<ulong, string> EntityIDTagTable
        { get; }
        
        /// <summary>
        /// Relational-Map of a tag -> its entities.
        /// </summary>
        Dictionary<string, List<ulong>> EntityTagEntityIDListTable
        { get; }
    }
}
