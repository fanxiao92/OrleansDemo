using System;
using System.Threading.Tasks;
using GrainInterfaces.Model;
using Orleans;

namespace GrainInterfaces
{
    public interface IRoomMgr : IGrainWithIntegerKey
    {
        /// <summary>
        /// 房间列表
        /// </summary>
        /// <returns></returns>
        Task<RoomBriefInfo[]> RoomList();
        
        Task<RoomDetailInfo> RoomCreate();

        Task<RoomDetailInfo> RoomEnter(long roomId);
    }
}