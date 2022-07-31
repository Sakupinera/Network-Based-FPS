using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.GetInstance().ShowPanel<RoomListPanel>("RoomListPanel");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
