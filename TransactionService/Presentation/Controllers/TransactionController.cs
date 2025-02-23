using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Core.Domain.Entities;
using TransactionService.Core.Domain.Interfaces;
using TransactionService.Presentation.DTOs;

namespace TransactionService.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/transactions")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        
        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("{transactionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] string transactionId)
        {
            if (transactionId == null || !Guid.TryParse(transactionId, out Guid transactionExternalId))
            {
                return BadRequest(ModelState);
            }

            var existingTransaction = await _transactionService.GetById(transactionExternalId);
            var transactionToDeliver = _mapper.Map<RetrieveTransactionDTO>(existingTransaction);
            return Ok(transactionToDeliver);
        }

        [AllowAnonymous] //Permite a tods para el test
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newTransaction = await _transactionService.Create(transaction);
            return Ok(newTransaction);
        }
    }
}
