using SGRH.Web.Models.ServiceModule;
using SGRH.Web.Models.ServiceModule.Response;

namespace SGRH.Web.Interfaces.HttpClients.ServiceModule
{
    public interface IServiceHttpClient
    {
        public Task<GetAllServicesResponse> GetAllServicesAsync();
        public Task<GetServiceByIdResponse> GetServiceByIdAsync(int id);
        public Task<CreateServiceResponse> CreateServiceAsync(CreateServiceModel createServiceModel);
        public Task<EditServiceResponse> GetEditServiceByIdAsync(int id);
        public Task<EditServiceResponse> EditServiceAsync(ServiceModel serviceModel);
        public Task<DeleteServiceResponse> GetDeleteServiceByIdAsync(int id);
        public Task<DeleteServiceResponse> DeleteServiceAsync(DeleteServiceModel deleteServiceModel);


    }
}
