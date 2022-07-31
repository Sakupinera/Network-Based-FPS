using System;
using System.Collections.Generic;
using System.Text;
namespace GamePlayer
{
	public class PlayerPos : BaseData
	{
		public int id;
		public float posX;
		public float posY;
		public float posZ;
		public float rotX;
		public float rotY;
		public float rotZ;
		public float cmRotX;
		public float cmRotY;
		public float cmRotZ;
		public override int GetBytesNum()
		{
			int num = 0;
			num += 4;
			num += 4;
			num += 4;
			num += 4;
			num += 4;
			num += 4;
			num += 4;
			num += 4;
			num += 4;
			num += 4;
			return num;
		}
		public override byte[] Writing()
		{
			int index = 0;
			byte[] bytes = new byte[GetBytesNum()];
			WriteInt(bytes, id, ref index);
			WriteFloat(bytes, posX, ref index);
			WriteFloat(bytes, posY, ref index);
			WriteFloat(bytes, posZ, ref index);
			WriteFloat(bytes, rotX, ref index);
			WriteFloat(bytes, rotY, ref index);
			WriteFloat(bytes, rotZ, ref index);
			WriteFloat(bytes, cmRotX, ref index);
			WriteFloat(bytes, cmRotY, ref index);
			WriteFloat(bytes, cmRotZ, ref index);
			return bytes;
		}
		public override int Reading(byte[] bytes, int beginIndex = 0)
		{
			int index = beginIndex;
			id = ReadInt(bytes, ref index);
			posX = ReadFloat(bytes, ref index);
			posY = ReadFloat(bytes, ref index);
			posZ = ReadFloat(bytes, ref index);
			rotX = ReadFloat(bytes, ref index);
			rotY = ReadFloat(bytes, ref index);
			rotZ = ReadFloat(bytes, ref index);
			cmRotX = ReadFloat(bytes, ref index);
			cmRotY = ReadFloat(bytes, ref index);
			cmRotZ = ReadFloat(bytes, ref index);
			return index - beginIndex;
		}
	}
}