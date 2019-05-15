namespace BotkitLibrary
{
    /// <summary>
    /// Interface to cast result of web api calls
    /// </summary>
    public interface IChatPostMessageResult
    {
        string channel { get; }
        string ts { get; }
        string message { get; }
    }
}
