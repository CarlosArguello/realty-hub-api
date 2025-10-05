namespace Application.Property.Dtos;

public sealed record PropertySummaryDto(
    string Id,
    string Name,
    string Address,
    decimal Price,
    int Year,
    string? OwnerId,
    string? OwnerName,
    string? FileUrl
);