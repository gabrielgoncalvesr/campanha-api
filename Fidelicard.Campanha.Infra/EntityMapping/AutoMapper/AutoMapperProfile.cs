using AutoMapper;
using Fidelicard.Campanha.Core.Models;
using Fidelicard.Campanha.Infra.EntityMapping.DTO;

namespace Fidelicard.Campanha.Infra.EntityMapping.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CampanhaDTO, Campanhas>().ReverseMap();
        }
    }
}
