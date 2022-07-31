using GamePlayer;
using GameServer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPanel : BasePanel
{
    public GameObject roomPrefab;
    public GameObject content;

    // Start is called before the first frame update
    void Start()
    {
        if (NetMgr.Instance == null)
        {
            GameObject obj = new GameObject("NetAsync");
            obj.AddComponent<NetMgr>();
        }
        NetMgr.Instance.Connect("127.0.0.1", 8080);
        GetControl<Button>("Button_Login").onClick.AddListener(() =>
        {
            print("注册");
            PlayerInfoMsg playerInfoMsg = new PlayerInfoMsg();
            playerInfoMsg.playerID = NetMgr.Instance.ID;
            playerInfoMsg.data = new PlayerData();
            playerInfoMsg.data.name = "Yuanxzzz";
            playerInfoMsg.data.id = NetMgr.Instance.ID;
            playerInfoMsg.data.playerCamp = E_PLAYER_CAMP.A;
            NetMgr.Instance.Send(playerInfoMsg);
        });
        //for (int i = 0; i < 5; i++)
        //{
        //    //roomPrefab.
        //    Instantiate(roomPrefab, GameObject.Find("Content").transform);
        //}
        GetControl<Button>("Button_Test").onClick.AddListener(() =>
        {
            print("请求房间列表");
            RequestRoomList();
        });

        GetControl<Button>("Button_Create").onClick.AddListener(() =>
        {
            print("创建房间");
            CreateRoom();
        });

        EventCenter.GetInstance().AddEventListener<RoomListMsg>("RefreshRoomList", RefreshRoomList);
    }

    // Update is called once per frame
    void Update()
    {
    }

    //请求房间列表
    private void RequestRoomList()
    {
        GetRoomListMsg msg = new GetRoomListMsg();
        msg.id = NetMgr.Instance.ID;
        NetMgr.Instance.Send(msg);
    }

    //刷新房间列表
    public void RefreshRoomList(RoomListMsg msg)
    {
        print("刷新房间列表");
        content.BroadcastMessage("DestroyMySelf");

        for (int i = 0; i < msg.roomList.Count; i++)
        {
            GameObject rp = Instantiate(roomPrefab, content.transform);
            string status;
            if (msg.roomList[i].roomStatus == E_ROOM_STATUS.PREPARE)
            {
                status = "PREPARE";
            }
            else
            {
                status = "FIGHT";
            }
            rp.GetComponent<RoomPrefab>().SetText(i + 1, msg.roomList[i].roomPlayersNum, status);
        }
    }

    //新建房间 
    public void CreateRoom()
    {
        CteateRoomMsg msg = new CteateRoomMsg();
        msg.id = NetMgr.Instance.ID;
        NetMgr.Instance.Send(msg);
        ShowRoomPanel();
    }

    public void ShowRoomPanel()
    {
        UIManager.GetInstance().ShowPanel<RoomPanel>("RoomPanel");
    }
}
