using Fcg.User.Service.Application.Dtos.Usuario;
using Fcg.User.Service.Application.Interfaces;
using Fcg.User.Service.Domain.Exceptions.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Fcg.User.Service.Api.Controllers;

/// <summary>
/// Responsável pelos endpoints de autenticação e registro de usuários.
/// </summary>
[ApiController]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : MainController
{
    private readonly IUsuarioAppService _service;
    public AuthController(IUsuarioAppService service)
    {
        _service = service;
    }

    /// <summary>
    /// Realiza o login do usuário e retorna um token de autenticação.
    /// </summary>
    /// <param name="request">Dados de login do usuário.</param>
    /// <returns>Token de autenticação.</returns>
    [SwaggerOperation(
        Summary = "Autentica o usuário.",
        Description = "Realiza o login do usuário e retorna um token JWT para autenticação."
    )]
    [ProducesResponseType(typeof(TokenLoginDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [HttpPost("entrar")]
    public async Task<IActionResult> Entrar([FromBody] EfetuarLoginDto request)
    {
        var resultado = await _service.EfetuarLoginAsync(request.Email, request.Senha);
        return Ok(new TokenLoginDto(resultado));
    }

    /// <summary>
    /// Registra um novo usuário no sistema.
    /// </summary>
    /// <param name="request">Dados para cadastro do usuário.</param>
    /// <returns>Dados do usuário criado.</returns>
    [SwaggerOperation(
        Summary = "Registra um novo usuário.",
        Description = "Cria um novo usuário no sistema e retorna seus dados."
    )]
    [ProducesResponseType(typeof((string, UsuarioDto)), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] CadastrarUsuarioDto request)
    {
        var usuario = await _service.CadastrarAsync(request);
        return Created($"/usuarios/{usuario.Id}", usuario);
    }
}