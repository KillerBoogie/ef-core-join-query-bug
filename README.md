# Ef Core Join Query Bug
Demo project for EF Core [issue 34749](https://github.com/dotnet/efcore/issues/34749).

## Setup 
Update the sql server connection string in appsettings.json.

## Run
Run the application. The database will be automatically created and filled with test data.

To reproduce the error choose GET Locations and `Try Out`. Don't enter a language. Just choose `Execute`in Swagger UI.

(If you enter a language you can check [issue 34752](https://github.com/dotnet/efcore/issues/34751).)

## Error
The created SQL query has an extra faulty `LEFT JOIN`:

```
SELECT [l].[LocationId], [l].[Name], [t].[ImageId], [t].[DisplayOrder], [t].[Language], [t].[ScreenSize], [t].[FocusPointX], [t].[FocusPointY], [t].[Uri], [t].[c], [t].[c0], [t].[TId], [t].[ImageId0], [l].[Created], [l].[CreatedBy], [l].[CreatedInNameOf], [l].[LastModified], [l].[LastModifiedBy], [l].[LastModifiedInNameOf], [l].[TId], [l].[Version], [l].[Address_City], [l].[Address_CountryId], [l].[Address_CountryName], [l].[Address_DeliveryInstruction], [l].[Address_State], [l].[Address_Street], [l].[Address_StreetAffix], [l].[Address_StreetNumber], [l].[Address_ZipCode], [l1].[TId], [l1].[DisplayOrder], [l1].[FocusPointX], [l1].[FocusPointY], [l1].[ImageId], [l1].[Language], [l1].[LocationId], [l1].[ScreenSize]
FROM [VC].[Location] AS [l]
LEFT JOIN (
    SELECT [l0].[ImageId], [l0].[DisplayOrder], [l0].[Language], [l0].[ScreenSize], [l0].[FocusPointX], [l0].[FocusPointY], [i].[Uri], CAST([i].[MetaData_Width] AS bigint) AS [c], CAST([i].[MetaData_Height] AS bigint) AS [c0], [l0].[TId], [i].[ImageId] AS [ImageId0], [l0].[LocationId]
    FROM [VC].[LocationCoverImage] AS [l0]
    INNER JOIN [VC].[Image] AS [i] ON [l0].[ImageId] = [i].[ImageId]
) AS [t] ON [l].[LocationId] = [t].[LocationId]
LEFT JOIN [VC].[LocationCoverImage] AS [l1] ON [l].[LocationId] = [l1].[LocationId]
ORDER BY [l].[LocationId], [t].[TId], [t].[ImageId0]
```

The code crashes at the constructor of `ImageItem`.

Due to the generated SQL the exception is not thrown, when a `location` has no `coverImage`.

