using System.Net.Http.Json;

namespace ImageBot.API
{
    struct DogImageResponse
    {
        public string message{ get; set; }
        public string status { get; set; }
    }

    public class DogImageAPI : ImageAPI
    {
        public DogImageAPI() : this(new HttpClientHandler()) { }

        public DogImageAPI(HttpMessageHandler handler)
        {
            client = new(handler)
            {
                BaseAddress = new Uri("https://dog.ceo/api/")
            };
        }

        public async Task<string> GetRandomImageURLAsync()
        {
            var response = await GetRandomImageResponse();

            // The message contains the url on a successful random get
            return response.message;
        }

        private async Task<DogImageResponse> GetRandomImageResponse()
        {
            var response = await client.GetFromJsonAsync<DogImageResponse>("breeds/image/random");
            EnsureResponseIsValid(response);
            return response;
        }

        private void EnsureResponseIsValid(DogImageResponse response)
        {
            if (response.status != "success")
            {
                throw new ImageAPIException($"Unable to get an image of a dog. Error returned from API: {response.message}.");
            }
        }

        private readonly HttpClient client;
    }
}
