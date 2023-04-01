namespace ImageBot.API
{
    public interface ImageAPI
    {
        public Task<string> GetRandomImageURLAsync();
        public Task<string> GetRandomImageURLWithTagAsync(string imageTag);
    }
}
