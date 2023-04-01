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
            SetupGetRandomImageURLToReturn(imageAPIMock, expectedResponse);

            Assert.That(await dogImageCommand.TryToRespondAsync(new Dictionary<string, object>()), Is.EqualTo(expectedResponse));
        }

        [Test]
        public async Task RespondAsync_ShouldUseTagImageAPI_WhenSuppliedABreed()
        {
            Mock<ImageAPI> imageAPIMock = new();
            DogImageCommand dogImageCommand = new(imageAPIMock.Object);

            const string expectedResponse = "MockedResponse";
            const string fakeTag = "FakeTag";
            SetupGetRandomImageURLWithTagToReturn(imageAPIMock, fakeTag, expectedResponse);

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
            const string fakeTag = "FakeTag";
            ImageAPIException exceptionToThrow = new(expectedResponse);
            SetupGetRandomImageURLToThrow(imageAPIMock, exceptionToThrow);
            SetupGetRandomImageURLWithTagToThrow(imageAPIMock, fakeTag, exceptionToThrow);

            Assert.Multiple(async () =>
            {
                Assert.That(await dogImageCommand.TryToRespondAsync(new Dictionary<string, object>()), Is.EqualTo(expectedResponse));
                Assert.That(await dogImageCommand.TryToRespondAsync(new Dictionary<string, object>()
                {
                    { "breed", fakeTag }
                }), Is.EqualTo(expectedResponse));
            });
        }

        private void SetupGetRandomImageURLToReturn(Mock<ImageAPI> mock, string response)
        {
            mock.Setup(x => x.GetRandomImageURLAsync())
                .ReturnsAsync(response);
        }

        private void SetupGetRandomImageURLWithTagToReturn(Mock<ImageAPI> mock, string tag, string response)
        {
            mock.Setup(x => x.GetRandomImageURLWithTagAsync(tag))
                .ReturnsAsync(response);
        }

        private void SetupGetRandomImageURLToThrow(Mock<ImageAPI> mock, ImageAPIException exception)
        {
            mock.Setup(x => x.GetRandomImageURLAsync())
                .ThrowsAsync(exception);
        }

        private void SetupGetRandomImageURLWithTagToThrow(Mock<ImageAPI> mock, string tag, ImageAPIException exception)
        {
            mock.Setup(x => x.GetRandomImageURLWithTagAsync(tag))
                .ThrowsAsync(exception);
        }
    }
}
