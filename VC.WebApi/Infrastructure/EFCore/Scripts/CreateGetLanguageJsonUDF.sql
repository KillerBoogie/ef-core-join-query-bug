CREATE or ALTER FUNCTION[dbo].[GetLanguageJSON]
(
@Json nvarchar(max),
	@PreferredLanguages nvarchar(max)
)
RETURNS nvarchar(max)
AS
BEGIN

    DECLARE @out nvarchar(max)

    Set @out = (SELECT language, value
    FROM
        (SELECT TOP(1) c.language, c.value
        FROM
            (SELECT value AS language, CHARINDEX(',' + value + ',', ',' + @preferredLanguages + ',') AS rank
                    FROM STRING_SPLIT(@preferredLanguages, ',')
			)  l
            RIGHT JOIN
            (
            SELECT language, value FROM openjson(@Json) WITH(language varchar(8), value varchar(max))
            )  c
            ON l.language = c.language
        ORDER BY CASE WHEN rank IS NULL THEN 1 ELSE 0 END, rank
        ) out
	For JSON PATH)

	RETURN @out

END
