using GamePlayer;
using GameSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Button quitBtn;
    public Button connBtn;
    public Button loginBtn;

    // Start is called before the first frame update
    void Start()
    {
        connBtn.onClick.AddListener(() =>
        {
            if (NetMgr.Instance == null)
            {
                GameObject obj = new GameObject("NetAsync");
                obj.AddComponent<NetMgr>();
            }
            NetMgr.Instance.Connect("127.0.0.1", 8080);
            print("a:" + NetMgr.Instance.ID);
        });



        quitBtn.onClick.AddListener(() =>
        {
            print("b:" + NetMgr.Instance.ID);
            QuitMsg quitMsg = new QuitMsg();
            quitMsg.id = NetMgr.Instance.ID;
            print(quitMsg.id);
            NetMgr.Instance.Send(quitMsg);
        });

        loginBtn.onClick.AddListener(() =>
        {
            PlayerInfoMsg playerInfoMsg = new PlayerInfoMsg();
            playerInfoMsg.playerID = NetMgr.Instance.ID;
            playerInfoMsg.data = new PlayerData();
            playerInfoMsg.data.name = "Yuanxzzz";
            playerInfoMsg.data.id = NetMgr.Instance.ID;
            playerInfoMsg.data.playerCamp = E_PLAYER_CAMP.A;
            NetMgr.Instance.Send(playerInfoMsg);
            Battle.instance.StartGame(NetMgr.Instance.ID);
        });


    }

    // Update is called once per frame
    void Update()
    {
    }
}
