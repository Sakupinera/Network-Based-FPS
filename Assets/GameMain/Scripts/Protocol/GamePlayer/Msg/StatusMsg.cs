using System;
using System.Collections.Generic;
using System.Text;
namespace GamePlayer
{
	public class StatusMsg : BaseMsg
	{
		public PlayerStatus playerStatus;
		public override int GetBytesNum()
		{
			int num = 8;
			num += playerStatus.GetBytesNum();
			return num;
		}
		public override byte[] Writing()
		{
			int index = 0;
			byte[] bytes = new byte[GetBytesNum()];
			WriteInt(bytes, GetID(), ref index);
			WriteInt(bytes, bytes.Length - 8, ref index);
			WriteData(bytes, playerStatus, ref index);
			return bytes;
		}
		public override int Reading(byte[] bytes, int beginIndex = 0)
		{
			int index = beginIndex;
			playerStatus = ReadData<PlayerStatus>(bytes, ref index);
			return index - beginIndex;
		}
		public override int GetID()
		{
			return 2001;
		}
	}
}