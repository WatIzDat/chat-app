﻿using Application.Abstractions.Messaging;

namespace Application.Users.Register;

public sealed record RegisterUserCommand(string Username, string Email) : ICommand<Guid>;