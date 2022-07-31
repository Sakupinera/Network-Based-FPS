using System;
using System.Collections.Generic;
using System.Text;
namespace GameServer
{
    public class RoomData : BaseData
    {
        public int roomPlayersNum;
        public E_ROOM_STATUS roomStatus;
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
            WriteInt(bytes, roomPlayersNum, ref index);
            WriteInt(bytes, Convert.ToInt32(roomStatus), ref index);
            return bytes;
        }
        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            int index = beginIndex;
            roomPlayersNum = ReadInt(bytes, ref index);
            roomStatus = (E_ROOM_STATUS)ReadInt(bytes, ref index);
            return index - beginIndex;
        }
    }
}