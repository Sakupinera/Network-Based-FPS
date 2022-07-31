using GameSystem;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class ServerSocket
    {
        //单例
        private static ServerSocket instance;

        public static ServerSocket GetInstance()
        {
            if (instance == null)
                instance = new ServerSocket();
            return instance;
        }

        //服务器socket
        private Socket socket;
        //存放连接的客户端socket的字典 k=编号 v=对应socket
        private ConcurrentDictionary<int, ClientSocket> clientDic = new ConcurrentDictionary<int, ClientSocket>();

        //消息池对象 用于快速获取消息和消息处理类对象
        public MsgPool msgPool = new MsgPool();


        public void Start(string ip, int port, int num)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            try
            {
                socket.Bind(ipPoint);
                socket.Listen(num);
                //通过异步接收客户端连入
                socket.BeginAccept(AcceptCallBack, null);
            }
            catch (Exception e)
            {
                Console.WriteLine("启动服务器失败" + e.Message);
            }
        }

        private void AcceptCallBack(IAsyncResult result)
        {
            try
            {
                //获取连入的客户端
                Socket clientSocket = socket.EndAccept(result);
                //ClientSocket client = new ClientSocket(clientSocket);
                ClientSocket client = ConnClient();
                //client.socket = clientSocket;
                //连接并初始化客户端对象
                client.Init(clientSocket);
                Console.WriteLine("客户端连接成功，ID：" + client.clientID + ",地址：" + client.GetAddress());
                //记录客户端对象
                //clientDic.TryAdd(client.clientID, client);

                //单播回去该客户端的ID
                ConnIDMsg connID = new ConnIDMsg();
                connID.id = client.clientID;
                Unicast(client.clientID, connID);

                //继续让别的客户端连接
                socket.BeginAccept(AcceptCallBack, null);
            }
            catch (Exception e)
            {
                Console.WriteLine("客户端连入失败" + e.Message);
            }
        }

        //广播
        public void Broadcast(BaseMsg msg)
        {
            foreach (ClientSocket client in clientDic.Values)
            {
                client.Send(msg);
            }
        }

        //单播
        public void Unicast(int id, BaseMsg msg)
        {
            clientDic[id].Send(msg);
        }

        //关闭客户端连接
        public void CloseClientSocket(int clientID)
        {
            lock (clientDic)
            {
                if (clientDic.ContainsKey(clientID))
                {
                    clientDic[clientID].Close();
                }
            }
        }

        //获取指定ID 的Client
        public ClientSocket GetClient(int id)
        {
            if (clientDic.ContainsKey(id))
                return clientDic[id];
            return null;
        }

        //为连接获取新的索引
        //若有以断开连接的客户端则复用
        //没有则新建索引并添加
        public ClientSocket ConnClient()
        {
            //lock (clientDic)
            //{
            foreach (ClientSocket client in clientDic.Values)
            {
                if (client.isUse == false)
                    return client;
            }
            ClientSocket newClient = new ClientSocket();
            clientDic.TryAdd(newClient.clientID, newClient);
            return newClient;
            //}
        }

        //展示当前服务器连接信息
        public void Show()
        {
            Console.WriteLine("*********服务器连接信息如下*********");
            foreach (ClientSocket client in clientDic.Values)
            {
                if (client.isUse == false)
                    continue;

                string str = "地址：" + client.GetAddress();
                if (client.player != null)
                {
                    str += "玩家ID：" + client.player.playerData.id + " 玩家昵称：" + client.player.playerData.name;
                }
                Console.WriteLine(str);
            }
        }
    }
}
