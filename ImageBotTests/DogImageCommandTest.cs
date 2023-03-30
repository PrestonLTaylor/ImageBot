using Moq;

namespace ImageBotTests
{
    internal class DogImageCommandTest
    {
        [Test]
        public async Task RespondAsync_ShouldReturnImageFromAPI()
        {
            Mock<ImageAPI> imageAPIMock = new();
            DogImageCommand dogImageCommand = new(imageAPIMock.Object);

            const string expectedResponse = "MockedResponse";
            imageAPIMock.Setup(x => x.GetRandomImageURLAsync())
                .ReturnsAsync(expectedResponse);

            Assert.That(await dogImageCommand.TryToRespondAsync(), Is.EqualTo(expectedResponse));
        }

        [Test]
        public async Task RespondAsync_ShouldReturnErrorResponse_WhenAPIFails()
        {
            Mock<ImageAPI> imageAPIMock = new();
            DogImageCommand dogImageCommand = new(imageAPIMock.Object);

            const string expectedResponse = "MockedError";
            imageAPIMock.Setup(x => x.GetRandomImageURLAsync())
                .ThrowsAsync(new ImageAPIException(expectedResponse));

            Assert.That(await dogImageCommand.TryToRespondAsync(), Is.EqualTo(expectedResponse));
        }
    }
}
