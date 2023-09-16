namespace MiniApp.Core
{
    public interface IMinimalHttpClient
    {
        Task<TResponse> MakeHttpRequest<TResponse>(HttpRequestMessage httpRequestMessage);
    }
}
