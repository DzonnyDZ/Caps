--- <summary>Returns full translation of object that supports simple translation</summary>
--- <param name="ObjectType">String type of object to get simple translation of. This must be name of one of table table <see cref="dbo.SimpleTranslation"/> has relation to.</param>
--- <param name="ObjectID">ID of object to get translation of</param>
--- <param name="CultureNames">Contains names of cultures to get translation for. Cultures are evaluated in order they are passed to this parameter and 1st culture that object has translation for is used. If object does not have translation for any of given cultures invariant data are read from object itself.</param>
--- <returns>One row with following columns
--- <list type="table"><listheader><term>Column name</term><description>Description</description></listheader>
--- <item><term>Name</term><description>Localized name of the object</description></item>
--- <item><term>NameCulture</term><description>Name of culture Name is returned in. Null if Name was read from object itself.</description></item>
--- <item><term>Description</term><description>Localized description of the object</description></item>
--- <item><term>DescriptionCulture</term><description>Name of culture Description is returned in. Null if Description was read from object itself.</description></item>
--- </list></returns>
CREATE PROCEDURE [dbo].[TranslateSimpleObject]
	(
	@ObjectType varchar(15),
	@ObjectID int,
	@CultureNames dbo.VarCharTable READONLY
	)
AS
BEGIN

	DECLARE @Results TABLE(
		ID int IDENTITY(1,1) PRIMARY KEY,
		[SimpleTranslationID] [int] NULL,
		[CategoryID] [int] NULL,
		[KeywordID] [int] NULL,
		[ProductID] [int] NULL,
		[CompanyID] [int] NULL,
		[ProductTypeID] [int] NULL,
		[TargetID] [int] NULL,
		[MaterialID] [int] NULL,
		[CapTypeID] [int] NULL,
		[MainTypeID] [int] NULL,
		[CapSignID] [int] NULL,
		[StorageID] [int] NULL,
		[StorageTypeID] [int] NULL,
		[CapInstanceID] [int] NULL,
		[Culture] [varchar](15) NULL,
		[Name] [nvarchar](50) NULL,
		[Description] [nvarchar](max) NULL
	);
	DECLARE @CulturesOrdered TABLE(ID int IDENTITY(1,1) PRIMARY KEY, Culture varchar(15) UNIQUE);
    INSERT INTO @CulturesOrdered(Culture) SELECT cn.Value FROM @CultureNames cn;
   
	INSERT INTO @Results ([SimpleTranslationID],[CategoryID],[KeywordID],[ProductID],[CompanyID],[ProductTypeID],[TargetID],[MaterialID],[CapTypeID],[MainTypeID],[CapSignID],[StorageID],[StorageTypeID],[CapInstanceID],[Culture],[Name],[Description])
		SELECT st.* FROM dbo.SimpleTranslation st  INNER JOIN @CulturesOrdered co ON(st.Culture = co.Culture)
		WHERE ((@ObjectType = 'Category' AND st.CategoryID = @ObjectID) OR
			(@ObjectType = 'Keyword' AND st.KeywordID = @ObjectID) OR
			(@ObjectType = 'Product' AND st.ProductID = @ObjectID) OR
			(@ObjectType = 'Company' AND st.CompanyID = @ObjectID) OR
			(@ObjectType = 'ProductType' AND st.ProductTypeID = @ObjectID) OR
			(@ObjectType = 'Target' AND st.TargetID = @ObjectID) OR
			(@ObjectType = 'Material' AND st.MaterialID = @ObjectID) OR
			(@ObjectType = 'CapType' AND st.CapTypeID = @ObjectID) OR
			(@ObjectType = 'MainType' AND st.MainTypeID = @ObjectID) OR
			(@ObjectType = 'CapSign' AND st.CapSignID = @ObjectID) OR
			(@ObjectType = 'Storage' AND st.StorageID = @ObjectID) OR
			(@ObjectType = 'StorageType' AND st.StorageTypeID = @ObjectID) OR
			(@ObjectType = 'CapInstance' AND st.CapInstanceID = @ObjectID));
     
	IF @ObjectType = 'Category' 
		INSERT INTO @Results (CategoryID, Name, [Description]) SELECT t.CategoryID, t.CategoryName, t.[Description] FROM Category t WHERE t.CategoryID = @ObjectID;
	ELSE IF @ObjectType = 'Keyword' 
		INSERT INTO @Results (KeywordID, Name, [Description]) SELECT t.KeywordID, t.Keyword, NULL FROM Keyword t WHERE t.KeywordID = @ObjectID;
	ELSE IF @ObjectType = 'Product' 
		INSERT INTO @Results (ProductID, Name, [Description]) SELECT t.ProductID, t.ProductName, t.[Description] FROM Product t WHERE t.ProductID = @ObjectID;
	ELSE IF @ObjectType = 'Company' 
		INSERT INTO @Results (CompanyID, Name, [Description]) SELECT t.CompanyID, t.CompanyName, t.[Description] FROM Company t WHERE t.CompanyID = @ObjectID;
	ELSE IF @ObjectType = 'ProductType' 
		INSERT INTO @Results (ProductTypeID, Name, [Description]) SELECT t.ProductTypeID, t.ProductTypeName, t.[Description] FROM ProductType t WHERE t.ProductTypeID = @ObjectID;
	ELSE IF @ObjectType = 'Target' 
		INSERT INTO @Results (TargetID, Name, [Description]) SELECT t.TargetID, t.Name, t.[Description] FROM Target t WHERE t.TargetID = @ObjectID;
	ELSE IF @ObjectType = 'Material' 
		INSERT INTO @Results (MaterialID, Name, [Description]) SELECT t.MaterialID, t.Name, t.[Description] FROM Material t WHERE t.MaterialID = @ObjectID;
	ELSE IF @ObjectType = 'CapType' 
		INSERT INTO @Results (CapTypeID, Name, [Description]) SELECT t.CapTypeID, t.TypeName, t.[Description] FROM CapType t WHERE t.CapTypeID = @ObjectID;
	ELSE IF @ObjectType = 'MainType' 
		INSERT INTO @Results (MainTypeID, Name, [Description]) SELECT t.MainTypeID, t.TypeName, t.[Description] FROM MainType t WHERE t.MainTypeID = @ObjectID;
	ELSE IF @ObjectType = 'CapSign' 
		INSERT INTO @Results (CapSignID, Name, [Description]) SELECT t.CapSignID, t.Name, t.[Description] FROM CapSign t WHERE t.CapSignID = @ObjectID;
	ELSE IF @ObjectType = 'Storage' 
		INSERT INTO @Results (StorageID, Name, [Description]) SELECT t.StorageID, t.StorageNumber, t.[Description] FROM Storage t WHERE t.StorageID = @ObjectID;
	ELSE IF @ObjectType = 'StorageType' 
		INSERT INTO @Results (StorageTypeID, Name, [Description]) SELECT t.StorageTypeID, t.Name, t.[Description] FROM StorageType t WHERE t.StorageTypeID = @ObjectID;
	ELSE IF @ObjectType = 'CapInstance' 
		INSERT INTO @Results (CapInstanceID, Name, [Description]) SELECT t.CapInstanceID, NULL, t.Note FROM CapInstance t WHERE t.CapInstanceID = @ObjectID;

	SELECT * FROM
		(SELECT TOP 1 Name, Culture AS NameCulture FROM @Results WHERE Name IS NOT NULL ORDER BY ID) tName	
		CROSS JOIN
		(SELECT TOP 1 [Description], Culture AS DescriptionCulture FROM @Results WHERE [Description] IS NOT NULL ORDER BY ID) tCulture;
END;