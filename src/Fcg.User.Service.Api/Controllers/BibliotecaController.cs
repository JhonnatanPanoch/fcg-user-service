using Fcg.User.Service.Application.Dtos.Biblioteca;
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
    public class BibliotecaController : MainController
    {
        private readonly IBibliotecaAppService _service;
        private readonly IUsuarioAutenticadoAppService _userAppService;

        public BibliotecaController(
            IBibliotecaAppService service, 
            IUsuarioAutenticadoAppService userAppService)
        {
            _service = service;
            _userAppService = userAppService;
        }

        /// <summary>
        /// Obtém jogos da biblioteca da conta do usuário logado.
        /// </summary>
        [SwaggerOperation(
            Summary = "Lista jogos da biblioteca.",
            Description = "Obtém jogos da biblioteca da conta do usuário logado"
        )]
        [ProducesResponseType(typeof(BibliotecaJogosDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> BuscarJogosBiblioteca()
        {
            return Ok(await _service.ConsultarJogosBibliotecaAsync(_userAppService.ObterIdUsuario()));
        }
    }
}