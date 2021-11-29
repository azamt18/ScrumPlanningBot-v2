using System.Threading.Tasks;
using ScrumPlanningBot.Core.Services;

namespace ScrumPlanningBot.Application.Commands
{
    public class StartCommand : IBotCommand
    {
        private readonly RoomService _roomService;

        private readonly UserService _userService;

        public string Command => "start";

        public string Description => "Join to existing room";

        public bool InternalCommand => false;

        public StartCommand(RoomService roomService, UserService userService)
        {
            _roomService = roomService;
            _userService = userService;
        }
        public async Task Execute(IChatService chatService, long chatId, long userId, int messageId, string? commandText)
        {
            var responseText = "Send a RoomId";
            if (!string.IsNullOrEmpty(commandText))
            {
                var subs = commandText.Split('_');
                var roomId = subs[1];

                // find the room by id
                var room = _roomService.Get(roomId);
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
            else
            {
                responseText = "Hi, this bot is designed for Agile poker planning";
                responseText += "\nSend: ";
                responseText += "\n◾️ /newroom - to create a new room. You'll receive a link with <code>{room id}</code> to invite your teammates";
                responseText += "\n◾️ /jointoroom <code>{room id}</code> - to join the room, or you can join via link";
                responseText += "\n◾️ /newstory <code>{room id}</code> <code>{story title}</code> - to create a new story in the room";
                responseText += "\n◾️ /publish - to publish results after the story has been estimated";
                responseText += "\n◾️ /ping - to test if the bot is online";
            }

            await chatService.SendMessage(chatId, responseText);
        }
    }
}