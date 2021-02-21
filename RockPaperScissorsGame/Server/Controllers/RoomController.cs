using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Ending;
using Server.RoomModel;
using Server.UserModel;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/v1/room")]
    public class RoomController : ControllerBase
    {
        private RoomCollect roomCollect;
        private EndingRoom endingRoom;
        public RoomController(RoomCollect roomCollect, EndingRoom endingRoom)
        {
            this.roomCollect = roomCollect;
            this.endingRoom = endingRoom;
        }

        [HttpGet]
        public async Task<string> NewRoom()
        {
            // todo check authorization
            var room = roomCollect.AddToRoom(new User()); // ToDo add user who send request

            while (!room.IsFull)
            {
                await Task.Delay(1000);
            }

            return room.Id;
        }
    }
}
