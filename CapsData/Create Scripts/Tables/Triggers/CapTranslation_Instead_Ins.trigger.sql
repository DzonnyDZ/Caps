CREATE TRIGGER [dbo].[CapTranslation_Instead_Ins] 
   ON  [dbo].CapTranslation 
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.CapTranslation
					 (CapID, Culture, CapName, MainPicture, Note, AnotherPictures)
               output inserted.*
			 SELECT CapID, dbo.EmptyStrToNull(Culture),dbo.EmptyStrToNull(CapName),dbo.EmptyStrToNull(MainPicture),dbo.EmptyStrToNull(Note),dbo.EmptyStrToNull(AnotherPictures)
  FROM inserted	 ;

 --  select * from dbo.category where categoryid=scope_identity();

			 
END
