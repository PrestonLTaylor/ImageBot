namespace ImageBot.API
{
    public interface ImageAPI
    {
        public Task<string> GetRandomImageURLAsync();
    }
}
