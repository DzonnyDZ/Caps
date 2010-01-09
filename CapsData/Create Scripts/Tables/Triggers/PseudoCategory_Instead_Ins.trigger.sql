CREATE TRIGGER [dbo].[PseudoCategory_Instead_Ins] 
   ON  [dbo].[PseudoCategory]
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.PseudoCategory
					 (	 Name,[Description],Condition)
         output inserted.*
			 SELECT dbo.EmptyStrToNull(Name),dbo.EmptyStrToNull([Description]),	dbo.EmptyStrToNull(Condition)
  FROM inserted	 ;


     -- select * from dbo.shape where shapeid=scope_identity();
			 
END
