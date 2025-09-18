namespace Fcg.User.Service.Domain.Entities;
public class JogoAdquiridoEntity : EntityBase
{
    public Guid UsuarioId { get; set; }
    public virtual UsuarioEntity Usuario { get; set; } = null!;

    public Guid JogoId { get; set; }
    public virtual JogoEntity Jogo { get; set; } = null!;

    public DateTime DataAquisicao { get; set; } = DateTime.UtcNow;
    public decimal PrecoPago { get; set; }
}
