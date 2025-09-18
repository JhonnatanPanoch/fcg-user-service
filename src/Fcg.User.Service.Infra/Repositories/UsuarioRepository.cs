using Fcg.User.Service.Domain.Entities;
using Fcg.User.Service.Domain.Interfaces;
using Fcg.User.Service.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Fcg.User.Service.Infra.Repositories;
public class UsuarioRepository(AppDbContext context) : Repository<UsuarioEntity>(context), IUsuarioRepository
{
    public async Task<UsuarioEntity?> ObterPorEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
    }
}
