namespace VC.WebApi.Features.Addresses
{
    public record AddressResponse(
        string? DeliveryInstruction,  // c/o ... z.Hdn. ...
        string Street,
        string StreetNumber,
        string? StreetAffix,
        string ZIP,
        string City,
        string? State,
        string CountryId,
        string CountryName
        );
}
