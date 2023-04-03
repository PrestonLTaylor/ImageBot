using ImageBot.Commands;

namespace ImageBot
{
    internal class Program
    {
        public static Task Main(string[] _) => new Program().MainAsync();

        public async Task MainAsync()
        {
            var token = await TryToReadTokenFileAsync();

            var bot = await DiscordBot.CreateBotWithToken(token);

            bot.AddImageCommand(new DogImageCommand());

            await bot.StartAsync();

            await Task.Delay(-1);
        }

        private async Task<string> TryToReadTokenFileAsync()
        {
            const string tokenFilePath = "token.txt";
            try
            {
                return await File.ReadAllTextAsync(tokenFilePath);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"""Did you forget to create a file called "{tokenFilePath}" with only the bot's token inside?""");
                Console.WriteLine(ex.Message);
                Environment.Exit(-1);
                throw ex;
            }
        }
    }
}