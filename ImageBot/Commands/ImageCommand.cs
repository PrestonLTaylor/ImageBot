using Discord;

namespace ImageBot.Commands
{

    public interface ImageCommand
    {
        public string GetName();
        public string GetDescription();
        public SlashCommandOptionBuilder[] GetCommandOptions();

        public Task<string> TryToRespondAsync(IReadOnlyDictionary<string, object> parameters);
    }
}
