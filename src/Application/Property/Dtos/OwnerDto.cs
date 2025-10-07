public sealed record OwnerDto(
    string Id,
    string Name,
    string? Address,
    string? Photo,
    DateTime? Birthday
);