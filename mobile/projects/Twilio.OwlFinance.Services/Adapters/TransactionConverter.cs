using System.Globalization;
using AutoMapper;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Model.Data;
using Twilio.OwlFinance.Domain.Model.ServiceModel;

namespace Twilio.OwlFinance.Services.Adapters
{
    public class TransactionConverter : IConverter<Transaction, TransactionServiceModel>
    {
        public TransactionServiceModel Convert(Transaction input)
        {
			NumberFormatInfo currencyFormat = new CultureInfo(CultureInfo.CurrentCulture.ToString()).NumberFormat;
			currencyFormat.CurrencyNegativePattern = 1;

			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Transaction, TransactionServiceModel>()
				   .ForMember(dest => dest.Id, option => option.MapFrom(source => source.Id))
				   .ForMember(dest => dest.TransactionId, option => option.MapFrom(source => source.TransactionId))
				   .ForMember(dest => dest.DisplayDate, option => option.MapFrom(source => source.Date.ToString("d")))
				   .ForMember(dest => dest.Merchant, option => option.MapFrom(source => source.MerchantName))
				   .ForMember(dest => dest.Summary, option => option.MapFrom(source => source.Description))
				   .ForMember(dest => dest.MerchantImageUrl, option => option.MapFrom(source => source.MerchantImageUrl))
				   .ForMember(dest => dest.CardNumber, option => option.MapFrom(source => source.CardNumber))
				   .ForMember(dest => dest.CardHolderName, option => option.MapFrom(source => source.CardHolderName))
				   .ForMember(dest => dest.ExpirationDisplayDate, option => option.MapFrom(source => source.CardExpirationDate.ToString("MM/yy")))
				   .ForMember(dest => dest.DisplayAmount, option => option.MapFrom(source => string.Format(currencyFormat, "{0:c}", (source.Amount))));
			});
            IMapper mapper = config.CreateMapper();

            var transaction = mapper.Map<Transaction, TransactionServiceModel>(input);

            return transaction;
        }
    }
}
