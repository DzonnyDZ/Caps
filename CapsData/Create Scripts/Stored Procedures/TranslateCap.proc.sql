--- <summary>Returns full translation of <see cref="dbo.Cap"/> object</summary>
--- <param name="CapID">ID of <see cref="dbo.Cap"/> to get translation of</param>
--- <param name="CultureNames">Contains names of cultures to get translation for. Cultures are evaluated in order they are passed to this parameter and 1st culture that object has translation for is used. If object does not have translation for any of given cultures invariant data are read from object itself.</param>
--- <returns>One row with following columns
--- <list type="table"><listheader><term>Column name</term><description>Description</description></listheader>
--- <item><term>CapID</term><description><paramref name="CapID"/></description></item>
--- <item><term>CapName</term><description>Localized name of the cap</description></item>
--- <item><term>CapNameCulture</term><description>Name of culture CapName is returned in. Null if Name was read diersctly from <see cref="dbo.Cap"/>.</description></item>
--- <item><term>MainPicture</term><description>Localized description of cap main picture</description></item>
--- <item><term>MainPictureCulture</term><description>Name of culture MainPicture is returned in. Null if Name was read diersctly from <see cref="dbo.Cap"/>.</description></item>
--- <item><term>Note</term><description>Localized note on cap</description></item>
--- <item><term>NoteCulture</term><description>Name of culture Note is returned in. Null if Name was read diersctly from <see cref="dbo.Cap"/>.</description></item>
--- <item><term>AnotherPictures</term><description>Localized description of other pictures on shape</description></item>
--- <item><term>AnotherPicturesCulture</term><description>Name of culture AnotherPictures is returned in. Null if Description was read diersctly from <see cref="dbo.Cap"/>.</description></item>
--- </list></returns>
CREATE PROCEDURE [dbo].[TranslateCap](
	@CapID int,
	@CultureNames dbo.VarCharTable READONLY
) AS BEGIN

	DECLARE @Results TABLE(
		ID int IDENTITY(1,1) PRIMARY KEY,
		[CapTranslationID] [int] NULL,
		[CapID] [int] NULL,
		[Culture] [varchar](15) NULL,
		[CapName] [nvarchar](255) NULL,
		MainPicture nvarchar(255) NULL,
		Note nvarchar(MAX) NULL,
		AnotherPictures [nvarchar](max) NULL
	);
	DECLARE @CulturesOrdered TABLE(ID int IDENTITY(1,1) PRIMARY KEY, Culture varchar(15) UNIQUE);
    INSERT INTO @CulturesOrdered(Culture) SELECT cn.Value FROM @CultureNames cn;
   
	INSERT INTO @Results (CapTranslationID,CapID,Culture,CapName,MainPicture,Note,AnotherPictures)
		SELECT ct.* FROM  dbo.CapTranslation ct INNER JOIN @CulturesOrdered co ON(ct.Culture = co.Culture)
		WHERE ct.CapID = @CapID
		ORDER BY co.ID;
	INSERT INTO @Results (CapID,CapName,MainPicture,Note,AnotherPictures)
		SELECT c.CapID,c.CapName,c.MainPicture,c.Note,c.AnotherPictures FROM Cap c WHERE c.CapID = @CapID   ;


	SELECT @CapID AS CapID, tName.*, tMainPicture.*, tNote.*, tAnotherPictures.* FROM
		(SELECT TOP 1 CapName, Culture AS CapNameCulture FROM @Results WHERE CapName IS NOT NULL ORDER BY ID) tName	
		CROSS JOIN
		(SELECT TOP 1 MainPicture, Culture AS MainPictureCulture FROM @Results WHERE MainPicture IS NOT NULL ORDER BY ID) tMainPicture	
		CROSS JOIN
		(SELECT TOP 1 Note, Culture AS NoteCulture FROM @Results WHERE Note IS NOT NULL ORDER BY ID) tNote
		CROSS JOIN
		(SELECT TOP 1 AnotherPictures, Culture AS AnotherPicturesCulture FROM @Results WHERE AnotherPictures IS NOT NULL ORDER BY ID) tAnotherPictures
		;
END;