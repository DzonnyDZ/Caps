CREATE VIEW [dbo].[CapEx]
AS
SELECT
	c.*,
	mt.TypeName AS [MainType.TypeName], mt.[Description] AS [MainType.Description],
	ct.TypeName AS [CapType.TypeName], ct.Height AS [CapType.Height], ct.Size AS [CapType.Size], ct.Size2 AS [CapType.Size2],
		ct.MainTypeID AS [CapType.MainTypeID], ct.MaterialID AS [CapType.MaterialID], ct.ShapeID AS [CapType.ShapeID],
		ct.TargetID AS [CapType.TargetID],
	[ct.mt].TypeName AS [CapType.MainType.TypeName], [ct.mt].[Description] AS [CapType.MainType.Description],
	[ct.s].Name AS [CapType.Shape.Name], [ct.s].Size1Name AS [CapType.Shape.Size1Name], [ct.s].Size2Name AS [CapType.Shape.Size2Name],
		[ct.s].[Description] AS [CapType.Shape.Description],
	[ct.m].Name AS [CapType.Material.Name], [ct.m].[Description] AS [CapType.Material.Description],
	[ct.t].Name AS [CapType.Target.Name], [ct.t].[Description] AS [CapType.Target.Description],
	s.Name AS [Shape.Name], s.Size1Name AS [Shape.Size1Name], s.Size2Name AS [Shape.Size2Name], s.[Description] AS [Shape.Description],
	m.Name AS [Material.Name], m.[Description] AS [Material.Description],
	t.Name AS [Target.Name], t.[Description] AS [Target.Description],
	cmp.CompanyName AS [Company.CompanyName], cmp.[Description] AS [Company.Description],
	p.ProductName AS [Product.Name], p.ProductTypeID AS [Product.ProductTypeID], p.[Description] AS [Product.Description],
	pt.ProductTypeName AS [ProductType.ProductTypeName], pt.IsAlcoholic AS [ProductType.IsAlcoholic], pt.IsDrink AS [ProductType.IsDrink],
		pt.[Description] AS [ProductType.Description],
	strg.StorageNumber AS [Storage.StorageNumber], strg.HasCaps AS [Storage.HasCaps], strg.StorageTypeID AS [Storage.StorageTypeID],
		strg.ParentStorageID AS [Storage.ParentStorage], strg.[Description] AS [Storage.Description],
	st.Name AS [Storage.StorageType.Name], st.[Description] AS [Storage.StorageType.Description],
	[strg.ps].StorageNumber AS [Storage.ParentStorage.StorageNumber], [strg.ps].HasCaps AS [Storage.ParentStorage.HasCaps],
		[strg.ps].StorageTypeID AS [Storage.ParentStorage.StorageTypeID], [strg.ps].ParentStorageID AS [Storage.ParentStorage.ParentStorage],
		[strg.ps].[Description] AS [Storage.ParentStorage.Description]	
	FROM dbo.Cap c
		INNER JOIN dbo.MainType mt ON(c.MainTypeID = mt.MainTypeID)
		LEFT OUTER JOIN dbo.CapType ct ON(c.CapTypeID = ct.CapTypeID)
			LEFT OUTER JOIN dbo.MainType [ct.mt] ON (ct.MainTypeID = [ct.mt].MainTypeID)
			LEFT OUTER JOIN dbo.Shape [ct.s] ON (ct.ShapeID = [ct.s].ShapeID)
			LEFT OUTER JOIN dbo.Material [ct.m] ON (ct.MaterialID = [ct.m].MaterialID)
			LEFT OUTER JOIN dbo.Target [ct.t] ON (ct.TargetID = [ct.t].TargetID)
		INNER JOIN dbo.Shape s ON(c.ShapeID = s.ShapeID)
		INNER JOIN dbo.Material m ON (c.MaterialID = m.MaterialID)
		LEFT OUTER JOIN dbo.[Target] t ON(c.TargetID = t.TargetID)
		
		LEFT OUTER JOIN dbo.Company cmp ON(c.CompanyID = cmp.CompanyID)
		LEFT OUTER JOIN dbo.Product p ON(p.ProductID = c.ProductID)
		LEFT OUTER JOIN dbo.ProductType pt ON(c.ProductTypeID = pt.ProductTypeID)
		
		INNER JOIN dbo.Storage strg ON (c.StorageID = strg.StorageID)
			INNER JOIN dbo.StorageType st ON(strg.StorageTypeID = st.StorageTypeID)
			LEFT OUTER JOIN dbo.Storage [strg.ps] ON(strg.ParentStorageID = [strg.ps].StorageID)