using GamePlayer;
using GameServer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public GameObject prefab;

    //字典维护在场游戏玩家
    Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();

    int playerID;

    public float lastMoveTime;

    public static Battle instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    public void Update()
    {
        Move();
    }

    //添加玩家
    private void AddPlayer(int id, Vector3 pos/*, int score*/)
    {
        if (players.ContainsKey(id))
            return;
        GameObject player = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
        TextMesh textMesh = player.GetComponentInChildren<TextMesh>();
        textMesh.text = id.ToString() /*+ ":" + score*/;
        players.Add(id, player);
    }

    //删除玩家
    private void DelPalyer(int id)
    {
        if (players.ContainsKey(id))
        {
            Destroy(players[id]);
            players.Remove(id);
        }
    }


    public void SetPosList(UpdatePosInfoMsg msg)
    {
        foreach (PlayerPos pp in msg.posInfoList)
        {
            UpdateInfo(pp.id, new Vector3(pp.posX, pp.posY, pp.posZ));
        }
    }

    //更新信息
    public void UpdateInfo(int id, Vector3 pos)
    {
        //若是自己则直接返回
        if (id == playerID)
            return;
        if (players.ContainsKey(id))
        {
            players[id].transform.position = pos;
        }
        else
        {
            AddPlayer(id, pos);
        }
    }

    //发送位置
    public void SendPos()
    {
        GameObject player = players[playerID];
        Vector3 pos = player.transform.position;
        //消息
        MoveMsg moveMsg = new MoveMsg();
        moveMsg.playerPos = new PlayerPos();
        moveMsg.playerPos.id = playerID;
        moveMsg.playerPos.posX = pos.x;
        moveMsg.playerPos.posY = pos.y;
        moveMsg.playerPos.posZ = pos.z;
        moveMsg.playerPos.rotX = pos.x;
        moveMsg.playerPos.rotY = pos.y;
        moveMsg.playerPos.rotZ = pos.z;
        moveMsg.playerPos.cmRotX = pos.z;
        moveMsg.playerPos.cmRotY = pos.z;
        moveMsg.playerPos.cmRotZ = pos.z;
        NetMgr.Instance.Send(moveMsg);
    }

    //更新列表
    public void GetList(UpdatePosInfoMsg updatePosInfoMsg)
    {
        //遍历
        foreach (PlayerPos pp in updatePosInfoMsg.posInfoList)
        {
            UpdateInfo(pp.id, new Vector3(pp.posX, pp.posY, pp.posZ));
        }
    }

    void Move()
    {
        if (playerID == 0)
            return;
        if (players[playerID] == null)
            return;
        if (Time.time - lastMoveTime < 0.1)
            return;
        lastMoveTime = Time.time;


        GameObject player = players[playerID];
        //上
        if (Input.GetKey(KeyCode.UpArrow))
        {
            player.transform.position += new Vector3(0, 0, 1);
            SendPos();
        }
        //下
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            player.transform.position += new Vector3(0, 0, -1); ;
            SendPos();
        }
        //左
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            player.transform.position += new Vector3(-1, 0, 0);
            SendPos();
        }
        //右
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            player.transform.position += new Vector3(1, 0, 0);
            SendPos();
        }
    }

    public void StartGame(int id)
    {
        playerID = id;
        //产生自己
        float x = UnityEngine.Random.Range(-20, 20);
        float y = 0;
        float z = UnityEngine.Random.Range(-10, 10);
        Vector3 pos = new Vector3(x, y, z);
        AddPlayer(playerID, pos);
        //同步
        SendPos();
        //获取列表
    }

    //更新分数
    //public void UpdateScore(int id, int score)
    //{
    //    GameObject player = players[id];
    //    if (player != null)
    //    {
    //        TextMesh textMesh = player.GetComponentInChildren<TextMesh>();
    //        textMesh.text = id + ":" + score;
    //    }
    //}

    //public void UpdateInfo(int id, Vector3 pos, int score)
    //{
    //    if (id == playerID)
    //    {
    //        UpdateScore(playerID, score);
    //        return;
    //    }
    //    if (players.ContainsKey(id))
    //    {
    //        players[id].transform.position = pos;
    //        UpdateScore(id, score);
    //    }
    //    else
    //    {
    //        AddPlayer(id, pos, score);
    //    }
    //}


}
