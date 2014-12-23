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
namespace BlurryRoots.Asteroid.Data
{
    using System;
    
    using BlurryRoots.Yanecos.Core;
    
    using OpenTK.Graphics;
    using OpenTK.Graphics.OpenGL;
    
    /// <summary>
    /// Description of ClearOption.
    /// </summary>
    public class ClearOptionData : IData
    {
        public ClearOptionData()
        {
            this.BufferMask = 
                OpenTK.Graphics.OpenGL.ClearBufferMask.ColorBufferBit 
                | OpenTK.Graphics.OpenGL.ClearBufferMask.DepthBufferBit;
            
            this.Color = Color4.Black;
        }
        
        public
            OpenTK.Graphics.OpenGL.ClearBufferMask BufferMask { get; set; }
        
        public
            Color4 Color { get; set; }
    }
}
