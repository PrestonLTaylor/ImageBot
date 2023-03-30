using Discord.Net;

namespace ImageBotTests
{
    // TODO: Refactor all tests into cleaner tests
    internal class DiscordBotTest
    {
        [Test]
        public async Task WhenLoggingInWithAShortInvalidToken_ThrowsArgumentException()
        {
            // TODO: Replace Task.Delay with something that FakeDiscordLogger can interrupt
            FakeDiscordLogger fakeLogger = new();
            DiscordBot bot = new(fakeLogger);
            await bot.LoginWithTokenAsync("INVALID-SHORT-TOKEN");
            await bot.StartAsync();

            await Task.Delay(5000);

            Assert.That(fakeLogger.BotHasThrown<ArgumentException>(), Is.True);
        }

        [Test]
        public async Task WhenLoggingInWithAnInvalidToken_ThrowsHttpException()
        {
            FakeDiscordLogger fakeLogger = new();
            DiscordBot bot = new(fakeLogger);

            await bot.LoginWithTokenAsync("INVALID-TOKEN-INVALID-TOKEN-INVALID-TOKEN-INVALID-TOKEN-INVALID-TOKEN");
            await bot.StartAsync();

            await Task.Delay(5000);

            Assert.That(fakeLogger.BotHasThrown<HttpException>(), Is.True);
        }

        // TODO: This test is a reminder that the temporal coupiling should be refactored to make this impossible
        [Test]
        public async Task WhenStartingWithoutLoggingIn_ThrowsInvalidOperationException()
        {
            FakeDiscordLogger fakeLogger = new();
            DiscordBot bot = new(fakeLogger);

            await bot.StartAsync();

            await Task.Delay(5000);

            Assert.That(fakeLogger.BotHasThrown<InvalidOperationException>(), Is.True);
        }
    }
}
