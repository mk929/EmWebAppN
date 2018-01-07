
using AutoMapper;
using EmWebApp.Domain;
using System.Collections.Generic;

namespace EmWebApp.Data
{
    public static class MVMMapper
    {
        static MVMMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ConsularApptVM, ConsularAppointment>().ForMember(dest => dest.ActivationCode, opt => opt.ResolveUsing<ActivationCodeRsvlr>());
                cfg.CreateMap<ConsularAppointment, ConsularApptVM>();
            });
        }
        public static ConsularAppointment MapToModel(this ConsularApptVM self)
        {
            var model = Mapper.Map<ConsularApptVM, ConsularAppointment>(self);
            return model;
        }
        public static ConsularApptVM MapToViewModel(this ConsularAppointment self)
        {
            var viewModel = Mapper.Map<ConsularAppointment, ConsularApptVM>(self);
            return viewModel;
        }

        public static List<ConsularApptVM> MapToViewModelList(this List<ConsularAppointment> self)
        {
            var viewModelList = Mapper.Map<List<ConsularApptVM>>(self);
            return viewModelList;

        }
    }

    public class ActivationCodeRsvlr : IValueResolver<ConsularApptVM, ConsularAppointment, string>
    {
        public string Resolve(ConsularApptVM source, ConsularAppointment destination, string destMember, ResolutionContext context)
        {
            if (destination.ActivationCode.Length > 0)
                return destination.ActivationCode;

            return source.ActivationCode;
        }

    }
}
