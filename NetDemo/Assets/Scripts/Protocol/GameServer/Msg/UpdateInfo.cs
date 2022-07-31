using System;
using System.Collections.Generic;
using System.Text;
namespace GameServer
{
	public class UpdateInfo : BaseMsg
	{
		public override int GetBytesNum()
		{
			int num = 8;
			return num;
		}
		public override byte[] Writing()
		{
			int index = 0;
			byte[] bytes = new byte[GetBytesNum()];
			WriteInt(bytes, GetID(), ref index);
			WriteInt(bytes, bytes.Length - 8, ref index);
			return bytes;
		}
		public override int Reading(byte[] bytes, int beginIndex = 0)
		{
			int index = beginIndex;
			return index - beginIndex;
		}
		public override int GetID()
		{
			return 3001;
		}
	}
}