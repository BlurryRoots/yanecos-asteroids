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
    /// Manager class responsible for all registered processors.
    /// </summary>
    public 
        class DataProcessorManager 
            : IDataProcessorManager
    {     
        /// <summary>
        /// Reference to DataCenter.
        /// </summary>      
        private 
            IDataCenter dataCenter;
        
        /// <summary>
        /// List of registered processors.
        /// </summary>
        private 
            List<IDataProcessor> processorList;
                
        /// <summary>
        /// Dictionary holding type information for registered processors.
        /// </summary>
        private 
            Dictionary<Type, IDataProcessor> typeDictionary;
        
        /// <summary>
        /// Creates a new instance of DataProcessorManager.
        /// </summary>
        public 
            DataProcessorManager()
        {            
            processorList = new List<IDataProcessor>();
            typeDictionary = new Dictionary<Type, IDataProcessor>();
        }
        
        /// <summary>
        /// Gets called when manager is attached to data center.
        /// </summary>
        /// <param name="someDataCenter">Data center to be attached to.</param>        
        public
            void OnAttach( IDataCenter someDataCenter )
        {
            if( someDataCenter == null )
                throw new ArgumentNullException( "someDataCenter" );
            dataCenter = someDataCenter;            
        }
        
        /// <summary>
        /// Adds a new processor.
        /// </summary>
        /// <param name="someProcessor">Some processor.</param>        
        public 
            void AddProcessor(IDataProcessor someProcessor) 
        {
            Type typeToAdd = someProcessor.GetType();
            if( typeDictionary.ContainsKey( typeToAdd ) )
            {
                throw new ArgumentException( 
                    String.Format(
                        "Cannot add same type of processor twice! Type: {0}"
                        ,typeToAdd
                    )
                   );
            }
            someProcessor.OnAttach(this.dataCenter);
            
            typeDictionary.Add( typeToAdd, someProcessor );
            processorList.Add( someProcessor );
        }
        
        /// <summary>
        /// Gets a processor of given type.
        /// </summary>
        /// <returns>A processor.</returns>
        /// <exception cref="ArgumentException">If given type is not found.</exception>
        public 
            TProcessorType GetProcessor<TProcessorType>()
                where TProcessorType : IDataProcessor
        {
            Type typeToGet = typeof( TProcessorType );
            if( ! typeDictionary.ContainsKey( typeToGet ) )
            {
                throw new ArgumentException( 
                    "No processor with given type registered!" 
                );
            }
            
            return (TProcessorType)typeDictionary[typeToGet];
        }
        
        /// <summary>
        /// Removes processor of given type.
        /// </summary>
        /// <exception cref="ArgumentException">If given type is not found.</exception>
        public 
            void RemoveProcessor<TProcessorType>() 
                where TProcessorType : IDataProcessor
        {            
            Type typeToRemove = typeof( TProcessorType );
            if( ! typeDictionary.ContainsKey( typeToRemove ) )
            {
                throw new ArgumentException( 
                    "No processor with given type registered!" 
                );
            }
            
            IDataProcessor processorToRemove = typeDictionary[typeToRemove];
            typeDictionary.Remove( typeToRemove );
            
            processorList.Remove( processorToRemove );
        }
        
        /// <summary>
        /// Gets an enumerator for iterating over regsitered processors.
        /// </summary>
        /// <returns>Enumerator</returns>
        public 
            IEnumerator<IDataProcessor> GetEnumerator()
        {
            return processorList.GetEnumerator();
        }
        
        /// <summary>
        /// Gets an enumerator for iterating over regsitered processors.
        /// </summary>
        /// <returns>Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return processorList.GetEnumerator();
        }
    }
}
