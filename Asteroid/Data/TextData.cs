namespace BlurryRoots.Asteroid.Data {

	using System;
	using System.Drawing;

	using BlurryRoots.Yanecos.Core;

	/// <summary>
	/// Description of TextData.
	/// </summary>
	public
	class TextData : IData {

		public
		TextData () {
		}

		public
		string Text { get; set; }

		public
		Font Font { get; set; }

		public
		Brush Brush { get; set; }

	}

}
