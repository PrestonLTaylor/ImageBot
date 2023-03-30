using ImageBot.API;

namespace ImageBot.Commands
{
    public class DogImageCommand : ImageCommand
    {
        public DogImageCommand() : this(new DogImageAPI()) { }
        public DogImageCommand(ImageAPI dogImageAPI) { this.dogImageAPI = dogImageAPI; }

        public string GetName() { return "dog"; }
        public string GetDescription() { return "Get a random image of a dog!"; }

        public async Task<string> TryToRespondAsync()
        {
            try
            {
                return await RespondAsync();
            }
            catch (ImageAPIException ex)
            {
                return ex.Message;
            }
        }

        private async Task<string> RespondAsync() { return await dogImageAPI.GetRandomImageURLAsync(); }

        private readonly ImageAPI dogImageAPI;
    }
}
