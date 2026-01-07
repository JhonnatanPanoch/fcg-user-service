using Fcg.User.Service.Application.Dtos.Conta;
using Fcg.User.Service.Application.Interfaces;
using Fcg.User.Service.Domain.Exceptions.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Fcg.User.Service.Api.Controllers
{
    /// <summary>
    /// Responsável pelos endpoints de gerenciamento da conta do usuário logado.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class ContaController : MainController
    {
        private readonly IContaAppService _service;

        public ContaController(IContaAppService service)
        {
            _service = service;
        }
            
        /// <summary>
        /// Obtém dados da conta do usuário logado.
        /// </summary>
        [SwaggerOperation(
            Summary = "Obtém dados da conta do usuário logado.",
            Description = "Retorna os detalhes da conta do usuário a partir da informação no token."
        )]
        [ProducesResponseType(typeof(ContaDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> ObterMeusDados()
        {
            ContaDto dados = await _service.ObterAsync();
            return Ok(dados);
        }

        /// <summary>
        /// Atualizar dados do usuário logado.
        /// </summary>
        /// <param name="dto">Novos dados do usuário.</param>
        [SwaggerOperation(
            Summary = "Atualizar dados do usuário logado.",
            Description = "Atualiza as informações do usuário autenticado."
        )]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpPut]
        public async Task<IActionResult> Atualizar([FromBody] AtualizarContaDto dto)
        {
            await _service.AtualizarAsync(dto);
            return NoContent();
        }

        /// <summary>
        /// Efetua o descadastro do usuário autenticado do sistema.
        /// </summary>
        [SwaggerOperation(
            Summary = "Descadastro do usuário autenticado do sistema.",
            Description = "Inatva o usuário autenticado no sistema."
        )]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpDelete]
        public async Task<IActionResult> ExcluirCadastro()
        {
            await _service.DescadastrarAsync();
            return NoContent();
        }
    }
}