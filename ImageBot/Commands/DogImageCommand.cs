using ImageBot.API;

namespace ImageBot.Commands
{
    public class DogImageCommand : ImageCommand
    {
        public DogImageCommand() : this(new DogImageAPI()) { }
        public DogImageCommand(ImageAPI dogImageAPI) { this.dogImageAPI = dogImageAPI; }

        public string GetName() { return "dog"; }
        public string GetDescription() { return "Get a random image of a dog!"; }

        public async Task<string> TryToRespondAsync(IReadOnlyDictionary<string, object> parameters)
        {
            try
            {
                return await RespondAsync(parameters);
            }
            catch (ImageAPIException ex)
            {
                return ex.Message;
            }
        }

        private async Task<string> RespondAsync(IReadOnlyDictionary<string, object> parameters)
        {
            if (parameters.TryGetValue("breed", out object? breed))
            {
                return await RespondWithUserBreedAsync(breed.ToString()!);
            }
            else
            {
                return await RespondWithRandomBreedAsync();
            }
        }

        private async Task<string> RespondWithUserBreedAsync(string breed) { return await dogImageAPI.GetRandomImageURLWithTagAsync(breed); }
        private async Task<string> RespondWithRandomBreedAsync() { return await dogImageAPI.GetRandomImageURLAsync(); }

        private readonly ImageAPI dogImageAPI;
    }
}
