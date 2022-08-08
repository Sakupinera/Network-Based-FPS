//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2022-08-07 20:58:51.133
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
    /// 枪械表。
    /// </summary>
    public class DRGun : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取枪械编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取攻击力。
        /// </summary>
        public int Attack
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取开火间隔。
        /// </summary>
        public float AttackInterval
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取弹夹容量。
        /// </summary>
        public int MagazineSize
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取换弹时间。
        /// </summary>
        public float ReloadTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取子弹编号。
        /// </summary>
        public int BulletId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取子弹速度。
        /// </summary>
        public float BulletSpeed
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取子弹总量。
        /// </summary>
        public int BulletMaxSize
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取开火声音编号。
        /// </summary>
        public int FireSoundId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取枪口火花特效编号。
        /// </summary>
        public int MuzzleSparkId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取弹孔特效编号。
        /// </summary>
        public int BulletHoleId
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
            Attack = int.Parse(columnStrings[index++]);
            AttackInterval = float.Parse(columnStrings[index++]);
            MagazineSize = int.Parse(columnStrings[index++]);
            ReloadTime = float.Parse(columnStrings[index++]);
            BulletId = int.Parse(columnStrings[index++]);
            BulletSpeed = float.Parse(columnStrings[index++]);
            BulletMaxSize = int.Parse(columnStrings[index++]);
            FireSoundId = int.Parse(columnStrings[index++]);
            MuzzleSparkId = int.Parse(columnStrings[index++]);
            BulletHoleId = int.Parse(columnStrings[index++]);

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
                    Attack = binaryReader.Read7BitEncodedInt32();
                    AttackInterval = binaryReader.ReadSingle();
                    MagazineSize = binaryReader.Read7BitEncodedInt32();
                    ReloadTime = binaryReader.ReadSingle();
                    BulletId = binaryReader.Read7BitEncodedInt32();
                    BulletSpeed = binaryReader.ReadSingle();
                    BulletMaxSize = binaryReader.Read7BitEncodedInt32();
                    FireSoundId = binaryReader.Read7BitEncodedInt32();
                    MuzzleSparkId = binaryReader.Read7BitEncodedInt32();
                    BulletHoleId = binaryReader.Read7BitEncodedInt32();
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
