using FiapCloudGames.Domain.Enums;

namespace FiapCloudGames.Application.DTOs;

public record UserResponse(
    Guid Id,
    string Name,
    string Email,
    UserRole Role,
    DateTime CreatedAt
);

public record UpdateUserRequest(
    string? Name,
    string? Email
);
