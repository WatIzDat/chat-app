﻿using Application.Abstractions.Messaging;

namespace Application.Users.RegisterUser;

public sealed record RegisterUserCommand(string Username, string Email) : ICommand<Guid>;
