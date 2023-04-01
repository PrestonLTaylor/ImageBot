using Discord;
using ImageBot.API;

namespace ImageBot.Commands
{
    public class DogImageCommand : ImageCommand
    {
        public DogImageCommand() : this(new DogImageAPI()) { }
        public DogImageCommand(ImageAPI dogImageAPI) { this.dogImageAPI = dogImageAPI; }

        public string GetName() { return "dog"; }
        public string GetDescription() { return "Get a random image of a dog!"; }

        public SlashCommandOptionBuilder[] GetCommandOptions()
        {
            return new SlashCommandOptionBuilder[]
            {
                new()
                {
                    Name = "breed",
                    Description = "The breed of dog to get.",
                    IsRequired = false,
                    Type = ApplicationCommandOptionType.String
                }
            };
        }

        public async Task<EmbedBuilder> TryToRespondAsync(IReadOnlyDictionary<string, object> parameters)
        {
            try
            {
                return await RespondAsync(parameters);
            }
            catch (ImageAPIException ex)
            {
                return new EmbedBuilder().WithTitle($"Error: {ex.Message}");
            }
        }

        private async Task<EmbedBuilder> RespondAsync(IReadOnlyDictionary<string, object> parameters)
        {
            var imageURL = await GetRandomImageOfADog(parameters);

            return new EmbedBuilder()
                .WithTitle("Dog!")
                .WithImageUrl(imageURL)
                .WithColor(new Color(1F, 0F, 1F));
        }

        private async Task<string> GetRandomImageOfADog(IReadOnlyDictionary<string, object> parameters)
        {
            if (parameters.TryGetValue("breed", out object? breed))
            {
                return await GetRandomImageURLWithBreedAsync(breed.ToString()!);
            }
            else
            {
                return await GetRandomImageURLAsync();
            }
        }

        private async Task<string> GetRandomImageURLWithBreedAsync(string breed) { return await dogImageAPI.GetRandomImageURLWithTagAsync(breed); }
        private async Task<string> GetRandomImageURLAsync() { return await dogImageAPI.GetRandomImageURLAsync(); }

        private readonly ImageAPI dogImageAPI;
    }
}
