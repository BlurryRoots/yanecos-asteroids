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
    /// Base class for data processors.
    /// </summary>
    public
        abstract class DataProcessor 
            : IDataProcessor
    {
        private 
            static ulong IDCounter = 0;
        
        #region fields
        
        /// <summary>
        /// Backing field for ID.
        /// </summary>
        private 
            readonly ulong id;
        
        /// <summary>
        /// Indicates wheater this processor is ready to roll.
        /// </summary>
        private 
            bool isInitialized;
        
        /// <summary>
        /// Backing field for IsActive.
        /// </summary>
        private 
            bool isActive;
        
        /// <summary>
        /// Backing field for DataCenter.
        /// </summary>
        private 
            IDataCenter dataCenter;
        
        #endregion fields
        
        /// <summary>
        /// Creates a new instance of DataProcessor.
        /// </summary>
        public 
            DataProcessor()
        {
            this.id = IDCounter++;
            
            this.isInitialized = false;
            
            this.isActive = true;
        }
        
        /// <summary>
        /// Called when attached to data center.
        /// </summary>
        /// <param name="someDataCenter">Some data center.</param>        
        public
            void OnAttach( IDataCenter someDataCenter )
        {            
            if( someDataCenter == null )
            {
                throw new ArgumentNullException( "someDataCenter" );
            }
            
            this.dataCenter = someDataCenter;
        }
        
        /// <summary>
        /// Gets the id for this processor.
        /// </summary>
        public 
            ulong ID
        {
            get { return this.id; }
        }
        
        /// <summary>
        /// Get or set whether this processor is active and therefor
        /// should be updated.
        /// </summary>
        public 
            bool IsActive
        {
            get { return this.isActive; }
            set 
            {
                if( this.isActive == value )
                {
                    return;
                }
                
                this.isActive = value;
            }
        }
        
        /// <summary>
        /// Gets reference to data center.
        /// </summary>
        public 
            IDataCenter DataCenter
        {
            get { return this.dataCenter; }
        }
        
        /// <summary>
        /// Gets called when data processor should process.
        /// </summary>
        public 
            void Process()
        {
            if( !this.isInitialized )
            {
                this.OnInitialize();
                this.isInitialized = true;
            }

            if( this.isActive )
            {
                this.OnProcessing( this.dataCenter.DeltaTime );
            }
        }
        
        /// <summary>
        /// Gets called when processor is ought to be initialized.
        /// </summary>
        protected 
            abstract void OnInitialize();
        
        /// <summary>
        /// Gets called when processor is ought to process.
        /// </summary>
        /// <param name="someDeltaTime"></param>
        protected 
            abstract void OnProcessing( double someDeltaTime );
    }
}
