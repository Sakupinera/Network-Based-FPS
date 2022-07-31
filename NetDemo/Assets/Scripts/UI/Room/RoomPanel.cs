using GamePlayer;
using GameServer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomPanel : BasePanel
{
    public GameObject content;
    public GameObject playerInfoPrefab;

    private E_PLAYER_CAMP nowCamp;
    private bool isOner = false;

    // Start is called before the first frame update
    void Start()
    {
        GetControl<Button>("Button_Leave").onClick.AddListener(LeaveRoom);
        GetControl<Button>("Button_Camp").onClick.AddListener(ChangeCamp);
        GetControl<Button>("Button_Ready").onClick.AddListener(StartGame);
        EventCenter.GetInstance().AddEventListener<RoomInfoMsg>("RefreshPlayerList", RefreshPlayerList);

        RequestRoomInfo();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void RequestRoomInfo()
    {
        GetRoomInfoMsg msg = new GetRoomInfoMsg();
        msg.id = NetMgr.Instance.ID;
        NetMgr.Instance.Send(msg);
    }

    //刷新列表
    public void RefreshPlayerList(RoomInfoMsg msg)
    {
        print("刷新玩家列表");
        content.BroadcastMessage("DestroyMySelf");

        string camp = "";
        //是否为房主
        if (msg.Oner == msg.Oner)
            isOner = true;
        else
            isOner = false;
        for (int i = 0; i < msg.roomPlayersList.Count; i++)
        {

            //玩家信息
            GameObject pip = Instantiate(playerInfoPrefab, content.transform);
            pip.GetComponent<PlayerInfoPrefab>().SetText(msg.roomPlayersList[i].name, isOner);
            //显示阵营
            if (msg.roomPlayersList[i].playerCamp == E_PLAYER_CAMP.A)
            {
                if (msg.roomPlayersList[i].id == NetMgr.Instance.ID)
                {
                    camp = "A";
                    nowCamp = E_PLAYER_CAMP.A;
                }
                pip.GetComponent<Image>().color = Color.red;
            }
            else if (msg.roomPlayersList[i].playerCamp == E_PLAYER_CAMP.B)
            {
                if (msg.roomPlayersList[i].id == NetMgr.Instance.ID)
                {
                    camp = "B";
                    nowCamp = E_PLAYER_CAMP.B;
                }
                pip.GetComponent<Image>().color = Color.blue;
            }
            else
            {
                if (msg.roomPlayersList[i].id == NetMgr.Instance.ID)
                {
                    camp = "Other";
                    nowCamp = E_PLAYER_CAMP.Other;
                }
                pip.GetComponent<Image>().color = Color.white;
            }
        }
        GetControl<Button>("Button_Camp").GetComponentInChildren<TMP_Text>().text = camp;
    }

    //离开房间
    public void LeaveRoom()
    {
        LeaveRoomMsg msg = new LeaveRoomMsg();
        msg.id = NetMgr.Instance.ID;
        NetMgr.Instance.Send(msg);
        UIManager.GetInstance().HidePanel("RoomPanel");
    }

    //切换阵营
    public void ChangeCamp()
    {
        print("当前阵营" + nowCamp.ToString());
        ChangeCampMsg msg = new ChangeCampMsg();
        msg.id = NetMgr.Instance.ID;
        if (nowCamp == E_PLAYER_CAMP.A)
            msg.CAMP = E_PLAYER_CAMP.B;
        if (nowCamp == E_PLAYER_CAMP.B)
            msg.CAMP = E_PLAYER_CAMP.A;
        NetMgr.Instance.Send(msg);
    }

    //开始游戏
    public void StartGame()
    {
        //不是房主则显示弹窗 请等待房主开始游戏
        if (!isOner)
        {
            //显示弹窗
            print("不是房主 不能开启战斗");
            return;
        }
        print("请求开始战斗");
        //发送开始战斗协议
        RequestFightMsg msg = new RequestFightMsg();
        msg.id = NetMgr.Instance.ID;
        NetMgr.Instance.Send(msg);
    }

    public override void HideMe()
    {
        EventCenter.GetInstance().RemoveEventListener<RoomInfoMsg>("RefreshPlayerList", RefreshPlayerList);
    }
}
