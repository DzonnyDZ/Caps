create TRIGGER [dbo].[CapSign_Instead_Ins] 
   ON  [dbo].[CapSign]
   instead of INSERT
AS 
BEGIN
	SET NOCOUNT ON;
			 insert into dbo.CapSign
					 (	 Name,[Description])
          output inserted.*
			 SELECT Name,dbo.EmptyStrToNull([Description])
  FROM inserted	 ;
END
