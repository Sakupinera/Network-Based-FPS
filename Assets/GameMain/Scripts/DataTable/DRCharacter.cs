//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2022-08-11 11:20:44.899
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace NetworkBasedFPS
{
    /// <summary>
    /// 角色表。
    /// </summary>
    public class DRCharacter : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取角色编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取最大生命值。
        /// </summary>
        public int MaxHP
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取移动速度。
        /// </summary>
        public float MoveSpeed
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取死亡特效编号。
        /// </summary>
        public int DeadEffectId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取死亡声音编号。
        /// </summary>
        public int DeadSoundId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取脚步声音编号。
        /// </summary>
        public int FootStepSoundId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取受击声音编号。
        /// </summary>
        public int HitSoundId
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            MaxHP = int.Parse(columnStrings[index++]);
            MoveSpeed = float.Parse(columnStrings[index++]);
            DeadEffectId = int.Parse(columnStrings[index++]);
            DeadSoundId = int.Parse(columnStrings[index++]);
            FootStepSoundId = int.Parse(columnStrings[index++]);
            HitSoundId = int.Parse(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    MaxHP = binaryReader.Read7BitEncodedInt32();
                    DeadEffectId = binaryReader.Read7BitEncodedInt32();
                    DeadSoundId = binaryReader.Read7BitEncodedInt32();
                    FootStepSoundId = binaryReader.Read7BitEncodedInt32();
                    HitSoundId = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
