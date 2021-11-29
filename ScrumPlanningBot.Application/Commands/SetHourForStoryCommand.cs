using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ScrumPlanningBot.Core.Services;

namespace ScrumPlanningBot.Application.Commands
{
    public class SetHourForStoryCommand : IBotCommand
    {
        private readonly RoomService _roomService;

        private readonly UserService _userService;

        public string Command => "sethour";

        public string Description => "Vote for a story";

        public bool InternalCommand => false;

        public SetHourForStoryCommand(RoomService roomService, UserService userService)
        {
            _roomService = roomService;
            _userService = userService;
        }
        public async Task Execute(IChatService chatService, long chatId, long userId, int messageId, string? commandText)
        {
            var responseText = "";
            Dictionary<string, string> buttons = null;
            if (!string.IsNullOrEmpty(commandText))
            {
                // vote h roomId storyId
                var subs = commandText.Split(' ');
                var hourValue = subs[1];
                var roomId = subs[2];
                var storyId = subs[3];

                // find the room by id
                var room = _roomService.Get(roomId);
                if (room != null)
                {
                    var story = room.Stories?.Where(s => s.Id == storyId).FirstOrDefault();
                    if (story != null)
                    {
                        var user = _userService.GetByTelegramId(userId);
                        if (user != null)
                        {
                            var storyUser = story.Users?.Where(su => su.UserId == userId).FirstOrDefault();
                            if (storyUser != null)
                            {
                                storyUser.Hour = Convert.ToDouble(hourValue);
                            }

                            story.UpdatedAt = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                            _roomService.Update(room.Id, room);

                            if (userId == story.CreatorUserId)
                            {
                                buttons = new Dictionary<string, string>()
                                {
                                    {"Publish results", $"publish {roomId} {storyId}"}
                                };
                            }
                        }
                        else
                            responseText = $"No user found with such id: {userId}";
                    }
                    else
                        responseText = $"No story found with such id: {storyId}.";

                    responseText = $"Your vote is successfully saved. {story?.Title}: {hourValue}h";
                }
                else
                    responseText = $"Room was not found. RoomId: {commandText}";
            }

            await chatService.SendMessage(chatId, responseText, buttons);
        }
    }
}