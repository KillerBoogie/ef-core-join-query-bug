CREATE or Alter PROCEDURE GetEventDances
	@EventDanceLevelRanges NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

CREATE TABLE #TempEventDanceLevelRanges (DanceId uniqueidentifier, FromLevelId uniqueidentifier, ToLevelId uniqueidentifier);

	INSERT INTO #TempEventDanceLevelRanges (DanceId, FromLevelId, ToLevelId)
    SELECT 
        DanceId,
    FromLevelId,
    CASE WHEN ToLevelId = 'null' THEN NULL ELSE Cast(ToLevelId as uniqueidentifier) END
    FROM OPENJSON(@EventDanceLevelRanges)
    WITH (
        DanceId uniqueidentifier,
    FromLevelId uniqueidentifier,
    ToLevelId NVARCHAR(36)
    );

SELECT
        r.DanceId,
        d.Name,
		d.Category,
		r.FromLevelId,
		lFrom.Name as FromLevelName,
		lFrom.ShortName as FromLevelShortName,
		dlFrom.Code as FromDanceLevelCode,
		r.ToLevelId,
		lTo.Name as ToLevelName,
		lTo.ShortName as ToLevelShortName,
		dlTo.Code as ToDanceLevelCode
    FROM
        #TempEventDanceLevelRanges r
        JOIN VC.Dance d ON d.DanceId = r.DanceId
		JOIN VC.DanceLevel dlFrom on dlFrom.DanceId=r.DanceId and dlFrom.LevelId=r.FromLevelId		
        JOIN VC.Level lFrom ON lFrom.LevelId = r.FromLevelId
		LEFT JOIN VC.DanceLevel dlTo on dlTo.DanceId=r.DanceId and dlTo.LevelId=r.ToLevelId
		LEFT JOIN VC.Level lTo ON lTo.LevelId = r.ToLevelId;

DROP TABLE #TempEventDanceLevelRanges;
END