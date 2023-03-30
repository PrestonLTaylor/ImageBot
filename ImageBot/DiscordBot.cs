using Discord.WebSocket;
using Discord;
using ImageBot.Loggers;
using ImageBot.Commands;
using Discord.Net;

namespace ImageBot
{
    public class DiscordBot
    {
        public DiscordBot() : this(new ConsoleDiscordLogger()) {}

        public DiscordBot(DiscordLogger logger)
        {
            client.Log += logger.Log;
            client.Ready += OnReady;
            client.SlashCommandExecuted += OnSlashCommandExecuted;
        }

        public void AddImageCommand(ImageCommand commandToAdd)
        {
            if (commandToAdd is null)
            {
                throw new ArgumentNullException(nameof(commandToAdd));
            }

            imageCommands.TryAdd(commandToAdd.GetName(), commandToAdd);
        }

        public async Task LoginWithTokenAsync(string token)
        {
            await client.LoginAsync(TokenType.Bot, token);
        }

        public async Task StartAsync()
        {
            await client.StartAsync();
        }

        private async Task OnReady()
        {
            foreach (var imageCommand in imageCommands)
            {
                // TODO: Create if command changes instead of always using GetGlobalApplicationCommandsAsync
                var slashCommandBuilder = new SlashCommandBuilder()
                    .WithName(imageCommand.Value.GetName())
                    .WithDescription(imageCommand.Value.GetDescription());

                // TODO: Replace with Task.WaitAll instead of individually awaiting each command
                try
                {
                    await client.CreateGlobalApplicationCommandAsync(slashCommandBuilder.Build());
                }
                catch (HttpException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private async Task OnSlashCommandExecuted(SocketSlashCommand command)
        {
            
            if (imageCommands.TryGetValue(command.CommandName, out ImageCommand? commandToExecute))
            {
                var response = await commandToExecute.TryToRespondAsync();
                await command.RespondAsync(response);
            }
            else
            {
                await command.RespondAsync("Command was not found.", ephemeral: true);
            }
        }

        private readonly DiscordSocketClient client = new();
        private readonly Dictionary<string, ImageCommand> imageCommands = new();
    }
}
