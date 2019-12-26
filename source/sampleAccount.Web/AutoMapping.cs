using AutoMapper;
using sampleAccount.DAL.Data;
using sampleAccount.Models;
using sampleAccount.Web.Models;

namespace sampleAccount.Web
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Account, AccountEntity>()
                .ForMember(x => x.Balance, o => o.MapFrom(x => x.Balance))
                .ForMember(x => x.OwenerId, o => o.MapFrom(x => x.Owner))
                .ForMember(x => x.IBAN, o => o.MapFrom(x => x.AccountName))
                .ReverseMap();

            CreateMap<TransactionModel, AccountTransaction>()
                .ForMember(x => x.Amount, o => o.MapFrom(x => x.Amount))
                .ForMember(x => x.AccountName, o => o.MapFrom(x => x.TargetAccountNumber))
                .ForMember(x => x.Type, o => o.MapFrom(x => x.Type))
                .ReverseMap();

            CreateMap<TransactionEntity, AccountTransaction>()
                .ForMember(x => x.AccountName, o => o.MapFrom(x => x.From.IBAN))
                .ForMember(x => x.Amount, o => o.MapFrom(x => x.Amount))
                .ForMember(x => x.Type, o => o.MapFrom(x => x.Type))
                .ReverseMap();
        }
    }
}