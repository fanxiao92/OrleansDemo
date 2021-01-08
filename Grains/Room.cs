using System.Threading.Tasks;
using GrainInterfaces;
using GrainInterfaces.Model;

namespace Grains
{
    public class Room : Orleans.Grain, IRoom
    {
        private RoomDetailInfo roomInfo;
        
        public Task<RoomDetailInfo> RoomEnter()
        {
            throw new System.NotImplementedException();
        }

        public Task RoomLeave()
        {
            throw new System.NotImplementedException();
        }
    }
}