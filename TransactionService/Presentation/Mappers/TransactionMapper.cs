using AutoMapper;
using TransactionService.Core.Domain.Entities;
using TransactionService.Presentation.DTOs;

namespace TransactionService.Presentation.Mappers
{
    public class TransactionMapper : Profile
    {
        public TransactionMapper() 
        {
            CreateMap<Transaction, RetrieveTransactionDTO>().ReverseMap();
        }
    }
}
