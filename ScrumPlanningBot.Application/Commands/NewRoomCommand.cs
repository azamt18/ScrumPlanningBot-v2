using System.Collections.Generic;
using System.Threading.Tasks;
using ScrumPlanningBot.Core.Entities;
using ScrumPlanningBot.Core.Services;

namespace ScrumPlanningBot.Application.Commands
{
    public class NewRoomCommand : IBotCommand
    {
        private readonly RoomService _roomService;
        private readonly UserService _userService;
        public string Command => "newroom";

        public string Description => "Create a new room";

        public bool InternalCommand => false;

        public NewRoomCommand(RoomService roomService, UserService userService)
        {
            _roomService = roomService;
            _userService = userService;
        }

        public async Task Execute(IChatService chatService, long chatId, long userId, int messageId,
            string? commandText)
        {
            // create new room
            var responseText = "Send a title for an instant room";
            if (!string.IsNullOrEmpty(commandText))
            {
                var room = new Room()
                {
                    Title = commandText,
                    Users = new List<User>(),
                    Stories = new List<Story>()
                };
                var creator = _userService.GetByTelegramId(userId);
                if (creator != null)
                    room.Users.Add(creator);

                _roomService.Create(room);
                responseText = "⬆️ Room successfully created." +
                               $"\n<b>RoomId: {room.Id}</b>." +
                               $"Share the link to your teammates to join the room <a href='https://t.me/ScrumPlanning_Bot?start=join_{room.Id}'>https://t.me/ScrumPlanning_Bot?start=join_{room.Id}</a>";
            }

            await chatService.SendMessage(chatId, responseText);
        }
    }
}