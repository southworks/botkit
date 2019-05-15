namespace BotkitLibrary
{
    /// <summary>
    /// Interface to cast result of web api calls
    /// </summary>
    public interface IAuthTestResult
    {
        string user { get; }
        string team { get; }
        string userId { get; }
        string teamId { get; }
        bool ok { get; }
    }
}
