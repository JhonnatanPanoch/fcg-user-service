using Fcg.User.Service.Application.Dtos.Conta;
using Fcg.User.Service.Application.Interfaces;
using Fcg.User.Service.Domain.Entities;
using Fcg.User.Service.Domain.Exceptions;
using Fcg.User.Service.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Fcg.User.Service.Application.AppServices;
public class ContaAppService : IContaAppService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUsuarioAutenticadoAppService _usuarioAutenticadoAppService;
    private readonly ILogger<ContaAppService> _logger;

    public ContaAppService(
        IUsuarioRepository usuarioRepository,
        IUsuarioAutenticadoAppService usuarioAutenticadoAppService,
        ILogger<ContaAppService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _usuarioAutenticadoAppService = usuarioAutenticadoAppService;
        _logger = logger;
    }

    public async Task AtualizarAsync(AtualizarContaDto dto)
    {
        _logger.LogInformation("Tentativa de atualizar conta: {@Dto}", dto);
        var usuario = await _usuarioAutenticadoAppService.ObterUsuarioAutenticadoAsync();

        var dbDataEmail = await _usuarioRepository.ObterAsync(x => x.Id != usuario.Id && x.Email == dto.Email);
        if (dbDataEmail?.Count != 0)
        {
            _logger.LogWarning($"O endereço de e-mail {dto.Email} informado já está cadastrado.");
            throw new ConflictException("O endereço de e-mail informado já está cadastrado.");
        }

        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email;

        await _usuarioRepository.Atualizar(usuario);
    }

    public async Task DescadastrarAsync()
    {
        await _usuarioRepository.Remover(
            await _usuarioAutenticadoAppService.ObterUsuarioAutenticadoAsync());
    }

    public async Task<ContaDto> ObterAsync()
    {
        var usuario = await _usuarioAutenticadoAppService.ObterUsuarioAutenticadoAsync();

        return new ContaDto()
        {
            Email = usuario.Email,
            Nome = usuario.Nome,
            Id = usuario.Id,
            Jogos = ObterJogosDaConta(usuario)
        };
    }

    private static List<JogosContaDto> ObterJogosDaConta(UsuarioEntity usuario)
    {
        // TODO: Implementar quando o microserviço de jogos estiver pronto
        return null;

        //return usuario.JogosAdquiridos.Select(jogo => new JogosContaDto
        //{
        //    IdComprovante = jogo.Id,
        //    Nome = jogo.Jogo.Nome,
        //    Descricao = jogo.Jogo.Descricao,
        //    ValorPago = jogo.PrecoPago,
        //    DataAquisicao = jogo.DataAquisicao,
        //}).ToList();
    }
}
