using Discord;

namespace ImageBot.Loggers
{
    public interface DiscordLogger
    {
        public Task Log(LogMessage message);
    }
}
