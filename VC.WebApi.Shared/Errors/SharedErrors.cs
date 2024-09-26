using System.Reflection;
using VC.WebApi.Shared.Languages;

namespace VC.WebApi.Shared.Errors
{
    public sealed partial class Error
    {
        /// <summary>
        /// HTTP-400
        /// </summary>
        public static class BadRequest
        {
            public static Error Resource_At_Uri_Cant_Be_Read(string resourceName, string uri)
            {
                return new Error(
                    ErrorClass.BadRequest,
                    MethodBase.GetCurrentMethod()!.Name,
                    detailParameters: new Dictionary<string, object?> {
                         { "resourceName", resourceName },
                         { "uri", uri }
                    }
                    );
            }

            public static Error No_File_Uploaded()
            {
                return new Error(
                    ErrorClass.BadRequest,
                    MethodBase.GetCurrentMethod()!.Name
                    );
            }
            public static Error Not_a_Valid_MultiLanguage_Json_Format(string fieldName)
            {
                return new Error(
                    ErrorClass.BadRequest,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } }
                    );
            }
            public static Error Missing_Value_for_Valid_MultiLanguage_Json(string fieldName)
            {
                return new Error(
                    ErrorClass.BadRequest,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } }
                    );
            }

            public static Error Uploaded_File_MaxSize_Exceeded()
            {
                return new Error(
                    ErrorClass.BadRequest,
                    MethodBase.GetCurrentMethod()!.Name
                    );
            }

            public static Error File_Upload_Failed(string fieldName, string fileName)
            {
                return new Error(
                    ErrorClass.BadRequest,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                   detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName }, { "fileName", fileName } }
                   );
            }

        }

        /// <summary>
        /// HTTP-401
        /// </summary>
        public static class NotAuthenticated
        {
            public static Error Not_Authenticated()
            {
                return new Error(ErrorClass.NotAuthenticated,
                     MethodBase.GetCurrentMethod()!.Name);
            }
        }

        /// <summary>
        /// HTTP-403
        /// </summary>
        public static class Forbidden
        {
            public static Error No_Permission_For_Object_Fields(string fieldName)
            {
                return new Error(ErrorClass.Forbidden,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                     detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error No_Permission_For_Object(string @object)
            {
                return new Error(ErrorClass.Forbidden,
                     MethodBase.GetCurrentMethod()!.Name,
                     detailParameters: new Dictionary<string, object?> { { "object", @object } });
            }

            public static Error No_Permission_For_Request()
            {
                return new Error(ErrorClass.Forbidden,
                    MethodBase.GetCurrentMethod()!.Name);
            }
            public static Error No_Permission_For_Command()
            {
                return new Error(ErrorClass.Forbidden,
                     MethodBase.GetCurrentMethod()!.Name);
            }
            public static Error No_Permission_For_Field(string fieldName)
            {
                return new Error(ErrorClass.Forbidden,
                     MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } }
                );
            }
        }

        /// <summary>
        /// HTTP-404
        /// </summary>
        public static class NotFound
        {
            public static Error Not_Found(string fieldName)
            {
                return new Error(ErrorClass.NotFound,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error Id_Not_Found(string fieldName, string id)
            {
                return new Error(
                    ErrorClass.NotFound,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName }, { "id", id } }
                    );
            }
            public static Error Id_Not_Found(string fieldName, Guid id)
            {
                return new Error(
                    ErrorClass.NotFound,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName }, { "id", id } }
                    );
            }
            public static Error Page_Not_Found()
            {
                return new Error(
                    ErrorClass.NotFound,
                    MethodBase.GetCurrentMethod()!.Name
                    );
            }

            public static Error Resource_Not_Found_At_Uri(string resourceName, string uri)
            {
                return new Error(
                    ErrorClass.NotFound,
                    MethodBase.GetCurrentMethod()!.Name,
                    detailParameters: new Dictionary<string, object?> { { "resourceName", resourceName }, { "uri", uri } }
                    );
            }

            public static Error Not_Found_Resource_With_Id(string resourceName, Guid id)
            {
                return new Error(
                    ErrorClass.NotFound,
                    MethodBase.GetCurrentMethod()!.Name,
                    detailParameters: new Dictionary<string, object?> { { "resourceName", resourceName }, { "id", id } }
                   );
            }
        }


        /// <summary>
        /// HTTP-409
        /// </summary>
        public static class Conflict
        {
            public static Error No_Deletion_For_Linked_Resource(string linkedResourceName, string referencingResourceName, string referencingTable, string constraintName)
            {
                return new Error(ErrorClass.Conflict,
                     MethodBase.GetCurrentMethod()!.Name,
                     detailParameters: new Dictionary<string, object?> {
                        { "linkedResourceName", linkedResourceName },
                        { "referencingResource", referencingResourceName}
                     },
                     logInfo: new Dictionary<string, object?> {
                        { "referencingTable", referencingTable },
                        { "constraintName", constraintName} }
                     );
            }
        }

        /// <summary>
        /// HTTP-422
        /// </summary>
        public static class Validation
        {
            public static Error A_Or_B_Required(string a, string b)
            {
                return new Error(
                    ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    detailParameters: new Dictionary<string, object?> {
                         { "a", a },
                         { "b", b}
                     });
            }

            public static Error Max_Char_Length_Exceeded(string fieldName, int maxLength)
            {
                return new Error(
                    ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                     new Dictionary<string, object?> { { "fieldName", fieldName }, { "maxLength", maxLength } }
                     );
            }

            public static Error Unique_Value_Required(string fieldName, string value)
            {
                return new Error(
                    ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                     new Dictionary<string, object?> { { "fieldName", fieldName }, { "value", value } }
                     );
            }

            public static Error Unique_MLValue_Required(string fieldName, string value, string language)
            {
                return new Error(
                    ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    new Dictionary<string, object?> {
                        { "fieldName", fieldName },
                         { "value", value },
                         { "language", language}}
                    );
            }

            public static Error Unique_MLValue_Required(string fieldName, string value, Language language)
            {
                return new Error(
                     ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                     new Dictionary<string, object?> {
                         { "fieldName", fieldName },
                         { "value", value },
                         { "language", language.ToString()}}
                     );
            }

            public static Error Duplicate_Values(string fieldName, string values)
            {
                return new Error(
                    ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                     new Dictionary<string, object?> {
                         { "fieldName", fieldName },
                         { "values", values }}
                     );
            }
            public static Error Invalid_RGBA_Format()
            {
                return new Error(
                    ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name);
            }
            public static Error Invalid_Period(string fieldName)
            {
                return new Error(
                    ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName
                    );
            }
            public static Error Must_Not_Be_Empty(string fieldName)
            {
                return new Error(
                    ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error Must_Be_Empty(string fieldName)
            {
                return new Error(
                    ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error Must_Not_Be_Zero(string fieldName)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } }
                    );
            }
            public static Error Must_Be_Zero(string fieldName, int value)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } },
                    logInfo: new Dictionary<string, object?> { { "value", value } }
                    );
            }
            public static Error Must_Be_Pos_Int(string fieldName, int value)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } },
                    logInfo: new Dictionary<string, object?> { { "value", value } }
                    );
            }
            public static Error Must_Be_Pos_Int_Or_Zero(string fieldName, int value)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } },
                    logInfo: new Dictionary<string, object?> { { "value", value } }
                    );
            }
            public static Error Not_Only_Whitespace(string fieldName)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error Only_Digits(string fieldName)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error No_Leading_Or_Trailing_Space(string fieldName)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error ML_Cant_Remove_Language_Requires_At_Least_One_Language(Language language)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    detailParameters: new Dictionary<string, object?> { { "language", language } }
                    );
            }
            public static Error ML_Requires_At_Least_One_Language(string fieldName)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error ML_Cant_Remove_Language_Does_Not_Exists(Language language)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     detailParameters: new Dictionary<string, object?> { { "language", language } }
                     );
            }
            public static Error ML_Language_Does_Not_Exists(string language)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                    detailParameters: new Dictionary<string, object?> { { "language", language } }
                    );
            }
            public static Error Date_Cant_Be_In_Future(string fieldName)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } }
                     );
            }
            public static Error Date_Cant_Be_Before_Min_Date(string fieldName, DateTime date)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                      pointer: fieldName,
                      detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName }, { "minDate", date.ToString() } }
                      );
            }
            public static Error Phone_Country_Code_Invalid(int maxDigits)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    detailParameters: new Dictionary<string, object?> { { "maxDigits", maxDigits.ToString() } }
                    );
            }
            public static Error Phone_Public_Part_Max_Length_Exceeded(int maxDigits)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     detailParameters: new Dictionary<string, object?> { { "maxDigits", maxDigits.ToString() } }
                    );
            }

            public static Error Min_Char_Length_Deceeded(string fieldName, int minLength)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName }, { "minLength", minLength.ToString() } }
                    );
            }
            public static Error Max_Digit_Length_Exceeded(string fieldName, int maxLength)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName }, { "maxLength", maxLength.ToString() } }
                    );
            }
            public static Error Min_Digit_Length_Deceeded(string fieldName, int minLength)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName }, { "minLength", minLength.ToString() } }
                    );
            }
            public static Error Char_Length_Incorrect(string fieldName, int length)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName }, { "length", length.ToString() } }
                    );
            }

            public static Error Exists_With_Id(string fieldName, string id)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName }, { "id", id } }
                    );
            }
            public static Error Exists_Already(string fieldName)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } }
                     );
            }
            public static Error Not_Successful(string fieldName, string id, string command)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName }, { "id", id }, { "command", command } }
                    );
            }
            public static Error Affected_Rows(string fieldName, string id, int expectedRows, int affectedRows)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> {
                         { "fieldName", fieldName } ,
                        { "id", id },
                        { "expectedRows", expectedRows.ToString() },
                        { "affectedRows", affectedRows.ToString() } }
                    );
            }
            public static Error Modified_By_Other_User(string fieldName)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error At_Least_One_Uppercase_Character(string fieldName)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error At_Least_One_Lowercase_Character(string fieldName)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error At_Least_One_Special_Character(string fieldName)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error At_Least_One_Number(string fieldName)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error Contains_Invalid_Characters(string fieldName)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error Database_Not_Available()
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name);
            }

            public static Error Failed_Unexpectedly()
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name);
            }
            public static Error Route_ID_Not_Match_DTO_ID()
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name);
            }
            public static Error Min_Value_Deceeded(string fieldName, long minValue)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName }, { "minValue", minValue } }
                    );
            }
            public static Error Min_Value_Deceeded(string fieldName, decimal minValue)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName }, { "minValue", minValue } }
                    );
            }
            public static Error Max_Value_Exceeded(string fieldName, long maxValue)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName }, { "maxValue", maxValue } }
                    );
            }

            public static Error Max_Value_Exceeded(string fieldName, decimal maxValue)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName }, { "maxValue", maxValue } }
                    );
            }
            public static Error Max_Decimal_Places_Exceeded(string fieldName, int decimalPlaces)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName }, { "decimalPlaces", decimalPlaces.ToString() } }
                    );
            }
            public static Error Required_Field(string fieldName)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error Required_Parameter(string parameterName)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: parameterName);
            }

            public static Error Length_Out_Of_Range(string fieldName, int from, int to)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName }, { "from", from.ToString() }, { "to", to.ToString() } }
                    );
            }
            public static Error FromDate_Must_Be_Before_UntilDate(DateTime fromDate, DateTime untilDate)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    detailParameters: new Dictionary<string, object?> { { "fromDate", fromDate.ToString() }, { "untilDate", untilDate.ToString() } }
                    );
            }
            public static Error StartDate_Cant_Be_After_EndDate(DateTime startDate, DateTime endDate)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    detailParameters: new Dictionary<string, object?> { { "startDate", startDate.ToString() }, { "endDate", endDate.ToString() } }
                    );
            }
            public static Error FromDate_Or_UntilDate_Must_Be_Not_Null()
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name);
            }
            public static Error Invalid_Date(string fieldName)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }

            public static Error Invalid_Language(string language)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    detailParameters: new Dictionary<string, object?> { { "language", language } }
                    );
            }

            public static Error Invalid_System_Language(string cultureName)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     detailParameters: new Dictionary<string, object?> { { "language", cultureName } });
            }

            public static Error Invalid_Guid(string fieldName)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                      pointer: fieldName,
                      detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error Invalid_Uri(string fieldName)
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name,
                     pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error Invalid(string fieldName)
            {
                return new Error(
                    ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    detailParameters: new Dictionary<string, object?> { { "fieldName", fieldName } });
            }
            public static Error Invalid_Value(string fieldName, string value)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    pointer: fieldName,
                    logInfo: new Dictionary<string, object?> { { "fieldName", fieldName }, { "value", value } });
            }
            public static Error Invalid_Event_Visibility(string value)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    logInfo: new Dictionary<string, object?> { { "value", value } });
            }
            public static Error Invalid_Regex(string regex, string exception)
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    detailParameters: new Dictionary<string, object?> { { "regex", regex }, { "exception", exception } });
            }

            public static Error Request_Body_Requires_At_Least_One_Field()
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name);
            }
            public static Error Update_Data_Missing()
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name);
            }
            public static Error Unpaged_Request_Must_Not_Have_PageNumber_Or_PageSize_Params()
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name);
            }
            public static Error AllLangauages_Request_Must_Not_Have_Language_Params()
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name);
            }
            public static Error Hour_Skipped()
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name);
            }
            public static Error Hour_Ambigiush()
            {
                return new Error(ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name);
            }
            public static Error TimeDiscriminator_Not_Valid(string timeDiscriminator)
            {
                return new Error(
                    ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    detailParameters: new Dictionary<string, object?> { { "timeDiscriminator", timeDiscriminator } });
            }
            public static Error Resource_Does_Not_Exist(string resourceName, Guid id)
            {
                return new Error(
                    ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    detailParameters: new Dictionary<string, object?> { { "resourceName", resourceName }, { "id", id.ToString() } });
            }

            public static Error Resource_Does_Not_Exist(string resourceName, string id)
            {
                return new Error(
                    ErrorClass.ValidationError,
                    MethodBase.GetCurrentMethod()!.Name,
                    detailParameters: new Dictionary<string, object?> { { "resourceName", resourceName }, { "id", id } });
            }
            public static Error Uploaded_File_Is_Empty(string fileName)
            {
                return new Error(
                    ErrorClass.BadRequest,
                    MethodBase.GetCurrentMethod()!.Name,
                    detailParameters: new Dictionary<string, object?> { { "fileName", fileName } });
            }
            public static Error Not_An_Accepted_Image_Type()
            {
                return new Error(ErrorClass.ValidationError,
                     MethodBase.GetCurrentMethod()!.Name);
            }

            public static Error Price_Cant_Be_Negative(decimal amount)
            {
                return new Error(
                    ErrorClass.BadRequest,
                    MethodBase.GetCurrentMethod()!.Name,
                    logInfo: new Dictionary<string, object?> { { "amount", amount } });
            }
        }

        /// <summary>
        /// HTTP-500
        /// </summary>
        public static class Fatal
        {
            public static Error Error(string exception)
            {
                return new Error(ErrorClass.InternalServerError,
                     MethodBase.GetCurrentMethod()!.Name,
                     logInfo: new Dictionary<string, object?> { { "exception", exception } }
                     );
            }
        }
    }
}

