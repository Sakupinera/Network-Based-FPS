using GamePlayer;
using GameServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerPlayer
{
    /// <summary>
    /// 使用Player来封装对客户端的连接和玩家信息
    /// </summary>
    public class Player
    {
        //玩家信息
        public PlayerData playerData;
        //玩家对应的socket连接
        public ClientSocket clientSocket;
        //玩家状态
        public PlayerContext playerContext = new PlayerContext();

        public Player() { }

        public Player(PlayerData playerData, ClientSocket clientSocket)
        {
            this.playerData = playerData;
            this.clientSocket = clientSocket;
        }

        //发送消息
        public void Send(BaseMsg baseMsg)
        {
            if (clientSocket == null)
            {
                return;
            }
            clientSocket.Send(baseMsg);
        }
    }
}
