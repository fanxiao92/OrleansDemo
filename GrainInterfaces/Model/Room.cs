using System;
using System.Collections.Generic;
using Common;

namespace GrainInterfaces.Model
{
    /// <summary>
    /// 房间简介信息
    /// </summary>
    [Serializable]
    public class RoomBriefInfo 
    {
        /// <summary>
        /// 房间号
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 房间名
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 可用槽位数量
        /// </summary>
        public int FreeSlotCount { get; set; }
        
        /// <summary>
        /// 总槽位数量
        /// </summary>
        public int TotalSlotCount { get; set; }
    }
    
    /// <summary>
    /// 房间信息
    /// </summary>
    [Serializable]
    public class RoomDetailInfo : RoomBriefInfo
    {
        /// <summary>
        /// 房间玩家信息
        /// </summary>
        public class RoomUserInfo
        {
            /// <summary>
            /// 玩家处在房间卡槽 index
            /// </summary>
            public int SlotIdx { get; set; }
            
            /// <summary>
            /// 玩家名字
            /// </summary>
            public string Name { get; set; }
            
            /// <summary>
            /// 玩家经验
            /// </summary>
            public int Exp { get; set; }
        }
        
        /// <summary>
        /// 玩家信息列表
        /// </summary>
        public RoomUserInfo[] UserInfos { get; set; } = new RoomUserInfo[Constants.RoomSlotMaxSize];
    }
}