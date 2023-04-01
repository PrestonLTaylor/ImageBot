using Discord.WebSocket;

namespace ImageBot.Commands
{

    public interface ImageCommand
    {
        public string GetName();
        public string GetDescription();

        public Task<string> TryToRespondAsync(IReadOnlyDictionary<string, object> parameters);
    }
}
