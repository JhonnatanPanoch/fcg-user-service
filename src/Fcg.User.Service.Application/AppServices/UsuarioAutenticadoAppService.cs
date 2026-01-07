using Fcg.User.Service.Application.Interfaces;
using Fcg.User.Service.Domain.Entities;
using Fcg.User.Service.Domain.Exceptions;
using Fcg.User.Service.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Fcg.User.Service.Application.AppServices;
public class UsuarioAutenticadoAppService : IUsuarioAutenticadoAppService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioAutenticadoAppService(
        IHttpContextAccessor httpContextAccessor, 
        IUsuarioRepository usuarioRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _usuarioRepository = usuarioRepository;
    }

    public string? ObterEmail()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
            return null;

        return user.FindFirst(ClaimTypes.Email)?.Value;
    }

    public Guid ObterIdUsuario()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null || !Guid.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            throw new ApplicationException();

        return userId;
    }

    public async Task<UsuarioEntity> ObterUsuarioAutenticadoAsync()
    {
        return await _usuarioRepository.ObterPorEmailAsync(ObterEmail()) ?? 
            throw new NotFoundException("Usuário autenticado não foi encontrado.");
    }
}
