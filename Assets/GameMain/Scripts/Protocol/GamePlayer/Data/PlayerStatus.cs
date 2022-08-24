using System;
using System.Collections.Generic;
using System.Text;
namespace GamePlayer
{
	public class PlayerStatus : BaseData
	{
		public int id;
		public E_PLAYER_STATUS playerStatus;
		public override int GetBytesNum()
		{
			int num = 0;
			num += 4;
			num += 4;
			return num;
		}
		public override byte[] Writing()
		{
			int index = 0;
			byte[] bytes = new byte[GetBytesNum()];
			WriteInt(bytes, id, ref index);
			WriteInt(bytes, Convert.ToInt32(playerStatus), ref index);
			return bytes;
		}
		public override int Reading(byte[] bytes, int beginIndex = 0)
		{
			int index = beginIndex;
			id = ReadInt(bytes, ref index);
			playerStatus = (E_PLAYER_STATUS)ReadInt(bytes, ref index);
			return index - beginIndex;
		}
	}
}