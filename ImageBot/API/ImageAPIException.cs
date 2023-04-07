namespace ImageBot.API
{
    public sealed class ImageAPIException : Exception
    {
        public ImageAPIException()
        {
        }

        public ImageAPIException(string message)
            : base(message)
        {
        }

        public ImageAPIException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
