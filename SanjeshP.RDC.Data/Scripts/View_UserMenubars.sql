USE [SanjeshP_RDC1]
GO
/****** Object:  StoredProcedure [dbo].[Get_GroupAccess_With_CurrentUserAccess_For_TreeCheckBox]    Script Date: 4/23/2024 2:57:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[Get_GroupAccess_With_CurrentUserAccess_For_TreeCheckBox]
@CurrentUserId uniqueidentifier
,@GroupSelected_GroupId uniqueidentifier
as
Begin
	SELECT distinct(M.Id) AS DISTINCT_MenuID
	, M.Id
	, @CurrentUserId as UserId
	, M.Title
	, M.ParentId
	, M.PageCode
	, M.ShowMenu
	, M.CssClass
	, M.Icon
	, M.Link
	, M.ControllerName
	, M.ActionName
	-- لیست منو‌هایی که کاربر جاری به صورت فردی و گروهی به آن دسترسی دارد
	, CONVERT(bit,(SELECT CASE WHEN AM.IsDelete = 0 THEN 1 WHEN AM.IsDelete = 1 THEN 0 WHEN AM.IsDelete IS NULL THEN 0 END)) AS CurentUser_Person_Checkecd
	, CONVERT(bit,(SELECT CASE WHEN AMG_User.IsDelete = 0 THEN 1 WHEN AMG_User.IsDelete = 1 THEN 0 WHEN AMG_User.IsDelete IS NULL THEN 0 END)) AS CurentUser_Group_Checkecd
	, ISNULL((SELECT COUNT(Id) 
			FROM dbo.Menu AS MP
			WHERE (ParentId = M.Id)), 0) AS IsParent
	-- لیست منوهایی که گروه انتخاب شده به آن به دسترسی دراد
	, CONVERT(bit,0) as Person_Checkecd
	, CONVERT(bit,(SELECT CASE WHEN AMG_Group.IsDelete = 0 THEN 1 WHEN AMG_Group.IsDelete = 1 THEN 0 WHEN AMG_Group.IsDelete IS NULL THEN 0 END)) AS Group_Checkecd
	, CONVERT(bit,(SELECT CASE WHEN AMG_Group.IsDelete = 0 THEN 1 WHEN AMG_Group.IsDelete = 1 THEN 0 WHEN AMG_Group.IsDelete IS NULL THEN 0 END)) AS [disabled]
	FROM dbo.Menu AS M 
	LEFT OUTER JOIN dbo.AccessMenus AS AM ON AM.ListMenuId=M.Id and AM.UserId=@CurrentUserId
	LEFT OUTER JOIN dbo.AccessMenusGroup AS AMG_User ON AMG_User.ListMenuId=M.Id and AMG_User.GroupId in (select UG.GroupId 
																							from UserGroup as UG
																							where UG.UserId=@CurrentUserId 
																							and UG.GroupId=@GroupSelected_GroupId
																							and UG.IsActive=1 
																							and UG.IsDelete=0)
	LEFT OUTER JOIN dbo.AccessMenusGroup AS AMG_Group ON AMG_Group.ListMenuId=M.Id and AMG_Group.GroupId=@GroupSelected_GroupId
	WHERE (M.IsDelete = 0) AND (M.IsActive = 1)
	order by PageCode,ControllerName
--End
End
GO
/****** Object:  StoredProcedure [dbo].[Get_UserCurrentAccess]    Script Date: 4/23/2024 2:57:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[Get_UserCurrentAccess]
@CurrentUserId uniqueidentifier
as
Begin
	SELECT distinct(M.Id) AS DISTINCT_MenuID
	, M.Id
	, @CurrentUserId as UserId
	, M.Title
	, M.ParentId
	, M.PageCode
	, M.ShowMenu
	, M.CssClass
	, M.Icon
	, M.Link
	, M.ControllerName
	, M.ActionName
	, CONVERT(bit,0) AS CurentUser_Person_Checkecd
	, CONVERT(bit,0) AS CurentUser_Group_Checkecd
	, CONVERT(bit,(SELECT CASE WHEN AM.IsDelete = 0 THEN 1 WHEN AM.IsDelete = 1 THEN 0 WHEN AM.IsDelete IS NULL THEN 0 END)) AS Person_Checkecd
	, CONVERT(bit,(SELECT CASE WHEN AMG.IsDelete = 0 THEN 1 WHEN AMG.IsDelete = 1 THEN 0 WHEN AMG.IsDelete IS NULL THEN 0 END)) AS Group_Checkecd
	, CONVERT(bit,(SELECT CASE WHEN AMG.IsDelete = 0 THEN 1 WHEN AMG.IsDelete = 1 THEN 0 WHEN AMG.IsDelete IS NULL THEN 0 END)) AS [disabled]
	, ISNULL((SELECT COUNT(Id) 
				FROM dbo.Menu AS MP
				WHERE (ParentId = M.Id)), 0) AS IsParent
	FROM dbo.Menu AS M 
	LEFT OUTER JOIN dbo.AccessMenus AS AM ON AM.ListMenuId=M.Id and AM.UserId=@CurrentUserId
	LEFT OUTER JOIN dbo.AccessMenusGroup AS AMG ON AMG.ListMenuId=M.Id and AMG.GroupId in (select UG.GroupId 
																							from UserGroup as UG
																							where UG.UserId=@CurrentUserId and UG.IsActive=1 and UG.IsDelete=0) 
	WHERE (M.IsDelete = 0) AND (M.IsActive = 1)
	order by PageCode,ControllerName
End
GO
/****** Object:  StoredProcedure [dbo].[Get_UserSelectedAccess_With_CurrentUserAccess_For_TreeCheckBox]    Script Date: 4/23/2024 2:57:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[Get_UserSelectedAccess_With_CurrentUserAccess_For_TreeCheckBox]
@CurrentUserId uniqueidentifier
,@UserSelected_UserId uniqueidentifier
as
Begin
	SELECT distinct(M.Id) AS DISTINCT_MenuID
	, M.Id
	, @CurrentUserId as UserId
	, M.Title
	, M.ParentId
	, M.PageCode
	, M.ShowMenu
	, M.CssClass
	, M.Icon
	, M.Link
	, M.ControllerName
	, M.ActionName
	-- لیست منو‌هایی که کاربر جاری به صورت فردی و گروهی به آن دسترسی دارد
	, CONVERT(bit,(SELECT CASE WHEN AM_Current.IsDelete = 0 THEN 1 WHEN AM_Current.IsDelete = 1 THEN 0 WHEN AM_Current.IsDelete IS NULL THEN 0 END)) AS CurentUser_Person_Checkecd
	, CONVERT(bit,(SELECT CASE WHEN AMG_User.IsDelete = 0 THEN 1 WHEN AMG_User.IsDelete = 1 THEN 0 WHEN AMG_User.IsDelete IS NULL THEN 0 END)) AS CurentUser_Group_Checkecd
	, ISNULL((SELECT COUNT(Id) 
			FROM dbo.Menu AS MP
			WHERE (ParentId = M.Id)), 0) AS IsParent
	-- لیست منوهایی که کار انتخاب شده به آن به صورت فردی و گروهی دسترسی دراد
	, CONVERT(bit,(SELECT CASE WHEN AM_SelectedUser.IsDelete = 0 THEN 1 WHEN AM_SelectedUser.IsDelete = 1 THEN 0 WHEN AM_SelectedUser.IsDelete IS NULL THEN 0 END)) AS Person_Checkecd
	, CONVERT(bit,(SELECT CASE WHEN AMG_UserSelected_Group.IsDelete = 0 THEN 1 WHEN AMG_UserSelected_Group.IsDelete = 1 THEN 0 WHEN AMG_UserSelected_Group.IsDelete IS NULL THEN 0 END)) AS Group_Checkecd
	, CONVERT(bit,(SELECT CASE WHEN AMG_UserSelected_Group.IsDelete = 0 THEN 1 WHEN AMG_UserSelected_Group.IsDelete = 1 THEN 0 WHEN AMG_UserSelected_Group.IsDelete IS NULL THEN 0 END)) AS [disabled]

	FROM dbo.Menu AS M 
	LEFT OUTER JOIN dbo.AccessMenus AS AM_Current ON AM_Current.ListMenuId=M.Id and AM_Current.UserId=@CurrentUserId
	LEFT OUTER JOIN dbo.AccessMenusGroup AS AMG_User ON AMG_User.ListMenuId=M.Id and AMG_User.GroupId in (select UG.GroupId 
																							from UserGroup as UG
																							where UG.UserId=@CurrentUserId and UG.IsActive=1 and UG.IsDelete=0)
	
	LEFT OUTER JOIN dbo.AccessMenus AS AM_SelectedUser ON AM_SelectedUser.ListMenuId=M.Id and AM_SelectedUser.UserId=@UserSelected_UserId
	LEFT OUTER JOIN dbo.AccessMenusGroup AS AMG_UserSelected_Group ON AMG_UserSelected_Group.ListMenuId=M.Id and AMG_UserSelected_Group.GroupId in (select UG.GroupId 
																							from UserGroup as UG
																							where UG.UserId=@UserSelected_UserId and UG.IsActive=1 and UG.IsDelete=0)
	WHERE (M.IsDelete = 0) AND (M.IsActive = 1)
	order by PageCode,ControllerName
End
GO





--/****** Object:  View [dbo].[View_UserMenubar]    Script Date: 2/18/2025 6:23:07 PM ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO

--CREATE VIEW [dbo].[View_UserMenubars]
--AS
--SELECT    M.Id, M.Title, M.ParentId, M.PageCode, M.ShowMenu, M.CssClass, M.Icon, M.Link, M.ControllerName, M.ActionName, CONVERT(bit,
--                          (SELECT    CASE WHEN AM.IsDelete = 0 THEN 1 WHEN AM.IsDelete = 1 THEN 0 WHEN AM.IsDelete IS NULL THEN 0 END AS Expr1)) AS Person_Checkecd, CONVERT(bit,
--                          (SELECT    CASE WHEN AMG.IsDelete = 0 THEN 1 WHEN AMG.IsDelete = 1 THEN 0 WHEN AMG.IsDelete IS NULL THEN 0 END AS Expr1)) AS Group_Checkecd, AM.UserId, ISNULL
--                          ((SELECT    COUNT(Id) AS Expr1
--                              FROM         dbo.Menus AS MP
--                              WHERE     (ParentId = M.Id)), 0) AS IsParent
--FROM         dbo.Menus AS M LEFT OUTER JOIN
--                      dbo.AccessMenus AS AM ON AM.ListMenuId = M.Id LEFT OUTER JOIN
--                      dbo.AccessMenusGroups AS AMG ON AMG.ListMenuId = M.Id
--WHERE     (M.IsDelete = 0) AND (M.IsActive = 1)
--GO

