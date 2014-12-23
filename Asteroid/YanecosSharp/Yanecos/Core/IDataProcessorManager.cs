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
    /// Interface describing a processor manager.
    /// </summary>
    public 
        interface IDataProcessorManager : IEnumerable<IDataProcessor>
    {        
        /// <summary>
        /// Gets called when manager is attached to data center.
        /// </summary>
        /// <param name="someDataCenter">Data center to be attached to.</param>
        void OnAttach( IDataCenter someDataCenter );
        
        /// <summary>
        /// Adds a new processor.
        /// </summary>
        /// <param name="someProcessor">Some processor.</param>
        void AddProcessor( IDataProcessor someProcessor );
                
        /// <summary>
        /// Gets a processor of given type.
        /// </summary>
        /// <returns>A processor.</returns>
        /// <exception cref="ArgumentException">If given type is not found.</exception>
        TProcessorType GetProcessor<TProcessorType>()
            where TProcessorType : IDataProcessor;
        
        /// <summary>
        /// Removes processor of given type.
        /// </summary>
        /// <exception cref="ArgumentException">If given type is not found.</exception>
        void RemoveProcessor<TProcessorType>()
            where TProcessorType : IDataProcessor;        
    }
}
