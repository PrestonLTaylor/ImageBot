using Discord.Net;
using Moq;

namespace ImageBotTests
{
    internal class DiscordBotTest
    {
        [Test, Timeout(5000)]
        public async Task LoginWithTokenAsync_ThrowsArgumentException_WhenLoggingInWithAnInvalidShortToken()
        {
            FakeDiscordLogger fakeLogger = new();

            await LoginWithAnInvalidShortToken(fakeLogger);

            AssertThatExceptionIsThrownByBotOfType<ArgumentException>(fakeLogger);
        }

        private async Task LoginWithAnInvalidShortToken(DiscordLogger logger)
        {
            var bot = await DiscordBot.CreateBotWithTokenAndLogger("INVALID-TOKEN", logger);
            await bot.StartAsync();
        }

        [Test, Timeout(5000)]
        public async Task LoginWithTokenAsync_ThrowsHttpException_WhenLoggingInWithAnInvalidToken()
        {
            FakeDiscordLogger fakeLogger = new();

            await LoginWithAnInvalidToken(fakeLogger);

            AssertThatExceptionIsThrownByBotOfType<HttpException>(fakeLogger);
        }

        private async Task LoginWithAnInvalidToken(DiscordLogger logger)
        {
            var bot = await DiscordBot.CreateBotWithTokenAndLogger("INVALID-TOKEN-INVALID-TOKEN-INVALID-TOKEN-INVALID-TOKEN-INVALID-TOKEN", logger);
            await bot.StartAsync();
        }

        [Test]
        public async Task AddImageCommand_ThrowsArgumentNullException_WhenNullImageCommandIsPassed()
        {
            var bot = await DiscordBot.CreateBotWithToken("");

            Assert.That(() => bot.AddImageCommand(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task AddImageCommand_DoesntThrow_WhenTheSameCommandIsAddedMoreThanOnce()
        {
            var bot = await DiscordBot.CreateBotWithToken("");

            var mockedCommand = SetupMockedCommand();

            Assert.That(() => AddCommandToBotTwice(bot, mockedCommand.Object), Throws.Nothing);
        }

        private Mock<ImageCommand> SetupMockedCommand()
        {
            Mock<ImageCommand> mockedCommand = new();
            mockedCommand.Setup(x => x.GetName()).Returns("MockedCommand");
            return mockedCommand;
        }

        private void AddCommandToBotTwice(DiscordBot bot, ImageCommand command)
        {
            bot.AddImageCommand(command);
            bot.AddImageCommand(command);
        }

        private void AssertThatExceptionIsThrownByBotOfType<T>(FakeDiscordLogger fakeLogger) where T : Exception
        {
            while (!fakeLogger.BotHasThrown<T>()) { }
            Assert.Pass();
        }
    }
}
