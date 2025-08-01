namespace SGRH.Web.Infrastructure.Endpoints.Floor
{
    public interface IFloorEndpoints
    {
        string GetAllFloors { get; }
        string GetFloorById { get; }
        string CreateFloor { get; }
        string ModifyFloor { get; }
        string DisableFloor { get; }
    }
}