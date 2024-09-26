using System.Text;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Shared.Addresses
{
    public class Address : ContactInfo
    {
        public string? DeliveryInstruction { get; } // c/o ... z.Hdn. ...
        public string Street { get; }
        public string StreetNumber { get; }
        public string? StreetAffix { get; }
        public string ZipCode { get; }
        public string City { get; }
        public string? State { get; }
        public string CountryId { get; }
        public string CountryName { get; }

        public static readonly int MaxLengthReceipient = 100;
        public static readonly int MaxLengthDeliveryInstruction = 100;
        public static readonly int MaxLengthStreet = 50; //The longest street name discovered in Gramsbergen with 46 characters
        public static readonly int MaxLengthStreetNumber = 20;
        public static readonly int MaxLengthStreetAffix = 50;
        public static readonly int MaxLengthZip = 10; //the longest postal code currently in use in the world is 10 digits long.
        //The North Island of New Zealand has a place named Taumatawhakatangihangakoauauotamateaturipukakapikimaungahoronukupokaiwhenuakitanatahu.
        //It holds the Guinness World Record for longest place name with 85 characters. Locals call it Taumata or Taumata Hill.
        public static readonly int MaxLengthCity = 50;
        public static readonly int MaxLengthState = 50;
        public static readonly int MaxLengthCountryId = 3;

#pragma warning disable CS8618
        private Address() { }
#pragma warning restore CS8618

        private Address(string? deliveryInstruction, string street, string streetNumber,
            string? streetAffix, string zipCode, string city, string? state, string countryId, string countryName)
        {
            DeliveryInstruction = deliveryInstruction;
            Street = street;
            StreetNumber = streetNumber;
            StreetAffix = streetAffix;
            ZipCode = zipCode;
            City = city;
            State = state;
            CountryId = countryId;
            CountryName = countryName;
        }

        public static Result<Address> Create(string? deliveryInstruction, string street, string streetNumber,
            string? streetAffix, string zipCode, string city, string? state, string countryId, string countryName)
        {
            Result result = Validate(deliveryInstruction, street, streetNumber, streetAffix, zipCode, city, state, countryId);
            return result.IsFailure ?
                Result<Address>.Failure(result) :
                Result<Address>.Success(new Address(deliveryInstruction, street, streetNumber, streetAffix, zipCode, city, state, countryId, countryName));
        }

        public static Result Validate(string? deliveryInstruction, string street, string streetNumber,
            string? streetAffix, string zipCode, string city, string? state, string countryId)
        {
            ErrorList errors = new();
            Result result;

            if (deliveryInstruction is not null)
            {
                result = ValidateDeliveryInstruction(deliveryInstruction);
                errors.AddIfFailure(result);
            }

            result = ValidateStreet(street);
            errors.AddIfFailure(result);

            result = ValidateStreetNumber(streetNumber);
            errors.AddIfFailure(result);

            if (streetAffix is not null)
            {
                result = ValidateStreetAffix(streetAffix);
                errors.AddIfFailure(result);
            }

            result = ValidateZipCode(zipCode);
            errors.AddIfFailure(result);

            result = ValidateCity(city);
            errors.AddIfFailure(result);

            if (state is not null)
            {
                result = ValidateState(state);
                errors.AddIfFailure(result);
            }

            result = ValidateCountry(countryId);
            errors.AddIfFailure(result);

            return errors.HasErrors ?
                Result.Failure(errors) :
                Result.Success();
        }

        public static Result ValidateDeliveryInstruction(string deliveryInstruction)
        {
            if (string.IsNullOrWhiteSpace(deliveryInstruction))
            {
                return Result.Failure(Error.Validation.Must_Not_Be_Empty(nameof(deliveryInstruction)));
            }
            else if (deliveryInstruction.Length > MaxLengthDeliveryInstruction)
            {
                return Result.Failure(Error.Validation.Max_Char_Length_Exceeded(nameof(deliveryInstruction), MaxLengthDeliveryInstruction));
            }
            return Result.Success();
        }

        public static Result ValidateStreet(string street)
        {
            if (string.IsNullOrWhiteSpace(street))
            {
                return Result.Failure(Error.Validation.Must_Not_Be_Empty(nameof(street)));
            }
            else if (street.Length > MaxLengthStreet)
            {
                return Result.Failure(Error.Validation.Max_Char_Length_Exceeded(nameof(street), MaxLengthStreet));
            }
            return Result.Success();
        }

        public static Result ValidateStreetNumber(string streetNumber)
        {
            if (string.IsNullOrWhiteSpace(streetNumber))
            {
                return Result.Failure(Error.Validation.Must_Not_Be_Empty(nameof(streetNumber)));
            }
            else if (streetNumber.Length > MaxLengthStreetNumber)
            {
                return Result.Failure(Error.Validation.Max_Char_Length_Exceeded(nameof(streetNumber), MaxLengthStreetNumber));
            }
            return Result.Success();
        }

        public static Result ValidateStreetAffix(string streetAffix)
        {
            if (string.IsNullOrWhiteSpace(streetAffix))
            {
                return Result.Failure(Error.Validation.Must_Not_Be_Empty(nameof(streetAffix)));
            }
            else if (streetAffix?.Length > MaxLengthStreetAffix)
            {
                return Result.Failure(Error.Validation.Max_Char_Length_Exceeded(nameof(streetAffix), MaxLengthStreetAffix));
            }
            return Result.Success();
        }

        public static Result ValidateZipCode(string zipCode)
        {
            if (string.IsNullOrWhiteSpace(zipCode))
            {
                return Result.Failure(Error.Validation.Must_Not_Be_Empty(nameof(zipCode)));
            }
            else if (zipCode.Length > MaxLengthZip)
            {
                return Result.Failure(Error.Validation.Max_Char_Length_Exceeded(nameof(zipCode), MaxLengthZip));
            }
            return Result.Success();
        }

        public static Result ValidateCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return Result.Failure(Error.Validation.Must_Not_Be_Empty(nameof(city)));
            }
            else if (city.Length > MaxLengthCity)
            {
                return Result.Failure(Error.Validation.Max_Char_Length_Exceeded(nameof(city), MaxLengthCity));
            }
            return Result.Success();
        }

        public static Result ValidateState(string state)
        {
            if (string.IsNullOrWhiteSpace(state))
            {
                return Result.Failure(Error.Validation.Must_Not_Be_Empty(nameof(state)));
            }
            else if (state.Length > MaxLengthState)
            {
                return Result.Failure(Error.Validation.Max_Char_Length_Exceeded(nameof(state), MaxLengthState));
            }
            return Result.Success();
        }

        public static Result ValidateCountry(string countryId)
        {
            if (string.IsNullOrWhiteSpace(countryId))
            {
                return Result.Failure(Error.Validation.Must_Not_Be_Empty(nameof(countryId)));
            }
            else if (countryId.Length != MaxLengthCountryId)
            {
                return Result.Failure(Error.Validation.Invalid(nameof(countryId)));
            }
            return Result.Success();
        }

        public override string ToString()
        {
            char blank = ' ';
            StringBuilder text = new StringBuilder();
            if (DeliveryInstruction is not null)
            {
                text.Append(DeliveryInstruction);
                text.Append('\n');
            }
            text.Append(Street);
            text.Append(blank);
            text.Append(StreetNumber);
            text.Append('\n');
            if (StreetAffix is not null)
            {
                text.Append(StreetAffix);
                text.Append('\n');
            }
            text.Append(ZipCode);
            text.Append(blank);
            text.Append(City);
            text.Append('\n');
            if (State is not null)
            {
                text.Append(State);
                text.Append('\n');
            }
            text.Append(CountryId + '\n');
            return text.ToString();
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DeliveryInstruction?.ToLowerInvariant() ?? "";
            yield return Street.ToLowerInvariant();
            yield return StreetNumber.ToLowerInvariant();
            yield return StreetAffix?.ToLowerInvariant() ?? "";
            yield return ZipCode.ToLowerInvariant();
            yield return City.ToLowerInvariant();
            yield return State?.ToLowerInvariant() ?? "";
            yield return CountryId.ToLowerInvariant();
        }
    }
}
