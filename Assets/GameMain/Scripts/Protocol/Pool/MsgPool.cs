using System;
using System.Collections.Generic;
using GameSystem;
using GamePlayer;
using GameServer;
public class MsgPool
{
	private Dictionary<int, Type> messsages = new Dictionary<int, Type>();
	private Dictionary<int, Type> handlers = new Dictionary<int, Type>();
	public MsgPool()
	{
		Register(0000, typeof(ConnIDMsg), typeof(ConnIDMsgHandler));
		Register(1001, typeof(PlayerInfoMsg), typeof(PlayerInfoMsgHandler));
		Register(2000, typeof(MoveMsg), typeof(MoveMsgHandler));
		Register(2001, typeof(StatusMsg), typeof(StatusMsgHandler));
		Register(2002, typeof(ShootMsg), typeof(ShootMsgHandler));
		Register(2003, typeof(DamageMsg), typeof(DamageMsgHandler));
		Register(2004, typeof(WeaponMsg), typeof(WeaponMsgHandler));
		Register(2010, typeof(GetRoomListMsg), typeof(GetRoomListMsgHandler));
		Register(2011, typeof(CteateRoomMsg), typeof(CteateRoomMsgHandler));
		Register(2012, typeof(EnterRoomMsg), typeof(EnterRoomMsgHandler));
		Register(2013, typeof(GetRoomInfoMsg), typeof(GetRoomInfoMsgHandler));
		Register(2014, typeof(LeaveRoomMsg), typeof(LeaveRoomMsgHandler));
		Register(2015, typeof(ChangeCampMsg), typeof(ChangeCampMsgHandler));
		Register(2016, typeof(RequestFightMsg), typeof(RequestFightMsgHandler));
		Register(2017, typeof(LoadedSceneMsg), typeof(LoadedSceneMsgHandler));
		Register(1002, typeof(HeartMsg), typeof(HeartMsgHandler));
		Register(1003, typeof(QuitMsg), typeof(QuitMsgHandler));
		Register(3000, typeof(GetListMsg), typeof(GetListMsgHandler));
		Register(3001, typeof(UpdatePosInfoMsg), typeof(UpdatePosInfoMsgHandler));
		Register(3002, typeof(UpdateStatusInfoMsg), typeof(UpdateStatusInfoMsgHandler));
		Register(3003, typeof(RoomInfoMsg), typeof(RoomInfoMsgHandler));
		Register(3004, typeof(RoomListMsg), typeof(RoomListMsgHandler));
		Register(3005, typeof(ResponseFightMsg), typeof(ResponseFightMsgHandler));
		Register(3006, typeof(EndFightMsg), typeof(EndFightMsgHandler));
	}
	private void Register(int id, Type messageType, Type handlerType)
	{
		messsages.Add(id, messageType);
		handlers.Add(id, handlerType);
	}
	public BaseMsg GetMessage(int id)
	{
		if (!messsages.ContainsKey(id))
			return null;
		return Activator.CreateInstance(messsages[id]) as BaseMsg;
	}
	public BaseHandler GetHandler(int id)
	{
		if (!handlers.ContainsKey(id))
			return null;
		return Activator.CreateInstance(handlers[id]) as BaseHandler;
	}
}
