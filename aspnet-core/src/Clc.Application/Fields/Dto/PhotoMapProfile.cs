using AutoMapper;

namespace Clc.Fields.Dto
{
    public class PhotoMapProfile : Profile
    {
        public PhotoMapProfile()
        {
            CreateMap<WorkerDto, Worker>()
                .ForMember(s => s.Photo, opt => opt.Condition(s => s.Photo != null));

            CreateMap<VehicleDto, Vehicle>()
                .ForMember(s => s.Photo, opt => opt.Condition(s => s.Photo != null));
        }
    }
}
