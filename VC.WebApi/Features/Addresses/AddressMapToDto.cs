using VC.WebApi.Shared.Addresses;

namespace VC.WebApi.Features.Addresses
{
    internal static class AddressMapToDto
    {
        public static AddressRequest MapToDto(this Address address)
        {
            return new AddressRequest(
                address.DeliveryInstruction,
                address.Street,
                address.StreetNumber,
                address.StreetAffix,
                address.ZipCode,
                address.City,
                address.State,
                address.CountryId,
                address.CountryName
                );
        }
    }
}
