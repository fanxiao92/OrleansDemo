using System.Threading.Tasks;
using GrainInterfaces.Model;
using Orleans;

namespace GrainInterfaces
{
    public interface IRoom : IGrainWithIntegerKey
    {
        /// <summary>
        /// 进入房间
        /// </summary>
        /// <returns></returns>
        Task<RoomDetailInfo> RoomEnter();

        /// <summary>
        /// 离开房间
        /// </summary>
        /// <returns></returns>
        Task RoomLeave();
    }
}