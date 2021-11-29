using System.Threading.Tasks;
using ScrumPlanningBot.Core.Services;

namespace ScrumPlanningBot.Application.Commands
{
    public class JoinToRoomCommand : IBotCommand
    {
        private readonly RoomService _roomService;

        private readonly UserService _userService;
        public string Command => "jointoroom";

        public string Description => "Join to a room";

        public bool InternalCommand => false;

        public JoinToRoomCommand(RoomService roomService, UserService userService)
        {
            _roomService = roomService;
            _userService = userService;
        }

        public async Task Execute(IChatService chatService, long chatId, long userId, int messageId,
            string? commandText)
        {
            var responseText = "Send a RoomId";
            if (!string.IsNullOrEmpty(commandText))
            {
                // find the room by id
                var room = _roomService.Get(commandText);
                if (room != null)
                {
                    // add user to the room
                    var user = _userService.GetByTelegramId(userId);
                    if (user != null)
                    {
                        room.Users.Add(user);
                        _roomService.Update(room.Id, room);
                    }

                    responseText = $"You are successfully joined. RoomId: {room.Id}";
                }
                else
                    responseText = $"Room was not found. RoomId: {commandText}";
            }

            await chatService.SendMessage(chatId, responseText);
        }
    }
}