
CREATE TRIGGER dbo.Keyword_Instead_Ins 
   ON  dbo.Keyword
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.Keyword
					 (	 Keyword)
        output inserted.*
			 SELECT dbo.EmptyStrToNull(Keyword)
  FROM inserted	 ;


--   select * from dbo.keyword where keywordid=scope_identity();
			 
END
