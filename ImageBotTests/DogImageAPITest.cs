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

        [Test]
        public async Task GetRandomImageURLAsync_ReturnsAnImageFromTheAPI()
        {
            DogImageAPI dogImageAPI = new();

            var imageURL = await dogImageAPI.GetRandomImageURLAsync();

            Assert.That(imageURL, Does.Contain(dogAPIImageDomainName));
        }

        [Test]
        public async Task GetRandomImageURLWithTagAsync_ReturnsAnImageFromTheAPI()
        {
            DogImageAPI dogImageAPI = new();

            var imageURL = await dogImageAPI.GetRandomImageURLWithTagAsync("samoyed");

            Assert.That(imageURL, Does.Contain(dogAPIImageDomainName));
        }

        [Test]
        public void GetRandomImageURLWithTagAsync_ThrowsImageAPIException_WhenSuppliedInvalidDogBreed()
        {
            DogImageAPI dogImageAPI = new();
            
            Assert.That(async () => await dogImageAPI.GetRandomImageURLWithTagAsync("invalidbreed"), Throws.TypeOf<ImageAPIException>());
        }

        [Test]
        public void SuccessfulRandomImageResponse_ReturnsMessageField()
        {
            DogImageAPI dogImageAPI = new(mockHttpMessageHandler);

            const string successfulResponse = "MockedURL";
            SetupResponseForRandomDogImageAPI(JsonContent.Create(new
            {
                message = successfulResponse,
                status = "success",
            }));

            Assert.That(dogImageAPI.GetRandomImageURLAsync, Does.Contain(successfulResponse));
        }

        [Test]
        public void FailureToGetRandomImage_ThrowsImageAPIException()
        {
            DogImageAPI dogImageAPI = new(mockHttpMessageHandler);
            SetupResponseForRandomDogImageAPI(JsonContent.Create(new
            {
                message = "Mocked Failure",
                status = "error",
            }));

            Assert.That(dogImageAPI.GetRandomImageURLAsync, Throws.TypeOf<ImageAPIException>());
        }

        private void SetupResponseForRandomDogImageAPI(JsonContent jsonContent)
        {
            const string randomDogImageAPIURL = "https://dog.ceo/api/breeds/image/random";
            mockHttpMessageHandler.When(randomDogImageAPIURL).Respond(jsonContent);
        }

        private const string dogAPIImageDomainName = "images.dog.ceo";
        private MockHttpMessageHandler mockHttpMessageHandler;
    }
}
