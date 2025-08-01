using SGRH.Web.Infrastructure.Endpoints.Floor;

namespace SGRH.Web.Infrastructure.Endpoints.Floor
{
    public class FloorEndpoints : IFloorEndpoints
    {
        public string GetAllFloors { get; }
        public string GetFloorById { get; }
        public string CreateFloor { get; }
        public string ModifyFloor { get; }
        public string DisableFloor { get; }

        public FloorEndpoints()
        {
            GetAllFloors = "Floor/GetFloors";
            GetFloorById = "Floor/GetFloorById";
            CreateFloor = "Floor/CreateFloor";
            ModifyFloor = "Floor/ModifyFloor";
            DisableFloor = "Floor/DisableFloor";
        }
    }
}