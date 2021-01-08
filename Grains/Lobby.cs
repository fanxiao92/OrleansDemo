using System;
using System.Threading.Tasks;
using GrainInterfaces;
using GrainInterfaces.Model;

namespace Grains
{
    public class Lobby : Orleans.Grain, ILobby
    {
        //TODO 增加auth处理
        //TODO Error处理
        /// <summary>
        /// 大厅频道 Id
        /// </summary>
        private int lobbyChannelId;
        
        /// <summary>
        /// 当前房间
        /// </summary>
        private IRoom currentRoom;
        
        public async Task<RoomBriefInfo[]> RoomList()
        {
            var roomMgr =  GrainFactory.GetGrain<IRoomMgr>(lobbyChannelId);
            return await roomMgr.RoomList();
        }

        public async Task<RoomDetailInfo> RoomEnter(int roomId)
        {
            if (currentRoom != null)
            {
                throw new ArgumentException();
            }
            
            var roomMgr =  GrainFactory.GetGrain<IRoomMgr>(lobbyChannelId);
            var roomInfo = await roomMgr.RoomEnter(roomId);
            currentRoom = GrainFactory.GetGrain<IRoom>(roomId);
            return roomInfo;
        }

        public async Task RoomLeave()
        {
            if (currentRoom == null)
            {
               return ; 
            }

            await currentRoom?.RoomLeave();
        }

        public async Task<RoomDetailInfo> RoomCreate()
        {
            if (currentRoom != null)
            {
                throw new ArgumentException();
            }
            
            var roomMgr =  GrainFactory.GetGrain<IRoomMgr>(lobbyChannelId);
            return await roomMgr.RoomCreate();
        }
    }
}