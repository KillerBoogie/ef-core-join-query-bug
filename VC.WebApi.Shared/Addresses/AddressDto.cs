namespace VC.WebApi.Shared.Addresses
{
    public record AddressRequest
    (
        string? DeliveryInstruction,
        string Street,
        string StreetNumber,
        string? StreetAffix,
        string ZipCode,
        string City,
        string? State,
        string CountryId,
        string CountryName
    );
}
