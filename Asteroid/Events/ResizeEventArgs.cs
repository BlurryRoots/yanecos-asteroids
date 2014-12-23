//
//    Copyright (C) 2013 sven freiberg
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
namespace BlurryRoots.Asteroid.Events
{
    using System;
    
    /// <summary>
    /// Description of ResizeEventArgs.
    /// </summary>
    public
        class ResizeEventArgs : EventArgs
    {
        public
            ResizeEventArgs( int someWidth, int someHeight )
        {
            this.Width = someWidth;
            
            this.Height = someHeight;
        }
        
        public
            int Width { get; private set; }
        
        public
            int Height { get; private set; }
    }
}
