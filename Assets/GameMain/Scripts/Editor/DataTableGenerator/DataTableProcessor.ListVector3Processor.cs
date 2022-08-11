//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NetworkBasedFPS.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class ListVector3Processor : GenericDataProcessor<List<Vector3>>
        {
            public override bool IsSystem
            {
                get
                {
                    return false;
                }
            }

            public override string LanguageKeyword
            {
                get
                {
                    return "List<Vector3>";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "List<Vector3>",
                    "System.Collections.Generic.List<Vector3>"
                };
            }

            public override List<Vector3> Parse(string value)
            {
                List<Vector3> tmp = new List<Vector3>();
                if(value == "empty" || value == "")
                {
                    return tmp;
                }

                string[] splitedValues = value.Split(';');
                for(int i = 0; i < splitedValues.Length; i++)
                {
                    string[] splitedValue = splitedValues[i].Split(",");
                    tmp.Add(new Vector3(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2])));
                }
                return tmp;
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                List<Vector3> listVector3 = Parse(value);
                binaryWriter.Write((float)listVector3.Count);
                for(int i = 0; i < listVector3.Count; i++)
                {
                    binaryWriter.Write(listVector3[i].x);
                    binaryWriter.Write(listVector3[i].y);
                    binaryWriter.Write(listVector3[i].z);
                }
            }
        }
    }
}
