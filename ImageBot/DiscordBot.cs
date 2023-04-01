﻿using Discord.WebSocket;
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
            List<ApplicationCommandProperties> builtCommands = new();
            foreach (var imageCommand in imageCommands)
            {
                var slashCommandBuilder = new SlashCommandBuilder()
                    .WithName(imageCommand.Value.GetName())
                    .WithDescription(imageCommand.Value.GetDescription())
                    .AddOptions(imageCommand.Value.GetCommandOptions());

                builtCommands.Add(slashCommandBuilder.Build());
            }

            try
            {
                await client.BulkOverwriteGlobalApplicationCommandsAsync(builtCommands.ToArray());
            }
            catch (HttpException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task OnSlashCommandExecuted(SocketSlashCommand command)
        {
            var commandParameters = GetCommandParametersFromOptions(command.Data.Options);
            
            if (imageCommands.TryGetValue(command.CommandName, out ImageCommand? commandToExecute))
            {
                var response = await commandToExecute.TryToRespondAsync(commandParameters);
                await command.RespondAsync(response);
            }
            else
            {
                await command.RespondAsync("Command was not found.", ephemeral: true);
            }
        }

        private Dictionary<string, object> GetCommandParametersFromOptions(IReadOnlyCollection<SocketSlashCommandDataOption> options)
        {
            Dictionary<string, object> commandParameters = new();
            foreach (var option in options)
            {
                commandParameters.Add(option.Name, option.Value);
            }

            return commandParameters;
        }

        private readonly DiscordSocketClient client = new();
        private readonly Dictionary<string, ImageCommand> imageCommands = new();
    }
}
