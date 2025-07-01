using SGRH.Application.Dtos.Hotel.Rate;
using SGRH.Application.Dtos.ServiceModule;
using SGRH.Domain.Entities.Hotel;
using SGRH.Domain.Entities.ServiceModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Application.Interfaces.Mappers.Hotel
{
    public interface IRateMapper
    {
        Rate MapFromDto(CreateRateDto dto); // Para crear
        CreateRateDto MapToCreatedDto(Rate entity); 
        void ApplyDeleteDto(Rate entity, DeleteRateDto dto); // Si delete requiere entidad
        RateDto MapToDto(Rate entity);
        void ApplyUpdateDto(Rate entity, UpdateRateDto dto);

    }
}
