using System;
using System.Threading.Tasks;

namespace ScrumPlanningBot.Application.Commands
{
    public class HelpCommand : IBotCommand
    {
        private readonly IServiceProvider _serviceProvider;

        public string Command => "help";

        public string Description => "Get information about the functionality available for this bot";

        public bool InternalCommand => false;

        public HelpCommand(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IChatService chatService, long chatId, long userId, int messageId,
            string? commandText)
        {
            var responseText = "Hi, this bot is designed for Agile poker planning";
            responseText += "\nSend: ";
            responseText += "\n◾️ /newroom <code>{room title}</code> - to create a new room. You'll receive a link with <code>{room id}</code> to invite your teammates";
            responseText += "\n◾️ /jointoroom <code>{room id}</code> - to join the room, or you can join via link";
            responseText += "\n◾️ /newstory <code>{room id}</code> <code>{story title}</code> - to create a new story in the room";
            responseText += "\n◾️ /publish - to publish results after the story has been estimated";
            responseText += "\n◾️ /ping - to test if the bot is online";

            await chatService.SendMessage(chatId, responseText);
        }
    }
}