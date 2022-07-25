using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameFramework.DataTable;
using UnityEngine;

namespace NetworkBasedFPS
{
    public class DRHero : IDataRow
    {
        public int Id { get; protected set; }
        public string Name { get; private set; }
        public int Atk { get; private set; }

        public bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            Id = int.Parse(columnStrings[index++]);
            Name = columnStrings[index++];
            Atk = int.Parse(columnStrings[index++]);

            return true;
        }

        public bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    Id = binaryReader.Read7BitEncodedInt32();
                    Name = binaryReader.ReadString();
                    Atk = binaryReader.Read7BitEncodedInt32();
                }
            }

            return true;
        }
    }
}