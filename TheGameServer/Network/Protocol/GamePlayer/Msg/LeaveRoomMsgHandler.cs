using GameServer;

namespace GamePlayer
{
    public class LeaveRoomMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            LeaveRoomMsg msg = message as LeaveRoomMsg;
            RoomMsgLogic.MsgLeaveRoom(ServerSocket.GetInstance().GetClient(msg.id).player);
        }
    }
}
