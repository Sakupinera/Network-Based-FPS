using GamePlayer;
using System;
using System.Collections.Generic;
using System.Text;
namespace GameServer
{
    public class RoomInfoMsg : BaseMsg
    {
        public int Oner;
        public List<PlayerData> roomPlayersList;
        public override int GetBytesNum()
        {
            int num = 8;
            num += 4;
            num += 2;
            for (int i = 0; i < roomPlayersList.Count; ++i)
                num += roomPlayersList[i].GetBytesNum();
            return num;
        }
        public override byte[] Writing()
        {
            int index = 0;
            byte[] bytes = new byte[GetBytesNum()];
            WriteInt(bytes, GetID(), ref index);
            WriteInt(bytes, bytes.Length - 8, ref index);
            WriteInt(bytes, Oner, ref index);
            WriteShort(bytes, (short)roomPlayersList.Count, ref index);
            for (int i = 0; i < roomPlayersList.Count; ++i)
                WriteData(bytes, roomPlayersList[i], ref index);
            return bytes;
        }
        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            int index = beginIndex;
            Oner = ReadInt(bytes, ref index);
            roomPlayersList = new List<PlayerData>();
            short roomPlayersListCount = ReadShort(bytes, ref index);
            for (int i = 0; i < roomPlayersListCount; ++i)
                roomPlayersList.Add(ReadData<PlayerData>(bytes, ref index));
            return index - beginIndex;
        }
        public override int GetID()
        {
            return 3003;
        }
    }
}