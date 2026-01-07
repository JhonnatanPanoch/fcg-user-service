using Fcg.User.Service.Application.Dtos.Usuario;
using Fcg.User.Service.Application.Interfaces;
using Fcg.User.Service.Domain.Enums;
using Fcg.User.Service.Domain.Exceptions.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Fcg.User.Service.Api.Controllers
{
    /// <summary>
    /// Responsável pelos endpoints de gerenciamento de usuários.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsuarioController : MainController
    {
        private readonly IUsuarioAppService _service;

        public UsuarioController(IUsuarioAppService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtém os detalhes de um usuário específico pelo seu ID.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        [SwaggerOperation(
            Summary = "Obtém um usuário por ID.",
            Description = "Retorna os detalhes de um usuário específico a partir do seu identificador."
        )]
        [ProducesResponseType(typeof(UsuarioDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var usuario = await _service.ObterPorIdAsync(id);
            return Ok(usuario);
        }

        /// <summary>
        /// Lista todos os usuários cadastrados.
        /// </summary>
        [SwaggerOperation(
            Summary = "Lista todos os usuários.",
            Description = "Retorna uma lista com todos os usuários cadastrados no sistema."
        )]
        [ProducesResponseType(typeof(List<UsuarioDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var usuarios = await _service.ObterTodosAsync();
            return Ok(usuarios);
        }

        /// <summary>
        /// Atualiza os dados de um usuário existente.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <param name="usuarioDto">Novos dados do usuário.</param>
        [SwaggerOperation(
            Summary = "Atualiza um usuário.",
            Description = "Atualiza as informações de um usuário existente."
        )]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(
            [FromRoute] Guid id, 
            [FromBody] AtualizarUsuarioDto usuarioDto)
        {
            await _service.AtualizarAsync(id, usuarioDto);
            return NoContent();
        }

        /// <summary>
        /// Atualiza o perfil (role) de um usuário.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <param name="role">Novo perfil do usuário.</param>
        [SwaggerOperation(
            Summary = "Atualiza o perfil do usuário.",
            Description = "Atualiza o perfil (role) de um usuário existente."
        )]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpPut("{id:guid}/role")]
        public async Task<IActionResult> AtualizarRole(
            [FromRoute] Guid id,
            [FromBody] RoleEnum role)
        {
            await _service.AtualizarRoleAsync(id, role);
            return NoContent();
        }

        /// <summary>
        /// Remove um usuário do sistema.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        [SwaggerOperation(
            Summary = "Remove um usuário.",
            Description = "Remove um usuário do sistema pelo seu identificador."
        )]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            await _service.RemoverAsync(id);
            return NoContent();
        }
    }
}