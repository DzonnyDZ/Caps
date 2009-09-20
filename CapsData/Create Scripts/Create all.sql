/****** Object:  UserDefinedFunction [dbo].[GetDatabaseVersion]    Script Date: 09/20/2009 19:34:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Name
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetDatabaseVersion] 
(
	-- Add the parameters for the function here
	 
)
RETURNS nvarchar(50)
AS
BEGIN
declare @dbGuid nvarchar(38) = '{DAFDAE3F-2F0A-4359-81D6-50BA394D72D9}';
declare @dbVersion nvarchar(11) = '0.0.1.0';
return @dbGuid + @dbversion;

END
GO
/****** Object:  UserDefinedFunction [dbo].[EmptyStrToNull]    Script Date: 09/20/2009 19:34:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[EmptyStrToNull] 
(
	-- Add the parameters for the function here
	@str nvarchar(MAX) 
)
RETURNS nvarchar(MAX)
AS
BEGIN
	

	return case when  @str = '' then null else @str end;

END
GO
/****** Object:  Table [dbo].[ISO 3166-1]    Script Date: 09/20/2009 19:34:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ISO 3166-1](
	[Alpha-2] [char](2) COLLATE Latin1_General_BIN NOT NULL,
	[Alpha-3] [char](3) COLLATE Latin1_General_BIN NOT NULL,
 CONSTRAINT [PK_ISO 3166-1] PRIMARY KEY CLUSTERED 
(
	[Alpha-2] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_ISO 3166-1_Alpha-3] UNIQUE NONCLUSTERED 
(
	[Alpha-3] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  UserDefinedTableType [dbo].[IntTable]    Script Date: 09/20/2009 19:34:40 ******/
CREATE TYPE [dbo].[IntTable] AS TABLE(
	[Value] [int] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[Value] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[VarCharTable]    Script Date: 09/20/2009 19:34:23 ******/
CREATE TYPE [dbo].[VarCharTable] AS TABLE(
	[Value] [varchar](50) COLLATE Czech_CI_AS NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[Value] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  Table [dbo].[StorageType]    Script Date: 09/20/2009 19:34:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StorageType](
	[StorageTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) COLLATE Czech_CI_AS NOT NULL,
	[Description] [nvarchar](max) COLLATE Czech_CI_AS NULL,
 CONSTRAINT [PK_StorageType] PRIMARY KEY CLUSTERED 
(
	[StorageTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_StorageType_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Target]    Script Date: 09/20/2009 19:34:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Target](
	[TargetID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) COLLATE Czech_CI_AS NOT NULL,
	[Description] [nvarchar](max) COLLATE Czech_CI_AS NULL,
 CONSTRAINT [PK_Target] PRIMARY KEY CLUSTERED 
(
	[TargetID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_Target] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductType]    Script Date: 09/20/2009 19:34:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductType](
	[ProductTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ProductTypeName] [nvarchar](50) COLLATE Czech_CI_AS NOT NULL,
	[Description] [nvarchar](max) COLLATE Czech_CI_AS NULL,
	[IsDrink] [bit] NULL,
 CONSTRAINT [PK_ProductType] PRIMARY KEY CLUSTERED 
(
	[ProductTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_ProductTypeName] UNIQUE NONCLUSTERED 
(
	[ProductTypeName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'General cap product types such as beer or shampoo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductType'
GO
/****** Object:  Table [dbo].[Shape]    Script Date: 09/20/2009 19:34:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shape](
	[ShapeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) COLLATE Czech_CI_AS NOT NULL,
	[Size1Name] [nvarchar](50) COLLATE Czech_CI_AS NOT NULL,
	[Size2Name] [nvarchar](50) COLLATE Czech_CI_AS NULL,
	[Description] [nvarchar](max) COLLATE Czech_CI_AS NULL,
 CONSTRAINT [PK_Shape] PRIMARY KEY CLUSTERED 
(
	[ShapeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_Shape_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cap physical shapes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Shape'
GO
/****** Object:  Table [dbo].[Keyword]    Script Date: 09/20/2009 19:34:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Keyword](
	[KeywordID] [int] IDENTITY(1,1) NOT NULL,
	[Keyword] [nvarchar](50) COLLATE Czech_CI_AS NOT NULL,
 CONSTRAINT [PK_Keyword] PRIMARY KEY CLUSTERED 
(
	[KeywordID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_Keyword] UNIQUE NONCLUSTERED 
(
	[Keyword] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Automatically filled cap keywords' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Keyword'
GO
/****** Object:  Table [dbo].[MainType]    Script Date: 09/20/2009 19:34:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MainType](
	[MainTypeID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](50) COLLATE Czech_CI_AS NOT NULL,
	[Description] [nvarchar](max) COLLATE Czech_CI_AS NULL,
 CONSTRAINT [PK_MainType] PRIMARY KEY CLUSTERED 
(
	[MainTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_MainType_Name] UNIQUE NONCLUSTERED 
(
	[TypeName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'General cap types' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MainType'
GO
/****** Object:  Table [dbo].[Material]    Script Date: 09/20/2009 19:34:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Material](
	[MaterialID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) COLLATE Czech_CI_AS NOT NULL,
	[Description] [nvarchar](max) COLLATE Czech_CI_AS NULL,
 CONSTRAINT [PK_Material] PRIMARY KEY CLUSTERED 
(
	[MaterialID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_Material_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cap materials' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Material'
GO
/****** Object:  Table [dbo].[Category]    Script Date: 09/20/2009 19:34:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](50) COLLATE Czech_CI_AS NOT NULL,
	[Description] [nvarchar](max) COLLATE Czech_CI_AS NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_CategoryName] UNIQUE NONCLUSTERED 
(
	[CategoryName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Predefined cap categories' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category'
GO
/****** Object:  Table [dbo].[Company]    Script Date: 09/20/2009 19:34:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[CompanyID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](50) COLLATE Czech_CI_AS NOT NULL,
	[Description] [nvarchar](max) COLLATE Czech_CI_AS NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_CompanyName] UNIQUE NONCLUSTERED 
(
	[CompanyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Companies' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company'
GO
/****** Object:  Trigger [company_Instead_Upd]    Script Date: 09/20/2009 19:34:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[company_Instead_Upd]
   ON  [dbo].[Company] 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].company	
   SET	
   companyName=dbo.EmptyStrToNull(i.companyName),
   
      Description=dbo.EmptyStrToNull(i.description)

 from inserted	as i
 WHERE company.companyid=i.companyid

					;



			 
END
GO
/****** Object:  Trigger [Company_Instead_Ins]    Script Date: 09/20/2009 19:34:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[Company_Instead_Ins] 
   ON  [dbo].[Company] 
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.company
					 (	CompanyName,[Description])
             output inserted.*
			 SELECT dbo.EmptyStrToNull(CompanyName),dbo.EmptyStrToNull([Description])
  FROM inserted	 ;

  --  select * from dbo.company where CompanyID=scope_identity();

			 
END
GO
/****** Object:  Trigger [category_Instead_Upd]    Script Date: 09/20/2009 19:34:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[category_Instead_Upd]
   ON  [dbo].[Category] 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].category	
   SET	
   categoryName=dbo.EmptyStrToNull(i.categoryName),
   
      Description=dbo.EmptyStrToNull(i.description)

 from inserted	as i
 WHERE category.categoryid=i.categoryid

					;



			 
END
GO
/****** Object:  Trigger [Category_Instead_Ins]    Script Date: 09/20/2009 19:34:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[Category_Instead_Ins] 
   ON  [dbo].[Category] 
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.category
					 (	 CategoryName,[Description])
               output inserted.*
			 SELECT dbo.EmptyStrToNull(CategoryName),dbo.EmptyStrToNull([Description])
  FROM inserted	 ;

 --  select * from dbo.category where categoryid=scope_identity();

			 
END
GO
/****** Object:  Table [dbo].[CapType]    Script Date: 09/20/2009 19:34:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CapType](
	[CapTypeID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](50) COLLATE Czech_CI_AS NOT NULL,
	[ShapeID] [int] NOT NULL,
	[Size] [int] NOT NULL,
	[Size2] [int] NULL,
	[Height] [int] NOT NULL,
	[MaterialID] [int] NOT NULL,
	[MainTypeID] [int] NOT NULL,
	[Description] [nvarchar](max) COLLATE Czech_CI_AS NULL,
	[TargetID] [int] NULL,
 CONSTRAINT [PK_CapType] PRIMARY KEY CLUSTERED 
(
	[CapTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_CapType_Name] UNIQUE NONCLUSTERED 
(
	[TypeName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Predefined cap types' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CapType'
GO
/****** Object:  Trigger [MainType_Instead_Upd]    Script Date: 09/20/2009 19:34:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[MainType_Instead_Upd]
   ON  [dbo].[MainType] 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].MainType	
   SET	
   typename=dbo.EmptyStrToNull(i.typename)   ,
   
      Description=dbo.EmptyStrToNull(i.description)
     

 from inserted	as i
 WHERE MainType.MainTypeID=i.MainTypeID

					;



			 
END
GO
/****** Object:  Trigger [MainType_Instead_Ins]    Script Date: 09/20/2009 19:34:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[MainType_Instead_Ins] 
   ON  [dbo].[MainType]
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.MainType
					 (	 TypeName,[Description])
        output inserted.*
			 SELECT dbo.EmptyStrToNull(TypeName),dbo.EmptyStrToNull([Description])
  FROM inserted	 ;


 --  select * from dbo.maintype where maintypeid=scope_identity();
			 
END
GO
/****** Object:  Trigger [Keyword_Instead_Upd]    Script Date: 09/20/2009 19:34:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[Keyword_Instead_Upd]
   ON  [dbo].[Keyword] 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].Keyword	
   SET	
   keyword=dbo.EmptyStrToNull(i.keyword)
   
      --Description=dbo.EmptyStrToNull(i.description)
     

 from inserted	as i
 WHERE Keyword.Keywordid=i.Keywordid

					;



			 
END
GO
/****** Object:  Trigger [Keyword_Instead_Ins]    Script Date: 09/20/2009 19:34:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[Keyword_Instead_Ins] 
   ON  [dbo].[Keyword]
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
GO
/****** Object:  Table [dbo].[Storage]    Script Date: 09/20/2009 19:34:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Storage](
	[StorageID] [int] IDENTITY(1,1) NOT NULL,
	[StorageNumber] [nvarchar](10) COLLATE Czech_CI_AS NOT NULL,
	[Description] [nvarchar](max) COLLATE Czech_CI_AS NULL,
	[StorageTypeID] [int] NOT NULL,
 CONSTRAINT [PK_Storage] PRIMARY KEY CLUSTERED 
(
	[StorageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_StorageNumber] UNIQUE NONCLUSTERED 
(
	[StorageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Trigger [shape_Instead_Upd]    Script Date: 09/20/2009 19:35:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[shape_Instead_Upd]
   ON  [dbo].[Shape] 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].shape	
   SET	
   name=dbo.EmptyStrToNull(i.name)   ,
				size1name=dbo.EmptyStrToNull(i.size1name)   ,
				size2name=dbo.EmptyStrToNull(i.size2name)   , 
      Description=dbo.EmptyStrToNull(i.description)
     

 from inserted	as i
 WHERE shape.shapeid=i.shapeid

					;



			 
END
GO
/****** Object:  Trigger [Shape_Instead_Ins]    Script Date: 09/20/2009 19:34:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[Shape_Instead_Ins] 
   ON  [dbo].[Shape]
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.Shape
					 (	 Name,Size1Name,Size2Name,[Description])
         output inserted.*
			 SELECT dbo.EmptyStrToNull(Name),dbo.EmptyStrToNull(Size1Name),	dbo.EmptyStrToNull(Size2Name),
			 dbo.EmptyStrToNull([Description])
  FROM inserted	 ;


     -- select * from dbo.shape where shapeid=scope_identity();
			 
END
GO
/****** Object:  Trigger [productype_Instead_Upd]    Script Date: 09/20/2009 19:35:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[productype_Instead_Upd]
   ON  [dbo].[ProductType] 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].producttype	
   SET	
   producttypename=dbo.EmptyStrToNull(i.producttypename)   ,
				 
      Description=dbo.EmptyStrToNull(i.description)
     

 from inserted	as i
 WHERE producttype.producttypeid=i.producttypeid

					;



			 
END
GO
/****** Object:  Trigger [ProductType_Instead_Ins]    Script Date: 09/20/2009 19:35:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[ProductType_Instead_Ins] 
   ON  [dbo].[ProductType]
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.ProductType
					 (	 ProductTypeName,[Description])
         output inserted.*
			 SELECT dbo.EmptyStrToNull(ProductTypeName),dbo.EmptyStrToNull([Description])
  FROM inserted	 ;

   --  select * from dbo.producttype where producttypeid=scope_identity();

			 
END
GO
/****** Object:  Table [dbo].[Product]    Script Date: 09/20/2009 19:34:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](50) COLLATE Czech_CI_AS NOT NULL,
	[CompanyID] [int] NULL,
	[ProductTypeID] [int] NULL,
	[Description] [nvarchar](max) COLLATE Czech_CI_AS NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_ProductName] UNIQUE NONCLUSTERED 
(
	[CompanyID] ASC,
	[ProductName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Predefined cap products' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Product'
GO
/****** Object:  Trigger [material_Instead_Upd]    Script Date: 09/20/2009 19:34:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[material_Instead_Upd]
   ON  [dbo].[Material] 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].material	
   SET	
   name=dbo.EmptyStrToNull(i.name)   ,
   
      Description=dbo.EmptyStrToNull(i.description)
     

 from inserted	as i
 WHERE material.materialid=i.materialid

					;



			 
END
GO
/****** Object:  Trigger [Material_Instead_Ins]    Script Date: 09/20/2009 19:34:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[Material_Instead_Ins] 
   ON  [dbo].[Material]
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.Material
					 (	 Name,[Description])
        output inserted.*
			 SELECT dbo.EmptyStrToNull(Name),dbo.EmptyStrToNull([Description])
  FROM inserted	 ;


   --  select * from dbo.material where materialid=scope_identity();
			 
END
GO
/****** Object:  Trigger [target_Instead_Upd]    Script Date: 09/20/2009 19:35:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[target_Instead_Upd]
   ON  [dbo].[Target] 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].target
   SET	
   --name=dbo.EmptyStrToNull(i.name)   ,
			name=i.name,
      Description=dbo.EmptyStrToNull(i.description)

     

 from inserted	as i
 WHERE target.targetid=i.targetid

					;



			 
END
GO
/****** Object:  Trigger [Target_Instead_Ins]    Script Date: 09/20/2009 19:35:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create TRIGGER [dbo].[Target_Instead_Ins] 
   ON  [dbo].[Target]
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.Target
					 (	 Name,[Description])
          output inserted.*
			 SELECT Name,dbo.EmptyStrToNull([Description])
  FROM inserted	 ;


    --   select * from dbo.storage where storageid=scope_identity();
			 
END
GO
/****** Object:  Trigger [storagetype_Instead_Upd]    Script Date: 09/20/2009 19:35:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[storagetype_Instead_Upd]
   ON  [dbo].[StorageType] 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].storagetype	
   SET	
   name=dbo.EmptyStrToNull(i.name)   ,
			
      Description=dbo.EmptyStrToNull(i.description)
      
     

 from inserted	as i
 WHERE storagetype.storagetypeid=i.storagetypeid

					;



			 
END
GO
/****** Object:  Trigger [StorageType_Instead_Ins]    Script Date: 09/20/2009 19:35:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[StorageType_Instead_Ins] 
   ON  [dbo].[StorageType]
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.StorageType
					 (	 Name,[Description])
        output inserted.*
			 SELECT dbo.EmptyStrToNull(Name),dbo.EmptyStrToNull([Description])
  FROM inserted	 ;


    --  select * from dbo.storagetype where storagetypeid=scope_identity();
			 
END
GO
/****** Object:  Trigger [storage_Instead_Upd]    Script Date: 09/20/2009 19:35:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[storage_Instead_Upd]
   ON  [dbo].[Storage] 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].storage	
   SET	
   --name=dbo.EmptyStrToNull(i.name)   ,
			storagenumber=i.storagenumber,
      Description=dbo.EmptyStrToNull(i.description),
      storagetypeid=i.storagetypeid
     

 from inserted	as i
 WHERE storage.storageid=i.storageid

					;



			 
END
GO
/****** Object:  Trigger [Storage_Instead_Ins]    Script Date: 09/20/2009 19:35:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[Storage_Instead_Ins] 
   ON  [dbo].[Storage]
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.Storage
					 (	 StorageNumber,[Description],StorageTypeID)
          output inserted.*
			 SELECT StorageNumber,dbo.EmptyStrToNull([Description]),StorageTypeID
  FROM inserted	 ;


    --   select * from dbo.storage where storageid=scope_identity();
			 
END
GO
/****** Object:  Trigger [product_Instead_Upd]    Script Date: 09/20/2009 19:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[product_Instead_Upd]
   ON  [dbo].[Product] 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].product	
   SET	
   productname=dbo.EmptyStrToNull(i.productname)   ,
				 companyid=i.companyid,producttypeid=i.producttypeid,
      Description=dbo.EmptyStrToNull(i.description)
     

 from inserted	as i
 WHERE product.productid=i.productid

					;



			 
END
GO
/****** Object:  Trigger [Product_Instead_Ins]    Script Date: 09/20/2009 19:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[Product_Instead_Ins] 
   ON  [dbo].[Product]
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.Product
					 (	 ProductName,CompanyID,ProductTypeID,[Description])
 output inserted.*
			 SELECT dbo.EmptyStrToNull(ProductName),CompanyID,ProductTypeID,dbo.EmptyStrToNull([Description])
  FROM inserted	 ;
 --   select * from dbo.product where productid=scope_identity();


			 
END
GO
/****** Object:  Trigger [CapType_Instead_Upd]    Script Date: 09/20/2009 19:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[CapType_Instead_Upd] 
   ON  [dbo].[CapType] 
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
      Description=dbo.EmptyStrToNull(i.description)

 from inserted	as i
 WHERE captype.captypeid=i.captypeid

					;



			 
END
GO
/****** Object:  Trigger [CapType_Instead_Ins]    Script Date: 09/20/2009 19:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[CapType_Instead_Ins] 
   ON  [dbo].[CapType] 
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.captype
					 (	 [TypeName]
      ,[ShapeID]
      ,[Size]
      ,[Size2]
      ,[Height]
      ,[MaterialID]
      ,[MainTypeID]
      ,[Description])
                     output inserted.*
			 SELECT dbo.EmptyStrToNull([TypeName])
      ,[ShapeID]
      ,[Size]
      ,[Size2]
      ,[Height]
      ,[MaterialID]
      ,[MainTypeID]
      ,dbo.EmptyStrToNull([Description])
  FROM inserted	 ;

     --   select * from dbo.captype where captypeid=scope_identity();

			 
END
GO
/****** Object:  Table [dbo].[Cap]    Script Date: 09/20/2009 19:34:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Cap](
	[CapID] [int] IDENTITY(1,1) NOT NULL,
	[CapTypeID] [int] NULL,
	[MainTypeID] [int] NOT NULL,
	[ShapeID] [int] NOT NULL,
	[CapName] [nvarchar](255) COLLATE Czech_CI_AS NOT NULL,
	[MainText] [nvarchar](255) COLLATE Czech_CI_AS NULL,
	[SubTitle] [nvarchar](255) COLLATE Czech_CI_AS NULL,
	[BackColor1] [int] NOT NULL,
	[BackColor2] [int] NULL,
	[ForeColor] [int] NULL,
	[MainPicture] [nvarchar](255) COLLATE Czech_CI_AS NULL,
	[TopText] [nvarchar](max) COLLATE Czech_CI_AS NULL,
	[SideText] [nvarchar](max) COLLATE Czech_CI_AS NULL,
	[BottomText] [nvarchar](max) COLLATE Czech_CI_AS NULL,
	[MaterialID] [int] NOT NULL,
	[Surface] [char](1) COLLATE Czech_CI_AS NOT NULL,
	[Size] [int] NOT NULL,
	[Size2] [int] NULL,
	[Height] [int] NOT NULL,
	[Is3D] [bit] NOT NULL,
	[Year] [int] NULL,
	[CountryCode] [char](2) COLLATE Latin1_General_BIN NULL,
	[DateCreated] [datetime] NULL,
	[Note] [nvarchar](max) COLLATE Czech_CI_AS NULL,
	[CompanyID] [int] NULL,
	[ProductID] [int] NULL,
	[ProductTypeID] [int] NULL,
	[StorageID] [int] NOT NULL,
	[ForeColor2] [int] NULL,
	[PictureType] [char](1) COLLATE Czech_CI_AS NULL,
	[HasBottom] [bit] NOT NULL,
	[HasSide] [bit] NOT NULL,
	[AnotherPictures] [nvarchar](max) COLLATE Czech_CI_AS NULL,
	[CountryOfOrigin] [char](2) COLLATE Latin1_General_BIN NULL,
	[IsDrink] [bit] NULL,
	[State] [smallint] NOT NULL,
	[TargetID] [int] NULL,
 CONSTRAINT [PK_Cap] PRIMARY KEY CLUSTERED 
(
	[CapID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'CapID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reference to standartized capy type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'CapTypeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reference to general cap type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'MainTypeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reference to cap shape' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'ShapeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of cap (usually sma as MainText or MainText + SubTitle or MainPicture)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'CapName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Main writing on cap (if any)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'MainText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Second most visible writng on cap (if any)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'SubTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'First cap bacground color' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'BackColor1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Secondary background color of cap (if any)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'BackColor2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Main cap fore color (if it has any foreground)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'ForeColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of cap main pictre (if any)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'MainPicture'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Text of cap top side' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'TopText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Text of cap sides' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'SideText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Text of cap bottom side' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'BottomText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reference to cap material it is mainly made of' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'MaterialID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cap surface, M for matting, G for glossy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'Surface'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cap main size in mm' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'Size'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cap seconray size in mm (if shape defines it)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'Size2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cap height in mm' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'Height'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If cap surface i.e. writing is 3D' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'Is3D'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Year when cap was found, if known' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'Year'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ISO2 counry code of country where cap was found, if any' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'CountryCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'When DB record was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'DateCreated'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Any note' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'Note'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Geometry < Logo < Drawing < Photo (biggest applicable)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'COLUMN',@level2name=N'PictureType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap'
GO
/****** Object:  Table [dbo].[Cap_Keyword_Int]    Script Date: 09/20/2009 19:34:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cap_Keyword_Int](
	[CapID] [int] NOT NULL,
	[KeywordID] [int] NOT NULL,
 CONSTRAINT [PK_Cap_Keyword_Int] PRIMARY KEY CLUSTERED 
(
	[CapID] ASC,
	[KeywordID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Trigger [Cap_Instead_Upd]    Script Date: 09/20/2009 19:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[Cap_Instead_Upd] 
   ON  [dbo].[Cap] 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].[Cap]
   SET [CapTypeID] = i.CapTypeID
      ,[MainTypeID] = i.MainTypeID
      ,[ShapeID] = i.ShapeID
      ,[CapName] = dbo.EmptyStrToNull(i.CapName)
      ,[MainText] = dbo.EmptyStrToNull(i.MainText)
      ,[SubTitle] = dbo.EmptyStrToNull(i.SubTitle)
      ,[BackColor1] = i.BackColor1
      ,[BackColor2] = i.BackColor2
      ,[ForeColor] = i.ForeColor
      ,[MainPicture] = dbo.EmptyStrToNull(i.MainPicture)
      ,[TopText] = dbo.EmptyStrToNull(i.TopText)
      ,[SideText] = dbo.EmptyStrToNull(i.SideText)
      ,[BottomText] = dbo.EmptyStrToNull(i.BottomText)
      ,[MaterialID] = i.MaterialID
      ,[Surface] = i.Surface
      ,[Size] = i.Size
      ,[Size2] = i.Size2
      ,[Height] = i.Height
      ,[Is3D] = i.Is3D
      ,[Year] = i.Year
      ,[CountryCode] = dbo.EmptyStrToNull(i.CountryCode)
     -- ,[DateCreated] = i.DateCreated
      ,[Note] = dbo.EmptyStrToNull(i.Note)
      ,[CompanyID] = i.CompanyID
      ,[ProductID] = i.ProductID
      ,[ProductTypeID] = i.ProductTypeID
      ,[StorageID] = i.StorageID
      ,[ForeColor2] = i.ForeColor2
      ,[PictureType] = i.PictureType
      ,[HasBottom] = i.HasBottom
      ,[HasSide] = i.HasSide
      ,[AnotherPictures] = dbo.EmptyStrToNull(i.AnotherPictures),
      countryoforigin=i.countryoforigin,
      isdrink=i.isdrink,
      [state]=i.[state],
      targetid=i.targetid
 from inserted	as i
 WHERE cap.capid=i.capid

	   



			 
END
GO
/****** Object:  Trigger [Cap_Instead_Ins]    Script Date: 09/20/2009 19:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[Cap_Instead_Ins] 
   ON  [dbo].[Cap] 
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.cap 
					 (	[CapTypeID]
      ,[MainTypeID]
      ,[ShapeID]
      ,[CapName]
      ,[MainText]
      ,[SubTitle]
      ,[BackColor1]
      ,[BackColor2]
      ,[ForeColor]
      ,[MainPicture]
      ,[TopText]
      ,[SideText]
      ,[BottomText]
      ,[MaterialID]
      ,[Surface]
      ,[Size]
      ,[Size2]
      ,[Height]
      ,[Is3D]
      ,[Year]
      ,[CountryCode]
      ,[DateCreated]
      ,[Note]
      ,[CompanyID]
      ,[ProductID]
      ,[ProductTypeID]
      ,[StorageID]
      ,[ForeColor2]
      ,[PictureType]
      ,[HasBottom]
      ,[HasSide]
      ,[AnotherPictures],Countryoforigin,isdrink,[state],targetid)
                   output INSERTED.*
			 SELECT [CapTypeID]
      ,[MainTypeID]
      ,[ShapeID]
      ,dbo.EmptyStrToNull([CapName])
      ,dbo.EmptyStrToNull([MainText])
      ,dbo.EmptyStrToNull([SubTitle])
      ,[BackColor1]
      ,[BackColor2]
      ,[ForeColor]
      ,dbo.EmptyStrToNull([MainPicture])
      ,dbo.EmptyStrToNull([TopText])
      ,dbo.EmptyStrToNull([SideText])
      ,dbo.EmptyStrToNull([BottomText])
      ,[MaterialID]
      ,[Surface]
      ,[Size]
      ,[Size2]
      ,[Height]
      ,[Is3D]
      ,[Year]
      ,dbo.EmptyStrToNull([CountryCode])
      ,[DateCreated]
      ,dbo.EmptyStrToNull([Note])
      ,[CompanyID]
      ,[ProductID]
      ,[ProductTypeID]
      ,[StorageID]
      ,[ForeColor2]
      ,[PictureType]
      ,[HasBottom]
      ,[HasSide]
      ,dbo.EmptyStrToNull([AnotherPictures]),
      dbo.EmptyStrToNull(Countryoforigin),isdrink,[state],targetid
  FROM inserted	 ;

--select * from dbo.cap where capid=scope_identity();


			 
END
GO
/****** Object:  Table [dbo].[Cap_Category_Int]    Script Date: 09/20/2009 19:34:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cap_Category_Int](
	[CapID] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
 CONSTRAINT [PK_Cap_Category_Int] PRIMARY KEY CLUSTERED 
(
	[CapID] ASC,
	[CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CapInstance]    Script Date: 09/20/2009 19:34:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CapInstance](
	[CapInstanceID] [int] IDENTITY(1,1) NOT NULL,
	[CapID] [int] NOT NULL,
	[StorageID] [int] NOT NULL,
	[State] [smallint] NOT NULL,
	[Year] [int] NULL,
	[CountryCode] [char](2) COLLATE Czech_CI_AS NULL,
	[DateCreated] [datetime] NOT NULL,
	[Note] [nvarchar](max) COLLATE Czech_CI_AS NULL,
 CONSTRAINT [PK_CapInstance] PRIMARY KEY CLUSTERED 
(
	[CapInstanceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Image]    Script Date: 09/20/2009 19:34:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Image](
	[ImageID] [int] IDENTITY(1,1) NOT NULL,
	[RelativePath] [nvarchar](256) COLLATE Czech_CI_AS NOT NULL,
	[CapID] [int] NOT NULL,
 CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED 
(
	[ImageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_Image_RelativePath] UNIQUE NONCLUSTERED 
(
	[RelativePath] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Trigger [image_Instead_Upd]    Script Date: 09/20/2009 19:35:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[image_Instead_Upd]
   ON  [dbo].[Image] 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].image	
   SET	
   relativepath=dbo.EmptyStrToNull(i.relativepath),
   
      --Description=dbo.EmptyStrToNull(i.description)
      capid=i.capid

 from inserted	as i
 WHERE image.imageid=i.imageid

					;



			 
END
GO
/****** Object:  Trigger [Image_Instead_Ins]    Script Date: 09/20/2009 19:35:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[Image_Instead_Ins] 
   ON  [dbo].[Image]
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.[image]
					 (	RelativePath,CapID)
           output inserted.*
			 SELECT dbo.EmptyStrToNull(RelativePath),CapID
  FROM inserted	 ;


  --   select * from dbo.image where imageid=scope_identity();
			 
END
GO
/****** Object:  StoredProcedure [dbo].[GetSimilarCaps]    Script Date: 09/20/2009 19:34:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Name
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[GetSimilarCaps] 
	-- Add the parameters for the stored procedure here
	@CapTypeID int = null, 
	@MainTypeID int = null,
	@ShapeID int = null,
	@CapName varchar(255) = null,
	@MainText varchar(255) = null,
	@SubTitle varchar(255) = null,
	@BackColor1 int = null,
	@BackColor2 int = null,
	@ForeColor int = null,
	@MainPicture varchar(255) = null,
	@TopText varchar(max)=null,
	@SideText varchar(max)=null,
	@BottomText varchar(max)=null,
	@MaterialID int =null,
	@Surface char(1) =null,
	@Size int = null,
	@Size2 int = null,
	@Height int = null,
	@Is3D bit = null,
	@Year int = null,
	@CountryCode char(2)= null,
	@Note varchar(max)= null,
	@CompanyID int= null,
	@ProductID int = null,
	@ProductTypeID int = null,
	@StorageID int = null,
	@ForeColor2 int = null,
	@PictureType char(1) = null,
	@HasBottom bit =  null,
	@HasSide bit = null,
	@AnotherPictures varchar(max)= null,
	@CategoryIDs dbo.IntTable readonly,
	@Keywords dbo.VarCharTable readonly,
	@CountryOfOrigin char(3) = null,
	@IsDrink bit = null,
	@State smallint = null,
	@TargetID int = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT top 100 c.*,
			case when c.CapName = @CapName then 20 else 0 end
            +
            case when c.MainText = @MainText then 20 else 0 end 
            +
            case when c.SubTitle = @SubTitle then 20 else 0 end
            +
            case when c.CapTypeID = @CapTypeID then 5 else 0 end
            +
            case when c.MainTypeID = @MainTypeID then 2 else 0 end
            +
            case when c.ShapeID = @ShapeID then 2 else 0 end
            +
            case when c.BackColor1 = @BackColor1 then 4 else 0 end
            +
            case when c.BackColor2 = @BackColor2  then 5 else 0 end 
            +
            case when c.ForeColor = @ForeColor    then 5 else 0 end
            +
            case when c.ForeColor2 = @ForeColor   then 5 else 0 end
            +
            case when c.MainPicture = @MainPicture then 5 else 0 end
            +
            case when c.TopText = @TopText then 20 else 0 end
            +
            case when c.SideText = @SideText then 30 else 0 end
            +
            case when c.BottomText = @BottomText then 30 else 0 end
            +
            case when c.MaterialID = @MaterialID then 2 else 0 end
            +
            case when c.Surface = @Surface then 1 else 0 end
            +
            case when c.ShapeID = @ShapeID and c.Size=@Size then 1 else 0 end
            +
            case when c.ShapeID = @ShapeID and c.Size2=@Size2 then 1 else 0 end
            +
            case when c.Height = @Height then 1 else 0 end
            +
            case when c.Is3D = @Is3D and c.Is3D = 1 then 20 when c.Is3D=@Is3D then 1 else 0 end
            +
            case when c.Year = @Year then 1 else 0 end
            +
            case when c.CountryCode = @CountryCode then 1 else 0 end
            +
            case when c.Note = @Note then 2 else 0 end
            +
            case when c.CompanyID = @CompanyID then 5 else 0 end
            +
            case when c.ProductID = @ProductID then 5 else 0   end
            +
            case when c.ProductTypeID=@ProductTypeID then 2 else 0 end
            +
            case when c.StorageID=@StorageID then 1 else 0 end
            +
            case when c.PictureType=@PictureType then 1 else 0 end
            +
            case when c.HasBottom=@HasBottom and c.HasBottom = 1 then 5 when c.HasBottom=@HasBottom then 1 else 0 end
            +
            case when c.HasSide=@HasSide and c.HasSide = 1 then 4 when c.HasSide=@HasSide then 1 else 0 end
            +
            case when c.AnotherPictures = @AnotherPictures then 5 else 0 end
            +
            (select count(*) from dbo.cap_category_int cci where cci.capid=c.capid and cci.categoryid in(select value from @categoryids))*3
            +
            (select count(*) from dbo.cap_keyword_int cki inner join dbo.keyword k on(cki.keywordid=k.keywordid) where cki.CapID=c.CapID and  k.keyword in(select value from @keywords))*4
            +
            case when c.CountryOfOrigin = @CountryOfOrigin then 2 else 0 end
            +
            case when c.IsDrink = @IsDrink and @IsDrink=0 then 3 when c.IsDrink = @IsDrink then 1 else 0 end
            +
            case when c.State = @State then 1 else 0 end
            +
            case when c.TargetID = @TargetID then 2 else 0 end
            as Score
		from dbo.Cap c
        order by
            Score desc
            ;
    
END
GO
/****** Object:  Trigger [CapInstance_Instead_Upd]    Script Date: 09/20/2009 19:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create TRIGGER [dbo].[CapInstance_Instead_Upd]
   ON  [dbo].[CapInstance]
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 update dbo.CapInstance
					 
         set
			 capid=i.capid,
			 storageid=i.storageid,
			 state=i.state,year=i.year,
			 countrycode=dbo.EmptyStrToNull(i.countrycode),
			 note=dbo.EmptyStrToNull(i.note)
  FROM inserted	i ;

   --  select * from dbo.producttype where producttypeid=scope_identity();

			 
END
GO
/****** Object:  Trigger [CapInstance_Instead_Ins]    Script Date: 09/20/2009 19:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create TRIGGER [dbo].[CapInstance_Instead_Ins] 
   ON  [dbo].[CapInstance]
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.CapInstance
					 (	 CapID,Storageid,state,year,countrycode,datecreated,note)
         output inserted.*
			 SELECT capid,storageid,state,year,dbo.EmptyStrToNull(countrycode),isnull(datecreated,getdate()),dbo.EmptyStrToNull(note)
  FROM inserted	 ;

   --  select * from dbo.producttype where producttypeid=scope_identity();

			 
END
GO
/****** Object:  Trigger [Cap_Keyword_Int_AF_DEL_UPD]    Script Date: 09/20/2009 19:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Name
-- Create date: 
-- Description:	
-- =============================================
CREATE TRIGGER [dbo].[Cap_Keyword_Int_AF_DEL_UPD] 
   ON  [dbo].[Cap_Keyword_Int] 
   AFTER DELETE, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	delete from dbo.Keyword where (SELECT COUNT(*) FROM dbo.Cap_Keyword_Int CKI WHERE CKI.KeywordID=Keyword.KeywordID)=0;
END
GO
/****** Object:  Default [DF_Cap_DateCreated]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap] ADD  CONSTRAINT [DF_Cap_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_Cap_HasBottom]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap] ADD  CONSTRAINT [DF_Cap_HasBottom]  DEFAULT ((0)) FOR [HasBottom]
GO
/****** Object:  Default [DF_Cap_HasSide]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap] ADD  CONSTRAINT [DF_Cap_HasSide]  DEFAULT ((0)) FOR [HasSide]
GO
/****** Object:  Check [CHK_ISO 3166-1_Alpha-2]    Script Date: 09/20/2009 19:34:39 ******/
ALTER TABLE [dbo].[ISO 3166-1]  WITH CHECK ADD  CONSTRAINT [CHK_ISO 3166-1_Alpha-2] CHECK  (([Alpha-2] like '[A-Z][A-Z]'))
GO
ALTER TABLE [dbo].[ISO 3166-1] CHECK CONSTRAINT [CHK_ISO 3166-1_Alpha-2]
GO
/****** Object:  Check [CHK_ISO 3166-1_Alpha-3]    Script Date: 09/20/2009 19:34:39 ******/
ALTER TABLE [dbo].[ISO 3166-1]  WITH CHECK ADD  CONSTRAINT [CHK_ISO 3166-1_Alpha-3] CHECK  (([Alpha-3] like '[A-Z][A-Z][A-Z]'))
GO
ALTER TABLE [dbo].[ISO 3166-1] CHECK CONSTRAINT [CHK_ISO 3166-1_Alpha-3]
GO
/****** Object:  Check [CHK_Material_NoEmptyStrings]    Script Date: 09/20/2009 19:34:40 ******/
ALTER TABLE [dbo].[Material]  WITH CHECK ADD  CONSTRAINT [CHK_Material_NoEmptyStrings] CHECK  (([Name]<>'' AND [Description]<>''))
GO
ALTER TABLE [dbo].[Material] CHECK CONSTRAINT [CHK_Material_NoEmptyStrings]
GO
/****** Object:  Check [CHK_MainType_NoEmptyStrings]    Script Date: 09/20/2009 19:34:40 ******/
ALTER TABLE [dbo].[MainType]  WITH CHECK ADD  CONSTRAINT [CHK_MainType_NoEmptyStrings] CHECK  (([TypeName]<>'' AND [Description]<>''))
GO
ALTER TABLE [dbo].[MainType] CHECK CONSTRAINT [CHK_MainType_NoEmptyStrings]
GO
/****** Object:  Check [CHK_Keyword_NoEmptyStrings]    Script Date: 09/20/2009 19:34:40 ******/
ALTER TABLE [dbo].[Keyword]  WITH CHECK ADD  CONSTRAINT [CHK_Keyword_NoEmptyStrings] CHECK  (([Keyword]<>''))
GO
ALTER TABLE [dbo].[Keyword] CHECK CONSTRAINT [CHK_Keyword_NoEmptyStrings]
GO
/****** Object:  Check [CHK_Company_NoEmptyString]    Script Date: 09/20/2009 19:34:40 ******/
ALTER TABLE [dbo].[Company]  WITH CHECK ADD  CONSTRAINT [CHK_Company_NoEmptyString] CHECK  (([CompanyName]<>'' AND [Description]<>''))
GO
ALTER TABLE [dbo].[Company] CHECK CONSTRAINT [CHK_Company_NoEmptyString]
GO
/****** Object:  Check [CHK_Category_NoEmptyString]    Script Date: 09/20/2009 19:34:40 ******/
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [CHK_Category_NoEmptyString] CHECK  (([CategoryName]<>'' AND [Description]<>''))
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [CHK_Category_NoEmptyString]
GO
/****** Object:  Check [CHK_Shape_NoEmptyStrings]    Script Date: 09/20/2009 19:34:40 ******/
ALTER TABLE [dbo].[Shape]  WITH CHECK ADD  CONSTRAINT [CHK_Shape_NoEmptyStrings] CHECK  (([Name]<>'' AND [Size1Name]<>'' AND [Size2Name]<>'' AND [Description]<>''))
GO
ALTER TABLE [dbo].[Shape] CHECK CONSTRAINT [CHK_Shape_NoEmptyStrings]
GO
/****** Object:  Check [CHK_Shape_SizeName]    Script Date: 09/20/2009 19:34:40 ******/
ALTER TABLE [dbo].[Shape]  WITH CHECK ADD  CONSTRAINT [CHK_Shape_SizeName] CHECK  (([Size1Name]<>[Size2Name]))
GO
ALTER TABLE [dbo].[Shape] CHECK CONSTRAINT [CHK_Shape_SizeName]
GO
/****** Object:  Check [CHK_ProductType_NoEmptyStrings]    Script Date: 09/20/2009 19:34:40 ******/
ALTER TABLE [dbo].[ProductType]  WITH CHECK ADD  CONSTRAINT [CHK_ProductType_NoEmptyStrings] CHECK  (([ProductTypeName]<>'' AND [Description]<>''))
GO
ALTER TABLE [dbo].[ProductType] CHECK CONSTRAINT [CHK_ProductType_NoEmptyStrings]
GO
/****** Object:  Check [CHK_Target_NoEmptyStrings]    Script Date: 09/20/2009 19:34:40 ******/
ALTER TABLE [dbo].[Target]  WITH CHECK ADD  CONSTRAINT [CHK_Target_NoEmptyStrings] CHECK  (([Name]<>'' AND [Description]<>''))
GO
ALTER TABLE [dbo].[Target] CHECK CONSTRAINT [CHK_Target_NoEmptyStrings]
GO
/****** Object:  Check [CHK_StorageType_NoEmptyStrings]    Script Date: 09/20/2009 19:34:40 ******/
ALTER TABLE [dbo].[StorageType]  WITH CHECK ADD  CONSTRAINT [CHK_StorageType_NoEmptyStrings] CHECK  (([Name]<>'' AND [Description]<>''))
GO
ALTER TABLE [dbo].[StorageType] CHECK CONSTRAINT [CHK_StorageType_NoEmptyStrings]
GO
/****** Object:  Check [CHK_Storage_NoEmptyStrings]    Script Date: 09/20/2009 19:34:40 ******/
ALTER TABLE [dbo].[Storage]  WITH CHECK ADD  CONSTRAINT [CHK_Storage_NoEmptyStrings] CHECK  (([Description]<>'' AND [storagenumber]<>''))
GO
ALTER TABLE [dbo].[Storage] CHECK CONSTRAINT [CHK_Storage_NoEmptyStrings]
GO
/****** Object:  Check [CHK_Product_NoEmptyStrings]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [CHK_Product_NoEmptyStrings] CHECK  (([ProductName]<>'' AND [Description]<>''))
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [CHK_Product_NoEmptyStrings]
GO
/****** Object:  Check [CHK_CapType_Height]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[CapType]  WITH CHECK ADD  CONSTRAINT [CHK_CapType_Height] CHECK  (([Height]>=(0)))
GO
ALTER TABLE [dbo].[CapType] CHECK CONSTRAINT [CHK_CapType_Height]
GO
/****** Object:  Check [CHK_CapType_NoEmptyString]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[CapType]  WITH CHECK ADD  CONSTRAINT [CHK_CapType_NoEmptyString] CHECK  (([TypeName]<>'' AND [Description]<>''))
GO
ALTER TABLE [dbo].[CapType] CHECK CONSTRAINT [CHK_CapType_NoEmptyString]
GO
/****** Object:  Check [CHK_CapType_Size]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[CapType]  WITH CHECK ADD  CONSTRAINT [CHK_CapType_Size] CHECK  (([Size]>=(0)))
GO
ALTER TABLE [dbo].[CapType] CHECK CONSTRAINT [CHK_CapType_Size]
GO
/****** Object:  Check [CHK_CapType_Size2]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[CapType]  WITH CHECK ADD  CONSTRAINT [CHK_CapType_Size2] CHECK  (([Size2]>=(0)))
GO
ALTER TABLE [dbo].[CapType] CHECK CONSTRAINT [CHK_CapType_Size2]
GO
/****** Object:  Check [CK_Cap_CountryOfOrigin]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [CK_Cap_CountryOfOrigin] CHECK  (([CountryOfOrigin] like '[A-Z][A-Z]'))
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [CK_Cap_CountryOfOrigin]
GO
/****** Object:  Check [CHK_Cap_AnotherPictures_MainPicture]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [CHK_Cap_AnotherPictures_MainPicture] CHECK  ((case when [AnotherPictures] IS NOT NULL AND [MainPicture] IS NULL then (0) else (1) end<>(0)))
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [CHK_Cap_AnotherPictures_MainPicture]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AnotherPictures cannot be set when MainPicture is not set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'CONSTRAINT',@level2name=N'CHK_Cap_AnotherPictures_MainPicture'
GO
/****** Object:  Check [CHK_Cap_BottomText_HasBottom]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [CHK_Cap_BottomText_HasBottom] CHECK  ((case when [BottomText] IS NOT NULL AND [HasBottom]=(0) then (0) else (1) end<>(0)))
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [CHK_Cap_BottomText_HasBottom]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'HasBottom must be true when BottomText is set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'CONSTRAINT',@level2name=N'CHK_Cap_BottomText_HasBottom'
GO
/****** Object:  Check [CHK_Cap_CountryCode]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [CHK_Cap_CountryCode] CHECK  (([CountryCode] like '[A-Z][A-Z]'))
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [CHK_Cap_CountryCode]
GO
/****** Object:  Check [CHK_Cap_ForeColor_ForeColor2]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [CHK_Cap_ForeColor_ForeColor2] CHECK  ((case when [ForeColor] IS NULL AND [ForeColor2] IS NOT NULL then (0) else (1) end<>(0)))
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [CHK_Cap_ForeColor_ForeColor2]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If ForeColor2 is set ForeColor must be set as well' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'CONSTRAINT',@level2name=N'CHK_Cap_ForeColor_ForeColor2'
GO
/****** Object:  Check [CHK_Cap_Height]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [CHK_Cap_Height] CHECK  (([Height]>=(0)))
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [CHK_Cap_Height]
GO
/****** Object:  Check [CHK_Cap_MainImage_PictureType]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [CHK_Cap_MainImage_PictureType] CHECK  ((case when [MainPicture] IS NULL AND [PictureType] IS NOT NULL then (0) when [MainPicture] IS NOT NULL AND [PictureType] IS NULL then (0) else (1) end<>(0)))
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [CHK_Cap_MainImage_PictureType]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MainImage & PictureType - both set or both NULL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'CONSTRAINT',@level2name=N'CHK_Cap_MainImage_PictureType'
GO
/****** Object:  Check [CHK_Cap_NoEmptyString]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [CHK_Cap_NoEmptyString] CHECK  (([CapName]<>'' AND [MainText]<>'' AND [SubTitle]<>'' AND [MainPicture]<>'' AND [toptext]<>'' AND [sidetext]<>'' AND [bottomtext]<>'' AND [countrycode]<>'' AND [note]<>'' AND [anotherpictures]<>''))
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [CHK_Cap_NoEmptyString]
GO
/****** Object:  Check [CHK_Cap_PictureType]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [CHK_Cap_PictureType] CHECK  (([PictureType]='P' OR [PictureType]='D' OR [PictureType]='L' OR [PictureType]='G'))
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [CHK_Cap_PictureType]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'CONSTRAINT',@level2name=N'CHK_Cap_PictureType'
GO
/****** Object:  Check [CHK_Cap_Sate]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [CHK_Cap_Sate] CHECK  (([STATE]=(5) OR [STATE]=(4) OR [STATE]=(3) OR [STATE]=(2) OR [STATE]=(1)))
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [CHK_Cap_Sate]
GO
/****** Object:  Check [CHK_Cap_SideText_HasSide]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [CHK_Cap_SideText_HasSide] CHECK  ((case when [SideText] IS NOT NULL AND [HasSide]=(0) then (0) else (1) end<>(0)))
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [CHK_Cap_SideText_HasSide]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'HasSite must be true when SideText is set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cap', @level2type=N'CONSTRAINT',@level2name=N'CHK_Cap_SideText_HasSide'
GO
/****** Object:  Check [CHK_Cap_Size]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [CHK_Cap_Size] CHECK  (([Size]>=(0)))
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [CHK_Cap_Size]
GO
/****** Object:  Check [CHK_Cap_Size2]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [CHK_Cap_Size2] CHECK  (([Size2]>=(0)))
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [CHK_Cap_Size2]
GO
/****** Object:  Check [CHK_Cap_Surface]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [CHK_Cap_Surface] CHECK  (([SURFACE]='G' OR [SURFACE]='M'))
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [CHK_Cap_Surface]
GO
/****** Object:  Check [CHK_Cap_Year]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [CHK_Cap_Year] CHECK  (([Year]>(0) AND [Year]<=(9999)))
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [CHK_Cap_Year]
GO
/****** Object:  Check [CHK_CapInstance_CountryCode]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[CapInstance]  WITH CHECK ADD  CONSTRAINT [CHK_CapInstance_CountryCode] CHECK  (([CountryCode] like '[A-Z][A-Z]'))
GO
ALTER TABLE [dbo].[CapInstance] CHECK CONSTRAINT [CHK_CapInstance_CountryCode]
GO
/****** Object:  Check [CHK_CapInstance_NoEmptyStrings]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[CapInstance]  WITH CHECK ADD  CONSTRAINT [CHK_CapInstance_NoEmptyStrings] CHECK  (([Note]<>''))
GO
ALTER TABLE [dbo].[CapInstance] CHECK CONSTRAINT [CHK_CapInstance_NoEmptyStrings]
GO
/****** Object:  Check [CHK_CapInstance_State]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[CapInstance]  WITH CHECK ADD  CONSTRAINT [CHK_CapInstance_State] CHECK  (([STATE]=(5) OR [STATE]=(4) OR [STATE]=(3) OR [STATE]=(2) OR [STATE]=(1)))
GO
ALTER TABLE [dbo].[CapInstance] CHECK CONSTRAINT [CHK_CapInstance_State]
GO
/****** Object:  Check [CHK_CapInstance_Year]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[CapInstance]  WITH CHECK ADD  CONSTRAINT [CHK_CapInstance_Year] CHECK  (([Year]>(0) AND [Year]<=(9999)))
GO
ALTER TABLE [dbo].[CapInstance] CHECK CONSTRAINT [CHK_CapInstance_Year]
GO
/****** Object:  Check [CHK_Image_NoEmptyStrings]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Image]  WITH CHECK ADD  CONSTRAINT [CHK_Image_NoEmptyStrings] CHECK  (([RelativePath]<>''))
GO
ALTER TABLE [dbo].[Image] CHECK CONSTRAINT [CHK_Image_NoEmptyStrings]
GO
/****** Object:  ForeignKey [FK_Storage_StorageType]    Script Date: 09/20/2009 19:34:40 ******/
ALTER TABLE [dbo].[Storage]  WITH CHECK ADD  CONSTRAINT [FK_Storage_StorageType] FOREIGN KEY([StorageTypeID])
REFERENCES [dbo].[StorageType] ([StorageTypeID])
GO
ALTER TABLE [dbo].[Storage] CHECK CONSTRAINT [FK_Storage_StorageType]
GO
/****** Object:  ForeignKey [FK_Product_Company]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Company] FOREIGN KEY([CompanyID])
REFERENCES [dbo].[Company] ([CompanyID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Company]
GO
/****** Object:  ForeignKey [FK_Product_ProductType]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductType] FOREIGN KEY([ProductTypeID])
REFERENCES [dbo].[ProductType] ([ProductTypeID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_ProductType]
GO
/****** Object:  ForeignKey [FK_CapType_MainType]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[CapType]  WITH CHECK ADD  CONSTRAINT [FK_CapType_MainType] FOREIGN KEY([MainTypeID])
REFERENCES [dbo].[MainType] ([MainTypeID])
GO
ALTER TABLE [dbo].[CapType] CHECK CONSTRAINT [FK_CapType_MainType]
GO
/****** Object:  ForeignKey [FK_CapType_Material]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[CapType]  WITH CHECK ADD  CONSTRAINT [FK_CapType_Material] FOREIGN KEY([MaterialID])
REFERENCES [dbo].[Material] ([MaterialID])
GO
ALTER TABLE [dbo].[CapType] CHECK CONSTRAINT [FK_CapType_Material]
GO
/****** Object:  ForeignKey [FK_CapType_Shape]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[CapType]  WITH CHECK ADD  CONSTRAINT [FK_CapType_Shape] FOREIGN KEY([ShapeID])
REFERENCES [dbo].[Shape] ([ShapeID])
GO
ALTER TABLE [dbo].[CapType] CHECK CONSTRAINT [FK_CapType_Shape]
GO
/****** Object:  ForeignKey [FK_CapType_Target]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[CapType]  WITH CHECK ADD  CONSTRAINT [FK_CapType_Target] FOREIGN KEY([TargetID])
REFERENCES [dbo].[Target] ([TargetID])
GO
ALTER TABLE [dbo].[CapType] CHECK CONSTRAINT [FK_CapType_Target]
GO
/****** Object:  ForeignKey [FK_Cap_CapType]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [FK_Cap_CapType] FOREIGN KEY([CapTypeID])
REFERENCES [dbo].[CapType] ([CapTypeID])
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_CapType]
GO
/****** Object:  ForeignKey [FK_Cap_Company]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [FK_Cap_Company] FOREIGN KEY([CompanyID])
REFERENCES [dbo].[Company] ([CompanyID])
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_Company]
GO
/****** Object:  ForeignKey [FK_Cap_MainType]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [FK_Cap_MainType] FOREIGN KEY([MainTypeID])
REFERENCES [dbo].[MainType] ([MainTypeID])
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_MainType]
GO
/****** Object:  ForeignKey [FK_Cap_Material]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [FK_Cap_Material] FOREIGN KEY([MaterialID])
REFERENCES [dbo].[Material] ([MaterialID])
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_Material]
GO
/****** Object:  ForeignKey [FK_Cap_Product]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [FK_Cap_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_Product]
GO
/****** Object:  ForeignKey [FK_Cap_ProductType]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [FK_Cap_ProductType] FOREIGN KEY([ProductTypeID])
REFERENCES [dbo].[ProductType] ([ProductTypeID])
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_ProductType]
GO
/****** Object:  ForeignKey [FK_Cap_Shape]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [FK_Cap_Shape] FOREIGN KEY([ShapeID])
REFERENCES [dbo].[Shape] ([ShapeID])
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_Shape]
GO
/****** Object:  ForeignKey [FK_Cap_Storage]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [FK_Cap_Storage] FOREIGN KEY([StorageID])
REFERENCES [dbo].[Storage] ([StorageID])
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_Storage]
GO
/****** Object:  ForeignKey [FK_Cap_Tagret]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [FK_Cap_Tagret] FOREIGN KEY([TargetID])
REFERENCES [dbo].[Target] ([TargetID])
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_Tagret]
GO
/****** Object:  ForeignKey [FK_CapInstance_Cap]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[CapInstance]  WITH CHECK ADD  CONSTRAINT [FK_CapInstance_Cap] FOREIGN KEY([CapID])
REFERENCES [dbo].[Cap] ([CapID])
GO
ALTER TABLE [dbo].[CapInstance] CHECK CONSTRAINT [FK_CapInstance_Cap]
GO
/****** Object:  ForeignKey [FK_CapInstance_Storage]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[CapInstance]  WITH CHECK ADD  CONSTRAINT [FK_CapInstance_Storage] FOREIGN KEY([StorageID])
REFERENCES [dbo].[Storage] ([StorageID])
GO
ALTER TABLE [dbo].[CapInstance] CHECK CONSTRAINT [FK_CapInstance_Storage]
GO
/****** Object:  ForeignKey [FK_Cap_Keyword_Int_Cap]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap_Keyword_Int]  WITH CHECK ADD  CONSTRAINT [FK_Cap_Keyword_Int_Cap] FOREIGN KEY([CapID])
REFERENCES [dbo].[Cap] ([CapID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Cap_Keyword_Int] CHECK CONSTRAINT [FK_Cap_Keyword_Int_Cap]
GO
/****** Object:  ForeignKey [FK_Cap_Keyword_Int_Keyword]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap_Keyword_Int]  WITH CHECK ADD  CONSTRAINT [FK_Cap_Keyword_Int_Keyword] FOREIGN KEY([KeywordID])
REFERENCES [dbo].[Keyword] ([KeywordID])
GO
ALTER TABLE [dbo].[Cap_Keyword_Int] CHECK CONSTRAINT [FK_Cap_Keyword_Int_Keyword]
GO
/****** Object:  ForeignKey [FK_Cap_Category_Int_Cap]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap_Category_Int]  WITH CHECK ADD  CONSTRAINT [FK_Cap_Category_Int_Cap] FOREIGN KEY([CapID])
REFERENCES [dbo].[Cap] ([CapID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Cap_Category_Int] CHECK CONSTRAINT [FK_Cap_Category_Int_Cap]
GO
/****** Object:  ForeignKey [FK_Cap_Category_Int_Category]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Cap_Category_Int]  WITH CHECK ADD  CONSTRAINT [FK_Cap_Category_Int_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([CategoryID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Cap_Category_Int] CHECK CONSTRAINT [FK_Cap_Category_Int_Category]
GO
/****** Object:  ForeignKey [FK_Image_Cap]    Script Date: 09/20/2009 19:34:41 ******/
ALTER TABLE [dbo].[Image]  WITH CHECK ADD  CONSTRAINT [FK_Image_Cap] FOREIGN KEY([CapID])
REFERENCES [dbo].[Cap] ([CapID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Image] CHECK CONSTRAINT [FK_Image_Cap]
GO
