namespace Application.Property.Dtos;

public sealed record PropertyFullDetailsDto(
    string Code,
    string Name,
    string Address,
    decimal Price,
    int Year,
    OwnerDto? Owner,
    PropertyImageDto Images
);
