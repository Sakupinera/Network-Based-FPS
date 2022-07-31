using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GamePlayer;

public class RoomPrefab : BasePanel
{
    int roomNo;
    // Start is called before the first frame update
    void Start()
    {
        GetControl<Button>("Button_Enter").onClick.AddListener(EnterRoom);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetText(int roomNo, int num, string status)
    {
        print("设置房间属性");
        GetControl<TMP_Text>("Text_RoomNo").text = "RoomNo:" + roomNo.ToString();
        GetControl<TMP_Text>("Text_Num").text = "Num:" + num.ToString();
        GetControl<TMP_Text>("Text_Status").text = "Status:" + status;
        this.roomNo = roomNo;
    }

    public void EnterRoom()
    {
        print("加入房间，序号为：" + GetControl<TMP_Text>("Text_RoomNo").text);

        EnterRoomMsg msg = new EnterRoomMsg();
        msg.id = NetMgr.Instance.ID;
        msg.index = roomNo;
        NetMgr.Instance.Send(msg);

        UIManager.GetInstance().ShowPanel<RoomPanel>("RoomPanel");
    }



    public void DestroyMySelf()
    {
        Destroy(gameObject);
    }

}
