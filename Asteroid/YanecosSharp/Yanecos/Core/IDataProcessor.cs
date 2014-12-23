/*
 * Descrition: ...
 * 
 * Author: sven freiberg
 *
 */
namespace BlurryRoots.Yanecos.Core
{
    /// <summary>
    /// Interface describing a data processor.
    /// </summary>
    public
        interface IDataProcessor
    {        
        /// <summary>
        /// Called when attached to data center.
        /// </summary>
        /// <param name="someDataCenter">Some data center.</param>
        void OnAttach( IDataCenter someDataCenter );
        
        /// <summary>
        /// Gets the id for this processor.
        /// </summary>
        ulong ID { get; }
        
        /// <summary>
        /// Get or set whether this processor is active and therefor
        /// should be updated.
        /// </summary>
        bool IsActive { get; set; }
        
        /// <summary>
        /// Gets reference to data center.
        /// </summary>
        IDataCenter DataCenter { get; }
        
        /// <summary>
        /// Gets called when data processor should process.
        /// </summary>
        void Process();
    }
}
