namespace SGRH.Web.Infrastructure.Endpoints.Rate
{
    public interface IRateEndpoints
    {
        string GetAllRates { get; }
        string GetRateById { get; }
        string CreateRate { get; }
        string UpdateRate { get; }
        string DeleteRate { get; }
    }
}
