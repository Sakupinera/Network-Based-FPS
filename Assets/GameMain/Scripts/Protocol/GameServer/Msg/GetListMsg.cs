using GamePlayer;
using System;
using System.Collections.Generic;
using System.Text;
namespace GameServer
{
    public class GetListMsg : BaseMsg
    {
        public List<PlayerData> list;
        public override int GetBytesNum()
        {
            int num = 8;
            num += 2;
            for (int i = 0; i < list.Count; ++i)
                num += list[i].GetBytesNum();
            return num;
        }
        public override byte[] Writing()
        {
            int index = 0;
            byte[] bytes = new byte[GetBytesNum()];
            WriteInt(bytes, GetID(), ref index);
            WriteInt(bytes, bytes.Length - 8, ref index);
            WriteShort(bytes, (short)list.Count, ref index);
            for (int i = 0; i < list.Count; ++i)
                WriteData(bytes, list[i], ref index);
            return bytes;
        }
        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            int index = beginIndex;
            list = new List<PlayerData>();
            short listCount = ReadShort(bytes, ref index);
            for (int i = 0; i < listCount; ++i)
                list.Add(ReadData<PlayerData>(bytes, ref index));
            return index - beginIndex;
        }
        public override int GetID()
        {
            return 3000;
        }
    }
}