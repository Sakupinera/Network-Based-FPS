using GamePlayer;
using System;
using System.Collections.Generic;
using System.Text;
namespace GameServer
{
    public class UpdatePosInfoMsg : BaseMsg
    {
        public List<PlayerPos> posInfoList;
        public override int GetBytesNum()
        {
            int num = 8;
            num += 2;
            for (int i = 0; i < posInfoList.Count; ++i)
                num += posInfoList[i].GetBytesNum();
            return num;
        }
        public override byte[] Writing()
        {
            int index = 0;
            byte[] bytes = new byte[GetBytesNum()];
            WriteInt(bytes, GetID(), ref index);
            WriteInt(bytes, bytes.Length - 8, ref index);
            WriteShort(bytes, (short)posInfoList.Count, ref index);
            for (int i = 0; i < posInfoList.Count; ++i)
                WriteData(bytes, posInfoList[i], ref index);
            return bytes;
        }
        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            int index = beginIndex;
            posInfoList = new List<PlayerPos>();
            short posInfoListCount = ReadShort(bytes, ref index);
            for (int i = 0; i < posInfoListCount; ++i)
                posInfoList.Add(ReadData<PlayerPos>(bytes, ref index));
            return index - beginIndex;
        }
        public override int GetID()
        {
            return 3001;
        }
    }
}