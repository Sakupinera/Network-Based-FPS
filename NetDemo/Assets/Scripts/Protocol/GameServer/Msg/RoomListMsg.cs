using System;
using System.Collections.Generic;
using System.Text;
namespace GameServer
{
	public class RoomListMsg : BaseMsg
	{
		public List<RoomData> roomList;
		public override int GetBytesNum()
		{
			int num = 8;
			num += 2;
			for (int i = 0; i < roomList.Count; ++i)
				num += roomList[i].GetBytesNum();
			return num;
		}
		public override byte[] Writing()
		{
			int index = 0;
			byte[] bytes = new byte[GetBytesNum()];
			WriteInt(bytes, GetID(), ref index);
			WriteInt(bytes, bytes.Length - 8, ref index);
			WriteShort(bytes, (short)roomList.Count, ref index);
			for (int i = 0; i < roomList.Count; ++i)
				WriteData(bytes, roomList[i], ref index);
			return bytes;
		}
		public override int Reading(byte[] bytes, int beginIndex = 0)
		{
			int index = beginIndex;
			roomList = new List<RoomData>();
			short roomListCount = ReadShort(bytes, ref index);
			for (int i = 0; i < roomListCount; ++i)
				roomList.Add(ReadData<RoomData>(bytes, ref index));
			return index - beginIndex;
		}
		public override int GetID()
		{
			return 3004;
		}
	}
}