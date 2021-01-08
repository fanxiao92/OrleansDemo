using System.Threading.Tasks;
using GrainInterfaces;
using GrainInterfaces.Model;

namespace Grains
{
    public class RoomMgr : Orleans.Grain, IRoomMgr
    {
        public Task<RoomBriefInfo[]> RoomList()
        {
            throw new System.NotImplementedException();
        }

        public Task<RoomDetailInfo> RoomCreate()
        {
            throw new System.NotImplementedException();
        }

        public Task<RoomDetailInfo> RoomEnter(long roomId)
        {
            throw new System.NotImplementedException();
        }
    }
}