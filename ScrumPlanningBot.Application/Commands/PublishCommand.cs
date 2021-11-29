using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ScrumPlanningBot.Core.Services;

namespace ScrumPlanningBot.Application.Commands
{
    public class PublishCommand : IBotCommand
    {
        private readonly RoomService _roomService;

        private readonly UserService _userService;

        public string Command => "publish";

        public string Description => "Publish results";

        public bool InternalCommand => false;

        public PublishCommand(RoomService roomService, UserService userService)
        {
            _roomService = roomService;
            _userService = userService;
        }
        public async Task Execute(IChatService chatService, long chatId, long userId, int messageId, string? commandText)
        {
            string responseText = null;
            if (!string.IsNullOrEmpty(commandText))
            {
                var subs = commandText.Split(' ');
                var roomId = subs[1];
                var storyId = subs[2];

                // find the room by id
                var room = _roomService.Get(roomId);
                if (room != null)
                {
                    var story = room.Stories?.Where(s => s.Id == storyId).FirstOrDefault();
                    if (story != null)
                    {
                        story.AverageStoryPoint = story.Users.Select(x => x.StoryPoint).Sum() / story.Users.Count;
                        story.AverageHour = story.Users.Select(x => x.Hour).Sum() / story.Users.Count;
                        story.IsClosed = true;
                        story.ClosedAt = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                        story.UpdatedAt = DateTime.Now.ToString(CultureInfo.InvariantCulture);

                        responseText = "✅ Results:"
                                       + $"\nStory title: {story.Title}"
                                       + $"\nAverage story point: {story.AverageStoryPoint}"
                                       + $"\nAverage hour: {story.AverageHour}"
                                       + "\n";
                        foreach (var storyUser in story.Users)
                        {
                            var user = _userService.GetByTelegramId(storyUser.UserId);
                            responseText += $"\n◾️ <a href='tg://user?id={user?.TelegramId}'>{user?.UserName}</a> ({user?.FullName}): {storyUser.StoryPoint}sp / {storyUser.Hour}h";
                        }

                        responseText +=
                            $"\nVoting for the story {story.Title} was closed. To create a new story send /newstory <code>{{room id}}</code> <code>{{room title}}</code>";

                        // send publish result for every voted user in the story
                        foreach (var storyUser in story.Users)
                        {
                            var user = _userService.GetByTelegramId(storyUser.UserId);
                            await chatService.SendMessage(user.TelegramId, responseText);
                        }
                    }
                    else
                        responseText = $"No story found with such id: {storyId}.";
                }
                else
                    responseText = $"Room was not found. RoomId: {commandText}";
            }

            //await chatService.SendMessage(chatId, responseText);
        }
    }
}