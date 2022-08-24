using System;
using System.Collections.Generic;
using System.Text;
namespace GamePlayer
{
	public class PlayerData : BaseData
	{
		public int id;
		public string name;
		public E_PLAYER_CAMP playerCamp;
		public override int GetBytesNum()
		{
			int num = 0;
			num += 4;
			num += 4 + Encoding.UTF8.GetByteCount(name);
			num += 4;
			return num;
		}
		public override byte[] Writing()
		{
			int index = 0;
			byte[] bytes = new byte[GetBytesNum()];
			WriteInt(bytes, id, ref index);
			WriteString(bytes, name, ref index);
			WriteInt(bytes, Convert.ToInt32(playerCamp), ref index);
			return bytes;
		}
		public override int Reading(byte[] bytes, int beginIndex = 0)
		{
			int index = beginIndex;
			id = ReadInt(bytes, ref index);
			name = ReadString(bytes, ref index);
			playerCamp = (E_PLAYER_CAMP)ReadInt(bytes, ref index);
			return index - beginIndex;
		}
	}
}