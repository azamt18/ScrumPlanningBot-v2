using System.Collections.Generic;
using System.Threading.Tasks;
using ScrumPlanningBot.Core.Entities;
using ScrumPlanningBot.Core.Services;

namespace ScrumPlanningBot.Application.Commands
{
    public class NewStoryCommand : IBotCommand
    {
        private readonly RoomService _roomService;

        public string Command => "newstory";

        public string Description => "Insert a new story to room";

        public bool InternalCommand => false;

        public NewStoryCommand(RoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task Execute(IChatService chatService, long chatId, long userId, int messageId,
            string? commandText)
        {
            var responseText = "Send a RoomId and title of the story. Example /newstory 619a56486f342d25ad248bf7 FA-123";
            Dictionary<string, string> buttons = null;
            if (!string.IsNullOrEmpty(commandText))
            {
                var subs = commandText?.Split(' ');
                var roomId = subs[0];
                var storyTitle = subs[1];

                // find the room by id
                var room = _roomService.Get(roomId);
                if (room != null)
                {
                    // insert a new story to room
                    var story = new Story()
                    {
                        CreatorUserId = chatId,
                        Title = storyTitle,
                        Users = new List<StoryUser>()
                    };

                    room.Stories.Add(story);
                    _roomService.Update(roomId, room);

                    responseText = $"Story title: {story.Title}. Give an estimated story point(sp).";
                    buttons = new Dictionary<string, string>()
                    {
                        // vote sp roomId storyId
                        {"1sp", $"setsp 1 {roomId} {story.Id}"},
                        {"2sp", $"setsp 2 {roomId} {story.Id}"},
                        {"3sp", $"setsp 3 {roomId} {story.Id}"},
                        {"5sp", $"setsp 5 {roomId} {story.Id}"},
                        {"8sp", $"setsp 8 {roomId} {story.Id}"},
                        {"13sp", $"setsp 13 {roomId} {story.Id}"},
                        {"21sp", $"setsp 21 {roomId} {story.Id}"},
                        {"34sp", $"setsp 34 {roomId} {story.Id}"}
                    };

                    foreach (var roomUser in room.Users)
                    {
                        await chatService.SendMessage(roomUser.TelegramId, responseText, buttons);
                    }
                }
                else
                {
                    responseText = $"Room was not found. RoomId: {commandText}";
                    await chatService.SendMessage(chatId, responseText, buttons);
                }
            }
            else
            {
                await chatService.SendMessage(chatId, responseText, buttons);
            }
        }
    }
}