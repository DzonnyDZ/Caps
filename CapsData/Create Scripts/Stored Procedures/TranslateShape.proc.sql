--- <summary>Returns full translation of <see cref="dbo.Shape"/> object</summary>
--- <param name="ShapeID">ID of <see cref="dbo.Shape"/> to get translation of</param>
--- <param name="CultureNames">Contains names of cultures to get translation for. Cultures are evaluated in order they are passed to this parameter and 1st culture that object has translation for is used. If object does not have translation for any of given cultures invariant data are read from object itself.</param>
--- <returns>One row with following columns
--- <list type="table"><listheader><term>Column name</term><description>Description</description></listheader>
--- <item><term>Name</term><description>Localized name of the shape</description></item>
--- <item><term>NameCulture</term><description>Name of culture Name is returned in. Null if Name was read diersctly from <see cref="dbo.Shape"/>.</description></item>
--- <item><term>Size1Name</term><description>Localized name of primary size of the shape</description></item>
--- <item><term>Size1NameCulture</term><description>Name of culture Size1Name is returned in. Null if Name was read diersctly from <see cref="dbo.Shape"/>.</description></item>
--- <item><term>Size2Name</term><description>Localized name of secondary size of the shape</description></item>
--- <item><term>Size2NameCulture</term><description>Name of culture Size2Name is returned in. Null if Name was read diersctly from <see cref="dbo.Shape"/>.</description></item>
--- <item><term>Description</term><description>Localized description of the shape</description></item>
--- <item><term>DescriptionCulture</term><description>Name of culture Description is returned in. Null if Description was read diersctly from <see cref="dbo.Shape"/>.</description></item>
--- </list></returns>
CREATE PROCEDURE [dbo].[TranslateShape](
	@ShapeID int,
	@CultureNames dbo.VarCharTable READONLY
) AS BEGIN

	DECLARE @Results TABLE(
		ID int IDENTITY(1,1) PRIMARY KEY,
		[ShapeTranslationID] [int] NULL,
		[ShapeID] [int] NULL,
		[Culture] [varchar](15) NULL,
		[Name] [nvarchar](50) NULL,
		Size1Name nvarchar(50) NULL,
		Size2Name nvarchar(50) NULL,
		[Description] [nvarchar](max) NULL
	);
	DECLARE @CulturesOrdered TABLE(ID int IDENTITY(1,1) PRIMARY KEY, Culture varchar(15) UNIQUE);
    INSERT INTO @CulturesOrdered(Culture) SELECT cn.Value FROM @CultureNames cn;
   
	INSERT INTO @Results (ShapeTranslationID,ShapeID,Culture,Name,Size1Name,Size2Name,[Description])
		SELECT st.* FROM  dbo.ShapeTranslation st INNER JOIN @CulturesOrdered co ON(st.Culture = co.Culture)
		WHERE st.ShapeID = @ShapeID
		ORDER BY co.ID;
	INSERT INTO @Results (ShapeID,Name,Size1Name,Size2Name,[Description])
		SELECT s.ShapeID,s.Name,s.Size1Name,s.Size2Name,s.[Description] FROM Shape s WHERE s.ShapeID = @ShapeID


	SELECT * FROM
		(SELECT TOP 1 Name, Culture AS NameCulture FROM @Results WHERE Name IS NOT NULL ORDER BY ID) tName	
		CROSS JOIN
		(SELECT TOP 1 Size1Name, Culture AS Size1NameCulture FROM @Results WHERE Size1Name IS NOT NULL ORDER BY ID) tSize1Name	
		CROSS JOIN
		(SELECT TOP 1 Size2Name, Culture AS Size2NameCulture FROM @Results WHERE Size2Name IS NOT NULL ORDER BY ID) tSize2Name	
		CROSS JOIN
		(SELECT TOP 1 [Description], Culture AS DescriptionCulture FROM @Results WHERE [Description] IS NOT NULL ORDER BY ID) tCulture
		;
END;