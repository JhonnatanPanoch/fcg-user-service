﻿namespace Fcg.User.Service.Application.Interfaces;

public interface IJwtAppService
{
    string GerarToken(string email, string role);
}