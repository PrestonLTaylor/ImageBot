using RichardSzalay.MockHttp;
using System.Net.Http.Json;

namespace ImageBotTests
{
    internal class DogImageAPITest
    {
        [SetUp]
        public void TestSetup()
        {
            mockHttpMessageHandler = new();
        }

        // Refactor into cleaner tests
        [Test]
        public async Task Can_GetRandomImageURL()
        {
            DogImageAPI dogImageAPI = new();

            const string dogAPIImageDomainName = "images.dog.ceo";
            var imageURL = await dogImageAPI.GetRandomImageURLAsync();
            Assert.That(imageURL, Does.Contain(dogAPIImageDomainName));
        }

        [Test]
        public async Task SuccessfulRandomImageResponse_ReturnsMessageField()
        {
            DogImageAPI dogImageAPI = new(mockHttpMessageHandler);

            const string randomDogImageAPIURL = "https://dog.ceo/api/breeds/image/random";
            const string successfulResponse = "MockedURL";
            mockHttpMessageHandler.When(randomDogImageAPIURL)
                .Respond(JsonContent.Create(new
                {
                    message = successfulResponse,
                    status = "success",
                }));

            var imageURL = await dogImageAPI.GetRandomImageURLAsync();
            Assert.That(imageURL, Does.Contain(successfulResponse));
        }

        [Test]
        public void FailureToGetRandomImage_ThrowsImageAPIException()
        {
            DogImageAPI dogImageAPI = new(mockHttpMessageHandler);

            const string randomDogImageAPIURL = "https://dog.ceo/api/breeds/image/random";
            mockHttpMessageHandler.When(randomDogImageAPIURL)
                .Respond(JsonContent.Create(new
                {
                    message = "Mocked Failure",
                    status = "error",
                }));

            Assert.That(dogImageAPI.GetRandomImageURLAsync, Throws.TypeOf<ImageAPIException>());
        }

        private MockHttpMessageHandler mockHttpMessageHandler;
    }
}
