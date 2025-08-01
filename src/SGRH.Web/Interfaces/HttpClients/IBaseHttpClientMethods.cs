namespace SGRH.Web.Interfaces.HttpClients
{
    public interface IBaseHttpClientMethods
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<T> PostAsync<T>(string endpoint, object data);
    }
}
