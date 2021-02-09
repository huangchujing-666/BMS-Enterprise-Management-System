use master -- 设置当前数据库为master,以便访问sysdatabases表
go
if exists(select * from sysdatabases where name='pbxDB')
drop database pbxDB
go
create database pbxDB 
on  primary
(
    name='pbxDB_data',
    --filename='D:\pbxDB_data.mdf',
    filename='D:\dataBase\MSSQL10_50.PBX\MSSQL\DATA\pbxDB_data.mdf',
    size=50mb,
    maxsize=2000mb,
    filegrowth=15%
)
log on
(
    name='pbxDB_log',
    --filename='D:\pbxDB_log.ldf',
    filename='D:\dataBase\MSSQL10_50.PBX\MSSQL\DATA\pbxDB_log.mdf',
    size=20mb,
    filegrowth=1mb
)
go

use pbxDB
go


/*
***************类别配置表*****************
*/
if exists(select * from sysobjects where name='TypeConfig')
drop table TypeConfig
create table TypeConfig(
Id int identity(1,1) primary key,--自增(主键)
PersonaId nvarchar(30) ,	--角色Id
TypeId nvarchar(1000) ,		--小类别
UserId int,		            --操作人(外键）			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go
/*
***************用户个人类别配置表*****************
*/
if exists(select * from sysobjects where name='PersonaTypeConfit')---用户个人配置

drop table PersonaTypeConfit
create table PersonaTypeConfit(
Id int identity(1,1) primary key,--自增(主键)
CustomerId nvarchar(30) ,	--角色Id
TypeId nvarchar(4000) ,		--小类别
UserId int,		            --操作人(外键）			
Def1	nvarchar(50),		--默认1--供应商
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go

/*
***************品牌配置表*****************
*/
if exists(select * from sysobjects where name='BrandConfig')
drop table BrandConfig
create table BrandConfig(
Id int identity(1,1) primary key,--自增(主键)
PersonaId nvarchar(30) ,	--角色Id
BrandId nvarchar(1000) ,	--品牌编号
UserId int,		            --操作人(外键）			
Def1	nvarchar(50),		--默认1--供应商
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go
/*
***************品牌配置表个人配置*****************
*/
if exists(select * from sysobjects where name='BrandConfigPersion')
drop table BrandConfigPersion
create table BrandConfigPersion(
Id int identity(1,1) primary key,--自增(主键)
CustomerId nvarchar(30) ,	--角色Id
BrandId nvarchar(1000) ,	--品牌编号
UserId int,		            --操作人(外键）			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go
/*
***************存放table和filed表*****************
*/
if exists(select * from sysobjects where name='tableFiledPerssion')
drop table tableFiledPerssion
create table tableFiledPerssion(
Id	int identity(1,1) primary key,		            --自增(主键)
tableName	nvarchar(30) ,		--表名
tableFiled	nvarchar(30) ,		--表字段
tableLevel int,                         --所属
tableNameState int default(1),                     --表名称是否显示(默认显示为1，不显示为0)
tableFiledState int default(1),                    --表字段是否显示(默认显示为1，不显示为0)
tableIndex int,                         --排序
UserId	int,		                    --操作人(外键）
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go
select * from tableFiledPerssion



/*
***************角色表*****************
*/
if exists(select * from sysobjects where name='persona')
drop table persona
create table persona(
Id	int identity(1,1) primary key,		            --自增(主键)
PersonaName	nvarchar(20) not null,		--角色名称
PersonaIndex	int,		            --排序
UserId	int,		                    --操作人(外键）
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go



/*
***************用户表*****************
*/
if exists(select * from sysobjects where name='users')
drop table users
create table users(
Id	int identity(1,1) primary key,		                --自增(主键)
personaId	int FOREIGN KEY REFERENCES persona(ID),		                    --角色Id(外键)
userName	nvarchar(20) not null ,		    --用户名
userPwd	nvarchar(20) not null,		        --用户密码
userRealName	nvarchar(20) not null,		--真实姓名
userSex	int,	                            --性别(0-男，1-女)
UserPhone	nvarchar(20),		--电话
UserAddress	nvarchar(200),		--地址
UserEmail	nvarchar(20),		--邮箱
userIndex	int,		        --排序
UserManage	int,	            --(0=普通用户，1=角色管理，2=系统管理)	用户管理状态
UserId	int,		            --操作人(外键）
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go


/*
***************用户淘宝表*****************
*/
if exists(select * from sysobjects where name='taoAppUser')
drop table taoAppUser
create table taoAppUser(
Id	int identity(1,1) primary key,		                --自增(主键)
tbUsserId	nvarchar(20),		                    --淘宝用户ID
tbUserNick	nvarchar(20) not null ,		    --淘宝用户昵称
accessToken	nvarchar(150) not null,		--访问令牌
refreshToken	nvarchar(150) not null,		--刷新令牌
userId1	int FOREIGN KEY REFERENCES users(ID),					--用户ID（外键）
userId	int FOREIGN KEY REFERENCES users(ID),		            --操作人(外键）
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go


/**淘宝店铺表**/
if exists(select * from sysobjects where name='tbProductReMark')
drop table tbProductReMark
create table tbProductReMark(
Id int identity(1,1) primary key,       --自增(主键)
ProductReMarkId nvarchar(50),	        --淘宝店铺编号
ProductStyle nvarchar(1000),		    --款号
ProductTitle nvarchar(1000),		    --商品标题			
ProductImg nvarchar(500),		        --商品图片
ProductTaoBaoPrice numeric(13, 2),		--淘宝价格
ProductYJStock int,		                --预警库存
ProductSJ int,		                    --上架库存
ProductSJTime1 nvarchar(500),	        --上架时间
ProductXJTime2 nvarchar(500),            --下架时间
ProductState  nvarchar(50) ,            --商品状态 instock（仓库中）onsale（销售中）
ProductShopName nvarchar(500),          --淘宝商铺
ProductReMarkShopCar  int,              --购物车
ProductReMarkActivity nvarchar(500),    --活动
ProductReMarkKeep int,                  --收藏
ProductReMarkStockA int,                --库存A
ProductReMarkStockB int,                --库存B
ProductReMark1 nvarchar(1000),          --备注
ProductOther1  nvarchar(1000),          --其他1
ProductOther2  nvarchar(1000),          --其他2
ProductOther3  nvarchar(1000),          --其他3
ProductOther4  nvarchar(1000),          --其他4
Def1 nvarchar(1000),                    --默认1
Def2 nvarchar(1000),                    --默认2
Def3 nvarchar(1000),                    --默认3
Def4 nvarchar(1000),                    --默认4
Def5 nvarchar(1000),                    --默认5
Def6 nvarchar(1000),                    --默认6
Def7 nvarchar(1000),                    --默认7
Def8 nvarchar(1000),                    --默认8
Def9 nvarchar(1000),                    --默认9
Def10 nvarchar(1000),                   --默认10
)
Go


/*
***************菜单表*****************
*/
if exists(select * from sysobjects where name='menu')
drop table menu
create table menu(
Id	int identity(1,1) primary key,			--自增(主键)
menuName	nvarchar(20),		--菜单名称
MenuSrc	nvarchar(500),			--菜单链接
MenuLevel	int 	Default(0),	--菜单级别(根目录默认为0）
MenuIndex	int,				--排序
UserId	int,					--操作人(外键）
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go

insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('首页','',0,1,1)
insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('首页','',0,1,1)
insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('首页','',0,1,1)
insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('首页','',0,1,1)
insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('首页','',0,1,1)
insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('首页','',0,1,1)
insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('首页','',0,1,1)
insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('首页','',0,1,1)
insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('首页','',0,1,1)
go



/*
***************功能表*****************
*/
if exists(select * from sysobjects where name='funpermisson')
drop table funpermisson
create table funpermisson(
Id	int identity(1,1) primary key,		--自增(主键)
MenuId	int foreign key references menu(id),				--菜单Id(外键）
FunName	nvarchar(20),		--功能名称
FunIndex	int,			--排序
UserId	int,				--操作人(外键）
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go


/*
***************字段表*****************
*/
if exists(select * from sysobjects where name='filedpermisson')
drop table filedpermisson
create table filedpermisson(
Id	int identity(1,1) primary key,		--自增(主键)
FunId	int foreign key references funpermisson(id),		--功能Id(外键）
dbTable	nvarchar(50),		--数据表
Tabledescription	nvarchar(300),		--表格描述
IsTableShow	int,		--数据表是否显示
TableFiled	nvarchar(50),		--表字段
FiledDescription	nvarchar(300),		--表字段描述
IsFiledShow	int,		--表字段是否显示
FiledState	int	Default(0),	--字段状态(1=隐藏)，默认都显示
filedIndex	int,		--排序
UserId	int,		--操作人(外键）
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go



/*
***************用户角色权限表*****************
*/
if exists(select * from sysobjects where name='userspermisson')
drop table userspermisson
create table userspermisson(
Id	int identity(1,1) primary key,		--自增(主键)
UserId	int foreign key references users(id),		        --用户Id(外键）
PersonaId	int foreign key references persona(id),		        --角色Id(外键）
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go



/*
***************角色权限表*****************
*/
if exists(select * from sysobjects where name='personapermisson')
drop table personapermisson
create table personapermisson(
Id	int identity(1,1) primary key,						--自增(主键)
personaId	int foreign key references persona(id),		--角色Id(外键）
MemuId int ,--foreign key references menu(id),			--菜单Id(外键）
FunId	int ,--foreign key references funpermisson(id) 	--功能Id(外键）
FieldId	int ,--foreign key references filedpermisson(id)--字段Id(外键）
			
Def1	nvarchar(50),		--字段描述
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50),		--默认5
)
go


/*
***************品牌表*****************
*/
if exists(select * from sysobjects where name='brand')
drop table brand
create table brand(
Id	int identity(1,1) primary key,		--自增(主键)
BrandName	nvarchar(20),		--品牌名称
BrandAbridge	nvarchar(20),		--品牌名称缩写
BrandCode	nvarchar(20),		--名牌编码
BrandIndex	int,		--排序
UserId	int,		--操作人(外键）
			
Def1	nvarchar(50),		--对应淘宝编号
Def2	nvarchar(50),		--品牌归属地
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50),		--默认5
)
go



/*
***************大类别表*****************
*/
if exists(select * from sysobjects where name='productbigtype')
drop table productbigtype
create table productbigtype(
Id	int identity(1,1) primary key,		--自增(主键)
bigtypeName	nvarchar(20),		--大类别名称
bigtypeIndex	int,		--排序
UserId	int,		--操作人(外键）
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50),		--默认5
)
go



/*
***************类别表*****************
*/
if exists(select * from sysobjects where name='producttype')
drop table producttype
create table producttype(
Id	int identity(1,1) primary key,		--自增(主键)
BigId	Int	foreign key references productbigtype(id),	--大类别ID（外键）
TypeName	nvarchar(20),		--类别名称
TypeNo	nvarchar(20),		--类别编码
typeIndex	int,		--排序
UserId	int,		--操作人(外键）
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50),		--默认5
)
go


/*
***************商品类别属性表*****************
*/
if exists(select * from sysobjects where name='productPorperty')
drop table productPorperty
create table productPorperty(
Id	int identity(1,1) primary key,		--自增(主键)
TypeId	int  foreign key references producttype(id),		--类别ID
PropertyName	nvarchar(20),		--属性名称
PorpertyIndex	int,		--排序
UserId	int,		--操作人
			
Def1	nvarchar(50),		--淘宝类别编号
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50),		--默认5
)
go


/*
***************商品类别属性值表*****************
*/
if exists(select * from sysobjects where name='productPorpertyValue')
drop table productPorpertyValue
create table productPorpertyValue(
Id	int identity(1,1) primary key,		--自增(主键)
TypeId	int  foreign key references producttype(id),		--类别ID
PropertyId	int  foreign key references productPorperty(id),		--属性ID
Scode	int,		--货号
PropertyValue	nvarchar(20),		--属性值
PorpertyIndex	int,		--排序
UserId	int,		--操作人
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50),		--默认5
)
go





/*
***************日志表*****************
*/
if exists(select * from sysobjects where name='errorlog')
drop table errorlog
create table errorlog(
Id	int identity(1,1) primary key,	--自增(主键)
ErrorMsg	nvarchar(50),			--错误信息
errorSrc	nvarchar(500),			--错误路径
errorMsgDetails	nvarchar(500),		--错误详情
UserId	int,						--操作人(外键）
errorTime	datetime,				--日志时间
			
operation	nvarchar(50),		--日志类型：1-异常信息2.普通操作3.数据读取异常
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50),		--默认5
)
go




/*
***************供应商*****************
*/
if exists(select * from sysobjects where name='productsource')
drop table productsource
create table productsource(
Id	int identity(1,1)  primary key,		                --自增
SourceCode	nvarchar(10)  ,		--供应商编码(主键)
sourceName	nvarchar(20),		--供应商名称
sourcePhone	nvarchar(20),		--供应商电话
SourceManage	nvarchar(20),	--供应商联系人
SourceLevel	nvarchar(20),		--供应商等级
UserId	int,		            --操作人(外键)
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go
/*
**************数据源配置表
*/
if exists(select * from sysobjects where name='productsourceConfig')
drop table productsourceConfig
create table productsourceConfig(
 Id	int identity(1,1) primary key,		                --自增
 SourceCode	nvarchar(10) ,--供应商编号 外键
 SourcesAddress nvarchar(50),--server地址：端口
 UserId nvarchar(20),--登陆用户名
 UserPwd nvarchar(20),--登陆密码
 DataSources nvarchar(20),--数据源名称
 DataSourcesLevel nvarchar(10),--数据源级别 (等级由高到低：A B C D E)
 TimeStart int default(0),--多久启动一次 默认0只启动一次
 Def1 nvarchar(20),
 Def2  nvarchar(20),
 Def3  nvarchar(20),
 Def4  nvarchar(20),
 Def5  nvarchar(20)
)
go
/*
**************数据源表（库存更新）
*/
if exists(select * from sysobjects where name='ProductStockConfig')
drop table ProductStockConfig
create table ProductStockConfig(
 Id	int identity(1,1) primary key,		                --自增
 SourceCode	nvarchar(10) ,--供应商编号 外键
 DataSources nvarchar(20),--数据源名称
 TableName nvarchar(50),---数据表名
 UpdateState nvarchar(50),--更新状态
 StartTime nvarchar(60),---开始时间
 SetTime int default(0),--多久启动一次 默认0只启动一次
 Def1 nvarchar(20),
 Def2  nvarchar(20),
 Def3  nvarchar(20),
 Def4  nvarchar(20),
 Def5  nvarchar(20)
)
go

/*

***************原始数据库存表*****************
*/
if exists(select * from sysobjects where name='productsourcestock')
drop table productsourcestock
create table productsourcestock(
Id	int identity(1,1) primary key,		--自增(主键)
Scode	nvarchar(16),		--货号
Bcode	nvarchar(16),		--条码1
Bcode2	nvarchar(20),		--条码2
Descript	nvarchar(60),	--英文描述
Cdescript	nvarchar(60),	--中文描述
Unit	nvarchar(6),		--单位
Currency	nvarchar(3),	--货币
Cat	nvarchar(5),			--品牌
Cat1	nvarchar(5),		--季节
Cat2	nvarchar(5),		--类别
Clolor	nvarchar(20),		--颜色
Size	nvarchar(20),		--尺寸
Style	nvarchar(16),		--款号
Pricea	numeric(13,2),		--价格1
Priceb	numeric(13,2),		--价格2
Pricec	numeric(13,2),		--价格3
Priced	numeric(13,2),		--价格4
Pricee	numeric(13,2),		--价格5
Disca	numeric(6,2),		--折扣1
Discb	numeric(6,2),		--折扣2
Discc	numeric(6,2),		--折扣3
Discd	numeric(6,2),		--折扣4
Disce	numeric(6,2),		--折扣5
Vencode	nvarchar(10) foreign key references productsource(SourceCode),		--供应商编号(外键)
Model	nvarchar(30),		--型号
Rolevel	int,				--预警库存
Roamt	int,				--最少订货量
Stopsales	int,			--停售库存
Loc	nvarchar(5),			--店铺
Balance	int,				--供应商库存
Lastgrnd	datetime,			--收货日期(按货品)
Imagefile	nvarchar(500),			--货品图片
			
PrevStock	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50),		--默认5
Def6	nvarchar(50),		--默认6
Def7	nvarchar(50),		--默认7
Def8	nvarchar(50),		--默认8
Def9	nvarchar(50),		--默认9
Def10	nvarchar(50),		--默认10
Def11	nvarchar(50),		--默认11
)
go
/*
***************意大利原始数据数据表*****************
*/
if exists(select * from sysobjects where name='ProductItalySourceStock')---意大利数据原始表

drop table ProductItalySourceStock
create table ProductItalySourceStock(
Id int identity(1,1) primary key,--自增(主键)
productCode nvarchar(200),--产品编号
seasonCode nvarchar(200),--季节码
brand nvarchar(200),--品牌
styleCode nvarchar(200),--风格码
colorCode nvarchar(200),--布料颜色码
sizeType nvarchar(200),--大小类型
season nvarchar(200),--季节
parentGroup nvarchar(200),--组
childGroup nvarchar(200),--子组
styleDescribe nvarchar(200),--风格描述
color nvarchar(200),--颜色
cloth nvarchar(200),--布料
theme nvarchar(200),--主题
classes nvarchar(200),--类别
simplenessDescribe nvarchar(200),--简单描述
fullDescribe nvarchar(200),--完整描述
price1 money,--单价
price2 money,--网站单价
volume nvarcahr(200),--体积
price3 money,--网站单价（扣除增值税）
size nvarchar(200),--尺寸（国家）
presell nvarchar(200) ,--预售
isCancel int,--已取消（0=有效，1=已取消）
season1 nvarchar,--季节
shadeguide nvarchar(200),--色标
especiallyCarriage nvarchar(200),--特别运输
measure1 nvarchar(200),--测量1
measure2 nvarchar(200),--测量2
measure3 nvarchar(200),--测量3
measure4 nvarchar(200),--测量4
measure5 nvarchar(200),--测量5
measure6 nvarchar(200),--测量6
measure7 nvarchar(200),--测量7
measure8 nvarchar(200),--测量8
sizeAndFit nvarchar(200),--适合大小
presellTime nvarchar(200) ,--预售日期
PublishingPages nvarchar(200),--出版尺寸
packExplain nvarchar(500),--包装描述
packNum int ,--包装数量
especiallyExplain nvarchar(500),--特别说明
primaryProducingPlace nvarchar(200),--原产地
MIDId nvarchar(200),--MID码
constituteDetail nvarchar(200),--组成详细
brandIntro nvarchar(200),--品牌简介
sizeType1 nvarchar(200),--大小类型
SuperBrandCollection nvarchar(200),--品牌集合
SuperGroupSuper nvarchar(200),--?
SuperBrand nvarchar(200),
def1 nvarchar(200),
def2 nvarchar(200),
def3 nvarchar(200),
def4 nvarchar(200),
def5 nvarchar(200)
)
go
/*

***************意大利原始数据库存表*****************
*/
if exists(select * from sysobjects where name='ProductItalyStock')---意大利数据库存表
drop table ProductItalyStock
create table ProductItalyStock(
Id int identity(1,1) primary key,--自增(主键)
productCode nvarchar(200),--产品编号
size nvarchar(200),--尺寸
stockCount int ,--库存量
receiveNumber int,--接收数量
createDate nvarchar(200),--数据创建时间
barCode nvarchar(200),--条码
measure1 nvarchar(200),--测量1
measure2 nvarchar(200),--测量2
measure3 nvarchar(200),--测量3
measure4 nvarchar(200),--测量4
measure5 nvarchar(200),--测量5
measure6 nvarchar(200),--测量6
measure7 nvarchar(200),--测量7
measure8 nvarchar(200),--测量8
def1 nvarchar(200) null,
def2 nvarchar(200) null,
def3 nvarchar(200) null,
def4 nvarchar(200) null,
def5 nvarchar(200) null
)
go
/*
*意大利数据源更新错误日志*
*/
if exists(select * from sysobjects where name='ItalyUpdateError')---意大利数据更新错误日志表

drop table ItalyUpdateError
create table ItalyUpdateError(
Id int identity(1,1) primary key,--自增(主键)
ItalyCode nvarchar(10),-- 意大利数据源编号
createTime datetime ,--错误日志时间
msg nvarchar(500),--错误详情
methed nvarchar(500),--错误方法
def1 nvarchar(50),
def2 nvarchar(50),
def3 nvarchar(50),
def4 nvarchar(50),
def5 nvarchar(50),
)
--alter table productsourcestock alter column Imagefile nvarchar(500)




/*
***************库存表*****************
*/
if exists(select * from sysobjects where name='productstock')
drop table productstock
create table productstock(
Id	int identity(1,1) primary key,		--自增(主键)
Scode	nvarchar(16),		--货号
Bcode	nvarchar(16),		--条码1
Bcode2	nvarchar(20),		--条码2
Descript	nvarchar(60),	--英文描述
Cdescript	nvarchar(60),	--中文描述
Unit	nvarchar(6),		--单位
Currency	nvarchar(3),	--货币
Cat	nvarchar(5),			--品牌
Cat1	nvarchar(5),		--季节
Cat2	nvarchar(5),		--类别
Clolor	nvarchar(20),		--颜色
Size	nvarchar(20),		--尺寸
Style	nvarchar(16),		--款号
Pricea	numeric(13,2),		--价格1
Priceb	numeric(13,2),		--价格2
Pricec	numeric(13,2),		--价格3
Priced	numeric(13,2),		--价格4
Pricee	numeric(13,2),		--价格5
Disca	numeric(6,2),		--折扣1
Discb	numeric(6,2),		--折扣2
Discc	numeric(6,2),		--折扣3
Discd	numeric(6,2),		--折扣4
Disce	numeric(6,2),		--折扣5
Vencode	nvarchar(10),		--供应商编号(外键)
Model	nvarchar(30),		--型号
Rolevel	int,				--预警库存
Roamt	int,				--最少订货量
Stopsales	int,			--停售库存
Loc	nvarchar(5),			--店铺
Balance	int,				--供应商库存
Lastgrnd	datetime,			--收货日期(按货品)
Imagefile	nvarchar(500),			--货品图片
			
PrevStock	int,		--上次库存
Def2	nvarchar(50),		---是否为残次品
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50),		--默认5
Def6	nvarchar(50),		--默认6
Def7	nvarchar(50),		--默认7
Def8	nvarchar(50),		--默认8
Def9	nvarchar(50),		--默认9
Def10	nvarchar(50),		--默认10
Def11	nvarchar(50),		--默认11
)
go


/*
***************外部库存表*****************
*/
if exists(select * from sysobjects where name='outsideProduct')
drop table outsideProduct
create table outsideProduct(
Id	int identity(1,1) primary key,		--自增(主键)
Scode	nvarchar(16),		--货号
Bcode	nvarchar(16),		--条码1
Bcode2	nvarchar(20),		--条码2
Descript	nvarchar(60),	--英文描述
Cdescript	nvarchar(60),	--中文描述
Unit	nvarchar(6),		--单位
Currency	nvarchar(3),	--货币
Cat	nvarchar(5),			--品牌
Cat1	nvarchar(5),		--季节
Cat2	nvarchar(5),		--类别
Clolor	nvarchar(20),		--颜色
Size	nvarchar(20),		--尺寸
Style	nvarchar(16),		--款号
Pricea	numeric(13,2),		--价格1
Priceb	numeric(13,2),		--价格2
Pricec	numeric(13,2),		--价格3
Priced	numeric(13,2),		--价格4
Pricee	numeric(13,2),		--价格5
Disca	numeric(6,2),		--折扣1
Discb	numeric(6,2),		--折扣2
Discc	numeric(6,2),		--折扣3
Discd	numeric(6,2),		--折扣4
Disce	numeric(6,2),		--折扣5
Vencode	nvarchar(10),		--供应商编号(外键)
Model	nvarchar(30),		--型号
Rolevel	int,				--预警库存
Roamt	int,				--最少订货量
Stopsales	int,			--停售库存
Loc	nvarchar(5),			--店铺
Balance	int,				--供应商库存
Lastgrnd	datetime,		--收货日期(按货品)
Imagefile	nvarchar(500),	--货品图片
			
PrevStock	int,			--上次库存
ShowState int ,				--显示状态 1:显示 0：不显示
Def2	nvarchar(50),		--默认2 
Def3	nvarchar(50),		--默认3 
Def4	nvarchar(50),		--默认4 --退货库存
Def5	nvarchar(50),		--默认5 --操作人
Def6	nvarchar(50),		--默认6 --操作时间
Def7	nvarchar(50),		--默认7 --1   已经修改价格   
Def8	nvarchar(50),		--默认8 --1   已经修改 库存
Def9	nvarchar(50),		--默认9
Def10	nvarchar(50),		--默认10
Def11	nvarchar(50),		--默认11
)
go

/*
***************选货外部库存表*****************
*/
if exists(select * from sysobjects where name='SoCaloutsideProduct')
drop table SoCaloutsideProduct
create table SoCaloutsideProduct(
Id	int identity(1,1) primary key,		--自增(主键)
Scode	nvarchar(16),		--货号
Bcode	nvarchar(16),		--条码1
Bcode2	nvarchar(20),		--条码2
Descript	nvarchar(60),	--英文描述
Cdescript	nvarchar(60),	--中文描述
Unit	nvarchar(6),		--单位
Currency	nvarchar(3),	--货币
Cat	nvarchar(5),			--品牌
Cat1	nvarchar(5),		--季节
Cat2	nvarchar(5),		--类别
Clolor	nvarchar(20),		--颜色
Size	nvarchar(20),		--尺寸
Style	nvarchar(16),		--款号
Pricea	numeric(13,2),		--价格1
Priceb	numeric(13,2),		--价格2
Pricec	numeric(13,2),		--价格3
Priced	numeric(13,2),		--价格4
Pricee	numeric(13,2),		--价格5
Disca	numeric(6,2),		--折扣1
Discb	numeric(6,2),		--折扣2
Discc	numeric(6,2),		--折扣3
Discd	numeric(6,2),		--折扣4
Disce	numeric(6,2),		--折扣5
Vencode	nvarchar(10),		--供应商编号(外键)
Model	nvarchar(30),		--型号
Rolevel	int,				--预警库存
Roamt	int,				--最少订货量
Stopsales	int,			--停售库存
Loc	nvarchar(5),			--店铺
Balance	int,				--供应商库存
Lastgrnd	datetime,		--收货日期(按货品)
Imagefile	nvarchar(500),	--货品图片
			
PrevStock	int,			--上次库存
ShowState int ,				--显示状态 1:显示 0：不显示
Def2	nvarchar(50),		--默认2 
Def3	nvarchar(50),		--默认3 
Def4	nvarchar(50),		--默认4 --退货库存
Def5	nvarchar(50),		--默认5 --操作人
Def6	nvarchar(50),		--默认6 --操作时间
Def7	nvarchar(50),		--默认7 --1   已经修改价格   
Def8	nvarchar(50),		--默认8 --1   已经修改 库存
Def9	nvarchar(50),		--默认9
Def10	nvarchar(50),		--默认10
Def11	nvarchar(50),		--默认11
)
go

/*
***************商品表*****************
*/
if exists(select * from sysobjects where name='product')
drop table product
create table product(
Id	int,		--自增(主键)
Scode	nvarchar(16),		--货号
Bcode	nvarchar(16),		--条码1
Bcode2	nvarchar(20),		--条码2
Descript	nvarchar(60),	--英文描述
Cdescript	nvarchar(60),	--中文描述
Unit	nvarchar(6),		--单位
Currency	nvarchar(3),	--货币
Cat	nvarchar(5),			--品牌
Cat1	nvarchar(5),		--季节
Cat2	nvarchar(5),		--类别
Clolor	nvarchar(20),		--颜色
Size	nvarchar(20),		--尺寸
Style	nvarchar(16),		--款号
Pricea	numeric(13,2),		--价格1
Priceb	numeric(13,2),		--价格2
Pricec	numeric(13,2),		--价格3
Priced	numeric(13,2),		--价格4
Pricee	numeric(13,2),		--价格5
Disca	numeric(6,2),		--折扣1
Discb	numeric(6,2),		--折扣2
Discc	numeric(6,2),		--折扣3
Discd	numeric(6,2),		--折扣4
Disce	numeric(6,2),		--折扣5
Vencode	nvarchar(10),		--供应商编号(外键)
Model	nvarchar(30),		--型号
Rolevel	int,				--预警库存
Roamt	int,				--最少订货量
Stopsales	int,			--停售库存
Loc	nvarchar(5),			--店铺
Balance	int,				--供应商库存
Lastgrnd	datetime,		--收货日期(按货品)
Imagefile	nvarchar(500),	--货品图片
			
Def1	nvarchar(50),	    --图片最近上传时间
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--销售库存
ciqProductId nvarchar(128), --商品信息图  1.合格 2.未标记 3.不合格
ciqSpec nvarchar(200),      --规格型号
ciqHSNo nvarchar(200),      --商品HS编码
ciqAssemCountry nvarchar(200), --原产国/地区
ciqProductNo int  identity(1,1)  primary key ,--商品序号（订单报备EntGoodsNo字段）长度小于6
Def9	nvarchar(50),		--图片是否合格有4个状态  空为无图0.未检查1.合格2.不合格
Def10	nvarchar(50),		--条形码
Def11	nvarchar(50),		--商品海关备案编号 0.未检 1.检查 2.备案通过 3.备案失败
Def12	nvarchar(50),		--商检商品信息报备 1.合格 0.不合格
Def13	nvarchar(50),		--联邦商品信息报备 1.合格 0.不合格
QtyUnit nvarchar(50),       --计量单位	商品购买数/重量的计量单位
Def15 nvarchar(50),         --毛重
Def16 nvarchar(50),         --净重
Def17 nvarchar(200),        --产品链接
)
go
/*现货表*/
if exists(select * from sysobjects where name='SoCalProduct')  
drop table SoCalProduct
create table SoCalProduct(
Id	int identity(1,1) primary key,		--自增(主键)
Scode	nvarchar(16),		--货号
Bcode	nvarchar(16),		--条码1
Bcode2	nvarchar(20),		--条码2
Descript	nvarchar(60),	--英文描述
Cdescript	nvarchar(60),	--中文描述
Unit	nvarchar(6),		--单位
Currency	nvarchar(3),	--货币
Cat	        nvarchar(5),	--品牌
Cat1	nvarchar(5),		--季节
Cat2	nvarchar(5),		--类别
Clolor	nvarchar(20),		--颜色
Size	nvarchar(20),		--尺寸
Style	nvarchar(16),		--款号
Pricea	numeric(13,2),		--价格1
Priceb	numeric(13,2),		--价格2
Pricec	numeric(13,2),		--价格3
Priced	numeric(13,2),		--价格4
Pricee	numeric(13,2),		--价格5
Disca	numeric(6,2),		--折扣1
Discb	numeric(6,2),		--折扣2
Discc	numeric(6,2),		--折扣3
Discd	numeric(6,2),		--折扣4
Disce	numeric(6,2),		--折扣5
Vencode	nvarchar(10),		--供应商编号(外键)
Model	nvarchar(30),		--型号
Rolevel	int,				--预警库存
Roamt	int,				--最少订货量
Stopsales	int,			--停售库存
Loc	nvarchar(5),			--店铺
Balance	int,				--供应商库存
Lastgrnd	datetime,		--收货日期(按货品)
Imagefile	nvarchar(500),	--货品图片		
Def1	nvarchar(50),	    --默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4 nvarchar(128),         --默认4
Def5 nvarchar(200),         --默认5
Def6 nvarchar(200),         --默认6
Def7 nvarchar(200),         --默认7
Def8 nvarchar(200),         --默认8
Def9	nvarchar(50),		--默认9
Def10	nvarchar(50),		--默认10
Def11	nvarchar(50),		--默认11
)
go

/*
***************款号描述表*****************
*/
if exists(select * from sysobjects where name='styledescript')
drop table styledescript
create table styledescript(
Id	int identity(1,1) primary key,		--自增(主键)
style nvarchar(50) ,					--款号
productDescript	nvarchar(4000) ,		--商品描述
stylePicSrc	nvarchar(500) ,				--商品缩略图
isQualified int,                        --缩略图是否合格
UserId	int,		                    --操作人(外键）
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go


/*
***************款号图片表*****************
*/
if exists(select * from sysobjects where name='stylepic')
drop table stylepic
create table stylepic(
Id	int identity(1,1) primary key,		--自增(主键)
style nvarchar(50) ,		 --款号
stylePicSrc	nvarchar(500) , --商品主图
isQualified int,             --图片是否合格【5张以下，是不合格，5张以上合格】（5-10张）
stylePicState int,			 --图片状态
UserId	int,		                    --操作人(外键）
			
Def1	nvarchar(50),		--1.普通图 2.主图 
Def2	nvarchar(50),		--1.普通图 2.配图
Def3	nvarchar(50),		--1.普通图 2.描述图
Def4	nvarchar(50),		--1.使用中 2.被删除
Def5	nvarchar(50)		--图片名称
)
go



/*
***************货号图片表*****************
*/
if exists(select * from sysobjects where name='scodepic')
drop table scodepic
create table scodepic(
Id	int identity(1,1) primary key,		--自增(主键)
scode nvarchar(50) ,					--货号
scodePicSrc	nvarchar(500) ,				--商品图片
scodePicType	int ,					--图片类别【详情图为1、颜色图（即是尺寸图）为0】
isQualified int,                        --图片是否合格
scodePicState int,						--图片状态
UserId	int,		                    --操作人(外键）
			
Def1	nvarchar(50),					--1.普通图 2.主图 
Def2	nvarchar(50),					--默认2
Def3	nvarchar(50),					--默认3
Def4	nvarchar(50),					--1.使用中 2.被删除
Def5	nvarchar(50)					--图片名称
)
go





/*
***************店铺类型表*****************
*/
if exists(select * from sysobjects where name='shoptype')
drop table shoptype
create table shoptype(
Id	int identity(1,1) primary key,		                --自增
ShoptypeName	nvarchar(20),							--店铺类别名称
ShoptypeIndex int,										--排序
UserId	int,											--操作人(外键)
			
Def1	nvarchar(50),									--默认1
Def2	nvarchar(50),									--默认2
Def3	nvarchar(50),									--默认3
Def4	nvarchar(50),									--默认4
Def5	nvarchar(50)									--默认5
)
go










/*
***************店铺表*****************
*/
if exists(select * from sysobjects where name='shop')
drop table shop
create table shop(
Id	int identity(1,1) primary key,		                --自增
ShopName	nvarchar(20),								--店铺名称
ShoptypeId	int foreign key references shoptype(id),	--店铺类别（外键）
ShopState	int,										--店铺状态
ShopManageId	int foreign key references users(id),	--店铺管理人（外键）
ShopIdex int,											--排序
UserId	int,											--操作人(外键)
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go





/*
***************店铺商品表*****************
*/
if exists(select * from sysobjects where name='shopproduct')
drop table shopproduct
create table shopproduct(
Id	int identity(1,1) primary key,		--自增(主键)
Scode	nvarchar(16),		--货号
Bcode	nvarchar(16),		--条码1
Bcode2	nvarchar(20),		--条码2
Descript	nvarchar(60),	--英文描述
Cdescript	nvarchar(60),	--中文描述
Unit	nvarchar(6),		--单位
Currency	nvarchar(3),	--货币
Cat	nvarchar(5),			--品牌
Cat1	nvarchar(5),		--季节
Cat2	nvarchar(5),		--类别
Clolor	nvarchar(20),		--颜色
Size	nvarchar(20),		--尺寸
Style	nvarchar(16),		--款号
Pricea	numeric(13,2),		--价格1
Priceb	numeric(13,2),		--价格2
Pricec	numeric(13,2),		--价格3
Priced	numeric(13,2),		--价格4
Pricee	numeric(13,2),		--价格5
Disca	numeric(6,2),		--折扣1
Discb	numeric(6,2),		--折扣2
Discc	numeric(6,2),		--折扣3
Discd	numeric(6,2),		--折扣4
Disce	numeric(6,2),		--折扣5
Model	nvarchar(30),		--型号
Rolevel	int,				--预警库存
ShopId	int foreign key references shop(id),		--店铺(外键)
Balance1	int,			--店铺库存(入库)
Balance2	int,			--销售库存
Balance3	int,			--退货库存
Lastgrnd	datetime,		--收货日期
Imagefile	nvarchar(500),	--货品图片
UserId	int,				--操作人(外键）	
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50),		--默认5
Def6	nvarchar(50),		--默认6
Def7	nvarchar(50),		--默认7
Def8	nvarchar(50),		--默认8
Def9	nvarchar(50),		--默认9
Def10	nvarchar(50),		--默认10
Def11	nvarchar(50),		--默认11
)
go





/*
***************订单表*****************
*/
if exists(select * from sysobjects where name='porder')
drop table porder
create table porder(
OrderId	nvarchar(20) primary key,		--订单编号(主键)--tid
--ProductId	nvarchar(20),		--商品ID（指淘宝上）--num_iid
ShopName	nvarchar(20),		--店铺名称--seller_nick
OrderPrice	numeric(10,2),		--价格--price
OrderTime	datetime,		--下单时间--created
OrderPayTime	datetime,		--付款时间--pay_time
OrderEditTime datetime,         --修改时间--modified
OrderSendTime	datetime,		--发货时间--consign_time
OrderSucessTime	datetime,		--交易成功时间--end_time
OrderNick	nvarchar(20),		--昵称--buyer_nick
PayState int,           --支付状态--status支付订单状态(0未支付，1已支付)
OrderState	int,		--订单状态(店铺)--status
OrderState1	int,		--订单状态(源头)--status
--CustomServerId	int foreign key references users(id),		--所属客服（外键）
CustomServerId	int,		--所属客服（外键）
UserId	int,		--操作人（外键）

			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go
--state：
--//TRADE_NO_CREATE_PAY(没有创建支付宝交易) ---0
--//WAIT_BUYER_PAY(等待买家付款)  ---1
--//WAIT_SELLER_SEND_GOODS(等待卖家发货,即:买家已付款)  ---2
--//SELLER_CONSIGNED_PART（卖家部分发货）  ---3
--//WAIT_BUYER_CONFIRM_GOODS(等待买家确认收货,即:卖家已发货)  ---4
--//TRADE_BUYER_SIGNED(买家已签收,货到付款专用)  ---5
--//TRADE_FINISHED(交易成功)  ---6
--//TRADE_CLOSED(交易关闭)  ---7
--//TRADE_CLOSED_BY_TAOBAO(交易被淘宝关闭)  ---8
--//ALL_WAIT_PAY(包含：WAIT_BUYER_PAY、TRADE_NO_CREATE_PAY)  ---9
--//ALL_CLOSED(包含：TRADE_CLOSED、TRADE_CLOSED_BY_TAOBAO)  ---10
--//WAIT_PRE_AUTH_CONFIRM(余额宝0元购合约中) ---11
--//12  退款
--//其他  ---100

--alter table porder add PayState int
--alter table porder add OrderEditTime datetime
--drop table porder
--drop table OrderDetails
--drop table OrderSplit
--drop table OrderProperty
--drop table OrderExpress
--drop table CustomAndOrder



/*
***************订单详情表*****************
*/
if exists(select * from sysobjects where name='OrderDetails')
drop table OrderDetails
create table OrderDetails(
--Id	int identity(1,1),		                --自增
OrderId	nvarchar(20) foreign key references porder(OrderId),		--订单编号（外键）
OrderChildenId	nvarchar(20) primary key,		--子订单编号(主键) ---oid
ProductId	nvarchar(20),		--商品ID（指淘宝上）---num_iid
DetailsName	nvarchar(100),		--商品名称 ---title
OrderScode	nvarchar(50),		--货号--sku_id
OrderColor	nvarchar(100),		--颜色--sku_properties_name
OrderImg	nvarchar(500),		--商品图片--pic_path
DetailsSum	int,		--商品数量--num
DetailsPrice	numeric(10,2),		--商品价格--payment
OrderTime	datetime,		--下单时间
OrderPayTime	datetime,		--付款时间
OrderSendTime	datetime,		--发货时间--consign_time
OrderSucessTime	datetime,		--交易成功时间--end_time

			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go





/*
***************订单拆分表*****************
*/
if exists(select * from sysobjects where name='OrderSplit')
drop table OrderSplit
create table OrderSplit(
Id	int identity(1,1) primary key,		                --自增
OrderChildenId	nvarchar(20) foreign key references OrderDetails(OrderChildenId),		--子订单编号（外键）
OrderSplitId	nvarchar(20) foreign key references OrderDetails(OrderChildenId),		--子订单拆分编号（外键）

			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go






/*
***************订单属性表*****************
*/
if exists(select * from sysobjects where name='OrderProperty')
drop table OrderProperty
create table OrderProperty(
Id	int identity(1,1) primary key,		                --自增
OrderChildenId	nvarchar(20) foreign key references OrderDetails(OrderChildenId),		--订单子编号（外键）
OrderType	nvarchar(20),		--订单类型
OrderForm	nvarchar(20),		--订单来源
OrderMethod	nvarchar(20),		--销售方式
OrderActivePrice	numeric(10,2),		--活动费用（扣点）
OrderOtherPrice	numeric(10,2),		--其他费用
ProductFormId	int,		--订单出库源头（外键）
OrderRemark1	nvarchar(20),		--订单备注旗帜
OrderRemark	nvarchar(500),		--订单备注
OrderIsSplit	int,		--订单是否已拆分（已拆分的订单不显示，显示拆分后的订单）

			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go
/*请求日志信息表**/
if exists(select * from sysobjects where name='ApiRequestMsg')
drop table ApiRequestMsg

create table ApiRequestMsg( -- API请求日志信息表
Id int identity(1,1) primary key,-- 主键
userId nvarchar(20),-- 登录名
RqIp nvarchar(20),-- 登陆ip地址
RqApi nvarchar(200),-- 请求api接口
RqSuccess int,-- 请求是否成功(1：成功 2：失败)
RqDetails nvarchar(500),-- 请求详情（失败详细信息，成功信息）
RqTime datetime,-- 请求时间
Def1 nvarchar(50),
Def2 nvarchar(50),
Def3 nvarchar(50),
Def4 nvarchar(50),
Def5 nvarchar(50)
)
go
/*数据获取异常信息表**/
if exists(select * from sysobjects where name='ApiDataGetException')
drop table ApiDataGetException

create table ApiDataGetException-- APi 数据获取异常信息表
(
ID int identity(1,1) primary key,-- 主键
ExceptionMess nvarchar(500),-- 异常信息
ExceptionUrl nvarchar(200),-- 异常路径
ExceptionDetails nvarchar(500),-- 异常详情
ExceptionTime datetime,-- 时间
Def1 nvarchar(50),
Def2 nvarchar(50),
Def3 nvarchar(50),
Def4 nvarchar(50),
Def5 nvarchar(50)
)
go




/*
***************订单物流表*****************
*/
if exists(select * from sysobjects where name='OrderExpress')
drop table OrderExpress
create table OrderExpress(
Id	int identity(1,1),		                --自增
OrderId	nvarchar(20) foreign key references porder(OrderId),		--订单编号（外键）
OrderChildenId	nvarchar(20) foreign key references OrderDetails(OrderChildenId),		--订单子编号（外键）
ExpressNo	nvarchar(20) primary key,		--快递单号(主键)
ExpressName	nvarchar(20),		--快递公司
ExpressPrice	nvarchar(20),		--运费
ExpressFollow	nvarchar(20),		--快递追踪
CustomName	nvarchar(20),		--收件人名称
CustomPhone	nvarchar(20),		--收件人电话
CustomAddress	nvarchar(20),		--收件地址
CustomCity1	nvarchar(20),		--收件人省份
CustomCity2	nvarchar(20),		--收件人城市
CustomCity3	nvarchar(20),		--收件人地区

			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go





/*
***************客户表*****************
*/
if exists(select * from sysobjects where name='custom')
drop table custom
create table custom(
Id	int identity(1,1) primary key,		                --自增
customName	nvarchar(20),		--客户名字
customPhone	nvarchar(20),		--客户电话
customSex	int,		--客户性别
customAge	int,		--客户年龄
customCity	nvarchar(20),		--客户城市
customAddress	nvarchar(200),		--客户地址
customIndex	int,		--排序
UserId	int,		--操作人（外键）
CustomServiceId	int foreign key references users(id),		--所属客服(外键)
CustomTime	datetime,		--创建时间


			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go




/*
***************客户属性表*****************
*/
if exists(select * from sysobjects where name='CustomProperty')
drop table CustomProperty
create table CustomProperty(
Id	int identity(1,1) primary key,		                --自增
CustomId	int foreign key references custom(id),		--客户编号(外键)
CustomLevel	nvarchar(5),		--客户等级
CustomBuyCount	int,		--客户购买次数
CustomBuyAmount	numeric(10,2),		--客户购买金额
CustomLoveBrand	nvarchar(50),		--客户喜爱品牌
CustomServiceComment	nvarchar(500),		--客服评价
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go




/*
***************客户与订单关系表*****************
*/
if exists(select * from sysobjects where name='CustomAndOrder')
drop table CustomAndOrder
create table CustomAndOrder(
Id	int identity(1,1) primary key,		                --自增
CustomId	int foreign key references custom(id),		--客户编号(外键)
OrderId	nvarchar(20) foreign key references porder(OrderId),		--订单编号(外键)
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go



/*
***************海关完税价格以及税率表*****************
*/
if exists(select * from sysobjects where name='Tarifftab')
drop table Tarifftab
create table Tarifftab(
Id	        int identity(1,1) primary key,      --自增(主键)
TariffNo        nvarchar(50),		            --税号
TariffName	nvarchar(50),		            --品名及规格
Unit      	nvarchar(50),		            --单位
DutiableValue	nvarchar(50),		            --完税价格
Tariff	        float,		                    --税率
ConditionOne	nvarchar(50),		            --商检监管条件
ConditionTwo	nvarchar(50),		            --海关监管条件
TariffIndex	int,		                    --排序
UserId  	int,		                    --操作人(外键）
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go

/*
***************类别和税号关系表*****************
*/
if exists(select * from sysobjects where name='TypeIdToTariffNo')
drop table TypeIdToTariffNo
create table TypeIdToTariffNo(
Id	        int identity(1,1) primary key,      --自增(主键)
TypeNo          nvarchar(50),		            --类别
TariffNo	nvarchar(50),		            --税号		            
NoIndex  	int,		                    --排序
UserId  	int,		                    --操作人(外键）
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go

if exists(select * from sysobjects where name='HSInfomation')---HS编码以及相关信息

drop table HSInfomation
create table HSInfomation(
Id int identity(1,1) primary key,--自增(主键)
HSNumber nvarchar(50) ,		--HS编码
TypeName nvarchar(500) ,	--商品名称
tariff nvarchar(50) ,		--最惠国税率    			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go

/*
***************存放excel的路径*****************
*/
if exists(select * from sysobjects where name='excelFilePath')
drop table excelFilePath
create table excelFilePath(
Id	int identity(1,1) primary key,      --自增(主键) 
FilePath	nvarchar(100) ,	         	--存放excel路径
tableIndex int,                         --排序
UserId	int,		                    --操作人(外键）			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go


/*

***************api订单表*****************
*/
create table apiOrder
(
	orderId nvarchar(20) primary key,
	realName nvarchar(10),
	provinceId nvarchar(10), --省ID
	cityId nvarchar(10),--=城市ID
	district nvarchar(10),--=区域ID
	buyNameAddress nvarchar(200),--=地址信息
	postcode nvarchar(10),--=邮政编码
	phone nvarchar(20),--=手机号码
	orderMsg nvarchar(200),-- =订单备注
	orderStatus int,--=订单状态 返回值中 OrderStatus字段值 1 待确认 2 确认 3 待发货 4 发货 5 收货（交易成功） 11 退单 12 取消订单
	itemPrice money,--=商品总价
	deliveryPrice money,--=快递费用
	favorablePrice money,--=优惠费用
	taxPrice money,--=税费
	orderPrice money,--=订单金额（商品总价+快递费用-优惠费用+税费）
	paidPrice money,--=实际支付金额
	isPay int,--=0：未支付，1：已支付
	payTime datetime,--=支付时间
	payOuterId nvarchar(100),--=支付流水号
	createTime datetime,--=订单创建时间
	
	invoiceType nvarchar(10),--=发票类型
	invoiceTitle nvarchar(500),--=发票抬头

	def1 nvarchar(20),--身份证号码
	def2 nvarchar(20),--是否已分配供应商
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20)
)
go
/*
***************订单信息表*****************
*/
CREATE TABLE [dbo].[apiOrder1](
	orderId nvarchar(20) primary key,
	realName nvarchar(10),
	provinceId nvarchar(10), --省ID
	cityId nvarchar(10),--=城市ID
	district nvarchar(10),--=区域ID
	buyNameAddress nvarchar(200),--=地址信息
	postcode nvarchar(10),--=邮政编码
	phone nvarchar(20),--=手机号码
	orderMsg nvarchar(200),-- =订单备注
	orderStatus int,--=订单状态 返回值中 OrderStatus字段值 1 待确认 2 确认 3 失败 4 发货 5 收货 11 退单
	itemPrice money,--=商品总价
	deliveryPrice money,--=快递费用
	favorablePrice money,--=优惠费用
	taxPrice money,--=税费
	orderPrice money,--=订单金额（商品总价+快递费用-优惠费用+税费）
	paidPrice money,--=实际支付金额
	isPay int,--=0：未支付，1：已支付
	payTime datetime,--=支付时间
	payOuterId nvarchar(100),--=支付流水号
	createTime datetime,--=订单创建时间
	
	invoiceType nvarchar(10),--=发票类型
	invoiceTitle nvarchar(500),--=发票抬头

	def1 nvarchar(20),--身份信息
	def2 nvarchar(20),--是否已分配供应商
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20)
	)
GO
--drop table apiOrder


/*
***************api订单详情表*****************
*/
create table apiOrderDetails(
	orderId nvarchar(20) foreign key references apiOrder(orderId),
	detailsOrderId nvarchar(20) primary key,
	detailsScode nvarchar(20),--=商品scode
	detailsColor nvarchar(20), --颜色
	detailsImg nvarchar(500), --图片
	detailsItemPrice money,--=商品售价
	detailsTaxPrice money,--=税费金额
	detailsSaleCount int,--=销售数量
	detailsDeliveryPrice money,--=快递费用
	detailsStatus int,--订单状态
	
	detailsTime datetime, --下单时间
	detailsPayTime datetime, --付款时间
	detailsEditTime datetime, --编辑时间
	detailsSendTime datetime, --发送时间
	detailsSucessTime datetime, --成交时间
	
	def1 nvarchar(20),--网址
	def2 nvarchar(20),
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20)
)
go
/*
***************订单详情*****************
*/


CREATE TABLE [dbo].[apiOrderDetails1](
	[orderId] [nchar](20) NOT NULL,--订单号
	[productScode] [nvarchar](50) NOT NULL,-- 货号
	[orderNum] [int] NOT NULL,--数量
	[orderCreateTime] [datetime] NOT NULL,--时间
	[detailsStatus] [int] NOT NULL,
	[UserName] [nvarchar](20) NOT NULL,--插入人
	[reserved] [int] NOT NULL,--状态
 CONSTRAINT [PK_apiOrderDetails1] PRIMARY KEY CLUSTERED 
(
	[orderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
--select * from  apiOrderDetails


/*
***************api订单支付详情表*****************
*/
create table apiOrderPayDetails(
	orderId  nvarchar(20) foreign key references apiOrder(orderId),
	payMentName nvarchar(20),--=支付方式
	payPlatform nvarchar(20),--=支付平台（ios/andriod/html5）
	payId nvarchar(20),--=支付内部流水号
	payOuterId nvarchar(20),--=支付外部流水号
	payPrice money,--=支付金额
	payTime datetime,--=支付时间
	sellerAccount nvarchar(20),--=卖家支付账号
	buyerAccount nvarchar(20),--=买家支付账号
	
	def1 nvarchar(20),
	def2 nvarchar(20),
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20)
)
go


/*
***************api订单活动优惠表*****************
*/
create table apiOrderDiscount
(
	orderId  nvarchar(20) foreign key references apiOrder(orderId),
	dicountType int,--=优惠类型
	dicountAmount money,--=优惠金额
	remark nvarchar(500),--=优惠备注
	
	def1 nvarchar(20),
	def2 nvarchar(20),
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20)
)
go


/*
***************app源订单表(供应商发货)*****************
*/
create table apiSendOrder
(
	orderId  nvarchar(20) foreign key references apiOrder(orderId),--订单
	detailsOrderId nvarchar(20) foreign key references apiOrderDetails(detailsOrderId),--子订单
	newOrderId  nvarchar(20) primary key ,--新订单ID
	newScode nvarchar(20),--货号
	newColor nvarchar(20),--颜色
	newSize nvarchar(20),--尺码
	newImg nvarchar(500),--图片
	newSaleCount int ,--数量
	newStatus int,--状态(订单当前状态：1为待确认，2为确认，3为待发货，4为发货，5交易成功，6通关异常，7，通关成功，11退货，12取消)
	createTime datetime,--创建时间
	editTime datetime,--发送时间
	showStatus int,--显示状态(0不显示，1显示)
	sendSource nvarchar(20),--属于供应商
	
	def1 nvarchar(20), --商检报关状态(0失败，1成功，2未上传)
	def2 nvarchar(20), --海关报关状态(0失败，1成功，2未上传)
	def3 nvarchar(20), --联邦报关状态(0失败，1成功，2未上传)
	def4 nvarchar(20),
	def5 nvarchar(20)
)
go

/*
***************api源订单快递表(供应商发货)*****************
*/
create table apiOrderExpress
(
	orderId  nvarchar(20) foreign key references apiOrder(orderId),--订单
	detailsOrderId  nvarchar(20) foreign key references apiOrderDetails(detailsOrderId),--订单
	newOrderId  nvarchar(20) foreign key references apiSendOrder(newOrderId) ,--新订单ID
	
	expressId nvarchar(20),--快递编号
	expressNo nvarchar(20),--快递单号
	
	sourceCity1 nvarchar(20),--发送地
	sourceCity2 nvarchar(20),--发送地
	sourceCity3 nvarchar(20),--发送地
	sourceAddress nvarchar(500),--发送地
	sourcePhone nvarchar(20) default(0755-25182899),--发送人电话(深圳)
	
	customCity1 nvarchar(20),--接收地
	customCity2 nvarchar(20),--接收地
	customCity3 nvarchar(20),--接收地
	customAddress nvarchar(500),--接收地
	
	productPrice money,--商品价格
	supportValuePrice money, --保价
	
	
	def1 nvarchar(20),
	def2 nvarchar(20),
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20)
)
go



/*
***************快递表*****************
*/
create table apiExpress
(
	expressId nvarchar(20), --快递编号
	expressName  nvarchar(20), --快递名称
	expressPrincipal nvarchar(20), --负责人
	expressPhone nvarchar(20), --电话
	expressAddress nvarchar(200), --快递公司地址
	
	def1 nvarchar(20),
	def2 nvarchar(20),
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20)
)
go

			

/*查询表中字段名称*/
select name from sys.syscolumns where id =object_id('tableFiledPerssion')


/*往tableFiledPerssion表中插入table表名，table表等级默认为0*/
insert into tableFiledPerssion(tableName,tableLevel) values('tableFiledPerssion',0)
insert into tableFiledPerssion(tableName,tableLevel) values('persona',0)
insert into tableFiledPerssion(tableName,tableLevel) values('users',0)
insert into tableFiledPerssion(tableName,tableLevel) values('taoAppUser',0)
insert into tableFiledPerssion(tableName,tableLevel) values('menu',0)
insert into tableFiledPerssion(tableName,tableLevel) values('funpermisson',0)
insert into tableFiledPerssion(tableName,tableLevel) values('filedpermisson',0)
insert into tableFiledPerssion(tableName,tableLevel) values('brand',0)
insert into tableFiledPerssion(tableName,tableLevel) values('productbigtype',0)
insert into tableFiledPerssion(tableName,tableLevel) values('producttype',0)
insert into tableFiledPerssion(tableName,tableLevel) values('errorlog',0)
insert into tableFiledPerssion(tableName,tableLevel) values('productsource',0)
insert into tableFiledPerssion(tableName,tableLevel) values('productsourcestock',0)
insert into tableFiledPerssion(tableName,tableLevel) values('productstock',0)
insert into tableFiledPerssion(tableName,tableLevel) values('product',0)
insert into tableFiledPerssion(tableName,tableLevel) values('shoptype',0)
insert into tableFiledPerssion(tableName,tableLevel) values('shop',0)
insert into tableFiledPerssion(tableName,tableLevel) values('shopproduct',0)
insert into tableFiledPerssion(tableName,tableLevel) values('porder',0)
insert into tableFiledPerssion(tableName,tableLevel) values('OrderDetails',0)
insert into tableFiledPerssion(tableName,tableLevel) values('OrderSplit',0)
insert into tableFiledPerssion(tableName,tableLevel) values('OrderProperty',0)
insert into tableFiledPerssion(tableName,tableLevel) values('OrderExpress',0)
insert into tableFiledPerssion(tableName,tableLevel) values('custom',0)
insert into tableFiledPerssion(tableName,tableLevel) values('CustomProperty',0)
insert into tableFiledPerssion(tableName,tableLevel) values('Tarifftab',0)
insert into tableFiledPerssion(tableName,tableLevel) values('TBBrand',0)
insert into tableFiledPerssion(tableName,tableLevel) values('TBProducttype',0)
insert into tableFiledPerssion(tableName,tableLevel) values('ItalyUpdateError',0)

/*往tableFiledPerssion表中插入table字段，table字段等级为table表名ID*/
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='tableFiledPerssion') from sys.syscolumns where id =object_id('tableFiledPerssion')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='persona') from sys.syscolumns where id =object_id('persona')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='users') from sys.syscolumns where id =object_id('users')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='taoAppUser') from sys.syscolumns where id =object_id('taoAppUser')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='menu') from sys.syscolumns where id =object_id('menu')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='funpermisson') from sys.syscolumns where id =object_id('funpermisson')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='filedpermisson') from sys.syscolumns where id =object_id('filedpermisson')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='brand') from sys.syscolumns where id =object_id('brand')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='productbigtype') from sys.syscolumns where id =object_id('productbigtype')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='producttype') from sys.syscolumns where id =object_id('producttype')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='errorlog') from sys.syscolumns where id =object_id('errorlog')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='productsource') from sys.syscolumns where id =object_id('productsource')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='productsourcestock') from sys.syscolumns where id =object_id('productsourcestock')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='productstock') from sys.syscolumns where id =object_id('productstock')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='product') from sys.syscolumns where id =object_id('product')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='shoptype') from sys.syscolumns where id =object_id('shoptype')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='shop') from sys.syscolumns where id =object_id('shop')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='shopproduct') from sys.syscolumns where id =object_id('shopproduct')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='porder') from sys.syscolumns where id =object_id('porder')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='OrderDetails') from sys.syscolumns where id =object_id('OrderDetails')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='OrderSplit') from sys.syscolumns where id =object_id('OrderSplit')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='OrderProperty') from sys.syscolumns where id =object_id('OrderProperty')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='OrderExpress') from sys.syscolumns where id =object_id('OrderExpress')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='custom') from sys.syscolumns where id =object_id('custom')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='CustomProperty') from sys.syscolumns where id =object_id('CustomProperty')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='Tarifftab') from sys.syscolumns where id =object_id('Tarifftab')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='TBBrand') from sys.syscolumns where id =object_id('TBBrand')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='TBProducttype') from sys.syscolumns where id =object_id('TBProducttype')
insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='ItalyUpdateError') from sys.syscolumns where id=object_id('ItalyUpdateError')

select * from tableFiledPerssion


select * from productstock













/*start*********************************************************lcg***********************************************************/
/*
***************计量单位表（海关和商检基本相同，只有部分不同，但是海关的数据比商检多，所以以海关计量单位作为计量单位）*****************
*/
create table apiUnit
(
	unitNo nvarchar(20), --单位编号
	unitName  nvarchar(20), --单位名称
	
	def1 nvarchar(20),
	def2 nvarchar(20),
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20)
)
go


/*
***************商品海关报备*****************
*/
create table CiqProductDetails
(
	ciqProductId nvarchar(128) not null,  --商品申请编号(最大128字符,跨境电商企业系统中的唯一编号字段)
	ciqRemark nvarchar(1000),--商品备注
	ciqScode nvarchar(50) primary key not null ,--货号
	ciqScodeName nvarchar(50) not null,--商品名称
	ciqSpec nvarchar(200) not null,--规格型号
	ciqHSNo nvarchar(200) not null,--商品HS编码
	ciqAssemCountry nvarchar(200) default('yidali') not null, --原产国/地区
	ciqProductNo int identity(1,1),--商品序号（订单报备EntGoodsNo字段）长度小于6
	
	def1 nvarchar(200),
	def2 nvarchar(200),
	def3 nvarchar(200),
	def4 nvarchar(200),
	def5 nvarchar(200)
)
go

--drop table CiqProductDetails
select * from CiqProductDetails




/*
***************淘宝店铺活动*****************
*/
create table activeShop(
	acId int identity(1,1) primary key,
	acName nvarchar(200), --活动名称
	actimePrepareStart datetime default(getdate()), --活动预热开始时间
	actimePrepareEnd  datetime default(getdate()), --活动预热结束时间
	acTimestart datetime default(getdate()), --活动开始时间
	acTimeend datetime default(getdate()), --活动结束时间
	acCreateTime datetime default(getdate()), --创建时间
	
	def1 nvarchar(200),
	def2 nvarchar(200),
	def3 nvarchar(200),
	def4 nvarchar(200),
	def5 nvarchar(200)
)
go
select * from activeShop



/*
***************淘宝店铺活动商品*****************
*/
create table activeShopOrder(
	asoId int identity(1,1) primary key,
	acId int foreign key references activeShop(acId),		--店铺活动ID(外键)
	asoScode nvarchar(200), --货号
	asoCreateTime  datetime default(getdate()), --商品加入时间
	
	def1 nvarchar(200),
	def2 nvarchar(200),
	def3 nvarchar(200),
	def4 nvarchar(200),
	def5 nvarchar(200)
)
go
select * from activeShopOrder



/*
***************订单留言其它*****************
*/
create table orderComments
(
	ocId int identity(1,1) primary key,
	ocOrderId nvarchar(20),--货号
	ocScode nvarchar(200),--货号
	ocComment nvarchar(500),--评价
	ocRemark nvarchar(500),--备注
	ocBanner nvarchar(10),--旗帜
	ocPostPrice money,--邮费
	ocOtherPrice money,--其它价格
	
	def1 nvarchar(200),
	def2 nvarchar(200),
	def3 nvarchar(200),
	def4 nvarchar(200),
	def5 nvarchar(200)
)
go


/*
***************支付报关结果*****************
*/
create table payCustomsResult
(
	OrderChildId nvarchar(20) primary key, --子订单ID

	--基本参数
	isSuccess nvarchar(2) not null,--请求是否成功请求是否成功。请求成功不代表业务处理成功。(T代表成功,F代表失败)
	sign_type nvarchar(5),--签名方式(DSA、RSA、MD5 三个值可选，必须大写)
	sign_qm nvarchar(50),--签名(请参见“7 签名机制”。)
	error nvarchar(500),--错误代码(请求成功时，不存在本参数；请求失败时，本参数为错误代码，参见“8.2 接入错误码”和“8.3 系统错误码”)
	
	--业务参数
	result_code nvarchar(32) not null, --响应码(处理结果响应码。SUCCESS：成功,FAIL：失败)
	trade_no nvarchar(64),--支付宝交易号(该交易在支付宝系统中的交易流水号。最长 64 位。)
	alipay_declare_no nvarchar(64),--支付宝报关流水号(支付宝报关流水号)
	detail_error_code nvarchar(48),--详细错误码(对返回响应码进行原因说明，请参见“8.1 业务错误码”。当 result_code 响应码为SUCCESS 时，不返回该参数)
	detail_error_des nvarchar(64),--详细错误描述(对详细错误码进行文字说明，请参见“8.1 业务错误码”中的“含义”。当 result_code 响应码为SUCCESS 时，不返回该参数。)

	def1 nvarchar(50),
	def2 nvarchar(50),
	def3 nvarchar(50),
	def4 nvarchar(50),
	def5 nvarchar(50),
)
go


/*
***************订单报关结果*****************
*/

--商检报关状态(0失败，1成功，2未上传,3上传成功)
--海关报关状态(0失败，1成功，2未上传,3上传成功)
--联邦报关状态(0失败，1成功，2未上传,3上传成功)
create table orderCustomsResult
(
	SJOrgOrderChildId nvarchar(20)  primary key, --子订单ID

	--商检
	--SJOrgMessageID nvarchar(50),--原发送报文中报文编号，最大50字符
	SJOrgReturnTime datetime,--回执时间	格式：YYYY-MM-DD HH:MI:SS  --OrgSendTime
	SJOrgSendTime datetime,--发送时间	格式：YYYYMMDDHHMISS  --OrgRecTime
	SJOrgStatus nvarchar(20),	--申报状态	申报状态，长度为2个字符，10：已接收（等待审核）20：申报失败
	SJOrgNotes nvarchar(1000),--申报反馈信息	如申报失败，则放错误信息。最大长度1000个字符。
	SJstatus nvarchar(1), --订单状态(0失败，1成功，2未上传,3上传成功)

	--海关
	--HGOldMessageId nvarchar(30), --对应原报文的编号
	HGSendDate datetime, --报文发送时间
	HGReturnDate datetime, --回执发送时间
	HGReturnCode nvarchar(5),--处理状态(c01成功)
	HGReturnInfo nvarchar(255),--回执说明
	HGAttachedFlag nvarchar(2),--0—无数据附件、1—有数据附件；=0—无数据附件：回执报文中，无业务数据；=1—有数据附件：回执报文中，有业务数据；
	HGstatus nvarchar(1), --订单状态(0失败，1成功，2未上传,3上传成功)

	--物流
	BBCask int,--1成功，0失败
	BBCSendDate datetime, --报文发送时间
	BBCReturnData datetime, --回执时间
	BBCmessage nvarchar(200), --返回信息
	BBCorderCode nvarchar(200), --oms的订单号
	BBCerrorMessage nvarchar(1000), --错误明细
	BBCstatus nvarchar(10), --订单状态(0失败，1成功)


	def1 nvarchar(50), --商检报文名称(不带后缀)
	def2 nvarchar(50), --海关报文名称(不带后缀)
	def3 nvarchar(50),
	def4 nvarchar(50),
	def5 nvarchar(50),
)
go


/*
***************商品报关结果*****************
*/
create table productCustomsResult
(
	productScode nvarchar(20) primary key,  --货号

	--商检
	SJOrgReturnTime datetime,--回执时间	格式：YYYY-MM-DD HH:MI:SS  --OrgSendTime
	SJOrgSendTime datetime,--发送时间	格式：YYYYMMDDHHMISS  --OrgRecTime
	SJOrgStatus nvarchar(20),	--申报状态	申报状态，长度为2个字符，10：已接收（等待审核）20：申报失败
	SJOrgNotes nvarchar(1000),--申报反馈信息	如申报失败，则放错误信息。最大长度1000个字符。
	SJstatus nvarchar(1), --订单状态(0失败，1成功，2未上传,3上传成功)
	--商检审核
	CIQGoodsNO nvarchar(50), --	商品备案号	最大50字符,商品审核通过时给的一个商品备案号。
	RegStatus  nvarchar(10), --ICIP回执状态 最大4字符 10:通过;20:不通过
	RegNotes nvarchar(256), --ICIP回执信息	最大256字符

	--海关



	--物流
	BBCask nvarchar(1), -- 1: 成功 0: 失败
	BBCSendDate datetime, --报文发送时间
	BBCReturnData datetime, --回执时间
	BBCmessage nvarchar(500), --系统返回的信息
	BBCskuNo nvarchar(50), --成功新增产品后的产品Sku代码
	BBCerrorMessage nvarchar(500), --错误信息

	def1 nvarchar(50),--报文名称(不带后缀)
	def2 nvarchar(50),
	def3 nvarchar(50),
	def4 nvarchar(50),
	def5 nvarchar(50)
)
go


--//添加列,默认为0
--alter table sysQuestionType(表名) add stSon(列名) int default(0)
--//删除列
--alter table usertable(表名) drop column ExtendPPCount(列名)
--//修改列的类型
--alter table usertable(表名) alter column ExtendPPCount(列名) nvarchar(50)(类型)
--//表格外部创建外键（ExamScore表为外键表，ExamMessage为主键表。）
--alter table ExamScore add constraint fk_message_score foreign key(messageId)  references ExamMessage(messageId)

/*
***************修改apiorder表，def1，是否已分配供应商(20150702)*****************
*/



/*
***************商品报关结果*****************
*/
create table customsCountry
(
	id int identity(1,1) primary key,--id
	countryName nvarchar(50),--国家名称
	countryNo nvarchar(20),--国家编号
	
	def1 nvarchar(20),
	def2 nvarchar(20),
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20),
)
go

select * from customsCountry


/*
***************系统日志*****************
*/
if exists(select * from sysobjects where name='systemErrorLog')
drop table systemErrorLog
create table systemErrorLog
(
	id int primary key,                             --编号
	errorMsg nvarchar(1000),                        --异常信息
	errorObject nvarchar(1000),                     --异常对象
	errorStackTrace nvarchar(1000),                 --调用堆栈
	errorMethod nvarchar(1000),                     --触发方法
	errorMenu nvarchar(50),                         --异常所在菜单
	errorFunc nvarchar(50),                         --异常所在功能
	errorUser nvarchar(50),                         --异常触发人
	errorIP nvarchar(50),                           --异常触发人所在地址
	createdatetime datetime default(getdate()),     --异常时间

	Def1	nvarchar(50),		     --默认1
	Def2	nvarchar(50),		     --默认2
	Def3	nvarchar(50),		     --默认3
	Def4	nvarchar(50),		     --默认4
	Def5	nvarchar(50)		     --默认5
)
go


/*
***************操作日志*****************
*/
if exists(select * from sysobjects where name='operatingLog')
drop table operatingLog
create table operatingLog
(
	id int primary key,                              --编号
	operatingMenu nvarchar(50),                      --操作所在菜单
	operatingFunc nvarchar(50),                      --操作所在功能
	operatingUser nvarchar(50),                      --操作人
	operatingIP nvarchar(50),                        --操作人所在地址
	operatingDate datetime default(getdate()),       --操作时间

	Def1	nvarchar(50),		     --默认1
	Def2	nvarchar(50),		     --默认2
	Def3	nvarchar(50),		     --默认3
	Def4	nvarchar(50),		     --默认4
	Def5	nvarchar(50)		     --默认5
)
go






/*end*********************************************************lcg***********************************************************/






/**********************************************************sqy***********************************************************/.

/*
***************淘宝品牌表*****************
*/    
if exists(select * from sysobjects where name='TBBrand')
drop table TBBrand
create table TBBrand(
Id int identity(1,1) primary key,--自增(主键)
TBBrandName nvarchar(30) ,	         --淘宝品牌名
vid nvarchar(1000) ,		     --淘宝vid		
Def1	nvarchar(50),		     --默认1
Def2	nvarchar(50),		     --默认2
Def3	nvarchar(50),		     --默认3
Def4	nvarchar(50),		     --默认4
Def5	nvarchar(50)		     --默认5
)
go

/*
***************淘宝类别表*****************
*/
  if exists(select * from sysobjects where name='TBProducttype')
drop table TBProducttype
create table TBProducttype(
Id int identity(1,1) primary key,--自增(主键)
TBtypeName nvarchar(30) ,	     --淘宝类别名
cid nvarchar(1000) ,		     --淘宝cid	
Def1	nvarchar(50),		     --默认1
Def2	nvarchar(50),		     --默认2
Def3	nvarchar(50),		     --默认3
Def4	nvarchar(50),		     --默认4
Def5	nvarchar(50)		     --默认5
)
go

/*
***************淘宝属性表*****************
*/
  if exists(select * from sysobjects where name='TBProductProperty')
drop table TBProductProperty
create table TBProductProperty(
Id int identity(1,1) primary key,--自增(主键)
TBPropertyName nvarchar(30) ,	     --淘宝属性名
vid nvarchar(1000) ,		     --淘宝vid
parent_cid nvarchar(1000) ,	     --淘宝cid				
Def1	nvarchar(50),		     --默认1
Def2	nvarchar(50),		     --默认2
Def3	nvarchar(50),		     --默认3
Def4	nvarchar(50),		     --默认4
Def5	nvarchar(50)		     --默认5
)
go

/*
***************淘宝属性值表*****************
*/
  if exists(select * from sysobjects where name='TBProductPropertyValue')
drop table TBProductPropertyValue
create table TBProductPropertyValue(
Id int identity(1,1) primary key,--自增(主键)
TBPropertyValue nvarchar(30) ,	     --淘宝属性值
vid nvarchar(1000) ,		     --淘宝属性值vid	
parent_vid nvarchar(1000) ,          --淘宝属性vid
Def1	nvarchar(50),		     --默认1
Def2	nvarchar(50),		     --默认2
Def3	nvarchar(50),		     --默认3
Def4	nvarchar(50),		     --默认4
Def5	nvarchar(50)		     --默认5
)
go

/*
***************供应商品牌对应表*****************
*/
if exists(select * from sysobjects where name='BrandVen')
drop table BrandVen
create table BrandVen(
Id	int identity(1,1) primary key,		--自增(主键)
BrandName	nvarchar(20),		--品牌名称
BrandAbridge	nvarchar(20),		--品牌名称缩写
BrandCode	nvarchar(20),		--名牌编码
BrandNameVen	nvarchar(20),		--供应商品牌名称
Vencode	nvarchar(20),			--品牌数据源
BrandIndex	int,		--排序
UserId	int,		--操作人(外键）
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50),		--默认5
)
go

/*
***************供应商类别表*****************
*/
if exists(select * from sysobjects where name='producttypeVen')
drop table producttypeVen
create table producttypeVen(
Id	int identity(1,1) primary key,		--自增(主键)
BigId	Int	foreign key references productbigtype(id),	--大类别ID（外键）
TypeName	nvarchar(20),		--类别名称
TypeNo	nvarchar(20),		--类别编码
TypeNameVen	nvarchar(20),		--供应商类别
Vencode	nvarchar(20),		--供应商
typeIndex	int,		--排序
UserId	int,		--操作人(外键）

Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50),		--默认5
)
go

/*
***************订单备注信息表*****************
*/
if exists(select * from sysobjects where name='apiOrderRemark')
drop table apiOrderRemark
create table apiOrderRemark(
Id int identity(1,1) primary key,--自增(主键)
OrderId nvarchar(30) ,	--主订单号
Remark nvarchar(max) ,		--备注信息
Edittime nvarchar(20),      --操作时间	
UserId int,		            --操作人(外键）		
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)

/*
***************商品报关结果*****************
*/
create table customsCountry
(
	id int identity(1,1) primary key,
	countryName nvarchar(50),
	countryNo nvarchar(20),
	
	def1 nvarchar(20),
	def2 nvarchar(20),
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20),
)
go
select * from customsCountry



/***************交易信息表******************/
if exists(select * from sysobjects where name='TradeInfo')
drop table TradeInfo
create table TradeInfo(
Id	int identity(1,1) primary key,		--自增(主键)
OrderId	nvarchar(50),					--订单编号
SellPrice	nvarchar(50),				--成交总金额
SaleStates	nvarchar(50),				--交易是否成功 1.已付款 2.未付款 3.交易成功 4.交易关闭(3和4需要判断通过Productinfo)
Evaluate	nvarchar(50),				--是否评价
ServiceId	nvarchar(50),				--接待客服
ServiceRemark	nvarchar(max),			--客服备注
OrderTime	datetime,				    --下单日期
UserId int,								--操作人
			
Def1	nvarchar(50),					--默认1
Def2	nvarchar(50),					--默认2
Def3	nvarchar(50),					--默认3
Def4	nvarchar(50),					--默认4
Def5	nvarchar(50),					--默认5
)
go


/***************订单客户信息表******************/
if exists(select * from sysobjects where name='CustomerInfo')
drop table CustomerInfo
create table CustomerInfo(
Id	int identity(1,1) primary key,		--自增(主键)
OrderId	nvarchar(50),					--订单编号
CustomerId	nvarchar(50),				--客户Id
Shop	nvarchar(50),					--店铺/平台
Contactperson	nvarchar(50),			--联系人
Telephone	nvarchar(50),				--电话
Phone	nvarchar(50),					--手机号
Weixin	nvarchar(50),					--微信
QQNo	nvarchar(50),					--QQ
Provinces	nvarchar(50),				--省
City	nvarchar(50),					--市
CusAddress	nvarchar(max),				--发货地址	
Payment	nvarchar(50),					--支付平台
Account	nvarchar(50),					--支付账号
Def1	nvarchar(50),					--默认1
Def2	nvarchar(50),					--默认2
Def3	nvarchar(50),					--默认3
Def4	nvarchar(50),					--默认4
Def5	nvarchar(50),					--默认5
)
go


/***************商品信息表******************/
if exists(select * from sysobjects where name='ProductInfo')
drop table ProductInfo
create table ProductInfo(
Id	int identity(1,1) primary key,		--自增(主键)
OrderId	nvarchar(50),					--订单编号
Scode	nvarchar(50),					--货品编号
Brand	nvarchar(50),					--品牌 -无效
Color	nvarchar(50),					--颜色 -无效
TypeNo	nvarchar(50),					--类别 -无效
Imagefile	nvarchar(200),				--商品示图 -无效
Size	nvarchar(50),					--尺码 -无效
Number	nvarchar(50),					--数量
ProDetails	nvarchar(max),				--货品描述
ProLink	nvarchar(50),					--商品链接
DeliveryAttri	nvarchar(50),			--发货属性
LastOrderId	nvarchar(50),				--上次订单号
SellPrice	nvarchar(50),				--成交金额
Warehouse	nvarchar(50),				--出货仓
UserId int,								--操作人

Def1	nvarchar(50),					--发货状态1.未发货 2.已发货  3.退货中  4.退货完成  5.退款中  6.退款完成   7.交易成功
Def2	nvarchar(50),					--系统单号
Def3	datetime,						--默认3--最后操作时间
Def4	nvarchar(50),					--默认4
Def5	nvarchar(50),					--默认5
)
go


/***************退货信息******************/
if exists(select * from sysobjects where name='RetProductInfo')
drop table RetProductInfo
create table RetProductInfo(
Id	int identity(1,1) primary key,		--自增(主键)
OrderId	nvarchar(50),					--订单编号
RetPrice	nvarchar(50),				--退款金额
Express	nvarchar(50),					--快递公司
ExpressNo	nvarchar(50),				--快递单号
RetDetails	nvarchar(max),				--退货说明
RetType	nvarchar(50),					--退换货类型 1.退款 2.退货 5.退货(未发货)
Receiver 	nvarchar(50),				--收货人	
ServiceId	nvarchar(50),				--处理客服
RetAccount	nvarchar(50),				--退款账号
UserId int,								--操作人 
		
Def1	nvarchar(50),					--退货状态 1.申请退款 2.申请退货3.退款成功 4.退货成功 5.申请退货(未发货) 6.退货已收
Def2	nvarchar(50),					--货品编号
Def3	nvarchar(50),					--退货理由 1.七天无理由 2.有色差 3.其他
Def4	datetime,						--默认4--最后操作时间
Def5	nvarchar(50),					--默认5
)
go



/***************出货记录******************/
if exists(select * from sysobjects where name='ShipmentRecord')
drop table ShipmentRecord
create table ShipmentRecord(
Id	int identity(1,1) primary key,		--自增(主键)
OrderId	nvarchar(50),					--订单编号
ExPrice	nvarchar(50),					--换货金额
SendTime	datetime,					--发货时间
Express	nvarchar(50),					--快递公司
ExpressNo		nvarchar(50),			--快递单号
YFHKD	nvarchar(50),					--运费HKD
YFRMB	nvarchar(50),					--运费RMB
RetRemark	nvarchar(max),				--退货说明
SendPerson	nvarchar(50),				--发货人
SendType	nvarchar(50),				--出货类型 1.新订单 
SendStatus	nvarchar(50),				--发货状态 1.已发货 2.订单关闭
UserId int,								--操作人
			
Def1	nvarchar(50),					--默认1
Def2	nvarchar(50),					--货品编号
Def3	nvarchar(50),					--默认3
Def4	nvarchar(50),					--默认4
Def5	nvarchar(50),					--默认5
)
go






/***************退货库存表******************/
if exists(select * from sysobjects where name='RetBalance')
drop table RetBalance
create table RetBalance(
Id	int identity(1,1) primary key,		--自增(主键)
OrderId	nvarchar(50),					--订单编号
Scode	nvarchar(50),					--货品编号
Brand	nvarchar(50),					--品牌 -无效
Color	nvarchar(50),					--颜色 -无效
TypeNo	nvarchar(50),					--类别 -无效
Imagefile	nvarchar(200),				--商品示图 -无效
Size	nvarchar(50),					--尺码 -无效
Price	nvarchar(50),					--交易价
Number	nvarchar(50),					--数量
QuaLevel	nvarchar(50),				--质量等级
RetTime	datetime,						--收货时间
ProDetails	nvarchar(max),				--货品描述
ProLink	nvarchar(50),					--商品链接
RetNum	nvarchar(50),					--退货次序
LastOrderId	nvarchar(50),				--上次订单号
SellPrice	nvarchar(50),				--成交金额
Heidui	nvarchar(50),					--核对
ExBalance	nvarchar(50),				--核对转库
UserId int,								--操作人

Def1	nvarchar(50),					--默认1
Def2	nvarchar(50),					--默认2
Def3	nvarchar(50),					--默认3
Def4	nvarchar(50),					--默认4
Def5	nvarchar(50),					--默认5
)
go



/***************操作记录表******************/
if exists(select * from sysobjects where name='OperationRecord')
drop table OperationRecord
create table OperationRecord(
Id	int identity(1,1) primary key,		--自增(主键)
OrderId	nvarchar(50),					--订单编号
OperaTable	nvarchar(50),				--操作表格
OperaType	nvarchar(50),				--操作类型
OperaTime	datetime,					--操作时间
UserId int,								--操作人
Def1	nvarchar(50),					--默认1
Def2	nvarchar(50),					--默认2
Def3	nvarchar(50),					--默认3
Def4	nvarchar(50),					--默认4
Def5	nvarchar(50),					--默认5
)


/***************客户信息表******************/
if exists(select * from sysobjects where name='custom')
drop table custom
create table custom(
Id	int identity(1,1) primary key,		--自增(主键)
CustomerId	nvarchar(50),				--客户Id
Shop	nvarchar(50),					--店铺/平台
Contactperson	nvarchar(50),			--联系人
Sex				int ,					--性别 0.女 1.男
Age				int,					--年龄
Birthday		nvarchar(50),			--生日
IDNumber		nvarchar(50),			--身份证
Telephone	nvarchar(50),				--电话
Phone	nvarchar(50),					--手机号
Weixin	nvarchar(50),					--微信
QQNo	nvarchar(50),					--QQ
CustomerLevel	nvarchar(50),				--用户等级
Remark	nvarchar(max),					--备注
CustomerServiceId  int,					--客服Id
UserId		int,						--操作人
Def1	nvarchar(50),					--默认1
Def2	nvarchar(50),					--默认2
Def3	nvarchar(50),					--默认3
Def4	nvarchar(50),					--默认4
Def5	nvarchar(50),					--默认5
)
go


/***************客户地址表******************/
if exists(select * from sysobjects where name='customAddress')
drop table customAddress
create table customAddress(
Id	int identity(1,1) primary key,		--自增(主键)
CustomerId	nvarchar(50),				--客户Id
Provinces	nvarchar(50),				--省
City	nvarchar(50),					--市
District nvarchar(50),					--区/县
CusAddress	nvarchar(max),				--收货地址
CustomerServiceId  int,					--客服Id
UserId		int,						--操作人	
Def1	nvarchar(50),					--默认1
Def2	nvarchar(50),					--默认2
Def3	nvarchar(50),					--默认3
Def4	nvarchar(50),					--默认4
Def5	nvarchar(50),					--默认5
)
go

/*
***************供应季节对应表*****************
*/
if exists(select * from sysobjects where name='SeasonVen')
drop table SeasonVen
create table SeasonVen(
Id	int identity(1,1) primary key,		--自增(主键)
Cat1	nvarchar(20),		--品牌名称
Cat1Ven	nvarchar(20),		--供应商品牌名称
Vencode	nvarchar(20),			--品牌数据源
BrandIndex	int,		--排序
UserId	int,		--操作人
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50),		--默认5
)
go


/*
***************季节表*****************
*/
if exists(select * from sysobjects where name='Season')
drop table Season
create table Season(
Id	int identity(1,1) primary key,		--自增(主键)
Cat1	nvarchar(20),		--品牌名称
Cat1Index	int,		--排序
UserId	int,		--操作人
			
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50),		--默认5
)
go


/*
***************地区表*****************
*/
if exists(select * from sysobjects where name='AreaTable')
drop table AreaTable
create table AreaTable(
Id	int identity(1,1) primary key,		--自增(主键)
Region_id	int,				--地区Id
Parent_id	int,				--继承Id
Region_name	nvarchar(50),		--地区名称
Post_code	int,				--邮编
Update_time datetime,			--更新时间
Def1	nvarchar(50),			--默认1
Def2	nvarchar(50),			--默认2
Def3	nvarchar(50),			--默认3
Def4	nvarchar(50),			--默认4
Def5	nvarchar(50),			--默认5
)
go


/*
***************客户端用户表*****************
*/
if exists(select * from sysobjects where name='ClientLogin')
drop table ClientLogin
create table ClientLogin(
Id	int identity(1,1) primary key,		--自增(主键)
userName	nvarchar(20),		--帐号
userPwd	nvarchar(20),		--密码
sourceId	int,		--供应商
			
Def1	nvarchar(50),		--邮箱
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50),		--默认5
)
go



/**********************************************************rwh***********************************************************/















/**********************************************************fj***********************************************************/
/*
***************残次品备注*****************
*/
if exists(select * from sysobjects where name='DefectiveRemark')
drop table DefectiveRemark
create table DefectiveRemark(
Id	int identity(1,1) primary key,		--自增(主键)
Scode	nvarchar(16),		--货号
ScodeMarKer nvarchar(50),	--残次品编号
Bcode	nvarchar(16),		--条码1
Bcode2	nvarchar(20),		--条码2
Descript	nvarchar(60),	--英文描述
Cdescript	nvarchar(60),	--中文描述
Unit	nvarchar(6),		--单位
Currency	nvarchar(3),	--货币
Cat	nvarchar(5),			--品牌
Cat1	nvarchar(5),		--季节
Cat2	nvarchar(5),		--类别
Clolor	nvarchar(20),		--颜色
Size	nvarchar(20),		--尺寸
Style	nvarchar(16),		--款号
Pricea	numeric(13,2),		--价格1
Priceb	numeric(13,2),		--价格2
Pricec	numeric(13,2),		--价格3
Priced	numeric(13,2),		--价格4
Pricee	numeric(13,2),		--价格5
Disca	numeric(6,2),		--折扣1
Discb	numeric(6,2),		--折扣2
Discc	numeric(6,2),		--折扣3
Discd	numeric(6,2),		--折扣4
Disce	numeric(6,2),		--折扣5
Vencode	nvarchar(10),		--供应商编号(外键)
Model	nvarchar(30),		--型号
Loc	nvarchar(5),			--店铺
Balance	int,				--数量
Lastgrnd	datetime,		--收货日期(按货品)
Imagefile	nvarchar(500),	--货品图片
ProductRemark nvarchar(1000),--标记说明
Def1	int,				--上次库存
Def2	nvarchar(50),		---是否为残次品
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50),		--默认5
Def6	nvarchar(50),		--默认6
Def7	nvarchar(50),		--默认7
Def8	nvarchar(50),		--默认8
Def9	nvarchar(50),		--默认9
Def10	nvarchar(50),		--默认10
Def11	nvarchar(50),		--默认11
)
go
/*客户端监听程序 运行日志*/
if exists(select * from sysobjects where name='ClientError')
drop table ClientError
create table ClientError(
Id	int identity(1,1) primary key,		                --自增(主键)
ErrorMsg	nvarchar(20),								--错误信息
ErrorTime	nvarchar(20) not null ,						--错误发生时间
ErrorClient	nvarchar(150) not null,						--发生错误的程序
ErrorDetails	nvarchar(150) not null,					--错误详情
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go

/*保存客户端*/
if exists(select * from sysobjects where name='ApplicationsClient')
drop table ApplicationsClient
create table ApplicationsClient(
Id int identity(1,1) primary key,--自增(主键)
ClientMark nvarchar(500) ,	--客户端说明
ClientName nvarchar(100) ,	--客户端名称	
Def1	nvarchar(50),		--默认1
Def2	nvarchar(50),		--默认2
Def3	nvarchar(50),		--默认3
Def4	nvarchar(50),		--默认4
Def5	nvarchar(50)		--默认5
)
go


/*
***************淘宝款号对应SKU货号*****************
*/
if exists(select * from sysobjects where name='TbSkuScode')
drop table TbSkuScode
create table TbSkuScode(
Id int identity(1,1) primary key,--自增(主键)
TbRemarkId nvarchar(30) ,	--淘宝商品编号
TbStyle nvarchar(50),		--淘宝款号
TbSkuId nvarchar(1000) ,	--淘宝SKUID
TbImage nvarchar(500),		--淘宝图片		
Scode	nvarchar(50),		--货号
Color	nvarchar(50),		--颜色
Balance	nvarchar(50),		--库存
Price	nvarchar(50),		--价格
SaleStatus nvarchar(20),	--销售状态   -- onsale 销售中 ---instock仓库中
CreateTime	nvarchar(50),	--创建时间
Def1 nvarchar(50),
Def2 nvarchar(50),
Def3 nvarchar(50),
Def4 nvarchar(50),
Def5 nvarchar(50),
Def6 nvarchar(50)
)
go









































