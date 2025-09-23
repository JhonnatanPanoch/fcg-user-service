Criar migration
dotnet ef migrations add AlteracoesTbAquisicoes --project C:\work\repositórios\pessoal\fiap\Repositorios\TechChallengeFiap2\fcg-user-service\src\Fcg.User.Service.Infra --startup-project C:\work\repositórios\pessoal\fiap\Repositorios\TechChallengeFiap2\fcg-user-service\src\Fcg.User.Service.Api

Atualizar migration
dotnet ef database update --project C:\work\repositórios\pessoal\fiap\Repositorios\TechChallengeFiap2\fcg-user-service\src\Fcg.User.Service.Infra --startup-project C:\work\repositórios\pessoal\fiap\Repositorios\TechChallengeFiap2\fcg-user-service\src\Fcg.User.Service.Api

Payload registrar
{
  "nome": "Jhonnatan",
  "email": "Jhonnatan@email.com",
  "senha": "Senha@123",
  "confirmaSenha": "Senha@123"
}

Payload auth
{
  "email": "Jhonnatan@email.com",
  "senha": "Senha@123"
}