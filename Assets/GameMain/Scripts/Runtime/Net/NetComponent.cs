using GameSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace NetworkBasedFPS
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Net")]
    public class NetComponent : GameFrameworkComponent
    {

        //客户端ID
        public int ID;

        //客户端用于通信的Socket
        private Socket socket;

        //缓存
        private byte[] cacheBytes = new byte[1024 * 1024];
        private int cacheNum = 0;

        //接收消息队列
        private Queue<BaseHandler> receiveQueue = new Queue<BaseHandler>();

        //设置心跳消息
        private int SEND_HEART_MSG_TIME = 2;
        private HeartMsg heartMsg = new HeartMsg();

        //消息池
        private MsgPool msgPool = new MsgPool();

        protected override void Awake()
        {
            base.Awake();
            Application.runInBackground = true;
            InvokeRepeating("SendHeartMsg", 0, SEND_HEART_MSG_TIME);
        }

        private void SendHeartMsg()
        {
            if (socket != null && socket.Connected)
            {
                //print("发送心跳消息");
                Send(heartMsg);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (receiveQueue.Count > 0)
            {
                //若有消息则出队列并执行对应MsgHandler

                receiveQueue.Dequeue().MsgHandle();
            }
        }

        //连接服务器
        public void Connect(string ip, int port)
        {
            if (socket != null && socket.Connected)
                return;

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.RemoteEndPoint = ipPoint;
            args.Completed += (socket, args) =>
            {
                if (args.SocketError == SocketError.Success)
                {
                    //GameEntry.Event.Fire(this, ConnEventArgs.Create("连接成功"));
                    print("连接成功");

                    SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
                    receiveArgs.SetBuffer(cacheBytes, 0, cacheBytes.Length);
                    receiveArgs.Completed += ReceiveCallBack;
                    this.socket.ReceiveAsync(receiveArgs);
                }
                else
                {
                    //GameEntry.Event.FireNow(this, ConnEventArgs.Create("连接出错"));
                    print("连接出错" + args.SocketError);
                }
            };
            socket.ConnectAsync(args);
        }

        //接收的回调
        private void ReceiveCallBack(object obj, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                HandleReceiveMsg(args.BytesTransferred);
                //读入缓存
                args.SetBuffer(cacheNum, args.Buffer.Length - cacheNum);
                //继续接收
                if (this.socket != null && this.socket.Connected)
                    socket.ReceiveAsync(args);
                else
                    Close();
            }
            else
            {
                print("接受消息出错" + args.SocketError);
                //关闭连接
                Close();
            }
        }

        public void Close(bool isSelf = false)
        {
            if (socket != null)
            {
                QuitMsg msg = new QuitMsg();
                socket.Send(msg.Writing());
                socket.Shutdown(SocketShutdown.Both);
                socket.Disconnect(false);
                socket.Close();
                socket = null;
            }

            //非主动关闭
            if (!isSelf)
            {
                //执行非主动关闭逻辑
            }
        }

        public void SendTest(byte[] bytes)
        {
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.SetBuffer(bytes, 0, bytes.Length);
            args.Completed += (socket, args) =>
            {
                if (args.SocketError != SocketError.Success)
                {
                    print("发送消息出错" + args.SocketError);
                    Close();
                }

            };
            this.socket.SendAsync(args);
        }

        public void Send(BaseMsg msg)
        {
            if (this.socket != null && this.socket.Connected)
            {
                byte[] bytes = msg.Writing();
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.SetBuffer(bytes, 0, bytes.Length);
                args.Completed += (socket, args) =>
                {
                    if (args.SocketError != SocketError.Success)
                    {
                        print("发送消息出错" + args.SocketError);
                        Close();
                    }

                };
                this.socket.SendAsync(args);
            }
            else
            {
                Close();
            }
        }

        //处理分包黏包
        private void HandleReceiveMsg(int receiveNum)
        {
            int msgID = 0;
            int msgLength = 0;
            int nowIndex = 0;

            cacheNum += receiveNum;

            while (true)
            {
                //每次将长度设置为-1 是避免上一次解析的数据 影响这一次的判断
                msgLength = -1;
                //处理解析一条消息
                if (cacheNum - nowIndex >= 8)
                {
                    //解析ID
                    msgID = BitConverter.ToInt32(cacheBytes, nowIndex);
                    nowIndex += 4;
                    //解析长度
                    msgLength = BitConverter.ToInt32(cacheBytes, nowIndex);
                    nowIndex += 4;
                }

                if (cacheNum - nowIndex >= msgLength && msgLength != -1)
                {
                    //查看是否有对应消息
                    BaseMsg baseMsg = msgPool.GetMessage(msgID);
                    if (baseMsg != null)
                    {
                        //读取消息
                        baseMsg.Reading(cacheBytes, nowIndex);
                        //获取消息处理器
                        BaseHandler baseHandler = msgPool.GetHandler(msgID);
                        baseHandler.message = baseMsg;
                        //放入消息队列
                        receiveQueue.Enqueue(baseHandler);
                    }

                    nowIndex += msgLength;
                    if (nowIndex == cacheNum)
                    {
                        cacheNum = 0;
                        break;
                    }
                }
                else
                {
                    //如果不满足 证明有分包 
                    //那么我们需要把当前收到的内容 记录下来
                    //有待下次接受到消息后 再做处理
                    //receiveBytes.CopyTo(cacheBytes, 0);
                    //cacheNum = receiveNum;
                    //如果进行了 id和长度的解析 但是 没有成功解析消息体 那么我们需要减去nowIndex移动的位置
                    if (msgLength != -1)
                        nowIndex -= 8;
                    //没解析完的消息放入缓存
                    Array.Copy(cacheBytes, nowIndex, cacheBytes, 0, cacheNum - nowIndex);
                    cacheNum = cacheNum - nowIndex;
                    break;
                }
            }

        }

        private void OnDestroy()
        {
            Close(true);
        }

    }
}
