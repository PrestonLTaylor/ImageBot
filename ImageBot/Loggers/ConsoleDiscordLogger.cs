using Discord;

namespace ImageBot.Loggers
{
    public class ConsoleDiscordLogger : DiscordLogger
    {
        public Task Log(LogMessage message) 
        {
            Console.WriteLine(message);
            return Task.CompletedTask; 
        }
    }
}
