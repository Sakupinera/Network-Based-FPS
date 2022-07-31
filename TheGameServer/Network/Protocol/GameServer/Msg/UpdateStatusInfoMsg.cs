using GamePlayer;
using System;
using System.Collections.Generic;
using System.Text;
namespace GameServer
{
    public class UpdateStatusInfoMsg : BaseMsg
    {
        public List<PlayerStatus> statusInfoList;
        public override int GetBytesNum()
        {
            int num = 8;
            num += 2;
            for (int i = 0; i < statusInfoList.Count; ++i)
                num += statusInfoList[i].GetBytesNum();
            return num;
        }
        public override byte[] Writing()
        {
            int index = 0;
            byte[] bytes = new byte[GetBytesNum()];
            WriteInt(bytes, GetID(), ref index);
            WriteInt(bytes, bytes.Length - 8, ref index);
            WriteShort(bytes, (short)statusInfoList.Count, ref index);
            for (int i = 0; i < statusInfoList.Count; ++i)
                WriteData(bytes, statusInfoList[i], ref index);
            return bytes;
        }
        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            int index = beginIndex;
            statusInfoList = new List<PlayerStatus>();
            short statusInfoListCount = ReadShort(bytes, ref index);
            for (int i = 0; i < statusInfoListCount; ++i)
                statusInfoList.Add(ReadData<PlayerStatus>(bytes, ref index));
            return index - beginIndex;
        }
        public override int GetID()
        {
            return 3001;
        }
    }
}