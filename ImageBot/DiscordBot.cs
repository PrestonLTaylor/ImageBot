using Discord.WebSocket;
using Discord;
using ImageBot.Loggers;

namespace ImageBot
{
    public class DiscordBot
    {
        public DiscordBot(DiscordLogger logger)
        {
            client.Log += logger.Log;
        }

        public async Task LoginWithTokenAsync(string token)
        {
            await client.LoginAsync(TokenType.Bot, token);
        }

        public async Task StartAsync()
        {
            await client.StartAsync();
        }

        private readonly DiscordSocketClient client = new();
    }
}
