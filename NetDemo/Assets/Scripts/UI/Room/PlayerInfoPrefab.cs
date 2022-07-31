using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerInfoPrefab : BasePanel
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //设置玩家信息
    public void SetText(string name, bool isOwner)
    {
        if (isOwner)
            name += "(Oner)";
        GetControl<TMP_Text>("Text_PlayerName").text = name;
    }

    public void DestroyMySelf()
    {
        Destroy(gameObject);
    }
}
