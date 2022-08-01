using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GameFramework;
using UnityGameFramework.Runtime;
using GameFramework.Event;

namespace NetworkBasedFPS
{
    public class TestWebRequest : MonoBehaviour
    {
        public Image image = null;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
                GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);

                WebRequestComponent webRequest = GameEntry.WebRequest.GetComponent<WebRequestComponent>();
                string url = "https://gimg2.baidu.com/image_search/src=http%3A%2F%2Ft.ki4.cn%2F2020%2F1%2FRriMZv.jpg&refer=http%3A%2F%2Ft.ki4.cn&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=auto?sec=1661833065&t=235c14618d73cbbfd8c7348852a2f093";
                webRequest.AddWebRequest(url, this);
            }
        }

        void OnWebRequestSuccess(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs eventArgs = e as WebRequestSuccessEventArgs;
            Debug.Log("OnWebRequestSuccess");
            Texture2D texture = new Texture2D(1920, 1080);
            texture.LoadImage(eventArgs.GetWebResponseBytes());
            Sprite sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), Vector2.zero);
            image.sprite = sprite;
        }

        void OnWebRequestFailure(object sender, GameEventArgs e)
        {
            Debug.Log("OnWebRequestFailure"); 
        }
    }
}