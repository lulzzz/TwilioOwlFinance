using AutoMapper;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Model.Data;
using Twilio.OwlFinance.Domain.Model.ServiceModel;

namespace Twilio.OwlFinance.Services.Adapters
{
	public class MessageConverter : IConverter<Message, MessageServiceModel>
	{
		public MessageServiceModel Convert(Message input)
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Message, MessageServiceModel>()
					.ForMember(dest => dest.Id, option => option.MapFrom(source => source.Id))
				   .ForMember(dest => dest.DisplayLastCorrespondenceDate, option => option.MapFrom(source => source.LastCorrespondence.ToString("d")))
				   .ForMember(dest => dest.Summary, option => option.MapFrom(source => source.Summary));
			});
			IMapper mapper = config.CreateMapper();

			var message = mapper.Map<Message, MessageServiceModel>(input);

			return message;
		}
	}
}
