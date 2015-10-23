namespace BlurryRoots.Asteroid.Data
{
    using System;
    
    using BlurryRoots.Yanecos.Core;
    
    /// <summary>
    /// Description of PlayerData.
    /// </summary>
    [Serializable]
    public
    class PlayerData : IData {
        public 
        PlayerData( string someName ) {
            this.Name = someName;
        }
        
        public
				string Name { get; set; }

    }

}
