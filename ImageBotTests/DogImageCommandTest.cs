using Moq;

namespace ImageBotTests
{
    internal class DogImageCommandTest
    {
        [Test]
        public async Task RespondAsync_ShouldReturnRandomImageFromAPI_WhenSuppliedNoParameters()
        {
            Mock<ImageAPI> imageAPIMock = new();
            DogImageCommand dogImageCommand = new(imageAPIMock.Object);

            const string expectedResponse = "MockedResponse";
            imageAPIMock.Setup(x => x.GetRandomImageURLAsync())
                .ReturnsAsync(expectedResponse);

            Assert.That(await dogImageCommand.TryToRespondAsync(new Dictionary<string, object>()), Is.EqualTo(expectedResponse));
        }

        [Test]
        public async Task RespondAsync_ShouldUseTagImageAPI_WhenSuppliedABreed()
        {
            Mock<ImageAPI> imageAPIMock = new();
            DogImageCommand dogImageCommand = new(imageAPIMock.Object);

            const string expectedResponse = "MockedResponse";
            const string fakeTag = "FakeTag";
            imageAPIMock.Setup(x => x.GetRandomImageURLWithTagAsync(fakeTag))
                .ReturnsAsync(expectedResponse);

            Assert.That(await dogImageCommand.TryToRespondAsync(new Dictionary<string, object>()
            {
                { "breed", fakeTag }
            }), Is.EqualTo(expectedResponse));
        }

        [Test]
        public void RespondAsync_ShouldReturnErrorResponse_WhenAPIFails()
        {
            Mock<ImageAPI> imageAPIMock = new();
            DogImageCommand dogImageCommand = new(imageAPIMock.Object);

            const string expectedResponse = "MockedError";
            imageAPIMock.Setup(x => x.GetRandomImageURLAsync())
                .ThrowsAsync(new ImageAPIException(expectedResponse));

            const string fakeTag = "FakeTag";
            imageAPIMock.Setup(x => x.GetRandomImageURLWithTagAsync(fakeTag))
                .ThrowsAsync(new ImageAPIException(expectedResponse));

             Assert.Multiple(async () =>
            {
                Assert.That(await dogImageCommand.TryToRespondAsync(new Dictionary<string, object>()), Is.EqualTo(expectedResponse));
                Assert.That(await dogImageCommand.TryToRespondAsync(new Dictionary<string, object>()
                {
                    { "breed", fakeTag }
                }), Is.EqualTo(expectedResponse));
            });
        }
    }
}
