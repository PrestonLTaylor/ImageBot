using Discord;

namespace ImageBot.Loggers
{
    public class FakeDiscordLogger : DiscordLogger
    {
        public Task Log(LogMessage message)
        {
            if (message.Exception is not null)
            {
                exceptionsThatHaveBeenThrown.Add(message.Exception.GetType());
            }

            return Task.CompletedTask;
        }

        public bool BotHasThrown<T>() where T : Exception
        {
            return exceptionsThatHaveBeenThrown.Contains(typeof(T));
        }

        private readonly HashSet<Type> exceptionsThatHaveBeenThrown = new();
    }
}
