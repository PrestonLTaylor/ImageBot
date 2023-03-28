using Discord;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using ImageBot.API;

namespace ImageBot
{
    internal class Program
    {
        public static Task Main(string[] _) => new Program().MainAsync();

        public async Task MainAsync()
        {
            client.Log += Log;
            client.Ready += CreateCommands;
            client.SlashCommandExecuted += HandleSlashCommand;

            var token = File.ReadAllText("token.txt");

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task CreateCommands()
        {
            // TODO: Create if command changes instead of always
            var dogImageCommand = new SlashCommandBuilder()
                .WithName("dog")
                .WithDescription("Get a random image of a dog!");

            try
            {
                await client.CreateGlobalApplicationCommandAsync(dogImageCommand.Build());
            } 
            catch (HttpException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task HandleSlashCommand(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "dog":
                    await command.RespondAsync(await dogImageAPI.GetRandomImageURLAsync());
                    break;
            }
        }

        private readonly DogImageAPI dogImageAPI = new();
        private readonly DiscordSocketClient client = new();
    }
}