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
            DiscordBot bot = new(fakeLogger);

            await LoginWithAnInvalidShortToken(bot);

            AssertThatExceptionIsThrownByBotOfType<ArgumentException>(fakeLogger);
        }

        private async Task LoginWithAnInvalidShortToken(DiscordBot bot)
        {
            await bot.LoginWithTokenAsync("INVALID-TOKEN");
            await bot.StartAsync();
        }

        [Test, Timeout(5000)]
        public async Task LoginWithTokenAsync_ThrowsHttpException_WhenLoggingInWithAnInvalidToken()
        {
            FakeDiscordLogger fakeLogger = new();
            DiscordBot bot = new(fakeLogger);

            await LoginWithAnInvalidToken(bot);

            AssertThatExceptionIsThrownByBotOfType<HttpException>(fakeLogger);
        }

        private async Task LoginWithAnInvalidToken(DiscordBot bot)
        {
            await bot.LoginWithTokenAsync("INVALID-TOKEN-INVALID-TOKEN-INVALID-TOKEN-INVALID-TOKEN-INVALID-TOKEN");
            await bot.StartAsync();
        }

        // TODO: This test is a reminder that the temporal coupiling should be refactored to make this impossible
        [Test, Timeout(5000)]
        public async Task StartAsync_ThrowsInvalidOperationException_WhenNotLoggedIn()
        {
            FakeDiscordLogger fakeLogger = new();
            DiscordBot bot = new(fakeLogger);

            await bot.StartAsync();

            AssertThatExceptionIsThrownByBotOfType<InvalidOperationException>(fakeLogger);
        }

        [Test]
        public void AddImageCommand_ThrowsArgumentNullException_WhenNullImageCommandIsPassed()
        {
            DiscordBot bot = new();

            Assert.That(() => bot.AddImageCommand(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void AddImageCommand_DoesntThrow_WhenTheSameCommandIsAddedMoreThanOnce()
        {
            DiscordBot bot = new();
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
