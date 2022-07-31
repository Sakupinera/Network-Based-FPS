using GamePlayer;
using GameServer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理战斗
/// </summary>
public class BattleMgr : MonoBehaviour
{
    //单例
    private static BattleMgr instance;
    public static BattleMgr Instance => instance;

    //玩家预制体
    public GameObject[] PlayerPrefab;

    //本机玩家ID
    public int id;

    void Start()
    {
        instance = this;
        id = NetMgr.Instance.ID;
    }

    void Update()
    {

    }

    //开始战斗
    public void StartFight(GetListMsg msg)
    {
        print("开始战斗");

    }

    /// <summary> 
    ///实例化玩家
    /// </summary>
    /// <param name="name">玩家姓名</param>
    /// <param name="id">绑定socket的id</param>
    /// <param name="camp">阵营</param>
    /// <param name="startPos">起始点</param>
    /// <param name="prefabID">所选模型</param>
    public void InstantiatePlayer(string name, int id, E_PLAYER_CAMP camp, int startPos, int prefabID)
    {

    }
}
