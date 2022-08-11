using System;
using System.Collections.Generic;
using System.Text;
namespace GamePlayer
{
	public class RequestFightMsg : BaseMsg
	{
		public int id;
		public override int GetBytesNum()
		{
			int num = 8;
			num += 4;
			return num;
		}
		public override byte[] Writing()
		{
			int index = 0;
			byte[] bytes = new byte[GetBytesNum()];
			WriteInt(bytes, GetID(), ref index);
			WriteInt(bytes, bytes.Length - 8, ref index);
			WriteInt(bytes, id, ref index);
			return bytes;
		}
		public override int Reading(byte[] bytes, int beginIndex = 0)
		{
			int index = beginIndex;
			id = ReadInt(bytes, ref index);
			return index - beginIndex;
		}
		public override int GetID()
		{
			return 2016;
		}
	}
}