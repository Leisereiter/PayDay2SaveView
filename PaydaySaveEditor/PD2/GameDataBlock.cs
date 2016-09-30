using System.Collections.Generic;
using System.IO;

namespace PaydaySaveEditor.PD2
{
	public class GameDataBlock : DataBlock
	{
		public Dictionary<object, object> Dictionary { get; set; }

		public GameDataBlock(BinaryReader br) : base(br)
		{
			BinaryReader dataBr = new BinaryReader(new MemoryStream(data));

			// The game data block could potentially not be
			// just a dictionary, but we assume that it must
			// be as I haven't seen any saves that don't con-
			// tain a dictionary as a root node
			this.Dictionary = (Dictionary<object, object>) GameData.DeserializeData(dataBr);
		}

		new public byte[] ToArray()
		{
			this.data = GameData.SerializeData(Dictionary);
			return base.ToArray();
		}
	}
}