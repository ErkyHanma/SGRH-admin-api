using SGRH.Web.Infrastructure.Endpoints.Rate;

public class RateEndpoints : IRateEndpoints
{
    public string GetAllRates { get; }
    public string GetRateById { get; }
    public string CreateRate { get; }
    public string UpdateRate { get; }
    public string DeleteRate { get; }
    public RateEndpoints()
    {
        GetAllRates = "Rate/GetRates";
        GetRateById = "Rate/GetRateById";
        CreateRate = "Rate/CreateRate";
        UpdateRate = "Rate/UpdateRate";
        DeleteRate = "Rate/DeleteRate";
    }
}
