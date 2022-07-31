using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GamePlayer;
using UnityEngine.SceneManagement;

public class StartSceneUI : MonoBehaviour
{
    public TMP_InputField IPinput;
    public TMP_InputField Portinput;
    public TMP_InputField Nameinput;

    public Button ConnBtn;
    public Button LoginBtn;

    // Start is called before the first frame update
    void Start()
    {
        ConnBtn.onClick.AddListener(() =>
        {
            if (NetMgr.Instance == null)
            {
                GameObject obj = new GameObject("NetAsync");
                obj.AddComponent<NetMgr>();
            }
            int port = 8080;
            if (int.TryParse(Portinput.text, out port))
            {
                NetMgr.Instance.Connect(IPinput.text, port);
            }

            //PlayerInfoMsg playerInfoMsg = new PlayerInfoMsg();
            //playerInfoMsg.playerID = NetMgr.Instance.ID;
            //playerInfoMsg.data = new PlayerData();
            //playerInfoMsg.data.name = "Yuanxzzz";
            //playerInfoMsg.data.id = NetMgr.Instance.ID;
            //playerInfoMsg.data.playerCamp = E_PLAYER_CAMP.A;
            //NetMgr.Instance.Send(playerInfoMsg);
        });

        LoginBtn.onClick.AddListener(() =>
        {
            PlayerInfoMsg playerInfoMsg = new PlayerInfoMsg();
            playerInfoMsg.playerID = NetMgr.Instance.ID;
            playerInfoMsg.data = new PlayerData();
            playerInfoMsg.data.name = "Yuanxzzz";
            playerInfoMsg.data.id = NetMgr.Instance.ID;
            playerInfoMsg.data.playerCamp = E_PLAYER_CAMP.A;
            NetMgr.Instance.Send(playerInfoMsg);
            SceneManager.LoadSceneAsync("RoomScene");
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
