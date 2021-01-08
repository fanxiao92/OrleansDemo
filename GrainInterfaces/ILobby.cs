using System.Threading.Tasks;
using GrainInterfaces.Model;

namespace GrainInterfaces
{
    public interface ILobby : Orleans.IGrainWithIntegerKey
    {
        /// <summary>
        /// 显示房间
        /// </summary>
        /// <returns></returns>
        Task<RoomBriefInfo[]> RoomList();

        /// <summary>
        /// 创建房间
        /// </summary>
        /// <returns></returns>
        Task<RoomDetailInfo> RoomCreate();

        /// <summary>
        /// 进入房间
        /// </summary>
        /// <returns></returns>
        Task<RoomDetailInfo> RoomEnter(int roomId);

        /// <summary>
        /// 离开房间
        /// </summary>
        /// <returns></returns>
        Task RoomLeave();
        
        //TODO KickOut
    }
}