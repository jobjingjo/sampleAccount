﻿using AutoMapper;
using sampleAccount.DAL.Data;
using sampleAccount.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sampleAccount.DAL
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Account, AccountEntity>()
                .ForMember(x=>x.Balance,o=>o.MapFrom(x=>x.Balance))
                .ForMember(x => x.OwenerId, o => o.MapFrom(x => x.Owner))
                .ForMember(x => x.IBAN, o => o.MapFrom(x => x.AccountName))
                .ReverseMap();
                //.ForMember(x => x.ModelCount, o => o.MapFrom(x => x.Models.Count()));
        }

    }
}