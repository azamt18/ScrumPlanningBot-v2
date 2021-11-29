using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ScrumPlanningBot.Core.Entities;
using ScrumPlanningBot.Core.Services;

namespace ScrumPlanningBot.Application.Commands
{
    public class SetSPForStoryCommand : IBotCommand
    {
        private readonly RoomService _roomService;

        private readonly UserService _userService;

        public string Command => "setsp";

        public string Description => "Vote for a story";

        public bool InternalCommand => false;

        public SetSPForStoryCommand(RoomService roomService, UserService userService)
        {
            _roomService = roomService;
            _userService = userService;
        }
        public async Task Execute(IChatService chatService, long chatId, long userId, int messageId, string? commandText)
        {
            var responseText = "Invalid command. Send (/newstory) to create new story";
            if (!string.IsNullOrEmpty(commandText))
            {
                // setsp sp roomId storyId
                var subs = commandText.Split(' ');
                var storyPointValue = subs[1];
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
                            var storyUser = new StoryUser()
                            {
                                UserId = userId,
                                StoryPoint = Convert.ToDouble(storyPointValue),
                                Hour = 0
                            };

                            story.Users.Add(storyUser);
                            story.UpdatedAt = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                            _roomService.Update(room.Id, room);

                            responseText = $"Your vote is successfully saved. {story?.Title}: {storyPointValue}sp";
                            var buttons = new Dictionary<string, string>()
                            {
                                // vote h roomId storyId
                                {"1h", $"sethour 1 {roomId} {story.Id}"},
                                {"2h", $"sethour 2 {roomId} {story.Id}"},
                                {"3h", $"sethour 3 {roomId} {story.Id}"},
                                {"4h", $"sethour 4 {roomId} {story.Id}"},
                                {"5h", $"sethour 5 {roomId} {story.Id}"},
                                {"6h", $"sethour 6 {roomId} {story.Id}"},
                                {"7h", $"sethour 7 {roomId} {story.Id}"},
                                {"8h", $"sethour 8 {roomId} {story.Id}"}
                            };

                            await chatService.SendMessage(chatId, responseText, buttons);
                        }
                        else
                            responseText = $"No user found with such id: {userId}";
                    }
                    else
                        responseText = $"No story found with such id: {storyId}.";
                }
                else
                    responseText = $"Room was not found. RoomId: {commandText}";
            }
            else
            {
                await chatService.SendMessage(chatId, responseText, null);
            }
        }
    }
}