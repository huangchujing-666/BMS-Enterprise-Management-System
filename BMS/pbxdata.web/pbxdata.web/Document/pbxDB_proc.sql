/*
查询数据源更新日志
*/

CREATE PROCEDURE pro_SelectDataSourcesErrorLog --查询数据源更新日志
	-- Add the parameters for the stored procedure here
		@skipId int,--跳过多少行
	@takeId int,--去多少条数据
	@SourceCode nvarchar(50),--供应商编号
	@Def2 nvarchar(50),--查询类型
	@beginTime nvarchar(50),--查询开始时间
	@endTime nvarchar(50)--查询结束时间
AS
BEGIN
if(@Def2='1')
	begin
		if(@SourceCode='-1')
			begin
				if(@beginTime!='' and @endTime!='')
					begin
						select d.Id,d.ErrorMsg,d.errorSrc,d.errorMsgDetails,d.UserId,d.errorTime,d.operation,d.Def2,d.Def3,d.Def4,d.Def5,d.sourceName  from (select ROW_NUMBER() over( order by c.errorTime desc) as row_num, c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName 
						from errorlog as c join productsource as b on c.Def3=b.SourceCode left join users as a on a.UserId=c.UserId where errorTime>=@beginTime and errorTime <=@endTime) as d  where d.operation=3  and d.row_num between @skipId and @takeId
					end
				else if(@beginTime!='')
					begin
						select d.Id,d.ErrorMsg,d.errorSrc,d.errorMsgDetails,d.UserId,d.errorTime,d.operation,d.Def2,d.Def3,d.Def4,d.Def5,d.sourceName  from (select ROW_NUMBER() over( order by c.errorTime desc) as row_num, c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName 
						from errorlog as c join productsource as b on c.Def3=b.SourceCode left join users as a on a.UserId=c.UserId where errorTime>=@beginTime) as d  where d.operation=3  and d.row_num between @skipId and @takeId
					end
				else if(@endTime!='')
					begin
						select d.Id,d.ErrorMsg,d.errorSrc,d.errorMsgDetails,d.UserId,d.errorTime,d.operation,d.Def2,d.Def3,d.Def4,d.Def5,d.sourceName  from (select ROW_NUMBER() over( order by c.errorTime desc) as row_num, c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName 
						from errorlog as c join productsource as b on c.Def3=b.SourceCode left join users as a on a.UserId=c.UserId where errorTime <=@endTime) as d  where d.operation=3  and d.row_num between @skipId and @takeId
					end
				else
					begin
						select d.Id,d.ErrorMsg,d.errorSrc,d.errorMsgDetails,d.UserId,d.errorTime,d.operation,d.Def2,d.Def3,d.Def4,d.Def5,d.sourceName  from (select ROW_NUMBER() over( order by c.errorTime desc) as row_num, c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName 
						from errorlog as c join productsource as b on c.Def3=b.SourceCode left join users as a on a.UserId=c.UserId) as d  where d.operation=3  and d.row_num between @skipId and @takeId
					end
			end
		else
			begin
				if(@beginTime!='' and @endTime!='')
					begin
						select d.Id,d.ErrorMsg,d.errorSrc,d.errorMsgDetails,d.UserId,d.errorTime,d.operation,d.Def2,d.Def3,d.Def4,d.Def5,d.sourceName  from (select ROW_NUMBER() over( order by c.errorTime desc) as row_num, c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName 
						from errorlog as c join productsource as b on c.Def3=b.SourceCode left join users as a on a.UserId=c.UserId  where errorTime>=@beginTime and errorTime <=@endTime) as d  where d.operation=3 and d.Def3=@SourceCode and d.row_num between @skipId and @takeId
					end
				else if(@beginTime!='')
					begin
						select d.Id,d.ErrorMsg,d.errorSrc,d.errorMsgDetails,d.UserId,d.errorTime,d.operation,d.Def2,d.Def3,d.Def4,d.Def5,d.sourceName  from (select ROW_NUMBER() over( order by c.errorTime desc) as row_num, c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName 
						from errorlog as c join productsource as b on c.Def3=b.SourceCode left join users as a on a.UserId=c.UserId  where errorTime>=@beginTime ) as d  where d.operation=3 and d.Def3=@SourceCode and d.row_num between @skipId and @takeId
					end
				else if(@endTime!='')
					begin
						select d.Id,d.ErrorMsg,d.errorSrc,d.errorMsgDetails,d.UserId,d.errorTime,d.operation,d.Def2,d.Def3,d.Def4,d.Def5,d.sourceName  from (select ROW_NUMBER() over( order by c.errorTime desc) as row_num, c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName 
						from errorlog as c join productsource as b on c.Def3=b.SourceCode left join users as a on a.UserId=c.UserId  where  errorTime <=@endTime) as d  where d.operation=3 and d.Def3=@SourceCode and d.row_num between @skipId and @takeId
					end
				else
					begin
						select d.Id,d.ErrorMsg,d.errorSrc,d.errorMsgDetails,d.UserId,d.errorTime,d.operation,d.Def2,d.Def3,d.Def4,d.Def5,d.sourceName  from (select ROW_NUMBER() over( order by c.errorTime desc) as row_num, c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName 
						from errorlog as c join productsource as b on c.Def3=b.SourceCode left join users as a on a.UserId=c.UserId ) as d  where d.operation=3 and d.Def3=@SourceCode and d.row_num between @skipId and @takeId
					end
			end
	end
else if(@Def2='2')
	begin
	if(@SourceCode='-1')
			begin
				if(@beginTime!='' and @endTime!='')
					begin
						select d.Id,d.ErrorMsg,d.errorSrc,d.errorMsgDetails,d.UserId,d.errorTime,d.operation,d.Def2,d.Def3,d.Def4,d.Def5,d.sourceName  from (select ROW_NUMBER() over( order by c.errorTime desc) as row_num, c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName 
						from errorlog as c join productsource as b on c.Def3=b.SourceCode left join users as a on a.UserId=c.UserId where c.Def2!='0'  and errorTime>=@beginTime and errorTime <=@endTime) as d  where d.operation=3 and d.row_num between @skipId and @takeId
					end
				else if(@beginTime!='')
					begin
						select d.Id,d.ErrorMsg,d.errorSrc,d.errorMsgDetails,d.UserId,d.errorTime,d.operation,d.Def2,d.Def3,d.Def4,d.Def5,d.sourceName  from (select ROW_NUMBER() over( order by c.errorTime desc) as row_num, c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName 
						from errorlog as c join productsource as b on c.Def3=b.SourceCode left join users as a on a.UserId=c.UserId where c.Def2!='0'  and errorTime>=@beginTime) as d  where d.operation=3 and d.row_num between @skipId and @takeId
					end
				else if(@endTime!='')
					begin
						select d.Id,d.ErrorMsg,d.errorSrc,d.errorMsgDetails,d.UserId,d.errorTime,d.operation,d.Def2,d.Def3,d.Def4,d.Def5,d.sourceName  from (select ROW_NUMBER() over( order by c.errorTime desc) as row_num, c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName 
						from errorlog as c join productsource as b on c.Def3=b.SourceCode left join users as a on a.UserId=c.UserId where c.Def2!='0'  and errorTime <=@endTime) as d  where d.operation=3 and d.row_num between @skipId and @takeId
					end
				else 
					begin
						select d.Id,d.ErrorMsg,d.errorSrc,d.errorMsgDetails,d.UserId,d.errorTime,d.operation,d.Def2,d.Def3,d.Def4,d.Def5,d.sourceName  from (select ROW_NUMBER() over( order by c.errorTime desc) as row_num, c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName 
						from errorlog as c join productsource as b on c.Def3=b.SourceCode left join users as a on a.UserId=c.UserId where c.Def2!='0') as d  where d.operation=3 and d.row_num between @skipId and @takeId
					end
			end
		else
			begin
				if(@beginTime!='' and @endTime!='')
					begin
						select d.Id,d.ErrorMsg,d.errorSrc,d.errorMsgDetails,d.UserId,d.errorTime,d.operation,d.Def2,d.Def3,d.Def4,d.Def5,d.sourceName  from (select ROW_NUMBER() over( order by c.errorTime desc) as row_num, c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName 
						from errorlog as c join productsource as b on c.Def3=b.SourceCode left join users as a on a.UserId=c.UserId  where c.Def2!='0'  and errorTime>=@beginTime and errorTime <=@endTime) as d  where d.operation=3  and d.Def3=@SourceCode and d.row_num between @skipId and @takeId
					end
				else if(@beginTime!='')
					begin
						select d.Id,d.ErrorMsg,d.errorSrc,d.errorMsgDetails,d.UserId,d.errorTime,d.operation,d.Def2,d.Def3,d.Def4,d.Def5,d.sourceName  from (select ROW_NUMBER() over( order by c.errorTime desc) as row_num, c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName 
						from errorlog as c join productsource as b on c.Def3=b.SourceCode left join users as a on a.UserId=c.UserId  where c.Def2!='0'  and errorTime>=@beginTime) as d  where d.operation=3  and d.Def3=@SourceCode and d.row_num between @skipId and @takeId
					end
				else if(@endTime!='')
					begin
						select d.Id,d.ErrorMsg,d.errorSrc,d.errorMsgDetails,d.UserId,d.errorTime,d.operation,d.Def2,d.Def3,d.Def4,d.Def5,d.sourceName  from (select ROW_NUMBER() over( order by c.errorTime desc) as row_num, c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName 
						from errorlog as c join productsource as b on c.Def3=b.SourceCode left join users as a on a.UserId=c.UserId  where c.Def2!='0'  and  errorTime <=@endTime) as d  where d.operation=3  and d.Def3=@SourceCode and d.row_num between @skipId and @takeId
					end
				else
					begin
						select d.Id,d.ErrorMsg,d.errorSrc,d.errorMsgDetails,d.UserId,d.errorTime,d.operation,d.Def2,d.Def3,d.Def4,d.Def5,d.sourceName  from (select ROW_NUMBER() over( order by c.errorTime desc) as row_num, c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName 
						from errorlog as c join productsource as b on c.Def3=b.SourceCode left join users as a on a.UserId=c.UserId  where c.Def2!='0') as d  where d.operation=3  and d.Def3=@SourceCode and d.row_num between @skipId and @takeId
					end
			end
	end
END
GO
/*
查询主菜单和子菜单
*/
create proc menuSelect
--@menuName nvarchar(20)
as
begin
	select * from menu
end
go
--drop proc menuSelect


--查询主菜单列表（非导航条）
alter proc menuSelectFList
@roleId int
as
begin
	select * from menu where MenuLevel=0
end
go


--/*
--查询主菜单
--*/
alter proc menuSelectF
@roleId int
as
begin
	--select * from menu where MenuLevel=0
	select * from menu where Id in(
	select MenuLevel from menu where Id in(
	select MemuId from personapermisson as pp where personaId=@roleId
	)
	)order by MenuIndex 
end
go
--drop proc menuSelectF

/*
查询子菜单
*/
alter proc menuSelectC
@roleId int,
@menuid int
as
begin
	--select * from menu where MenuLevel=@menuid
	select * from menu where MenuLevel=@menuid and Id in(
	select MemuId from personapermisson as pp where personaId=@roleId
	) order by MenuIndex 
end
go


/*
查询所有子菜单
*/
alter proc menuSelectAllC
--@menuid int
as
begin
	select * from menu where MenuLevel<>0
end
go



/*
编辑菜单
*/
create proc menuEdit
@id int
as
begin
	select * from menu where Id=@id
end
go



/*
更新菜单
*/
create proc menuUpdate
@id int,
@menuName nvarchar(20)
      ,@MenuSrc nvarchar(500)
      ,@MenuLevel int
      ,@MenuIndex int
      ,@UserId int
as
begin
	update menu set menuName=@menuName,MenuSrc=@MenuSrc,MenuLevel=@MenuLevel,MenuIndex=@MenuIndex,UserId=@UserId  where Id=@id 
end
go




/*
添加菜单
*/
create proc menuAdd
@menuName nvarchar(20)
      ,@MenuSrc nvarchar(500)
      ,@MenuLevel int
      ,@MenuIndex int
      ,@UserId int
as
begin
	insert into menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values(@menuName,@MenuSrc,@MenuLevel,@MenuIndex,@UserId)
end
go





/*
查找用户
*/
create proc usersSelect
@userName nvarchar(20)
,@userPwd nvarchar(20)
as
begin
	select * from users where userName=@userName and userPwd = @userPwd
end
go




/*
查找用户
*/
alter proc usersRole
@roleId int
as
begin
	if @roleId<>0
		select * from users where personaId=@roleId 
	else
		select * from users
end
go





/*
获取用户访问淘宝权限
*/
  alter proc taoAppUsersSelect
  @userid1 int
  as
  begin
  select * from taoAppUser where userId1=@userid1
  end
  go



/*
根据淘宝昵称获取淘宝访问令牌和session令牌
*/
alter proc taoAppUsersNickSelect
@tbUserNick nvarchar(200)
as
begin
	select top 1 * from taoAppUser where refreshToken=@tbUserNick
end
go




/*
获取菜单功能是否存在访问权限
*/
alter proc funPermissonSelect
@MenuId int
as
begin
	--select * from funpermisson where MenuId in (select Id from menu where MenuLevel <> 0) order by MenuId
	select * from funpermisson where MenuId =@MenuId
end
go




/*
添加功能
*/
create proc funPermissonAdd
@funName nvarchar(20)
      ,@menuId int
      ,@funIndex int
      ,@UserId int
as
begin
	insert into funpermisson(FunName,MenuId,FunIndex,UserId) values(@funName,@menuId,@funIndex,@UserId)
end
go


/*
获取菜单功能是否存在访问权限
*/
create proc menuChild
--@MenuId int
as
begin
	select * from menu where MenuLevel <>0
end
go




/*
获取数据库表格名称
*/
create proc tableSelect
--@MenuId int
as
begin
	select * from tableFiledPerssion where tableLevel =0
end
go



/*
获取数据库表格字段
*/
create proc filedSelect
@tableLevel int
as
begin
	select * from tableFiledPerssion where tableLevel =@tableLevel
end
go



/*
显示字段
*/
create proc filedShow
@tableNameState nvarchar(200)
as
begin
	update tableFiledPerssion set tableNameState=1 where id in(@tableNameState)
end
go



/*
隐藏字段
*/
create proc filedHide
@tableNameState nvarchar(200)
as
begin
	update tableFiledPerssion set tableNameState=0 where id in(@tableNameState)
end
go



/*
角色获取
*/
create proc roleSelect
--@tableNameState nvarchar(200)
as
begin
	select * from persona
end
go


/*
角色编辑
*/


/*
角色添加
*/
create proc roleAdd
@PersonaName nvarchar(20),
@PersonaIndex int,
@UserId int
as
begin
	insert into persona(PersonaName,PersonaIndex,UserId) values(@PersonaName,@PersonaIndex,@UserId)
end
go




/*
角色更新
*/
create proc roleUpdate
@PersonaName nvarchar(20),
@PersonaIndex int,
@UserId int,
@Id int
as
begin
	update persona set PersonaName=@PersonaName,PersonaIndex=@PersonaIndex,UserId=@UserId where Id=@Id
end
go



/*
分配权限
*/


/*
功能编辑
*/
create proc funpermissonEdit
@id int
as
begin
	select * from funpermisson where Id=@id
end
go



/*
更新功能
*/
create proc funpermissonUpdate
@id int,
@MenuId nvarchar(20)
      ,@FunName nvarchar(500)
      ,@FunIndex int
      ,@UserId int
as
begin
	update funpermisson set MenuId=@MenuId,FunName=@FunName,FunIndex=@FunIndex,UserId=@UserId  where Id=@id 
end
go





/*
--------------------------------------------------------权限访问-------------------------------------------------------------
*/
/*
主菜单访问权限
*/
alter proc menuSelectFF
@roleId int
as
begin
	select * from menu where Id in(
	select MenuLevel from menu where Id in(
	select MemuId from personapermisson as pp where personaId=@roleId
	)
	)
end
go


/*
子菜单访问权限
*/
alter proc menuSelectCC
@roleId int,
@menuid  nvarchar(500)
as
begin
	exec('select * from menu where MenuLevel in ('+@menuid+') and Id in(
	select MemuId from personapermisson as pp where personaId='+@roleId+'
	)')
end
go


/*
功能访问权限
*/
alter proc funSelect
@roleId int,
@menuid nvarchar(500)
as
begin
	exec('select * from funpermisson where MenuId in(
	select MemuId from personapermisson as pp where personaId='+@roleId+' and MemuId in ('+@menuid+')
	)
	and Id in (select FunId from personapermisson as pp where personaId='+@roleId+' and MemuId in ('+@menuid+'))
	')
end
go

/*
字段访问权限
*/
alter proc tableFiledSelect
@roleId int,
@menuid nvarchar(500),
@funId nvarchar(500)
as
begin
	exec('select * from tableFiledPerssion where Id in(
	select FieldId from personapermisson as pp where personaId='+@roleId+' and MemuId in ('+@menuid+') and FunId in ('+@funId+')
	)')
end
go
/*
--------------------------------------------------------权限访问-------------------------------------------------------------
*/
--1,13,4

--select * from tableFiledPerssion where   Id in(
--	select FieldId from personapermisson as pp where personaId='1' and MemuId in ('13') and FunId in ('4')
--	)


/*
--------------------------------------------------------用户-------------------------------------------------------------
*/
--添加用户
create proc addUsers
@usersName nvarchar(20),
@usersPwd nvarchar(100),
@usersRealName nvarchar(20),
@usersSex int,
@usersPhone nvarchar(20),
@usersAddress nvarchar(200),
@usersEmail nvarchar(20),
@usersIndex int,
@usersManage int,
@usersRole int,
@usersId int
as
begin
insert into users(userName,userPwd,userRealName,usersex,userPhone,userAddress,userEmail,userIndex,userManage,personaId,userId) 
	values(@usersName,@usersPwd,@usersRealName,@usersSex,@usersPhone,@usersAddress,@usersEmail,@usersIndex,@usersManage,@usersRole,@usersId)
end


--修改用户
alter proc updateUsers
@id int,
@usersName nvarchar(20),
@usersPwd nvarchar(100),
@usersRealName nvarchar(20),
@usersSex int,
@usersPhone nvarchar(20),
@usersAddress nvarchar(200),
@usersEmail nvarchar(20),
@usersIndex int,
@usersManage int,
@personaId int,
@userId int,
@Def1 nvarchar(50),
@Def2 nvarchar(50),
@Def3 nvarchar(50),
@Def4 nvarchar(50),
@Def5 nvarchar(50)
as
begin
update users set userName=@usersName,userPwd=@usersPwd,userRealName=@usersRealName,usersex=@usersSex,userPhone=@usersPhone,userAddress=@usersAddress,userEmail=@usersEmail,userIndex=@usersIndex,userManage=@usersManage,personaId=@personaId,userId=@userId,Def1=@Def1,Def2=@Def2,Def3=@Def3,Def4=@Def4,Def5=@Def5 where id=@id
end

-------------------------------------------------商品模块存储过程----------------------------------------
--通过款号查询商品库存信息
ALTER proc [dbo].[SelectbyStyle]

@style nvarchar(16),--款号
@Cat nvarchar(5),--品牌
@Cat2 nvarchar(5),--类别
@MinPricea nvarchar(20),--最低价格
@MaxPricea nvarchar(20),--最大价格
@MinBalance nvarchar(20),--最低库存
@MaxBalance nvarchar(20),--最大库存
@Page nvarchar(20),--多少页
@PageCount nvarchar(20),--每页多少条
@sqlhead nvarchar(max),--拼接头
@sqlbody nvarchar(max),--拼接条件
@sqlrump  nvarchar(max),--拼接尾部
@sql varchar(max),--完整sql
@StyPic varchar(20),--是否存在图片
@CustomerId nvarchar(10),
@shopName nvarchar(100)
as
begin

set @sqlhead='select tmw.*,tbsc.stylePicSrc from (select ROW_NUMBER() over(order by tm.Balance desc,tb.style) 
as nid,tm.*,tb.Cat,tb.Cat2,tb.TypeName from (select style,Cat,Cat2,MAX(TypeName) 
as TypeName from product a 
left join producttype b on Cat2=TypeNo 
left join BrandConfigPersion e on a.Cat=e.BrandId 
left join PersonaTypeConfit f on a.Cat2=f.TypeId 
where (Style !='''' and Style is not null) and e.CustomerId='''+@CustomerId+''' and f.CustomerId='''+@CustomerId+''' 
group by style,Cat,Cat2) as tb right join (select * from(select style,MAX(Pricea) as Pricea,SUM(Balance) as Balance from product a 
left join BrandConfigPersion e on a.Cat=e.BrandId 
left join PersonaTypeConfit f on a.Cat2=f.TypeId 
where (Style !='''' and Style is not null) and e.CustomerId='''+@CustomerId+''' and f.CustomerId='''+@CustomerId+''''
set @sqlbody=''
if(@style<>'')
begin
set @sqlbody=@sqlbody+' and Style like ''%'+@style+'%'''
end
if(@Cat<>'')
begin
set @sqlbody=@sqlbody+' and Cat='''+@Cat+''''
end
if(@Cat2<>'')
begin
set @sqlbody=@sqlbody+' and Cat2='''+@Cat2+''''
end
if(@MinPricea<>'')
begin
 if(@MaxPricea<>'')
  begin
  set @sqlbody=@sqlbody+' and Pricea between '''+@MinPricea+''''
  set @sqlbody=@sqlbody+' and '''+@MaxPricea+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and Pricea>='''+@MinPricea+''''
  end
end
else
 begin
 if(@MaxPricea<>'')
 begin
 set @sqlbody=@sqlbody+' and Pricea<='''+@MaxPricea+''''
 end
end

if(@StyPic='1')
begin
set @sqlbody=@sqlbody+' and exists(select style from stylepic where Def4=''1'' and a.style=style group by style)'
end
if(@StyPic='0')
begin
set @sqlbody=@sqlbody+' and not exists(select style from stylepic where Def4=''1'' and a.style=style group by style)'
end

if(@shopName<>'')
 begin
 if(@shopName='0')
  begin
  set @sqlbody=@sqlbody+' and Style in(select ProductStyle from tbProductReMark) '
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Style in(select ProductStyle from tbProductReMark where ProductShopName='''+@shopName+''') '
  end
 end
 set @sqlbody=@sqlbody+' group by style) as tt2 where 1=1 '
 
 
 if(@MinBalance<>'')
begin
 if(@MaxBalance<>'')
  begin
  set @sqlbody=@sqlbody+' and Balance between '''+@MinBalance+'''' 
  set @sqlbody=@sqlbody+' and '''+@MaxBalance+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and Balance>='''+@MinBalance+''''
  end
end
else
 begin
 if(@MaxBalance<>'')
 begin
 set @sqlbody=@sqlbody+' and Balance<='''+@MaxBalance+''''
 end
 end

set @sqlrump=')tm on tb.style = tm.style) as  tmw left join styledescript as tbsc on tmw.Style=tbsc.style where nid>'+@Page
set @sqlrump=@sqlrump+' and nid <='+@PageCount+'  order by Balance desc '
set @sql=@sqlhead+@sqlbody+@sqlrump
print @sql
exec(@sql)
end



--查询数量 款号表
ALTER proc [dbo].[SelectbyStyleCount]

@style nvarchar(16),--款号
@Cat nvarchar(5),--品牌
@Cat2 nvarchar(5),--类别
@MinPricea nvarchar(20),--最低价格
@MaxPricea nvarchar(20),--最大价格
@MinBalance nvarchar(20),--最低库存
@MaxBalance nvarchar(20),--最大库存
@Page nvarchar(20),--多少页
@PageCount nvarchar(20),--每页多少条
@sqlhead nvarchar(max),--拼接头
@sqlbody nvarchar(max),--拼接条件
@sqlrump  nvarchar(max),--拼接尾部
@sql varchar(max),--完整sql
@StyPic varchar(20),--是否存在图片
@CustomerId nvarchar(10), --用户Id
@shopName nvarchar(100)
as
begin


set @sqlhead='select COUNT(*) from (select ROW_NUMBER() over(order by tm.Balance desc,tb.style) 
as nid,tm.*,tb.Cat,tb.Cat2,tb.TypeName from (select style,Cat,Cat2,MAX(TypeName) 
as TypeName from product a 
left join producttype b on Cat2=TypeNo 
left join BrandConfigPersion e on a.Cat=e.BrandId 
left join PersonaTypeConfit f on a.Cat2=f.TypeId 
where (Style !='''' and Style is not null) and e.CustomerId='''+@CustomerId+''' and f.CustomerId='''+@CustomerId+''' 
group by style,Cat,Cat2) as tb right join (select * from(select style,MAX(Pricea) as Pricea,SUM(Balance) as Balance from product a 
left join BrandConfigPersion e on a.Cat=e.BrandId 
left join PersonaTypeConfit f on a.Cat2=f.TypeId 
where (Style !='''' and Style is not null) and e.CustomerId='''+@CustomerId+''' and f.CustomerId='''+@CustomerId+''''
set @sqlbody=''
if(@style<>'')
begin
set @sqlbody=@sqlbody+' and Style like ''%'+@style+'%'''
end
if(@Cat<>'')
begin
set @sqlbody=@sqlbody+' and Cat='''+@Cat+''''
end
if(@Cat2<>'')
begin
set @sqlbody=@sqlbody+' and Cat2='''+@Cat2+''''
end
if(@MinPricea<>'')
begin
 if(@MaxPricea<>'')
  begin
  set @sqlbody=@sqlbody+' and Pricea between '''+@MinPricea+''''
  set @sqlbody=@sqlbody+' and '''+@MaxPricea+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and Pricea>='''+@MinPricea+''''
  end
end
else
 begin
 if(@MaxPricea<>'')
 begin
 set @sqlbody=@sqlbody+' and Pricea<='''+@MaxPricea+''''
 end
end

if(@StyPic='1')
begin
set @sqlbody=@sqlbody+' and exists(select style from stylepic where Def4=''1'' and a.style=style group by style)'
end
if(@StyPic='0')
begin
set @sqlbody=@sqlbody+' and not exists(select style from stylepic where Def4=''1'' and a.style=style group by style)'
end

if(@shopName<>'')
 begin
 if(@shopName='0')
  begin
  set @sqlbody=@sqlbody+' and Style in(select ProductStyle from tbProductReMark) '
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Style in(select ProductStyle from tbProductReMark where ProductShopName='''+@shopName+''') '
  end
 end
 set @sqlbody=@sqlbody+' group by style) as tt2 where 1=1 '
 
 
 if(@MinBalance<>'')
begin
 if(@MaxBalance<>'')
  begin
  set @sqlbody=@sqlbody+' and Balance between '''+@MinBalance+'''' 
  set @sqlbody=@sqlbody+' and '''+@MaxBalance+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and Balance>='''+@MinBalance+''''
  end
end
else
 begin
 if(@MaxBalance<>'')
 begin
 set @sqlbody=@sqlbody+' and Balance<='''+@MaxBalance+''''
 end
 end
 
set @sqlrump=')tm on tb.style = tm.style) as  tmw'


--set @sqlrump='group by style)tm on tb.style = tm.style) as tmw where nid>1 and nid <=10 order by Balance desc'

set @sql=@sqlhead+@sqlbody+@sqlrump
print @sql
exec(@sql)
end

--查询所有的库存数量和--款号表
ALTER proc [dbo].[SelectSumBalanceByStyle]

@style nvarchar(16),--款号
@Cat nvarchar(5),--品牌
@Cat2 nvarchar(5),--类别
@MinPricea nvarchar(20),--最低价格
@MaxPricea nvarchar(20),--最大价格
@MinBalance nvarchar(20),--最低库存
@MaxBalance nvarchar(20),--最大库存
@Page nvarchar(20),--多少页
@PageCount nvarchar(20),--每页多少条
@sqlhead nvarchar(max),--拼接头
@sqlbody nvarchar(max),--拼接条件
@sqlrump  nvarchar(max),--拼接尾部
@sql varchar(max),--完整sql
@StyPic varchar(20),--是否存在图片
@CustomerId nvarchar(10), --用户Id
@shopName nvarchar(100)
as
begin
set @sqlhead='select SUM(Balance) from (select ROW_NUMBER() over(order by tm.Balance desc,tb.style) 
as nid,tm.*,tb.Cat,tb.Cat2,tb.TypeName from (select style,Cat,Cat2,MAX(TypeName) 
as TypeName from product a 
left join producttype b on Cat2=TypeNo 
left join BrandConfigPersion e on a.Cat=e.BrandId 
left join PersonaTypeConfit f on a.Cat2=f.TypeId 
where (Style !='''' and Style is not null) and e.CustomerId='''+@CustomerId+''' and f.CustomerId='''+@CustomerId+''' 
group by style,Cat,Cat2) as tb right join (select * from(select style,MAX(Pricea) as Pricea,SUM(Balance) as Balance from product a 
left join BrandConfigPersion e on a.Cat=e.BrandId 
left join PersonaTypeConfit f on a.Cat2=f.TypeId 
where (Style !='''' and Style is not null) and e.CustomerId='''+@CustomerId+''' and f.CustomerId='''+@CustomerId+''''
set @sqlbody=''
if(@style<>'')
begin
set @sqlbody=@sqlbody+' and Style like ''%'+@style+'%'''
end
if(@Cat<>'')
begin
set @sqlbody=@sqlbody+' and Cat='''+@Cat+''''
end
if(@Cat2<>'')
begin
set @sqlbody=@sqlbody+' and Cat2='''+@Cat2+''''
end
if(@MinPricea<>'')
begin
 if(@MaxPricea<>'')
  begin
  set @sqlbody=@sqlbody+' and Pricea between '''+@MinPricea+''''
  set @sqlbody=@sqlbody+' and '''+@MaxPricea+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and Pricea>='''+@MinPricea+''''
  end
end
else
 begin
 if(@MaxPricea<>'')
 begin
 set @sqlbody=@sqlbody+' and Pricea<='''+@MaxPricea+''''
 end
end

if(@StyPic='1')
begin
set @sqlbody=@sqlbody+' and exists(select style from stylepic where Def4=''1'' and a.style=style group by style)'
end
if(@StyPic='0')
begin
set @sqlbody=@sqlbody+' and not exists(select style from stylepic where Def4=''1'' and a.style=style group by style)'
end

if(@shopName<>'')
 begin
 if(@shopName='0')
  begin
  set @sqlbody=@sqlbody+' and Style in(select ProductStyle from tbProductReMark) '
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Style in(select ProductStyle from tbProductReMark where ProductShopName='''+@shopName+''') '
  end
 end
 set @sqlbody=@sqlbody+' group by style) as tt2 where 1=1 '
 
 
 if(@MinBalance<>'')
begin
 if(@MaxBalance<>'')
  begin
  set @sqlbody=@sqlbody+' and Balance between '''+@MinBalance+'''' 
  set @sqlbody=@sqlbody+' and '''+@MaxBalance+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and Balance>='''+@MinBalance+''''
  end
end
else
 begin
 if(@MaxBalance<>'')
 begin
 set @sqlbody=@sqlbody+' and Balance<='''+@MaxBalance+''''
 end
 end
 
set @sqlrump=')tm on tb.style = tm.style) as tmw'


--set @sqlrump='group by style)tm on tb.style = tm.style) as tmw where nid>1 and nid <=10 order by Balance desc'

set @sql=@sqlhead+@sqlbody+@sqlrump
print @sql
exec(@sql)
end


--product表查询货号信息
ALTER proc [dbo].[SelectbyScode]

@style nvarchar(16),--款号
@Scode nvarchar(16),--货号
@Name nvarchar(16),--名字
@Cat nvarchar(5),--品牌
@Cat1 nvarchar(5),--季节
@Cat2 nvarchar(5),--类别
@MinPricea nvarchar(20),--最低价格
@MaxPricea nvarchar(20),--最大价格
@MinBalance nvarchar(20),--最低库存
@MaxBalance nvarchar(20),--最大库存
@MinLastgrnd nvarchar(20),--最小时间
@MaxLastgrnd nvarchar(20),--最大时间
@AttrState nvarchar(20),--属性状态
@PicState nvarchar(20),--图片状态
@Page nvarchar(20),--多少页
@PageCount nvarchar(20),--每页多少条
@sqlhead nvarchar(max),--拼接头
@sqlbody nvarchar(max),--拼接条件
@sqlrump  nvarchar(max),--拼接尾部
@sql varchar(max),--完整sql
@MinPictime nvarchar(20),--图片上传时间
@MaxPictime nvarchar(20),--图片上传时间
@CustomerId nvarchar(10), --用户Id
@UserId nvarchar(10),      --操作人
@Def9 nvarchar(20),      --图片是否合格
@Color nvarchar(20),      --颜色
@Def11 nvarchar(20),     --商检备案
@ciqProductId nvarchar(20)     --商品信息图

as
begin
set @sqlhead='select * from (select ROW_NUMBER() over(order by a.Balance desc) as 
nid,a.*,b.TypeName,d.userRealName from product as a 
left join producttype as b on a.Cat2=b.TypeNo 
left join (select * from scodepic where Id in(select min(Id) from scodepic where 
Def4=''1'' group by scode)) as c on a.Scode=c.scode 
left join users d on c.UserId=d.Id 
inner join (select BrandId from BrandConfigPersion where CustomerId='''+@CustomerId+''' group by BrandId) e on a.Cat=e.BrandId
inner join (select TypeId from PersonaTypeConfit where CustomerId='''+@CustomerId+''' group by TypeId) f on a.Cat2=f.TypeId
where (Style !='''' and Style is not null) '
set @sqlbody=''
if(@style<>'')
begin
set @sqlbody=@sqlbody+' and Style like ''%'+@style+'%'''
end

if(@Scode<>'')
begin
set @sqlbody=@sqlbody+' and a.Scode='''+@Scode+''''
end

if(@Name<>'')
begin
set @sqlbody=@sqlbody+' and Descript='''+@Name+''''
set  @sqlbody= @sqlbody+' or Cdescript='''+@Name+''''
end

if(@Cat<>'')
begin
set @sqlbody=@sqlbody+' and Cat='''+@Cat+''''
end

if(@Cat1<>'')
begin
set @sqlbody=@sqlbody+' and Cat1='''+@Cat1+''''
end

if(@Cat2<>'')
begin
set @sqlbody=@sqlbody+' and Cat2='''+@Cat2+''''
end

if(@Color<>'')
begin
set @sqlbody=@sqlbody+' and Clolor='''+@Color+''''
end

if(@MinBalance<>'')
begin
 if(@MaxBalance<>'')
  begin
  set @sqlbody=@sqlbody+' and Balance between '''+@MinBalance+'''' 
  set @sqlbody=@sqlbody+' and '''+@MaxBalance+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and Balance>='''+@MinBalance+''''
  end
end
else
 begin
 if(@MaxBalance<>'')
 begin
 set @sqlbody=@sqlbody+' and Balance<='''+@MaxBalance+''''
 end
 end
 
if(@MinPricea<>'')
begin
 if(@MaxPricea<>'')
  begin
  set @sqlbody=@sqlbody+' and Pricea between '''+@MinPricea+''''
  set @sqlbody=@sqlbody+' and '''+@MaxPricea+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and Pricea>='''+@MinPricea+''''
  end
end
else
 begin
 if(@MaxPricea<>'')
 begin
 set @sqlbody=@sqlbody+' and Pricea<='''+@MaxPricea+''''
 end
end

if(@MinLastgrnd<>'')
begin
 if(@MaxLastgrnd<>'')
  begin
  set @sqlbody=@sqlbody+' and Lastgrnd between '''+@MinLastgrnd+''''
  set @sqlbody=@sqlbody+' and '''+@MaxLastgrnd+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and Lastgrnd>='''+@MinLastgrnd+''''
  end
end
else
 begin
 if(@MaxLastgrnd<>'')
 begin
 set @sqlbody=@sqlbody+' and Lastgrnd<='''+@MaxLastgrnd+''''
 end

if(@MinPictime<>'')
begin
 if(@MaxPictime<>'')
  begin
  set @sqlbody=@sqlbody+' and a.Def1 between '''+@MinPictime+''''
  set @sqlbody=@sqlbody+' and '''+@MaxPictime+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and a.Def1>='''+@MinPictime+''''
  end
end
else
 begin
 if(@MaxPictime<>'')
 begin
 set @sqlbody=@sqlbody+' and a.Def1<='''+@MaxPictime+''''
 end
end

 
if(@PicState='1')
begin
set @sqlbody=@sqlbody+' and a.Imagefile !='''''
end
if(@PicState='0')
begin
set @sqlbody=@sqlbody+' and a.Imagefile ='''' or a.Imagefile is null'
end

if(@AttrState='1')
begin
set @sqlbody=@sqlbody+' and exists (select scode from productPorpertyValue where scode=a.scode group by scode)'
end
if(@AttrState='0')
begin
set @sqlbody=@sqlbody+' and not exists (select scode from productPorpertyValue where scode=a.scode group by scode)'
end
end

if(@UserId<>'')
begin
set @sqlbody=@sqlbody+' and c.UserId='''+@UserId+''''
end

if(@Def9<>'')
begin
set @sqlbody=@sqlbody+' and a.Def9='''+@Def9+''''
end

if(@Def11='0')
begin
set @sqlbody=@sqlbody+' and a.ciqSpec ='''' or a.ciqHSNo ='''' or a.ciqAssemCountry ='''' or a.ciqProductNo ='''' or a.QtyUnit ='''' or a.Def15 ='''' or a.Def16 ='''''
end

if(@Def11='1')
begin
set @sqlbody=@sqlbody+' and a.ciqSpec !='''' and a.ciqHSNo !='''' and a.ciqAssemCountry !='''' and a.ciqProductNo !='''' and a.QtyUnit !='''' and a.Def15 !='''' and a.Def16 !='''''
end

if(@Def11='2')
begin
set @sqlbody=@sqlbody+' and a.Def11 !='''''
end

if(@Def11='3')
begin
set @sqlbody=@sqlbody+' and a.Def11 ='''''
end

if(@ciqProductId!='')
begin
if(@ciqProductId='2')
begin
set @sqlbody=@sqlbody+' and a.ciqProductId='''' or  a.ciqProductId is null'
end
else
begin
set @sqlbody=@sqlbody+' and a.ciqProductId='''+@ciqProductId+''''
end
end

set @sqlrump=') as tb where nid>'+@Page
set @sqlrump=@sqlrump+' and nid<='+@PageCount

set @sql=@sqlhead+@sqlbody+@sqlrump
print @sql
exec(@sql)
end



--通过货号查询商品库存信息数量
ALTER proc [dbo].[SelectbyScodeCount]

@style nvarchar(16),--款号
@Scode nvarchar(16),--货号
@Name nvarchar(16),--名字
@Cat nvarchar(5),--品牌
@Cat1 nvarchar(5),--季节
@Cat2 nvarchar(5),--类别
@MinPricea nvarchar(20),--最低价格
@MaxPricea nvarchar(20),--最大价格
@MinBalance nvarchar(20),--最低库存
@MaxBalance nvarchar(20),--最大库存
@MinLastgrnd nvarchar(20),--最小时间
@MaxLastgrnd nvarchar(20),--最大时间
@AttrState nvarchar(20),--属性状态
@PicState nvarchar(20),--图片状态
@Page nvarchar(20),--多少页
@PageCount nvarchar(20),--每页多少条
@sqlhead nvarchar(max),--拼接头
@sqlbody nvarchar(max),--拼接条件
@sqlrump  nvarchar(max),--拼接尾部
@sql varchar(max),--完整sql
@MinPictime nvarchar(20),--图片上传时间
@MaxPictime nvarchar(20),--图片上传时间
@CustomerId nvarchar(10), --用户Id
@UserId nvarchar(10),      --操作人
@Def9 nvarchar(20),      --图片是否合格
@Color nvarchar(20),      --颜色
@Def11 nvarchar(20),     --商检备案
@ciqProductId nvarchar(20)     --商品信息图
as
begin
set @sqlhead='select COUNT(*) from product as a 
left join (select * from scodepic where Id in(select min(Id) from scodepic where Def4=''1'' group by scode)) as c on a.Scode=c.scode 
left join users d on c.UserId=d.Id 
inner join (select BrandId from BrandConfigPersion where CustomerId='''+@CustomerId+''' group by BrandId) e on a.Cat=e.BrandId
inner join (select TypeId from PersonaTypeConfit where CustomerId='''+@CustomerId+''' group by TypeId) f on a.Cat2=f.TypeId
where (Style !='''' and Style is not null) '
set @sqlbody=''
if(@style<>'')
begin
set @sqlbody=@sqlbody+' and Style like ''%'+@style+'%'''
end

if(@Scode<>'')
begin
set @sqlbody=@sqlbody+' and a.Scode='''+@Scode+''''
end

if(@Name<>'')
begin
set @sqlbody=@sqlbody+' and Descript='''+@Name+''''
set  @sqlbody= @sqlbody+' or Cdescript='''+@Name+''''
end

if(@Cat<>'')
begin
set @sqlbody=@sqlbody+' and Cat='''+@Cat+''''
end

if(@Cat1<>'')
begin
set @sqlbody=@sqlbody+' and Cat1='''+@Cat1+''''
end

if(@Cat2<>'')
begin
set @sqlbody=@sqlbody+' and Cat2='''+@Cat2+''''
end

if(@Color<>'')
begin
set @sqlbody=@sqlbody+' and Clolor='''+@Color+''''
end

if(@MinBalance<>'')
begin
 if(@MaxBalance<>'')
  begin
  set @sqlbody=@sqlbody+' and Balance between '''+@MinBalance+'''' 
  set @sqlbody=@sqlbody+' and '''+@MaxBalance+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and Balance>='''+@MinBalance+''''
  end
end
else
 begin
 if(@MaxBalance<>'')
 begin
 set @sqlbody=@sqlbody+' and Balance<='''+@MaxBalance+''''
 end
 end
 
if(@MinPricea<>'')
begin
 if(@MaxPricea<>'')
  begin
  set @sqlbody=@sqlbody+' and Pricea between '''+@MinPricea+''''
  set @sqlbody=@sqlbody+' and '''+@MaxPricea+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and Pricea>='''+@MinPricea+''''
  end
end
else
 begin
 if(@MaxPricea<>'')
 begin
 set @sqlbody=@sqlbody+' and Pricea<='''+@MaxPricea+''''
 end
end

if(@MinLastgrnd<>'')
begin
 if(@MaxLastgrnd<>'')
  begin
  set @sqlbody=@sqlbody+' and Lastgrnd between '''+@MinLastgrnd+''''
  set @sqlbody=@sqlbody+' and '''+@MaxLastgrnd+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and Lastgrnd>='''+@MinLastgrnd+''''
  end
end
else
 begin
 if(@MaxLastgrnd<>'')
 begin
 set @sqlbody=@sqlbody+' and Lastgrnd<='''+@MaxLastgrnd+''''
 end
 
 if(@MinPictime<>'')
begin
 if(@MaxPictime<>'')
  begin
  set @sqlbody=@sqlbody+' and a.Def1 between '''+@MinPictime+''''
  set @sqlbody=@sqlbody+' and '''+@MaxPictime+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and a.Def1>='''+@MinPictime+''''
  end
end
else
 begin
 if(@MaxPictime<>'')
 begin
 set @sqlbody=@sqlbody+' and a.Def1<='''+@MaxPictime+''''
 end
end
 
if(@PicState='1')
begin
set @sqlbody=@sqlbody+' and a.Imagefile !='''''
end
if(@PicState='0')
begin
set @sqlbody=@sqlbody+' and a.Imagefile ='''' or a.Imagefile is null'
end

if(@AttrState='1')
begin
set @sqlbody=@sqlbody+' and a.scode in (select scode from productPorpertyValue where scode=a.scode group by scode)'
end
if(@AttrState='0')
begin
set @sqlbody=@sqlbody+' and not exists (select scode from productPorpertyValue where scode=a.scode group by scode)'
end
end

if(@UserId<>'')
begin
set @sqlbody=@sqlbody+' and c.UserId='''+@UserId+''''
end

if(@Def9<>'')
begin
set @sqlbody=@sqlbody+' and a.Def9='''+@Def9+''''
end

if(@Def11='0')
begin
set @sqlbody=@sqlbody+' and a.ciqSpec ='''' or a.ciqHSNo ='''' or a.ciqAssemCountry ='''' or a.ciqProductNo ='''' or a.QtyUnit ='''' or a.Def15 ='''' or a.Def16 ='''''
end

if(@Def11='1')
begin
set @sqlbody=@sqlbody+' and a.ciqSpec !='''' and a.ciqHSNo !='''' and a.ciqAssemCountry !='''' and a.ciqProductNo !='''' and a.QtyUnit !='''' and a.Def15 !='''' and a.Def16 !='''''
end

if(@Def11='2')
begin
set @sqlbody=@sqlbody+' and a.Def11 !='''''
end

if(@Def11='3')
begin
set @sqlbody=@sqlbody+' and a.Def11 ='''''
end

if(@ciqProductId!='')
begin
if(@ciqProductId='2')
begin
set @sqlbody=@sqlbody+' and a.ciqProductId='''' or  a.ciqProductId is null'
end
else
begin
set @sqlbody=@sqlbody+' and a.ciqProductId='+@ciqProductId
end
end

--set @sqlrump=') as tb'

set @sql=@sqlhead+@sqlbody+@sqlrump
print @sql
exec(@sql)
end




--查询所有的库存数量和--货号表
ALTER proc [dbo].[SelectSumBalanceByScode]

@style nvarchar(16),--款号
@Scode nvarchar(16),--货号
@Name nvarchar(16),--名字
@Cat nvarchar(5),--品牌
@Cat1 nvarchar(5),--季节
@Cat2 nvarchar(5),--类别
@MinPricea nvarchar(20),--最低价格
@MaxPricea nvarchar(20),--最大价格
@MinBalance nvarchar(20),--最低库存
@MaxBalance nvarchar(20),--最大库存
@MinLastgrnd nvarchar(20),--最小时间
@MaxLastgrnd nvarchar(20),--最大时间
@AttrState nvarchar(20),--属性状态
@PicState nvarchar(20),--图片状态
@Page nvarchar(20),--多少页
@PageCount nvarchar(20),--每页多少条
@sqlhead nvarchar(max),--拼接头
@sqlbody nvarchar(max),--拼接条件
@sqlrump  nvarchar(max),--拼接尾部
@sql varchar(max),--完整sql
@MinPictime nvarchar(20),--图片上传时间
@MaxPictime nvarchar(20),--图片上传时间
@CustomerId nvarchar(10), --用户Id
@UserId nvarchar(10),      --操作人
@Def9 nvarchar(20),      --图片是否合格
@Color nvarchar(20),      --颜色
@Def11 nvarchar(20),     --商检备案
@ciqProductId nvarchar(20)     --商品信息图
as
begin

set @sqlhead='select SUM(Balance) from product as a 
left join (select * from scodepic where Id in(select min(Id) from scodepic where Def4=''1'' group by scode)) as c on a.Scode=c.scode 
left join users d on c.UserId=d.Id 
inner join (select BrandId from BrandConfigPersion where CustomerId='''+@CustomerId+''' group by BrandId) e on a.Cat=e.BrandId
inner join (select TypeId from PersonaTypeConfit where CustomerId='''+@CustomerId+''' group by TypeId) f on a.Cat2=f.TypeId
where (Style !='''' and Style is not null) '
set @sqlbody=''
if(@style<>'')
begin
set @sqlbody=@sqlbody+' and Style like ''%'+@style+'%'''
end

if(@Scode<>'')
begin
set @sqlbody=@sqlbody+' and a.Scode='''+@Scode+''''
end

if(@Name<>'')
begin
set @sqlbody=@sqlbody+' and Descript='''+@Name+''''
set  @sqlbody= @sqlbody+' or Cdescript='''+@Name+''''
end

if(@Cat<>'')
begin
set @sqlbody=@sqlbody+' and Cat='''+@Cat+''''
end

if(@Cat1<>'')
begin
set @sqlbody=@sqlbody+' and Cat1='''+@Cat1+''''
end

if(@Cat2<>'')
begin
set @sqlbody=@sqlbody+' and Cat2='''+@Cat2+''''
end

if(@Color<>'')
begin
set @sqlbody=@sqlbody+' and Clolor='''+@Color+''''
end

if(@MinBalance<>'')
begin
 if(@MaxBalance<>'')
  begin
  set @sqlbody=@sqlbody+' and Balance between '''+@MinBalance+'''' 
  set @sqlbody=@sqlbody+' and '''+@MaxBalance+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and Balance>='''+@MinBalance+''''
  end
end
else
 begin
 if(@MaxBalance<>'')
 begin
 set @sqlbody=@sqlbody+' and Balance<='''+@MaxBalance+''''
 end
 end
 
if(@MinPricea<>'')
begin
 if(@MaxPricea<>'')
  begin
  set @sqlbody=@sqlbody+' and Pricea between '''+@MinPricea+''''
  set @sqlbody=@sqlbody+' and '''+@MaxPricea+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and Pricea>='''+@MinPricea+''''
  end
end
else
 begin
 if(@MaxPricea<>'')
 begin
 set @sqlbody=@sqlbody+' and Pricea<='''+@MaxPricea+''''
 end
end

if(@MinLastgrnd<>'')
begin
 if(@MaxLastgrnd<>'')
  begin
  set @sqlbody=@sqlbody+' and Lastgrnd between '''+@MinLastgrnd+''''
  set @sqlbody=@sqlbody+' and '''+@MaxLastgrnd+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and Lastgrnd>='''+@MinLastgrnd+''''
  end
end
else
 begin
 if(@MaxLastgrnd<>'')
 begin
 set @sqlbody=@sqlbody+' and Lastgrnd<='''+@MaxLastgrnd+''''
 end
 
 if(@MinPictime<>'')
begin
 if(@MaxPictime<>'')
  begin
  set @sqlbody=@sqlbody+' and a.Def1 between '''+@MinPictime+''''
  set @sqlbody=@sqlbody+' and '''+@MaxPictime+''''
  end
 else
  begin
  set @sqlbody=@sqlbody+' and a.Def1>='''+@MinPictime+''''
  end
end
else
 begin
 if(@MaxPictime<>'')
 begin
 set @sqlbody=@sqlbody+' and a.Def1<='''+@MaxPictime+''''
 end
end
 
if(@PicState='1')
begin
set @sqlbody=@sqlbody+' and a.Imagefile !='''''
end
if(@PicState='0')
begin
set @sqlbody=@sqlbody+' and a.Imagefile ='''' or a.Imagefile is null'
end

if(@AttrState='1')
begin
set @sqlbody=@sqlbody+' and a.scode in (select scode from productPorpertyValue where scode=a.scode group by scode)'
end
if(@AttrState='0')
begin
set @sqlbody=@sqlbody+' and not exists (select scode from productPorpertyValue where scode=a.scode group by scode)'
end
end

if(@UserId<>'')
begin
set @sqlbody=@sqlbody+' and c.UserId='''+@UserId+''''
end

if(@Def9<>'')
begin
set @sqlbody=@sqlbody+' and a.Def9='''+@Def9+''''
end

if(@Def11='0')
begin
set @sqlbody=@sqlbody+' and a.ciqSpec ='''' or a.ciqHSNo ='''' or a.ciqAssemCountry ='''' or a.ciqProductNo ='''' or a.QtyUnit ='''' or a.Def15 ='''' or a.Def16 ='''''
end

if(@Def11='1')
begin
set @sqlbody=@sqlbody+' and a.ciqSpec !='''' and a.ciqHSNo !='''' and a.ciqAssemCountry !='''' and a.ciqProductNo !='''' and a.QtyUnit !='''' and a.Def15 !='''' and a.Def16 !='''''
end

if(@Def11='2')
begin
set @sqlbody=@sqlbody+' and a.Def11 !='''''
end

if(@Def11='3')
begin
set @sqlbody=@sqlbody+' and a.Def11 ='''''
end

if(@ciqProductId!='')
begin
if(@ciqProductId='2')
begin
set @sqlbody=@sqlbody+' and a.ciqProductId='''' or  a.ciqProductId is null'
end
else
begin
set @sqlbody=@sqlbody+' and a.ciqProductId='+@ciqProductId
end
end

--set @sqlrump=') as tb'

set @sql=@sqlhead+@sqlbody+@sqlrump
print @sql
exec(@sql)
end










--删除Product主货号
alter proc DeletebyScode
@Id int
as
begin
 delete from product where Id=@Id
end
go



--更新商品列表信息
ALTER proc [dbo].[UpdateProductinfo]
@Cdescript nvarchar(20),
@Cat1 nvarchar(20),
@Cat2 nvarchar(20),
@Rolevel nvarchar(20),
@Scode nvarchar(20),
@ciqProductId  nvarchar(200),
@ciqSpec nvarchar(200),
@ciqHSNo nvarchar(200),
@ciqAssemCountry nvarchar(200),
@Def9 nvarchar(200),
@Def10 nvarchar(200),
@sql nvarchar(max),
@Imagefile nvarchar(200),
@QtyUnit nvarchar(200),
@Def15 nvarchar(200),
@Def16 nvarchar(200)
as
begin
set @sql='update product 
set Cdescript='''+@Cdescript+''',Cat2='''+@Cat2+''',Cat1='''+@Cat1+''',Rolevel='''+@Rolevel+''',
ciqProductId ='''+@ciqProductId +''',ciqSpec='''+@ciqSpec+''',ciqHSNo='''+@ciqHSNo+''',
ciqAssemCountry='''+@ciqAssemCountry+''',Def9='''+@Def9+''',Def10='''+@Def10+''',Imagefile='''+@Imagefile+''',
QtyUnit='''+@QtyUnit+''',Def15='''+@Def15+''',Def16='''+@Def16+''' where Scode ='''+@Scode+''''
end
print @sql
exec (@sql)


--更新商品属性值
alter proc UpdateProductinfo
@Cdescript nvarchar(20),
@Cat2 nvarchar(20),
@Rolevel nvarchar(20),
@Scode nvarchar(20),
@sql nvarchar(200)
as
begin
set @sql='update product set Cdescript='''+@Cdescript+''',Cat2='''+@Cat2+''',Rolevel='''+@Rolevel+''' where Scode ='''+@Scode+''''
end
print @sql
exec (@sql)
go

--查询大类别表
ALTER proc [dbo].[Searchbigtype]
@Minnid nvarchar(20),
@Maxnid nvarchar(20),
@bigtypeName nvarchar(20),
@sql nvarchar(200)
as
begin
if(@bigtypeName<>'')
begin
set @sql='select * from(select ROW_NUMBER() over(order by bigtypeIndex) as nid,* from productbigtype where bigtypeName='+@bigtypeName+') as a where nid>'+@Minnid+' and nid<='+@Maxnid+''
end
else
begin
set @sql='select * from(select ROW_NUMBER() over(order by bigtypeIndex) as nid,* from productbigtype) as a where nid>'+@Minnid+' and nid<='+@Maxnid+''
end
end
exec(@sql)
print(@sql)



--查询适合的条数
alter proc bigtypeCount
@bigtypeName nvarchar(20)
as
begin
if(@bigtypeName<>'')
begin
exec('select COUNT(*) from productbigtype where bigtypeName='''+@bigtypeName+'''')
end
else
begin
exec('select COUNT(*) from productbigtype')
end
end
go

--插入属性值
alter proc InsertPropertyValue
@Cat2 nvarchar(20),
@PropertyId nvarchar(20),
@Scode nvarchar(20),
@PropertyValue nvarchar(20),
@PropertyIndex nvarchar(20),
@UserId nvarchar(20)
as
declare @TypeId varchar(20)
set @TypeId = (select Id as TypeId from producttype where TypeNo =@Cat2)
begin
exec('insert into productPorpertyValue (TypeId,PropertyId,Scode,PropertyValue,PorpertyIndex,UserId) values ('''+ @TypeId+''','''+@PropertyId+''','''+ @Scode+''','''+@PropertyValue+''','''+@PropertyIndex+''','''+ @UserId+''')')
end
go


--更新属性值
alter proc UpdateproductPorperty
@PropertyName nvarchar(20),
@Id nvarchar(20)
as
begin
exec('update productPorperty set PropertyName=''' + @PropertyName + ''' where Id=''' + @Id + '''')
end

--获得属性以及属性值
alter proc GetProductAttr
@Scode nvarchar(50),
@TypeNo nvarchar(20)
as
begin
exec('select gt.TypeName,Cat2,ga.PropertyName,gp.Id,gp.PropertyValue,ga.Id as PropertyId from productPorperty as ga 
inner join dbo.producttype as gt on ga.TypeId = gt.Id and gt.TypeNo = '''+@TypeNo+''' 
inner join product as cd on cd.Scode ='''+@Scode+''' 
left join productPorpertyValue as gp on gp.Scode = cd.Scode and ga.Id = gp.PropertyId')
end

--插入属性
alter proc InsertproductPorperty
@TypeId nvarchar(20),
@PropertyName nvarchar(20),
@PorpertyIndex nvarchar(20),
@UserId nvarchar(20)
as
begin
exec('insert into productPorperty values('''+@TypeId+''','''+@PropertyName+''','''+@PorpertyIndex+''','''+@UserId+''','','','','','')')
end

--删除属性
alter proc DeleteproductPorperty
@Id nvarchar(20)
as
begin
exec('delete from productPorperty where Id='''+@Id+'''')
end

--查询类别表
ALTER proc [dbo].[Selectproducttype]
@TypeName nvarchar(50), --类别名称
@bigtypeName nvarchar(50), --大类别名称
@MinNid nvarchar(10), --分页最小
@MaxNid nvarchar(10), --分页最大
@sql nvarchar(500)
as
begin
set @sql='select * from(select ROW_NUMBER() over( order by BigId) as nid,* from
(select a.*,b.bigtypeName,e.TBtypeName,d.TariffName from producttype a 
left join productbigtype b on a.BigId=b.Id 
left join TypeIdToTariffNo as c on a.TypeNo=c.TypeNo 
left join Tarifftab as d on c.TariffNo=d.TariffNo 
left join TBProducttype as e on a.Def1=e.cid
) tb where 1=1 '
if(@TypeName<>'')
begin
set @sql=@sql+'and tb.TypeName like ''%'+@TypeName+'%'' '
end
if(@bigtypeName<>'')
begin
set @sql=@sql+'and tb.bigtypeName like ''%'+@bigtypeName+'%'' '
end
set @sql=@sql+') as tt where nid>'+@MinNid+' and nid<='+@MaxNid+''
print(@sql)
exec(@sql)
end



--查询类别表数量
alter proc SelectproducttypeCount
@TypeName nvarchar(50), --类别名称
@bigtypeName nvarchar(50), --大类别名称
@sql nvarchar(500)
as
begin
set @sql='select COUNT(*) from(select ROW_NUMBER() over( order by BigId) as nid,* from(select a.*,b.bigtypeName from producttype a left join productbigtype b on a.BigId=b.Id) tb where 1=1 '
if(@TypeName<>'')
begin
set @sql=@sql+'and tb.TypeName like ''%'+@TypeName+'%'' '
end
if(@bigtypeName<>'')
begin
set @sql=@sql+'and tb.bigtypeName like ''%'+@bigtypeName+'%'' '
end
set @sql=@sql+') as tt '
print(@sql)
exec(@sql)
end

--插入类别对应税号
alter proc InsertInTypeIdToTariffNo
@TypeNo nvarchar(50),   --类别
@TariffNo nvarchar(50), --税号
@UserId nvarchar(50)    --操作人Id
as
begin
exec('insert into TypeIdToTariffNo (TypeNo,TariffNo,UserId) values ('''+@TypeNo+''','''+@TariffNo+''','''+@UserId+''')')
end

--更新类别对应税号
alter proc UpdateInTypeIdToTariffNo
@TypeNo nvarchar(50),  --类别
@TariffNo nvarchar(50) --税号
as
begin
exec('update TypeIdToTariffNo set TariffNo='''+@TariffNo+''' where TypeNo='''+@TypeNo+''' ')
end

--根据用户Id获取类别权限
ALTER proc [dbo].[GetTypeDDlist]
@CustomerId nvarchar(10),--用户Id
@s nvarchar(max),
@sql nvarchar(max)
as
begin
set @sql='select TypeName,TypeNo from producttype where TypeNo in(select typeid from PersonaTypeConfit where CustomerId='''+@CustomerId+''') order by TypeName'
end
PRINT @sql
exec (@sql)
--根据用户Id获取品牌权限
ALTER proc [dbo].[GetCatDDlist]
@CustomerId nvarchar(10),--用户Id
@s nvarchar(max),
@sql nvarchar(max)
as
begin
set @sql='select BrandName,BrandAbridge from brand where BrandAbridge in(select BrandId from BrandConfigPersion where CustomerId='''+@CustomerId+''') order by Left(BrandName,1)'
end
PRINT @sql
exec (@sql)

--根据类别获取属性
ALTER proc [dbo].[GetproductPorpertyDT]
@TypeNo nvarchar(20)
as
begin
exec('select a.*,c.cid from productPorperty as a 
left join producttype as b on a.TypeId=b.Id 
left join TBProducttype as c on b.Def1=c.cid where TypeNo='''+@TypeNo+'''')
end

--查询供应商类别表
create proc [dbo].[SelectProducttypeVen]
@TypeName nvarchar(50), --类别名称
@TypeNameVen nvarchar(50), --供应商类别名称
@Vencode nvarchar(50), --供应商
@MinNid nvarchar(10), --分页最小
@MaxNid nvarchar(10), --分页最大
@sql nvarchar(500)
as
begin
set @sql='select * from(select ROW_NUMBER() over( order by BigId) as nid,* from
(select a.*,b.bigtypeName from producttypeVen a 
left join productbigtype b on a.BigId=b.Id 
) tb where 1=1 '

if(@TypeName<>'')
begin
set @sql=@sql+'and tb.TypeName like ''%'+@TypeName+'%'' '
end

if(@TypeNameVen<>'')
begin
set @sql=@sql+'and tb.TypeNameVen like ''%'+@TypeNameVen +'%'' '
end

set @sql=@sql+') as tt where nid>'+@MinNid+' and nid<='+@MaxNid+''
print(@sql)
exec(@sql)
end

--查询供应商类别表数量
alter proc [dbo].[SelectproducttypeVenCount]
@TypeName nvarchar(50), --类别名称
@TypeNameVen nvarchar(50), --供应商类别名称
@Vencode nvarchar(50), --供应商
@sql nvarchar(500)
as
begin
set @sql='select COUNT(*) from(select ROW_NUMBER() over( order by BigId) as nid,* from(select a.*,b.bigtypeName from producttypeVen a left join productbigtype b on a.BigId=b.Id) tb where 1=1 '

if(@TypeName<>'')
begin
set @sql=@sql+'and tb.TypeName like ''%'+@TypeName+'%'' '
end

if(@TypeNameVen<>'')
begin
set @sql=@sql+'and tb.TypeNameVen like ''%'+@TypeNameVen+'%'' '
end

set @sql=@sql+') as tt '
print(@sql)
exec(@sql)
end


-----------------------------------------------------*库存管理 开始---------------------------------------------------------
alter proc StorAge1
@sql nvarchar(1000),
@scode nvarchar(500)
as
begin
set @sql='select Scode from product where Scode='''+@scode+''''
 exec(@sql)
end
exec StorAge1 '','003D000*4'
alter proc StorAge2--如果不存在则在库存表中查出此条数据
@sql nvarchar(1000),
@scode nvarchar(500)
as
begin
set @sql='select Balance,123 from productstock where Scode='''+@scode+''''
exec(@sql)
end
exec StorAge2 '','003D000*4'
alter proc StorAge3--修改当前数据库存
@sqlhead nvarchar(1000),
@sqlbody nvarchar(1000),
@balance nvarchar(50),
@scode nvarchar(100)
as
begin
set @sqlhead='update product set Balance='''+@balance+''''
set @sqlbody=' where scode='''+@scode+''''
set @sqlhead=@sqlhead+@sqlbody
exec(@sqlhead)
end
exec StorAge3 '','','10','003D000*4'

alter proc StorAge4--如果不存在商品表中就插入]
@sql nvarchar(1000),
@scode nvarchar(500)
as
begin
set @sql='insert into product(Scode,Bcode,Bcode2,Descript,Cdescript,Unit,Currency,Cat,Cat1,Cat2,Clolor,Size,Style,Model,Rolevel,Roamt,Stopsales,Balance,Lastgrnd,Imagefile) select Scode,Bcode,Bcode2,Descript,Cdescript,Unit,Currency,Cat,Cat1,Cat2,Clolor,Size,Style,Model,Rolevel,Roamt,Stopsales,Balance,Lastgrnd,Imagefile from productstock where scode='''+@scode+''''
exec(@sql)
end
exec StorAge4 '','d'
-------------------------------
alter proc SelectByPj
@style nvarchar(50),--款号
@priceMin nvarchar(13),--最低价格
@priceMax nvarchar(13),--最大价格
@cat nvarchar(50),--品牌
@BalanceMin int,--最小库存
@BalanceMax int,--最大库存
@Cat2 nvarchar(50),--类别
@sqlhead nvarchar(200),--拼接头
@sqlbody nvarchar(200),--拼接条件
@sqlrump nvarchar(200),--拼接尾部
@sql nvarchar(300)--完整sql
as
begin
set @sqlhead='(select scode from productstock where 1=1 '--拼接头部
set @sqlbody=''--拼接条件
if(@style<>'')--判断款号
   begin
   set @sqlbody=@sqlbody+'and Style='+@style
   end
if(@priceMin<>'')--判断最小价格
begin
  if(@priceMax<>'')--判断最大价格
  begin
  set @sqlbody=@sqlbody+'and Pricea between'+@priceMin+' and  '+@priceMax
  end
  else
  begin
  set @sqlbody=@sqlbody+'and Pricea>='+@priceMin
  end
end
  else
begin
  if(@priceMax<>'')
  begin
  set @sqlbody=@sqlbody+'and Pricea<='+@priceMax
  end
end
if(@cat<>'')--判断品牌
 begin
 set @sqlbody=@sqlbody+'and Cat='+@cat
 end
if(@BalanceMin<>'')--判断最小库存
begin
  if(@BalanceMax<>'')--判断最大库存
  begin
  set @sqlbody=@sqlbody+'and Balance between'+@BalanceMin+' and  '+@BalanceMax
  end
  else
  begin
  set @sqlbody=@sqlbody+'and Balance>='+@priceMin
  end
end
  else
begin
  if(@BalanceMax<>'')
  begin
  set @sql=@sql+'and Balance<='+@BalanceMax
  end
end
if(@Cat2<>'')--判断类别
  begin
  set @sql=@sql+'and Cat2<='+@Cat2
  end
set @sqlrump=@sqlhead+@sqlbody+')'--拼接尾部
set @sql='select count(Scode) as count from productstock where 1=1 '+@sqlbody+' and Scode not in'+@sqlrump
end
-----------------------------------------

alter proc ShortcutStorAge1--没有入库过的数据
@sql nvarchar(1000)
as
begin
set @sql='select * from productstock where scode not in(select Scode from product)'--没有入库过的数据
exec(@sql)
print(@sql)
end

alter proc ShortcutStorAge2--已经入库过的数据
@sql nvarchar(1000)
as
begin
set @sql='select Scode from productstock where scode  in(select Scode from product)'--没有入库过的数据
exec(@sql)
print(@sql)
end

alter proc ShortcutStorAge3--查出库存
@sql nvarchar(1000),
@scode nvarchar(500)
as
begin
set @sql='select Balance,pricea,priceb,priceb,pricec,priced,disca,discb,discc,discd,disce from productstock where Scode='''+@scode+''''--查询库存
exec(@sql)
print(@sql)
end
alter proc ShortcutStorAge4--查出库存
@sql nvarchar(1000),
@scode nvarchar(500),
@balance nvarchar(500),
@pricea nvarchar(50),
@priceb nvarchar(50),
@pricec nvarchar(50),
@priced nvarchar(50),
@pricee nvarchar(50),
@disca nvarchar(50),
@discb nvarchar(50),
@discc nvarchar(50),
@discd nvarchar(50),
@disce nvarchar(50)
as
begin
set @sql='update product set Balance='''+@balance+''''--修改已经入库过的数据
set @sql+=',pricea='+@pricea
set @sql+=',priceb='+@priceb
set @sql+=',pricec='+@pricec
set @sql+=',priced='+@priced
set @sql+=',pricee='+@pricee
set @sql+=',disca='+@disca
set @sql+=',discb='+@discb
set @sql+=',discc='+@discc
set @sql+=',discd='+@discd
set @sql+=',disce='+@disce
set @sql=@sql+' where Scode='''+@scode+''''
exec(@sql)
print(@sql)
end

exec ShortcutStorAge2 ''
exec ShortcutStorAge3 '','004450285003741'
exec ShortcutStorAge4 '','',''
delete product



alter proc NoProductInStock --存在于库存表中的货号
@sql nvarchar(1000)
as
begin
set @sql='select Scode from (select  * from productsourcestock as a right join (select Scode as Scode1,SUM(Balance) as Balances from productsourcestock group by Scode) as b on a.Scode=b.Scode1) as c where Scode in (select Scode from productstock)'
print(@sql)
exec(@sql)
end
exec NoProductInStock ''




alter proc NoProductInStock --存在于库存表中的货号
@sql nvarchar(1000)
as
begin
set @sql='select Scode from (select  * from productsourcestock as a right join (select Scode as Scode1,SUM(Balance) as Balances from productsourcestock group by Scode) as b on a.Scode=b.Scode1) as c where Scode in (select Scode from productstock)'
print(@sql)
exec(@sql)
end
exec NoProductInStock ''
alter proc SelectBalanceBySocde--查找出存在的数据的库存
@sql nvarchar(1000),
@scode nvarchar(50)
as
begin
set @sql='select Balance from productsourcestock where scode='''+@scode+''''
print(@sql)
exec(@sql)
end
exec SelectBalanceBySocde '','ddd'
alter proc UpdateStock--修改存在的数据
@sql nvarchar(1000),
@balance nvarchar(500),
@scode nvarchar(500),
@tsql nvarchar(500)
as
begin
set @sql='update productstock set Balance='''+@balance+''''
set @tsql=' where  Scode='''+@scode+''''
set @sql=@sql+@tsql
print(@sql)
exec(@sql)
end
alter proc IsNotInProductStock---不在库存表中的数据
@sql nvarchar(1000)
as
begin
set @sql='select * from productsourcestock where Scode not in(select Scode from productstock)'
exec(@sql)
end

--货号查询   条件拼接 
ALTER proc [dbo].[SelectPjByScodePage]
@style nvarchar(50),--款号
@scode nvarchar(50),--货号
@cat1 nvarchar(50),--季节
@priceMin nvarchar(13),--最低价格
@priceMax nvarchar(13),--最大价格
@cat nvarchar(50),--品牌
@BalanceMin nvarchar(50),--最小库存
@BalanceMax nvarchar(50),--最大库存
@Cat2 nvarchar(50),--类别
@LastgrndMin varchar(50),--最小时间
@LastgrndMax varchar(50),--最大时间
@sqlhead nvarchar(200),--拼接头
@sqlbody nvarchar(200),--拼接条件
@sqlrump nvarchar(200),--拼接尾部
@sql varchar(300),--完整sql
@minid nvarchar(50),
@maxid nvarchar(50),
@vencode nvarchar(50)
as
begin
--set @sqlhead='  (select top ('+@count+') scode from productstock where 1=1 '--拼接头部
set @sqlbody=''--拼接条件
if(@scode<>'')
begin
 set @sqlbody=@sqlbody+' and Scode='''+@scode+''''
end
if(@vencode<>'')
begin
set @sqlbody=@sqlbody+' and Vencode='''+@vencode+''''
end
if(@style<>'')--判断款号
   begin
   set @sqlbody=@sqlbody+'and Style LIKE ''%'+@style+'%'''
   end
if(@priceMin<>'')--判断最小价格
begin
  if(@priceMax<>'')--判断最大价格
  begin
  set @sqlbody=@sqlbody+' and Pricea between'''+@priceMin+''''
  set @sqlbody=@sqlbody+' and  '''+@priceMax+''''
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Pricea>='''+@priceMin+''''
  end
end
  else
begin
  if(@priceMax<>'')
  begin
  set @sqlbody=@sqlbody+' and Pricea<='''+@priceMax+''''
  end
end
if(@cat<>'')--判断品牌
 begin
 set @sqlbody=@sqlbody+' and Cat='''+@cat+''''
 end
if(@BalanceMin<>'')--判断最小库存
begin
  if(@BalanceMax<>'')--判断最大库存
  begin
  set @sqlbody=@sqlbody+' and Balance between '+@BalanceMin
  set @sqlbody=@sqlbody+' and  '+@BalanceMax
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Balance>='+@BalanceMin
  end
end
  else
begin
  if(@BalanceMax<>'')
  begin
  set @sqlbody=@sqlbody+' and Balance<='+@BalanceMax
  end
end
if(@Cat2<>'')--判断类别
  begin
  set @sqlbody=@sqlbody+' and Cat2='''+@Cat2+''''
  end
if(@cat1<>'')
  begin
  set @sqlbody=@sqlbody+' and Cat1='''+@cat1+''''
  end
if(@LastgrndMin<>'')--判断最小时间
begin
  if(@LastgrndMax<>'')--判断最大时间
  begin
  set @sqlbody=@sqlbody+' and Lastgrnd between'''+@LastgrndMin+''''
  set @sqlbody=@sqlbody+' and  '''+@LastgrndMax+''''
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Lastgrnd>='''+@LastgrndMin+''''
  end
end
  else
begin
  if(@LastgrndMax<>'')
  begin
  set @sqlbody=@sqlbody+' and Lastgrnd<='''+@LastgrndMax+''''
  end
end
set @sqlrump='as tt where nid>'+@minid
set @sqlrump+=' and nid<='+@maxid--拼接尾部
set @sql='select * from(select *,row_number() over(order by Balance desc) as nid from productstock where 1=1 '+@sqlbody+')'+@sqlrump
print(@sql)
exec(@sql)
end
--货号查询   条件拼接 
ALTER proc [dbo].[SelectPjByScode]
@style nvarchar(50),--款号
@scode nvarchar(50),--货号
@cat1 nvarchar(50),--季节
@priceMin nvarchar(13),--最低价格
@priceMax nvarchar(13),--最大价格
@cat nvarchar(50),--品牌
@BalanceMin nvarchar(50),--最小库存
@BalanceMax nvarchar(50),--最大库存
@Cat2 nvarchar(50),--类别
@LastgrndMin varchar(50),--最小时间
@LastgrndMax varchar(50),--最大时间
@sqlhead nvarchar(200),--拼接头
@sqlbody nvarchar(200),--拼接条件
@sqlrump nvarchar(200),--拼接尾部
@sql nvarchar(300),--完整sql
@vencode nvarchar(50)
as
begin
set @sqlhead=' (select scode from productstock where 1=1 '--拼接头部
set @sqlbody=''--拼接条件
if(@style<>'')--判断款号
   begin
   set @sqlbody=@sqlbody+'and Style LIKE ''%'+@style+'%'''
   end
if(@scode<>'')
  begin
  set @sqlbody=@sqlbody+'and Scode='''+@scode+''''
  end
if(@vencode<>'')
begin
set @sqlbody=@sqlbody+'and Vencode='''+@vencode+''''
end
if(@priceMin<>'')--判断最小价格
begin
  if(@priceMax<>'')--判断最大价格
  begin
  set @sqlbody=@sqlbody+' and Pricea between'''+@priceMin+'''' 
  set @sqlbody=@sqlbody+'and  '''+@priceMax+''''
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Pricea>='''+@priceMin+''''
  end
end
  else
begin
  if(@priceMax<>'')
  begin
  set @sqlbody=@sqlbody+' and Pricea<='''+@priceMax+''''
  end
end
if(@cat<>'')--判断品牌
 begin
 set @sqlbody=@sqlbody+' and Cat='''+@cat+''''
 end
if(@BalanceMin<>'')--判断最小库存
begin
  if(@BalanceMax<>'')--判断最大库存
  begin
  set @sqlbody=@sqlbody+' and Balance between '+@BalanceMin
  set @sqlbody=@sqlbody+' and  '+@BalanceMax
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Balance>='+@BalanceMin
  end
end
  else
begin
  if(@BalanceMax<>'')
  begin
  set @sqlbody=@sqlbody+' and Balance<='''+@BalanceMax+''''
  end
end
if(@Cat2<>'')--判断类别
  begin
  set @sqlbody=@sqlbody+' and Cat2='''+@Cat2+''''
  end
if(@cat1<>'')
  begin
  set @sqlbody=@sqlbody+' and Cat1='''+@cat1+''''
  end
if(@LastgrndMin<>'')--判断最小时间
begin
  if(@LastgrndMax<>'')--判断最大时间
  begin
  set @sqlbody=@sqlbody+' and Lastgrnd between'''+@LastgrndMin+''''
  set @sqlbody=@sqlbody+' and  '''+@LastgrndMax+''''
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Lastgrnd>='''+@LastgrndMin+''''
  end
end
  else
begin
  if(@LastgrndMax<>'')
  begin
  set @sqlbody=@sqlbody+' and Lastgrnd<='''+@LastgrndMax+''''
  end
end
set @sqlrump=@sqlhead+@sqlbody+')'--拼接尾部
set @sql='select count(Scode) as count from productstock where 1=1 '+@sqlbody+' and Scode  in'+@sqlrump
print(@sql)
exec(@sql)
end
---款号查询
ALTER proc [dbo].[SelectByPj]
alter proc SelectNoInsert--查询出不存在库存表的数据进行插入
@sql nvarchar(1000)
as
begin
set @sql='select * from productsourcestock where Scode not in(select Scode from productstock)'
print(@sql)
exec(@sql)
end
alter proc IsInProduct---需要修改的总个数
@sql nvarchar(1000)
as
begin
set @sql='select COUNT (*) as coun  from  productstock where Scode in(select Scode from product )'
exec(@sql)
end
----------------------------------
--款号查询   条件拼接    并分页
alter proc SelectByPjPage
@style nvarchar(50),--款号
@priceMin nvarchar(13),--最低价格
@priceMax nvarchar(13),--最大价格
@cat nvarchar(50),--品牌
@BalanceMin int,--最小库存
@BalanceMax int,--最大库存w
@Cat2 nvarchar(50),--类别
@sqlhead nvarchar(200),--拼接头
@sqlbody nvarchar(200),--拼接条件
@sqlrump nvarchar(200),--拼接尾部
@sql varchar(300),--完整sql
@count  varchar(50) --跳过多少条
as
begin
set @sqlhead=' (select top ('+@count+') scode from productstock where 1=1 '--拼接头部
set @sqlbody=''--拼接条件
if(@style<>'')--判断款号
   begin
   set @sqlbody=@sqlbody+'and Style='+@style
   end
if(@priceMin<>'')--判断最小价格
begin
  if(@priceMax<>'')--判断最大价格
  begin
  set @sqlbody=@sqlbody+' and Pricea between'+@priceMin+' and  '+@priceMax
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Pricea>='+@priceMin
  end
end
  else
begin
  if(@priceMax<>'')
  begin
  set @sqlbody=@sqlbody+' and Pricea<='+@priceMax
  end
end
if(@cat<>'')--判断品牌
 begin
 set @sqlbody=@sqlbody+' and Cat='''+@cat+''''
 end
if(@BalanceMin<>'')--判断最小库存
begin
  if(@BalanceMax<>'')--判断最大库存
  begin
  set @sqlbody=@sqlbody+' and Balance between'+@BalanceMin+' and  '+@BalanceMax
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Balance>='+@priceMin
  end
end
  else
begin
  if(@BalanceMax<>'')
  begin
  set @sql=@sql+' and Balance<='+@BalanceMax
  end
end
if(@Cat2<>'')--判断类别
  begin
  set @sql=@sql+' and Cat2<='+@Cat2
  end
set @sqlrump=@sqlhead+@sqlbody+')'--拼接尾部
set @sql='select top 5 Id,Style,Descript,Cat,Cat2,Clolor,Pricea,Balance,Vencode from productstock where 1=1 '+@sqlbody+' and Scode not in '+@sqlrump
print @sql
exec(@sql)
end
-------------------------------
--货号查询   条件拼接 
alter proc SelectPjByScode
@style nvarchar(50),--款号
@scode nvarchar(50),--货号
@cat1 nvarchar(50),--季节
@priceMin nvarchar(13),--最低价格
@priceMax nvarchar(13),--最大价格
@cat nvarchar(50),--品牌
@BalanceMin int,--最小库存
@BalanceMax int,--最大库存
@Cat2 nvarchar(50),--类别
@LastgrndMin varchar(50),--最小时间
@LastgrndMax varchar(50),--最大时间
@sqlhead nvarchar(200),--拼接头
@sqlbody nvarchar(200),--拼接条件
@sqlrump nvarchar(200),--拼接尾部
@sql varchar(300)--完整sql
as
begin
set @sqlhead=' (select scode from productstock where 1=1 '--拼接头部
set @sqlbody=''--拼接条件
if(@style<>'')--判断款号
   begin
   set @sqlbody=@sqlbody+'and Style='+@style
   end
if(@priceMin<>'')--判断最小价格
begin
  if(@priceMax<>'')--判断最大价格
  begin
  set @sqlbody=@sqlbody+' and Pricea between'+@priceMin+' and  '+@priceMax
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Pricea>='+@priceMin
  end
end
  else
begin
  if(@priceMax<>'')
  begin
  set @sqlbody=@sqlbody+' and Pricea<='+@priceMax
  end
end
if(@cat<>'')--判断品牌
 begin
 set @sqlbody=@sqlbody+' and Cat='''+@cat+''''
 end
if(@BalanceMin<>'')--判断最小库存
begin
  if(@BalanceMax<>'')--判断最大库存
  begin
  set @sqlbody=@sqlbody+' and Balance between'+@BalanceMin+' and  '+@BalanceMax
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Balance>='+@priceMin
  end
end
  else
begin
  if(@BalanceMax<>'')
  begin
  set @sql=@sql+' and Balance<='+@BalanceMax
  end
end
if(@Cat2<>'')--判断类别
  begin
  set @sql=@sql+' and Cat2<='+@Cat2
  end
if(@cat1<>'')
  begin
  set @sql=@sql+' and Cat1<='+@cat1
  end
if(@LastgrndMin<>'')--判断最小时间
begin
  if(@LastgrndMax<>'')--判断最大时间
  begin
  set @sqlbody=@sqlbody+' and Lastgrnd between'+@LastgrndMin+' and  '+@LastgrndMax
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Lastgrnd>='+@LastgrndMin
  end
end
  else
begin
  if(@LastgrndMax<>'')
  begin
  set @sql=@sql+' and Lastgrnd<='+@LastgrndMax
  end
end
set @sqlrump=@sqlhead+@sqlbody+')'--拼接尾部
set @sql='select count(Scode) as count from productstock where 1=1 '+@sqlbody+' and Scode  in'+@sqlrump
exec(@sql)
end
----------------------------------
alter proc b
@style nvarchar(50),--款号
@priceMin nvarchar(13),--最低价格
@priceMax nvarchar(13),--最大价格
@cat nvarchar(50),--品牌
@BalanceMin varchar(50),--最小库存
@BalanceMax varchar(50),--最大库存
@Cat2 nvarchar(50),--类别
@sqlhead nvarchar(1000),--拼接头
@sqlbody nvarchar(1000),--拼接条件
@sqlrump nvarchar(1000),--拼接尾部
@sql nvarchar(1000),--完整sql
@Vencode nvarchar(50)
as
begin
set @sqlbody=''--拼接条件
if(@style<>'')--判断款号
   begin
   set @sqlbody=@sqlbody+' and Style='''+@style+''''
   end
if(@priceMin<>'')--判断最小价格
begin
  if(@priceMax<>'')--判断最大价格
  begin
  set @sqlbody=@sqlbody+' and Pricea between '+@priceMin 
  set @sqlbody=@sqlbody+'and '+@priceMax
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Pricea>='+@priceMin
  end
end
  else
begin
  if(@priceMax<>'')
  begin
  set @sqlbody=@sqlbody+' and Pricea<='+@priceMax
  end
end
if(@cat<>'')--判断品牌
 begin
 set @sqlbody=@sqlbody+' and Cat='''+@cat+''''
 end
if(@Cat2<>'')--判断类别
 begin
  set @sqlbody=@sqlbody+' and Cat2='''+@Cat2+''''
 end
  if(@Vencode<>'')--判断数据源
 begin
 set @sqlbody=@sqlbody+' and Vencode='+@Vencode
 end
 set @sqlbody=@sqlbody+'group by style) as tt2 where 1=1 '
if(@BalanceMin<>'')--判断最小库存
begin
  if(@BalanceMax<>'')--判断最大库存
  begin
  set @sqlbody=@sqlbody+' and Balance between'''+@BalanceMin+''''
  set @sqlbody=@sqlbody+' and  '''+@BalanceMax+''''
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Balance>='''+@BalanceMin+''''
  end
end
  else
begin
  if(@BalanceMax<>'')
  begin
  set @sqlbody=@sqlbody+' and Balance<='''+@BalanceMax+''''
  end
end

set @sql='select count(*) as count from (select ROW_NUMBER() over(order by tm.Balance desc,tb.style) 
as nid,tm.*,tb.Cat,tb.Cat2,tb.TypeName from (select style,Cat,Cat2,MAX(TypeName) 
as TypeName from productstock a left join producttype b on Cat2=TypeNo where (Style !='''' and Style is not null)
group by style,Cat,Cat2) as tb right join (select * from(select style,MAX(Pricea) as Pricea,SUM(Balance) as Balance from 
productstock where (Style !='''' and Style is not null) '+@sqlbody+') as tm on  tb.Style=tm.Style) as tt'
print(@sql)
exec(@sql)
end
---款号查询
ALTER proc [dbo].[SelectByPjPage]
@style nvarchar(50),--款号
@priceMin nvarchar(13),--最低价格
@priceMax nvarchar(13),--最大价格
@cat nvarchar(50),--品牌
@BalanceMin nvarchar(50),--最小库存
@BalanceMax nvarchar(50),--最大库存w
@Cat2 nvarchar(50),--类别
@sqlhead nvarchar(1000),--拼接头
@sqlbody nvarchar(1000),--拼接条件
@sqlrump nvarchar(1000),--拼接尾部
@sql nvarchar(1000),--完整sql
@minid nvarchar(50),
@maxid nvarchar(50),
@Vencode nvarchar(50)--数据源
as
begin
--set @sqlhead=' (select top ('+@count+') scode from productstock where 1=1 '--拼接头部
set @sqlbody=''--拼接条件
if(@style<>'')--判断款号
   begin
   set @sqlbody=@sqlbody+' and Style='''+@style+''''
   end
if(@priceMin<>'')--判断最小价格
begin
  if(@priceMax<>'')--判断最大价格
  begin
  set @sqlbody=@sqlbody+' and Pricea between '+@priceMin
  set @sqlbody=@sqlbody+' and  '+@priceMax
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Pricea>='+@priceMin
  end
end
  else
begin
  if(@priceMax<>'')
  begin
  set @sqlbody=@sqlbody+' and Pricea<='+@priceMax
  end
end
if(@cat<>'')--判断品牌
 begin
 set @sqlbody=@sqlbody+' and Cat='''+@cat+''''
 end
 if(@Vencode<>'')--判断数据源
 begin
 set @sqlbody=@sqlbody+' and Vencode='+@Vencode
 end
 if(@Cat2<>'')--判断类别
  begin
  set @sqlbody=@sqlbody+' and Cat2='''+@Cat2+''''
  end
  set @sqlbody=@sqlbody+' group by style) as tt2 where 1=1'
if(@BalanceMin<>'')--判断最小库存
begin
  if(@BalanceMax<>'')--判断最大库存
  begin
  set @sqlbody=@sqlbody+' and Balance between '+@BalanceMin
  set @sqlbody=@sqlbody+' and '+@BalanceMax
  end
  else
  begin
  set @sqlbody=@sqlbody+' and Balance>='+@BalanceMin
  end
end
  else
begin
  if(@BalanceMax<>'')
  begin
  set @sqlbody=@sqlbody+' and Balance<='+@BalanceMax
  end
end

set @sqlrump='as tt where nid>'+@minid
set @sqlrump+=' and nid<='+@maxid--拼接尾部
--set @sql='select* from( select Id,Style,Descript,Cat,Cat2,Clolor,Pricea,Balance,Vencode,row_number() over(order by Balance desc) as nid from productstock where 1=1 '+@sqlbody+')'+@sqlrump
set @sql='select * from (select ROW_NUMBER() over(order by tm.Balance desc,tb.Style) 
as nid,tm.*,tb.Cat,tb.Cat2,tb.TypeName from (select Style,Cat,Cat2,MAX(TypeName) 
as TypeName from productstock a left join producttype b on Cat2=TypeNo where (Style !='''' and Style is not null)
group by Style,Cat,Cat2) as tb right join (select * from(select Style,MAX(Pricea) as Pricea,SUM(Balance) as Balance from 
productstock where (Style !='''' and Style is not null) '+@sqlbody+') as tm on  tb.Style=tm.Style)'+@sqlrump
print @sql
exec(@sql)
end
ALTER proc [dbo].[SelectToRowNb]   ---按照序列取出100条需要修改的数据(存在库存表且存在数据源表)
@sql nvarchar(1000),
@minRn nvarchar(50),
@maxRn nvarchar(50),
@tablename nvarchar(100)
as
begin
set @sql='select * from (select ROW_NUMBER () over(Order by scode) as rownb,* from '+@tablename+' where Scode in(select Scode from productstock)) as a where a.rownb between '''+@minRn+''''
set @sql+=' and '''+@maxRn+''''
print(@sql)
exec(@sql)
end
ALTER proc [dbo].[UpdateStock]--修改存在的数据
@sql nvarchar(1000),
@balance nvarchar(500),
@scode nvarchar(500),
@tsql nvarchar(500),
@pricea nvarchar(50),
@priceb nvarchar(50),
@pricec nvarchar(50),
@priced nvarchar(50),
@pricee nvarchar(50),
@Disca nvarchar(50),
@Discb nvarchar(50),
@Discc nvarchar(50),
@Discd nvarchar(50),
@Disce nvarchar(50)
as
begin
set @sql='update productstock set Balance='''+@balance+''''
set @sql+=',Pricea='+@pricea
set @sql+=',Priceb='+@priceb
set @sql+=',Pricec='+@pricec
set @sql+=',Priced='+@priced
set @sql+=',Pricee='+@pricee
set @sql+=',Disca='+@Disca
set @sql+=',Discb='+@Discb
set @sql+=',Discc='+@Discc
set @sql+=',Discd='+@Discd
set @sql+=',Disce='+@Disce
set @tsql+=' where  Scode='''+@scode+''''
set @sql=@sql+@tsql
print(@sql)
exec(@sql)
end
ALTER proc [dbo].[IsInSourceCount]---已经存在库存表的个数
@sql nvarchar(1000)
as
begin
set @sql='select count(scode) as count from productsourcestock where Scode in(select Scode from productstock)'
exec(@sql)
end
ALTER proc [dbo].[IsInProduct]---已经存在商品表个数
@sql nvarchar(1000)
as
begin
set @sql='select COUNT (*) as coun  from  productstock where Scode in(select Scode from product )'
exec(@sql)
end

-----------------------------------------------------*库存管理 结束---------------------------------------------------------

-----------------------------------------------------*店铺管理 开始---------------------------------------------------------
ALTER proc [dbo].[SelectAllShop]---显示所有店铺信息
@sql nvarchar(1000),
@minid nvarchar(50),
@maxid nvarchar(50),
@Condition nvarchar(500),
@shopName nvarchar(50),
@shopManagerId nvarchar(50),
@ShopTypeId nvarchar(50)
as
begin
if(@shopName<>'')
   begin
   set @Condition=' and shop.ShopName like ''%'+@shopName+'%'''
   end
if(@ShopTypeId<>0)
   begin
   set @Condition+=' and shop.ShoptypeId='+@ShopTypeId
   end
if(@shopManagerId<>0)
   begin
   set @Condition+=' and shop.ShopManageId='+@shopManagerId
   end
set @sql=' select * from (select ROW_NUMBER() OVER(order by shop.Id) as nid ,shop.Id, shop.ShopName,shoptype.ShoptypeName as ShoptypeId,convert(varchar(20),shop.ShopState) as ShopState,users.userRealName as ShopManageId,shop.ShopIdex,shop.UserId ,shop.Def1,shop.Def2,shop.Def3,shop.Def4,shop.Def5  from shop,shoptype,users where shop.ShoptypeId=shoptype.Id and shop.ShopManageId=users.Id '+@Condition+') as a where 1=1  and nid>'+@minid
set @sql+=' and nid<='+@maxid
print(@sql)
exec(@sql)
end
ALTER proc [dbo].[SelectAllShopCount]---统计所有店铺个数
@sql nvarchar(1000),
@Condition nvarchar(500),
@shopName nvarchar(50),
@shopManagerId nvarchar(50),
@ShopTypeId nvarchar(50)
as
begin
if(@shopName<>'')
   begin
   set @Condition=' and shop.ShopName like ''%'+@shopName+'%'''
   end
if(@ShopTypeId<>0)
   begin
   set @Condition+=' and shop.ShoptypeId='+@ShopTypeId
   end
if(@shopManagerId<>0)
   begin
   set @Condition+=' and shop.ShopManageId='+@shopManagerId
   end
set @sql=' select count(*) as count from (select ROW_NUMBER() OVER(order by shop.Id) as nid ,shop.Id, shop.ShopName,shoptype.ShoptypeName as ShoptypeId,convert(varchar(20),shop.ShopState) as ShopState,users.userRealName as ShopManageId,shop.ShopIdex,shop.UserId ,shop.Def1,shop.Def2,shop.Def3,shop.Def4,shop.Def5  from shop,shoptype,users where shop.ShoptypeId=shoptype.Id and shop.ShopManageId=users.Id '+@Condition+') as a '
exec(@sql)
end
ALTER proc [dbo].[SelectIsIdIn]--删除店铺前先确认店铺中是否有商品
@sql nvarchar(1000),
@shopId nvarchar(50)
as
begin
set @sql='select * from shopproduct where ShopId='''+@shopId+''''
exec(@sql)
end
ALTER proc [dbo].[DeleteShop]--删除店铺
@sql nvarchar(1000),
@shopId nvarchar(50)
as 
begin
set @sql='delete shop where Id='''+@shopId+''''
exec(@sql)
end

ALTER proc [dbo].[InsertShop]--添加店铺
@sql nvarchar(1000),
@shopname nvarchar(50),
@ShoptypeId nvarchar(50),
@shopstate nvarchar(50),
@shopmanageId nvarchar(50)
as
begin
set @sql= 'insert into shop(ShopName,ShoptypeId,ShopState,ShopManageId) values('''+@shopname+''','''+@ShoptypeId+''','''+@shopstate+''','''+@shopmanageId+''')'
print(@sql)
exec(@sql)
end
ALTER proc [dbo].[ShopNameIsOn]---店铺名称是否存在
@sql nvarchar(1000),
@shopname nvarchar(50)
as
begin
set @sql='select * from shop where ShopName='''+@shopname+''''
exec(@sql)
end
ALTER proc [dbo].[StateIs]--判断店铺状态
@sql nvarchar(1000),
@Id nvarchar(50)
as
begin
set @sql='select ShopState from shop where Id='''+@Id+''''
exec(@sql)
end
ALTER proc [dbo].[UpdateState]--更改店铺状态
@sql nvarchar(1000),
@Id nvarchar(50),
@state nvarchar(20)
as
begin
set @sql='update shop set ShopState='''+@state+''''
set @sql+='where Id='''+@Id+''''
exec(@sql)
end
ALTER proc [dbo].[UpdateShop]--修改店铺信息
@sql nvarchar(1000),
@Id nvarchar(20),
@shopname nvarchar(50),
@shoptype nvarchar(50),
@shopstate nvarchar(50),
@ShopManageId nvarchar(50)
as
begin
set @sql='update shop set '
set @sql+='ShopName='''+@shopname+''''
set @sql+=',ShoptypeId='''+@shoptype+''''
set @sql+=',ShopState='''+@shopstate+''''
set @sql+=',ShopManageId='''+@ShopManageId+''''
set @sql+=' where Id='''+@Id+''''
print(@sql)
exec(@sql)
end
ALTER proc [dbo].[SelecAllShopTypeCount]--店铺类型总个数
@sql nvarchar(1000),
@sqlbody nvarchar(1000),
@shoptypename nvarchar(50)
as
begin
if(@shoptypename<>'')
  begin
  set @sqlbody='and ShoptypeName  like ''%'+@shoptypename+'%'''
  end
set @sql='select count(*) as count from (select ROW_NUMBER() over(order by Id) as nid,* from shoptype where 1=1 '+@sqlbody+') as s'
exec(@sql)
end
ALTER proc [dbo].[SelectAllShopType]--查看所有店铺类型
@sql nvarchar(1000),
@minid nvarchar(50),
@maxid nvarchar(50),
@shoptypename nvarchar(80),
@sqlbody nvarchar(50)
as
begin
if(@shoptypename<>'')
  begin
set @sqlbody='and ShoptypeName  like ''%'+@shoptypename+'%'''
  end
set @sql='select * from (select ROW_NUMBER() over(order by Id) as nid,* from shoptype where 1=1 '+@sqlbody+') as a where a.nid>'+@minid
set @sql+='and a.nid<='+@maxid
print(@sql)
exec(@sql)
end
ALTER proc [dbo].[InsertTypeName] --添加店铺类型名称
@sql nvarchar(1000),
@typename nvarchar(100)--店铺类型名称
as
begin
set @sql='insert into shoptype(ShoptypeName) values('''+@typename+''')'
print(@sql) 
exec(@sql)
end
ALTER proc [dbo].[SelectTypeIsIn]---添加时查询是否店铺名称已经存在
@sql nvarchar(1000),
@typename nvarchar(50)
as
begin
set @sql='select ShoptypeName from shoptype where ShoptypeName='''+@typename+''''
print(@sql)
exec(@sql)
end
ALTER proc [dbo].[ProuductIsIn]---店铺中是否存在商品
@sql nvarchar(1000),
@shopId nvarchar(50)
as
begin
set @sql='select * from shop where ShoptypeId ='''+@shopId+''''
print(@sql)
exec(@sql)
end
ALTER proc [dbo].[DeleteTypeName]---删除店铺类型
@sql nvarchar(1000),
@Id nvarchar(50)
as
begin
set @sql='delete shoptype where Id='''+@Id+''''
print(@sql)
exec(@sql)
end
ALTER proc [dbo].[SelectAllByShopId] --编辑需要得到需要编辑的数据
@sql nvarchar(1000),
@shopId nvarchar(50)
as
begin
set @sql='select * from shoptype where Id='''+@shopId+''''
print(@sql)
exec(@sql)
end
ALTER proc [dbo].[SelectShopById]--根据编号查找出店铺信息
@sql nvarchar(1000),
@Id nvarchar(50)
as

begin
set @sql='select * from shop where Id='+@Id
exec(@sql)
end
ALTER proc [dbo].[UpdateType]--修改店铺名称
@sql nvarchar(1000),
@shopname nvarchar(100),
@id nvarchar(50)
as
begin
set @sql='update shoptype set ShoptypeName='''+@shopname+''''
set @sql=@sql+'where Id='''+@id+''''
exec(@sql)
end
-----------------------------------------------------*店铺管理 结束---------------------------------------------------------
/*****************************************************************************
******************************************************************************
********************************触发器****************************************
******************************************************************************
******************************************************************************/

--alter TRIGGER Trg_productsourcestock_Insert--原始库存表添加数据时添加数据至库存表
--ON [productsourcestock]
--for Insert AS begin
--  Insert into [productstock]([Scode],[Bcode],[Bcode2],[Descript],[Cdescript]
--      ,[Unit],[Currency],[Cat] ,[Cat1] ,[Cat2] ,[Clolor],[Size] ,[Style] ,[Pricea],[Priceb],[Pricec]
--      ,[Priced],[Pricee] ,[Disca] ,[Discb],[Discc] ,[Discd] ,[Disce] ,[Vencode] ,[Model] ,[Rolevel]
--      ,[Roamt] ,[Stopsales] ,[Loc],[Balance],[Lastgrnd],[PrevStock] ,[Def2] ,[Def3],[Def4] ,[Def5]
--      ,[Def6] ,[Def7] ,[Def8],[Def9],[Def10],[Def11]
--  ) 
--  select 
--     [Scode],[Bcode],[Bcode2],[Descript],[Cdescript]
--      ,[Unit],[Currency],[Cat] ,[Cat1] ,[Cat2] ,[Clolor],[Size] ,[Style] ,[Pricea],[Priceb],[Pricec]
--      ,[Priced],[Pricee] ,[Disca] ,[Discb],[Discc] ,[Discd] ,[Disce] ,[Vencode] ,[Model] ,[Rolevel]
--      ,[Roamt] ,[Stopsales] ,[Loc],[Balance],[Lastgrnd],[PrevStock] ,[Def2] ,[Def3],[Def4] ,[Def5]
--      ,[Def6] ,[Def7] ,[Def8],[Def9],[Def10] ,[Def11]
--   from inserted
--end
--go
--CREATE TRIGGER Trg_productsourcestock_Update--原始库存表修改数据时添加修改至库存表
--ON [productsourcestock]
--for Update AS begin
--declare @Scode nvarchar(16)
--declare @Vencode nvarchar(10)
--declare @Pricea numeric(13,2)
--declare @Priceb numeric(13,2)
--declare @Pricec numeric(13,2)
--declare @Priced numeric(13,2)
--declare @Pricee numeric(13,2)
--declare @Disca numeric(6,2)
--declare @Discb numeric(6,2)
--declare @Discc numeric(6,2)
--declare @Discd numeric(6,2)
--declare @Disce numeric(6,2)
--declare @Balance int
--declare @PrevStock int

-- select 
--     @Scode=[Scode],@Pricea=[Pricea],@Priceb=[Priceb],@Pricec=[Pricec]
--      ,@Priced=[Priced],@Pricee=[Pricee] ,@Disca=[Disca] ,@Discb=[Discb],@Discc=[Discc] ,@Discd=[Discd] ,@Disce=[Disce] ,@Vencode=[Vencode],@Balance=[Balance],@PrevStock=[PrevStock] 
--   from inserted
--  update [productstock] set [Pricea]=@Pricea,[Priceb]=@Priceb,[Pricec]=@Pricec
--      ,[Priced]=@Priced,[Pricee]=@Pricee,[Disca]=@Disca,[Discb]=@Discb,[Discc]=@Discc,[Discd]=@Discd,[Disce]=@Disce,[Balance]=@Balance,[PrevStock]=@PrevStock
--  where Scode=@Scode and Vencode=@Vencode
 
--end
--go
--CREATE TRIGGER Trg_productsourcestock_Delete--原始库存表删除数据时删除库存表
--ON [productsourcestock]
--for Delete AS begin
--  delete productstock where productstock.Scode in(
--  select [Scode] from Deleted) and productstock.Vencode in(select [Vencode] from Deleted)
--end
--go















/**********************************************************lcg***********************************************************/
/*
------------------------------------------更新订单发送的数据源-------------------------------------------------------------
*/
create proc sendSource
@orderId nvarchar(20),
@detailsOrderId nvarchar(20),
@newOrderId nvarchar(20),
@newScode nvarchar(20),
@newColor nvarchar(20),
@newSize nvarchar(20),
@newImg nvarchar(20),
--@newSaleCount int,
@newStatus int,
@createTime datetime,
@editTime datetime,
@showStatus int,
@newSaleCount int,
@sendSource nvarchar(20)
as
	insert into apiSendOrder(orderId,detailsOrderId,
	newOrderId,newScode,newColor,newSize,newImg,
	newSaleCount,newStatus,createTime,editTime,showStatus,sendSource) 
	values(@orderId,@detailsOrderId,@newOrderId,@newScode,@newColor,
	@newSize,@newImg,@newSaleCount,@newStatus,@createTime,@editTime,
	@showStatus,@sendSource)
go


create proc selectOrderDetails
@orderId nvarchar(20),
@scode nvarchar(20)
as
	select * from apiOrderDetails where orderId=@orderId and detailsScode=@scode
go


/*创建视图1，合并两张表*/
CREATE VIEW mergeProductStock
As
select ROW_NUMBER() over(order by scode desc) as myid,* from (
select * from productstock
union all
select * from SoCalProduct
) as tb
GO


/*创建视图2，读取view1中排除重复数据*/
create view distinctProductStock
as
select * from mergeProductStock where 
myid in (
select MIN(myid) as y from mergeProductStock group by scode having COUNT(*)>1
)
union 
select * from mergeProductStock where 
myid in (
select MIN(myid) as y from mergeProductStock group by scode having COUNT(*)=1
)
go




















/**********************************************************sqy***********************************************************/
--查询供应商类别表
ALTER proc [dbo].[SelectProducttypeVen]
@TypeName nvarchar(50), --类别名称
@TypeNameVen nvarchar(50), --供应商类别名称
@Vencode nvarchar(50), --供应商
@bangd nvarchar(50), --是否绑定
@MinNid nvarchar(10), --分页最小
@MaxNid nvarchar(10), --分页最大
@sql nvarchar(500)
as
begin
set @sql='select * from( 
select ROW_NUMBER() over( order by Cat2) as nid,a.Cat2,a.Vencode as Vencode2,b.*,c.bigtypeName,d.sourceName from(select cat2,Vencode from ItalyPorductStock group by cat2,Vencode) a 
left join producttypeVen b on a.Cat2=b.TypeNameVen and a.Vencode=b.Vencode
left join productbigtype c on b.BigId=c.Id 
left join productsource d on a.Vencode=d.SourceCode 
where 1=1 '

if(@TypeName<>'')
begin
set @sql=@sql+'and b.TypeName like ''%'+@TypeName+'%'' '
end

if(@TypeNameVen<>'')
begin
set @sql=@sql+'and a.Cat2 like ''%'+@TypeNameVen +'%'' '
end

if(@Vencode<>'')
begin
set @sql=@sql+'and a.Vencode='''+@Vencode+''' '
end

if(@bangd='0')
begin
set @sql=@sql+'and b.TypeName!='''' '
end

if(@bangd='1')
begin
set @sql=@sql+'and b.TypeName='''' or b.TypeName is null '
end

set @sql=@sql+') as tt where nid>'+@MinNid+' and nid<='+@MaxNid+''
print(@sql)
exec(@sql)
end


GO
--查询供应商类别表数量
ALTER proc [dbo].[SelectproducttypeVenCount]
@TypeName nvarchar(50), --类别名称
@TypeNameVen nvarchar(50), --供应商类别名称
@Vencode nvarchar(50), --供应商
@bangd nvarchar(50), --是否绑定
@sql nvarchar(500)
as
begin
set @sql='select count(*) from(
select ROW_NUMBER() over( order by Cat2) as nid,a.Cat2,a.Vencode as Vencode2,b.*,c.bigtypeName from(select cat2,Vencode from ItalyPorductStock group by cat2,Vencode) a
left join producttypeVen b on a.Cat2=b.TypeNameVen and a.Vencode=b.Vencode
left join productbigtype c on b.BigId=c.Id where 1=1 '

if(@TypeName<>'')
begin
set @sql=@sql+'and b.TypeName like ''%'+@TypeName+'%'' '
end

if(@TypeNameVen<>'')
begin
set @sql=@sql+'and a.Cat2 like ''%'+@TypeNameVen+'%'' '
end

if(@Vencode<>'')
begin
set @sql=@sql+'and a.Vencode='''+@Vencode+''' '
end

if(@bangd='0')
begin
set @sql=@sql+'and b.TypeName!='''' '
end

if(@bangd='1')
begin
set @sql=@sql+'and b.TypeName='''' or b.TypeName is null '
end

set @sql=@sql+') as tt '
print(@sql)
exec(@sql)
end





--查询供应商品牌表
alter proc [dbo].[SelectBrandVen]
@BrandName nvarchar(50), --类别名称
@BrandNameVen nvarchar(50), --供应商品牌
@Vencode nvarchar(50), --供应商
@bangd nvarchar(50), --是否绑定
@MinNid nvarchar(10), --分页最小
@MaxNid nvarchar(10), --分页最大
@sql nvarchar(500)
as
begin
set @sql='select * from(
select ROW_NUMBER() over( order by cat) as nid,a.Cat,a.Vencode as Vencode2,b.*,c.sourceName from(select Cat,Vencode from ItalyPorductStock group by Cat,Vencode) a
left join BrandVen b on a.Cat=b.BrandNameVen and a.Vencode=b.Vencode 
left join productsource c on a.Vencode=c.SourceCode 
where 1=1 '

if(@BrandName<>'')
begin
set @sql=@sql+'and b.BrandName like ''%'+@BrandName+'%'' '
end

if(@BrandNameVen<>'')
begin
set @sql=@sql+'and a.Cat like ''%'+@BrandNameVen +'%'' '
end

if(@Vencode<>'')
begin
set @sql=@sql+'and a.Vencode='''+@Vencode+''' '
end

if(@bangd='0')
begin
set @sql=@sql+'and b.BrandName!='''' '
end

if(@bangd='1')
begin
set @sql=@sql+'and b.BrandName is null '
end

set @sql=@sql+') as tt where nid>'+@MinNid+' and nid<='+@MaxNid+''
print(@sql)
exec(@sql)
end


GO
--返回查询供应商品牌表数量
alter proc [dbo].[SelectBrandVenCount]
@BrandName nvarchar(50), --类别名称
@BrandNameVen nvarchar(50), --供应商类别名称
@Vencode nvarchar(50), --供应商
@bangd nvarchar(50), --是否绑定
@sql nvarchar(500)
as
begin
set @sql='select COUNT(*) from(
select ROW_NUMBER() over( order by cat) as nid,a.Cat,a.Vencode as Vencode2,b.*,c.sourceName from(select Cat,Vencode from ItalyPorductStock group by Cat,Vencode) a
left join BrandVen b on a.Cat=b.BrandNameVen and a.Vencode=b.Vencode
left join productsource c on a.Vencode=c.SourceCode
where 1=1 '

if(@BrandName<>'')
begin
set @sql=@sql+'and b.BrandName like ''%'+@BrandName+'%'' '
end

if(@BrandNameVen<>'')
begin
set @sql=@sql+'and a.Cat like ''%'+@BrandNameVen +'%'' '
end

if(@Vencode<>'')
begin
set @sql=@sql+'and a.Vencode='''+@Vencode+''' '
end

if(@bangd='0')
begin
set @sql=@sql+'and b.BrandName!='''' '
end

if(@bangd='1')
begin
set @sql=@sql+'and b.BrandName is null '
end

set @sql=@sql+') as tt '
print(@sql)
exec(@sql)
end



--查询供应商季节表数量
Create proc [dbo].[SelectSeasonVenCount]
@Cat1 nvarchar(50), --季节
@Cat1Ven nvarchar(50), --供应商季节
@Vencode nvarchar(50), --供应商
@bangd nvarchar(50), --是否绑定
@sql nvarchar(500)
as
begin
set @sql='select Count(*) from(
select ROW_NUMBER() over( order by a.Cat1) as nid,a.Cat1 as VenCat1,a.Vencode as Vencode2,b.*,c.sourceName from(select Cat1,Vencode from ItalyPorductStock group by Cat1,Vencode) a
left join SeasonVen b on a.Cat1=b.Cat1Ven and a.Vencode=b.Vencode 
left join productsource c on a.Vencode=c.SourceCode 
where 1=1 '

if(@Cat1<>'')
begin
set @sql=@sql+'and b.Cat1 like ''%'+@Cat1+'%'' '
end

if(@Cat1Ven<>'')
begin
set @sql=@sql+'and a.Cat1 like ''%'+@Cat1Ven +'%'' '
end

if(@Vencode<>'')
begin
set @sql=@sql+'and a.Vencode='''+@Vencode+''' '
end

if(@bangd='0')
begin
set @sql=@sql+'and b.Cat1!='''' '
end

if(@bangd='1')
begin
set @sql=@sql+' and b.Cat1 is null '
end

set @sql=@sql+') as tt '
print(@sql)
exec(@sql)
end


--查询供应商季节表
Create proc [dbo].[SelectSeasonVen]
@Cat1 nvarchar(50), --季节
@Cat1Ven nvarchar(50), --供应商季节
@Vencode nvarchar(50), --供应商
@bangd nvarchar(50), --是否绑定
@MinNid nvarchar(10), --分页最小
@MaxNid nvarchar(10), --分页最大
@sql nvarchar(500)
as
begin
set @sql='select * from(
select ROW_NUMBER() over( order by a.Cat1) as nid,a.Cat1 as VenCat1,a.Vencode as Vencode2,b.*,c.sourceName from(select Cat1,Vencode from ItalyPorductStock group by Cat1,Vencode) a
left join SeasonVen b on a.Cat1=b.Cat1Ven and a.Vencode=b.Vencode 
left join productsource c on a.Vencode=c.SourceCode 
where 1=1 '

if(@Cat1<>'')
begin
set @sql=@sql+'and b.Cat1 like ''%'+@Cat1+'%'' '
end

if(@Cat1Ven<>'')
begin
set @sql=@sql+'and a.Cat1 like ''%'+@Cat1Ven +'%'' '
end

if(@Vencode<>'')
begin
set @sql=@sql+'and a.Vencode='''+@Vencode+''' '
end

if(@bangd='0')
begin
set @sql=@sql+'and b.Cat1!='''' '
end

if(@bangd='1')
begin
set @sql=@sql+' and b.Cat1 is null '
end

set @sql=@sql+') as tt where nid>'+@MinNid+' and nid<='+@MaxNid+''
print(@sql)
exec(@sql)
end



--出货报表查询
alter proc GetShipmentReport
@Mindate nvarchar(10),--日期最小
@Maxdate nvarchar(10),--日期最大
@sendSource nvarchar(10),--供应商
@MinNid nvarchar(10), --分页最小
@MaxNid nvarchar(10), --分页最大
@sql nvarchar(max)
as
begin
set @sql='select * from(
select ROW_NUMBER() over(order by createTime desc) as nid,a.*,b.sourceName from (
            select CONVERT(nvarchar(10),createTime,23) as createTime,sendSource,
            sum(case when newStatus=3 then 1 else 0 end) as weifa,
            SUM(case when newStatus=4 then 1 else 0 end) as yifa 
            from apiSendOrder 
            group by CONVERT(nvarchar(10),createTime,23),sendSource) as a 
            left join productsource b on a.sendSource=b.SourceCode where 1=1 '

if(@sendSource<>'')
begin
set @sql=@sql+' and a.sendSource='''+@sendSource+''''
end
if(@Mindate<>'')
begin
if(@Maxdate<>'')
begin
set @sql=@sql+' and a.createTime>='''+@Mindate+''' and a.createTime<'''+@Maxdate+''''
end
else
begin
set @sql=@sql+' and a.createTime>'''+@Mindate+''''
end
end
else
begin
if(@Mindate<>'')
begin
set @sql=@sql+' and a.createTime>='''+@Mindate+''''
end
end

set @sql=@sql+' ) t where 1=1 and nid>'+@MinNid+' and nid<='+@MaxNid+''
print @sql
exec(@sql)
end









/**********************************************************rwh***********************************************************/















/**********************************************************fj***********************************************************/





/**********************************************************hcj---start***********************************************************/
--删除子菜单
USE [pbxDB]
GO

/****** Object:  StoredProcedure [dbo].[DeleteSubMenu]    Script Date: 2015-09-15 9:43:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER proc [dbo].[DeleteSubMenu]
@Id int,  --菜单ID
@PersondId int  --角色ID
as
begin
     begin tran
		 begin try
				delete filedpermisson where FunId in (select Id from funpermisson where MenuId=@Id)
		        delete funpermisson where MenuId=@Id
				delete menu where Id=@Id  	
				delete personapermisson where MemuId=@Id and personaId=@PersondId
		 end try
		 begin catch
			 if @@trancount>0
			 begin
				rollback tran
				return 0
			 end
		 end catch
	 if @@trancount>0
	 begin
		 commit tran
		 return 1
	 end
end


GO
/**********************************************************hcj---end***********************************************************/
























































