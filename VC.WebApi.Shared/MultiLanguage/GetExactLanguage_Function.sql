-- ================================================
-- Template generated from Template Explorer using:
-- Create Scalar Function (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the function.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Marcus Koch>
-- Create date: <2022-07-06>
-- Description:	<Filter Json with Multilanguage text by preferred languages and optional parameter to search exact>
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[GetExactLanguage] 
(
	@Json nvarchar(max),
	@PreferredLanguages nvarchar(max)
)
RETURNS nvarchar(max)
AS
BEGIN
	
	DECLARE @out nvarchar(max)

	Set @out=(SELECT language, value
	FROM 
		(SELECT TOP(1) c.language, c.value  
		FROM 
			(SELECT value AS language, CHARINDEX(',' + value + ',', ',' + @preferredLanguages + ',') AS rank
				 FROM STRING_SPLIT(@preferredLanguages, ',')
			)  l
			JOIN
			(
			SELECT language, value FROM openjson(@Json) WITH (language varchar(14), value varchar(max) )
			)  c
			ON l.language= c.language
		ORDER BY CASE WHEN rank IS NULL THEN 1 ELSE 0 END, rank
		) out
	For JSON PATH)

	RETURN @out

END
GO

