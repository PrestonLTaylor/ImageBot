using System.Text.Json;

namespace ImageBot.API
{
    struct DogImageResponse
    {
        public string message{ get; set; }
        public string status { get; set; }
    }

    public sealed class DogImageAPI : ImageAPI
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
            var response = await GetRandomImageResponseAsync("breeds/image/random");

            // The message contains the url on a successful random get
            return response.message;
        }

        public async Task<string> GetRandomImageURLWithTagAsync(string breed)
        {
            var response = await GetRandomImageResponseAsync($"breed/{breed}/images/random");

            return response.message;
        }

        private async Task<DogImageResponse> GetRandomImageResponseAsync(string apiPath)
        {
            var rawResponse = await client.GetAsync(apiPath);
            var response = JsonSerializer.Deserialize<DogImageResponse>(await rawResponse.Content.ReadAsStringAsync());
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
