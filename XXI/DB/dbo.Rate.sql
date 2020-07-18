CREATE FUNCTION [dbo].[Rate]
(
	@param1 int,
	@param2 datetime
)
RETURNS TABLE AS RETURN
(
	SELECT Rates.Value FROM Rates
	WHERE NumCode=@param1 and Rates.date=@param2
)