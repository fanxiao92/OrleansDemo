using System;

namespace Common
{
    public static class Constants
    {
        public const string ChatRoomStreamProvider = "ChatRoom";
        public const string CharRoomStreamNameSpace = "YOLO";
        public const string ClusterId = "chatroom-deployment1";
        public const string ServiceId = "ChatRoomApp";
        
        /// <summary>
        /// 每页房间的数量
        /// </summary>
        public const int RoomSizePrePage = 10;
        
        /// <summary>
        /// 房间的槽位最大数量
        /// </summary>
        public const int RoomSlotMaxSize = 8;
    }
}