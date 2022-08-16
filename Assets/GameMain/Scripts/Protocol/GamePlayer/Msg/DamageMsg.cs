using System;
using System.Collections.Generic;
using System.Text;
namespace GamePlayer
{
	public class DamageMsg : BaseMsg
	{
		public int id;
		public int damage;

		//	受击者ID
		public int injured;
		//	是否被击杀
		public bool isKilled;
		public override int GetBytesNum()
		{
			int num = 8;
			num += 4;
			num += 4;
			num += 4;
			num += 1;
			return num;
		}
		public override byte[] Writing()
		{
			int index = 0;
			byte[] bytes = new byte[GetBytesNum()];
			WriteInt(bytes, GetID(), ref index);
			WriteInt(bytes, bytes.Length - 8, ref index);
			WriteInt(bytes, id, ref index);
			WriteInt(bytes, damage, ref index);
			WriteInt(bytes, injured, ref index);
			WriteBool(bytes, isKilled, ref index);
			return bytes;
		}
		public override int Reading(byte[] bytes, int beginIndex = 0)
		{
			int index = beginIndex;
			id = ReadInt(bytes, ref index);
			damage = ReadInt(bytes, ref index);
			injured = ReadInt(bytes, ref index);
			isKilled = ReadBool(bytes, ref index);
			return index - beginIndex;
		}
		public override int GetID()
		{
			return 2003;
		}
	}
}