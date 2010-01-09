


CREATE TRIGGER [dbo].[CapType_Instead_Upd] 
   ON  [dbo].CapType 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].[Captype]
   SET	
   TypeName=dbo.EmptyStrToNull(i.TypeName),
    ShapeID=i.shapeid,
      Size=i.size	, Size2	=i.size2, 
      Height = i.height	,MaterialID	=i.materialid, MainTypeID=i.maintypeid,
      Description=dbo.EmptyStrToNull(i.description),
      TargetID=i.targetID

 from inserted	as i
 WHERE captype.captypeid=i.captypeid

					;



			 
END


