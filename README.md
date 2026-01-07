# FCG User Service

Serviço de gerenciamento de usuários, autenticação e autorização.

## Funcionalidades
- Cadastro de novos usuários.
- Login e geração de Tokens JWT.
- Gerenciamento de perfis.

## Detalhes Técnicos
Utiliza .NET 9 com PostgreSQL. A autenticação é via Bearer Token (JWT), compartilhado com os outros microsserviços para validar sessões.

## Kubernetes
Assim como os outros serviços, possui:
- Dockerfile otimizado.
- HPA configurado para escalabilidade automática.
- ConfigMaps e Secrets para gerenciamento de variáveis de ambiente sensíveis (como connection strings e chaves de assinatura de token).

## Endpoints e Permissões

| Recurso | Método | Rota | Permissão | Descrição |
| :--- | :--- | :--- | :--- | :--- |
| **Auth** | `POST` | `/api/v1/auth/entrar` | Público | Login (Retorna JWT) |
| | `POST` | `/api/v1/auth/registrar` | Público | Criação de conta (Cliente) |
| **Conta** | `GET` | `/api/v1/conta` | Autenticado | Dados do usuário logado |
| | `PUT` | `/api/v1/conta` | Autenticado | Atualiza dados próprios |
| | `DELETE` | `/api/v1/conta` | Autenticado | Exclui própria conta |
| **Biblioteca**| `GET` | `/api/v1/biblioteca` | Autenticado | Jogos que o usuário possui |
| **Usuários** | `GET` | `/api/v1/usuario` | **Admin** | Lista todos os usuários |
| | `PUT` | `/api/v1/usuario/{id}/role` | **Admin** | Altera perfil (Cliente/Admin) |
| | `DELETE` | `/api/v1/usuario/{id}` | **Admin** | Remove usuário |