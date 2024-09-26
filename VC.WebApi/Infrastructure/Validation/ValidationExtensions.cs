using NodaTime;
using NodaTime.Text;
using VC.WebApi.Domain.DisplayOrders;
using VC.WebApi.Shared.Auditing;
using VC.WebApi.Shared.BaseInterfaces;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.MultiLanguage;
using VC.WebApi.Shared.Results;
using VC.WebApi.Shared.Versioning;

namespace VC.WebApi.Infrastructure.Validation
{
    public static class ValidationExtensions
    {
        public static Result<MLRequired<T>> Create<T>(this Dictionary<string, string> value, ErrorList errors)
      where T : ICreate<T, string>
        {
            Result<MLRequired<T>> result = MLRequired<T>.Create<T>(value);
            errors.AddIfFailure(result);
            return result;
        }
        public static Result<CreatedInfo> Create(this CreatedInfoDto dto, ErrorList errors)
        {
            Result<CreatedInfo> result = CreatedInfo.Create(dto.Created, dto.CreatedBy, dto.CreatedInNameOf);
            errors.AddIfFailure(result);
            return result;
        }

        public static Result<DisplayOrder> Create(this int displayOrder, ErrorList errors)
        {
            Result<DisplayOrder> result = DisplayOrder.Create(displayOrder);
            errors.AddIfFailure(result);
            return result;
        }

       
    }
}
