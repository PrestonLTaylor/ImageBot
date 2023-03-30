using Discord.Net;
using Moq;

namespace ImageBotTests
{
    // TODO: Refactor all tests into cleaner tests
    internal class DiscordBotTest
    {
        [Test, Timeout(5000)]
        public async Task WhenLoggingInWithAShortInvalidToken_ThrowsArgumentException()
        {
            // TODO: Replace Task.Delay with something that FakeDiscordLogger can interrupt
            FakeDiscordLogger fakeLogger = new();
            DiscordBot bot = new(fakeLogger);
            await bot.LoginWithTokenAsync("INVALID-SHORT-TOKEN");
            await bot.StartAsync();

            while (!fakeLogger.BotHasThrown<ArgumentException>()) {}
            Assert.Pass();
        }

        [Test, Timeout(5000)]
        public async Task WhenLoggingInWithAnInvalidToken_ThrowsHttpException()
        {
            FakeDiscordLogger fakeLogger = new();
            DiscordBot bot = new(fakeLogger);

            await bot.LoginWithTokenAsync("INVALID-TOKEN-INVALID-TOKEN-INVALID-TOKEN-INVALID-TOKEN-INVALID-TOKEN");
            await bot.StartAsync();

            while (!fakeLogger.BotHasThrown<HttpException>()) {}
            Assert.Pass();
        }

        // TODO: This test is a reminder that the temporal coupiling should be refactored to make this impossible
        [Test, Timeout(5000)]
        public async Task WhenStartingWithoutLoggingIn_ThrowsInvalidOperationException()
        {
            FakeDiscordLogger fakeLogger = new();
            DiscordBot bot = new(fakeLogger);

            await bot.StartAsync();

            while (!fakeLogger.BotHasThrown<InvalidOperationException>()) { }
            Assert.Pass();
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

            Mock<ImageCommand> mockedCommand = new();
            mockedCommand.Setup(x => x.GetName()).Returns("MockedCommand");

            Assert.That(() => bot.AddImageCommand(mockedCommand.Object), Throws.Nothing);
            Assert.That(() => bot.AddImageCommand(mockedCommand.Object), Throws.Nothing);
        }
    }
}
