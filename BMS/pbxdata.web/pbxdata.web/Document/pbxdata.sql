use master -- ���õ�ǰ���ݿ�Ϊmaster,�Ա����sysdatabases��
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
***************������ñ�*****************
*/
if exists(select * from sysobjects where name='TypeConfig')
drop table TypeConfig
create table TypeConfig(
Id int identity(1,1) primary key,--����(����)
PersonaId nvarchar(30) ,	--��ɫId
TypeId nvarchar(1000) ,		--С���
UserId int,		            --������(�����			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go
/*
***************�û�����������ñ�*****************
*/
if exists(select * from sysobjects where name='PersonaTypeConfit')---�û���������

drop table PersonaTypeConfit
create table PersonaTypeConfit(
Id int identity(1,1) primary key,--����(����)
CustomerId nvarchar(30) ,	--��ɫId
TypeId nvarchar(4000) ,		--С���
UserId int,		            --������(�����			
Def1	nvarchar(50),		--Ĭ��1--��Ӧ��
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go

/*
***************Ʒ�����ñ�*****************
*/
if exists(select * from sysobjects where name='BrandConfig')
drop table BrandConfig
create table BrandConfig(
Id int identity(1,1) primary key,--����(����)
PersonaId nvarchar(30) ,	--��ɫId
BrandId nvarchar(1000) ,	--Ʒ�Ʊ��
UserId int,		            --������(�����			
Def1	nvarchar(50),		--Ĭ��1--��Ӧ��
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go
/*
***************Ʒ�����ñ��������*****************
*/
if exists(select * from sysobjects where name='BrandConfigPersion')
drop table BrandConfigPersion
create table BrandConfigPersion(
Id int identity(1,1) primary key,--����(����)
CustomerId nvarchar(30) ,	--��ɫId
BrandId nvarchar(1000) ,	--Ʒ�Ʊ��
UserId int,		            --������(�����			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go
/*
***************���table��filed��*****************
*/
if exists(select * from sysobjects where name='tableFiledPerssion')
drop table tableFiledPerssion
create table tableFiledPerssion(
Id	int identity(1,1) primary key,		            --����(����)
tableName	nvarchar(30) ,		--����
tableFiled	nvarchar(30) ,		--���ֶ�
tableLevel int,                         --����
tableNameState int default(1),                     --�������Ƿ���ʾ(Ĭ����ʾΪ1������ʾΪ0)
tableFiledState int default(1),                    --���ֶ��Ƿ���ʾ(Ĭ����ʾΪ1������ʾΪ0)
tableIndex int,                         --����
UserId	int,		                    --������(�����
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go
select * from tableFiledPerssion



/*
***************��ɫ��*****************
*/
if exists(select * from sysobjects where name='persona')
drop table persona
create table persona(
Id	int identity(1,1) primary key,		            --����(����)
PersonaName	nvarchar(20) not null,		--��ɫ����
PersonaIndex	int,		            --����
UserId	int,		                    --������(�����
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go



/*
***************�û���*****************
*/
if exists(select * from sysobjects where name='users')
drop table users
create table users(
Id	int identity(1,1) primary key,		                --����(����)
personaId	int FOREIGN KEY REFERENCES persona(ID),		                    --��ɫId(���)
userName	nvarchar(20) not null ,		    --�û���
userPwd	nvarchar(20) not null,		        --�û�����
userRealName	nvarchar(20) not null,		--��ʵ����
userSex	int,	                            --�Ա�(0-�У�1-Ů)
UserPhone	nvarchar(20),		--�绰
UserAddress	nvarchar(200),		--��ַ
UserEmail	nvarchar(20),		--����
userIndex	int,		        --����
UserManage	int,	            --(0=��ͨ�û���1=��ɫ����2=ϵͳ����)	�û�����״̬
UserId	int,		            --������(�����
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go


/*
***************�û��Ա���*****************
*/
if exists(select * from sysobjects where name='taoAppUser')
drop table taoAppUser
create table taoAppUser(
Id	int identity(1,1) primary key,		                --����(����)
tbUsserId	nvarchar(20),		                    --�Ա��û�ID
tbUserNick	nvarchar(20) not null ,		    --�Ա��û��ǳ�
accessToken	nvarchar(150) not null,		--��������
refreshToken	nvarchar(150) not null,		--ˢ������
userId1	int FOREIGN KEY REFERENCES users(ID),					--�û�ID�������
userId	int FOREIGN KEY REFERENCES users(ID),		            --������(�����
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go


/**�Ա����̱�**/
if exists(select * from sysobjects where name='tbProductReMark')
drop table tbProductReMark
create table tbProductReMark(
Id int identity(1,1) primary key,       --����(����)
ProductReMarkId nvarchar(50),	        --�Ա����̱��
ProductStyle nvarchar(1000),		    --���
ProductTitle nvarchar(1000),		    --��Ʒ����			
ProductImg nvarchar(500),		        --��ƷͼƬ
ProductTaoBaoPrice numeric(13, 2),		--�Ա��۸�
ProductYJStock int,		                --Ԥ�����
ProductSJ int,		                    --�ϼܿ��
ProductSJTime1 nvarchar(500),	        --�ϼ�ʱ��
ProductXJTime2 nvarchar(500),            --�¼�ʱ��
ProductState  nvarchar(50) ,            --��Ʒ״̬ instock���ֿ��У�onsale�������У�
ProductShopName nvarchar(500),          --�Ա�����
ProductReMarkShopCar  int,              --���ﳵ
ProductReMarkActivity nvarchar(500),    --�
ProductReMarkKeep int,                  --�ղ�
ProductReMarkStockA int,                --���A
ProductReMarkStockB int,                --���B
ProductReMark1 nvarchar(1000),          --��ע
ProductOther1  nvarchar(1000),          --����1
ProductOther2  nvarchar(1000),          --����2
ProductOther3  nvarchar(1000),          --����3
ProductOther4  nvarchar(1000),          --����4
Def1 nvarchar(1000),                    --Ĭ��1
Def2 nvarchar(1000),                    --Ĭ��2
Def3 nvarchar(1000),                    --Ĭ��3
Def4 nvarchar(1000),                    --Ĭ��4
Def5 nvarchar(1000),                    --Ĭ��5
Def6 nvarchar(1000),                    --Ĭ��6
Def7 nvarchar(1000),                    --Ĭ��7
Def8 nvarchar(1000),                    --Ĭ��8
Def9 nvarchar(1000),                    --Ĭ��9
Def10 nvarchar(1000),                   --Ĭ��10
)
Go


/*
***************�˵���*****************
*/
if exists(select * from sysobjects where name='menu')
drop table menu
create table menu(
Id	int identity(1,1) primary key,			--����(����)
menuName	nvarchar(20),		--�˵�����
MenuSrc	nvarchar(500),			--�˵�����
MenuLevel	int 	Default(0),	--�˵�����(��Ŀ¼Ĭ��Ϊ0��
MenuIndex	int,				--����
UserId	int,					--������(�����
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go

insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('��ҳ','',0,1,1)
insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('��ҳ','',0,1,1)
insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('��ҳ','',0,1,1)
insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('��ҳ','',0,1,1)
insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('��ҳ','',0,1,1)
insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('��ҳ','',0,1,1)
insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('��ҳ','',0,1,1)
insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('��ҳ','',0,1,1)
insert into  menu(menuName,MenuSrc,MenuLevel,MenuIndex,UserId) values('��ҳ','',0,1,1)
go



/*
***************���ܱ�*****************
*/
if exists(select * from sysobjects where name='funpermisson')
drop table funpermisson
create table funpermisson(
Id	int identity(1,1) primary key,		--����(����)
MenuId	int foreign key references menu(id),				--�˵�Id(�����
FunName	nvarchar(20),		--��������
FunIndex	int,			--����
UserId	int,				--������(�����
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go


/*
***************�ֶα�*****************
*/
if exists(select * from sysobjects where name='filedpermisson')
drop table filedpermisson
create table filedpermisson(
Id	int identity(1,1) primary key,		--����(����)
FunId	int foreign key references funpermisson(id),		--����Id(�����
dbTable	nvarchar(50),		--���ݱ�
Tabledescription	nvarchar(300),		--�������
IsTableShow	int,		--���ݱ��Ƿ���ʾ
TableFiled	nvarchar(50),		--���ֶ�
FiledDescription	nvarchar(300),		--���ֶ�����
IsFiledShow	int,		--���ֶ��Ƿ���ʾ
FiledState	int	Default(0),	--�ֶ�״̬(1=����)��Ĭ�϶���ʾ
filedIndex	int,		--����
UserId	int,		--������(�����
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go



/*
***************�û���ɫȨ�ޱ�*****************
*/
if exists(select * from sysobjects where name='userspermisson')
drop table userspermisson
create table userspermisson(
Id	int identity(1,1) primary key,		--����(����)
UserId	int foreign key references users(id),		        --�û�Id(�����
PersonaId	int foreign key references persona(id),		        --��ɫId(�����
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go



/*
***************��ɫȨ�ޱ�*****************
*/
if exists(select * from sysobjects where name='personapermisson')
drop table personapermisson
create table personapermisson(
Id	int identity(1,1) primary key,						--����(����)
personaId	int foreign key references persona(id),		--��ɫId(�����
MemuId int ,--foreign key references menu(id),			--�˵�Id(�����
FunId	int ,--foreign key references funpermisson(id) 	--����Id(�����
FieldId	int ,--foreign key references filedpermisson(id)--�ֶ�Id(�����
			
Def1	nvarchar(50),		--�ֶ�����
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50),		--Ĭ��5
)
go


/*
***************Ʒ�Ʊ�*****************
*/
if exists(select * from sysobjects where name='brand')
drop table brand
create table brand(
Id	int identity(1,1) primary key,		--����(����)
BrandName	nvarchar(20),		--Ʒ������
BrandAbridge	nvarchar(20),		--Ʒ��������д
BrandCode	nvarchar(20),		--���Ʊ���
BrandIndex	int,		--����
UserId	int,		--������(�����
			
Def1	nvarchar(50),		--��Ӧ�Ա����
Def2	nvarchar(50),		--Ʒ�ƹ�����
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50),		--Ĭ��5
)
go



/*
***************������*****************
*/
if exists(select * from sysobjects where name='productbigtype')
drop table productbigtype
create table productbigtype(
Id	int identity(1,1) primary key,		--����(����)
bigtypeName	nvarchar(20),		--���������
bigtypeIndex	int,		--����
UserId	int,		--������(�����
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50),		--Ĭ��5
)
go



/*
***************����*****************
*/
if exists(select * from sysobjects where name='producttype')
drop table producttype
create table producttype(
Id	int identity(1,1) primary key,		--����(����)
BigId	Int	foreign key references productbigtype(id),	--�����ID�������
TypeName	nvarchar(20),		--�������
TypeNo	nvarchar(20),		--������
typeIndex	int,		--����
UserId	int,		--������(�����
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50),		--Ĭ��5
)
go


/*
***************��Ʒ������Ա�*****************
*/
if exists(select * from sysobjects where name='productPorperty')
drop table productPorperty
create table productPorperty(
Id	int identity(1,1) primary key,		--����(����)
TypeId	int  foreign key references producttype(id),		--���ID
PropertyName	nvarchar(20),		--��������
PorpertyIndex	int,		--����
UserId	int,		--������
			
Def1	nvarchar(50),		--�Ա������
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50),		--Ĭ��5
)
go


/*
***************��Ʒ�������ֵ��*****************
*/
if exists(select * from sysobjects where name='productPorpertyValue')
drop table productPorpertyValue
create table productPorpertyValue(
Id	int identity(1,1) primary key,		--����(����)
TypeId	int  foreign key references producttype(id),		--���ID
PropertyId	int  foreign key references productPorperty(id),		--����ID
Scode	int,		--����
PropertyValue	nvarchar(20),		--����ֵ
PorpertyIndex	int,		--����
UserId	int,		--������
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50),		--Ĭ��5
)
go





/*
***************��־��*****************
*/
if exists(select * from sysobjects where name='errorlog')
drop table errorlog
create table errorlog(
Id	int identity(1,1) primary key,	--����(����)
ErrorMsg	nvarchar(50),			--������Ϣ
errorSrc	nvarchar(500),			--����·��
errorMsgDetails	nvarchar(500),		--��������
UserId	int,						--������(�����
errorTime	datetime,				--��־ʱ��
			
operation	nvarchar(50),		--��־���ͣ�1-�쳣��Ϣ2.��ͨ����3.���ݶ�ȡ�쳣
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50),		--Ĭ��5
)
go




/*
***************��Ӧ��*****************
*/
if exists(select * from sysobjects where name='productsource')
drop table productsource
create table productsource(
Id	int identity(1,1)  primary key,		                --����
SourceCode	nvarchar(10)  ,		--��Ӧ�̱���(����)
sourceName	nvarchar(20),		--��Ӧ������
sourcePhone	nvarchar(20),		--��Ӧ�̵绰
SourceManage	nvarchar(20),	--��Ӧ����ϵ��
SourceLevel	nvarchar(20),		--��Ӧ�̵ȼ�
UserId	int,		            --������(���)
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go
/*
**************����Դ���ñ�
*/
if exists(select * from sysobjects where name='productsourceConfig')
drop table productsourceConfig
create table productsourceConfig(
 Id	int identity(1,1) primary key,		                --����
 SourceCode	nvarchar(10) ,--��Ӧ�̱�� ���
 SourcesAddress nvarchar(50),--server��ַ���˿�
 UserId nvarchar(20),--��½�û���
 UserPwd nvarchar(20),--��½����
 DataSources nvarchar(20),--����Դ����
 DataSourcesLevel nvarchar(10),--����Դ���� (�ȼ��ɸߵ��ͣ�A B C D E)
 TimeStart int default(0),--�������һ�� Ĭ��0ֻ����һ��
 Def1 nvarchar(20),
 Def2  nvarchar(20),
 Def3  nvarchar(20),
 Def4  nvarchar(20),
 Def5  nvarchar(20)
)
go
/*
**************����Դ�������£�
*/
if exists(select * from sysobjects where name='ProductStockConfig')
drop table ProductStockConfig
create table ProductStockConfig(
 Id	int identity(1,1) primary key,		                --����
 SourceCode	nvarchar(10) ,--��Ӧ�̱�� ���
 DataSources nvarchar(20),--����Դ����
 TableName nvarchar(50),---���ݱ���
 UpdateState nvarchar(50),--����״̬
 StartTime nvarchar(60),---��ʼʱ��
 SetTime int default(0),--�������һ�� Ĭ��0ֻ����һ��
 Def1 nvarchar(20),
 Def2  nvarchar(20),
 Def3  nvarchar(20),
 Def4  nvarchar(20),
 Def5  nvarchar(20)
)
go

/*

***************ԭʼ���ݿ���*****************
*/
if exists(select * from sysobjects where name='productsourcestock')
drop table productsourcestock
create table productsourcestock(
Id	int identity(1,1) primary key,		--����(����)
Scode	nvarchar(16),		--����
Bcode	nvarchar(16),		--����1
Bcode2	nvarchar(20),		--����2
Descript	nvarchar(60),	--Ӣ������
Cdescript	nvarchar(60),	--��������
Unit	nvarchar(6),		--��λ
Currency	nvarchar(3),	--����
Cat	nvarchar(5),			--Ʒ��
Cat1	nvarchar(5),		--����
Cat2	nvarchar(5),		--���
Clolor	nvarchar(20),		--��ɫ
Size	nvarchar(20),		--�ߴ�
Style	nvarchar(16),		--���
Pricea	numeric(13,2),		--�۸�1
Priceb	numeric(13,2),		--�۸�2
Pricec	numeric(13,2),		--�۸�3
Priced	numeric(13,2),		--�۸�4
Pricee	numeric(13,2),		--�۸�5
Disca	numeric(6,2),		--�ۿ�1
Discb	numeric(6,2),		--�ۿ�2
Discc	numeric(6,2),		--�ۿ�3
Discd	numeric(6,2),		--�ۿ�4
Disce	numeric(6,2),		--�ۿ�5
Vencode	nvarchar(10) foreign key references productsource(SourceCode),		--��Ӧ�̱��(���)
Model	nvarchar(30),		--�ͺ�
Rolevel	int,				--Ԥ�����
Roamt	int,				--���ٶ�����
Stopsales	int,			--ͣ�ۿ��
Loc	nvarchar(5),			--����
Balance	int,				--��Ӧ�̿��
Lastgrnd	datetime,			--�ջ�����(����Ʒ)
Imagefile	nvarchar(500),			--��ƷͼƬ
			
PrevStock	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50),		--Ĭ��5
Def6	nvarchar(50),		--Ĭ��6
Def7	nvarchar(50),		--Ĭ��7
Def8	nvarchar(50),		--Ĭ��8
Def9	nvarchar(50),		--Ĭ��9
Def10	nvarchar(50),		--Ĭ��10
Def11	nvarchar(50),		--Ĭ��11
)
go
/*
***************�����ԭʼ�������ݱ�*****************
*/
if exists(select * from sysobjects where name='ProductItalySourceStock')---���������ԭʼ��

drop table ProductItalySourceStock
create table ProductItalySourceStock(
Id int identity(1,1) primary key,--����(����)
productCode nvarchar(200),--��Ʒ���
seasonCode nvarchar(200),--������
brand nvarchar(200),--Ʒ��
styleCode nvarchar(200),--�����
colorCode nvarchar(200),--������ɫ��
sizeType nvarchar(200),--��С����
season nvarchar(200),--����
parentGroup nvarchar(200),--��
childGroup nvarchar(200),--����
styleDescribe nvarchar(200),--�������
color nvarchar(200),--��ɫ
cloth nvarchar(200),--����
theme nvarchar(200),--����
classes nvarchar(200),--���
simplenessDescribe nvarchar(200),--������
fullDescribe nvarchar(200),--��������
price1 money,--����
price2 money,--��վ����
volume nvarcahr(200),--���
price3 money,--��վ���ۣ��۳���ֵ˰��
size nvarchar(200),--�ߴ磨���ң�
presell nvarchar(200) ,--Ԥ��
isCancel int,--��ȡ����0=��Ч��1=��ȡ����
season1 nvarchar,--����
shadeguide nvarchar(200),--ɫ��
especiallyCarriage nvarchar(200),--�ر�����
measure1 nvarchar(200),--����1
measure2 nvarchar(200),--����2
measure3 nvarchar(200),--����3
measure4 nvarchar(200),--����4
measure5 nvarchar(200),--����5
measure6 nvarchar(200),--����6
measure7 nvarchar(200),--����7
measure8 nvarchar(200),--����8
sizeAndFit nvarchar(200),--�ʺϴ�С
presellTime nvarchar(200) ,--Ԥ������
PublishingPages nvarchar(200),--����ߴ�
packExplain nvarchar(500),--��װ����
packNum int ,--��װ����
especiallyExplain nvarchar(500),--�ر�˵��
primaryProducingPlace nvarchar(200),--ԭ����
MIDId nvarchar(200),--MID��
constituteDetail nvarchar(200),--�����ϸ
brandIntro nvarchar(200),--Ʒ�Ƽ��
sizeType1 nvarchar(200),--��С����
SuperBrandCollection nvarchar(200),--Ʒ�Ƽ���
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

***************�����ԭʼ���ݿ���*****************
*/
if exists(select * from sysobjects where name='ProductItalyStock')---��������ݿ���
drop table ProductItalyStock
create table ProductItalyStock(
Id int identity(1,1) primary key,--����(����)
productCode nvarchar(200),--��Ʒ���
size nvarchar(200),--�ߴ�
stockCount int ,--�����
receiveNumber int,--��������
createDate nvarchar(200),--���ݴ���ʱ��
barCode nvarchar(200),--����
measure1 nvarchar(200),--����1
measure2 nvarchar(200),--����2
measure3 nvarchar(200),--����3
measure4 nvarchar(200),--����4
measure5 nvarchar(200),--����5
measure6 nvarchar(200),--����6
measure7 nvarchar(200),--����7
measure8 nvarchar(200),--����8
def1 nvarchar(200) null,
def2 nvarchar(200) null,
def3 nvarchar(200) null,
def4 nvarchar(200) null,
def5 nvarchar(200) null
)
go
/*
*���������Դ���´�����־*
*/
if exists(select * from sysobjects where name='ItalyUpdateError')---��������ݸ��´�����־��

drop table ItalyUpdateError
create table ItalyUpdateError(
Id int identity(1,1) primary key,--����(����)
ItalyCode nvarchar(10),-- ���������Դ���
createTime datetime ,--������־ʱ��
msg nvarchar(500),--��������
methed nvarchar(500),--���󷽷�
def1 nvarchar(50),
def2 nvarchar(50),
def3 nvarchar(50),
def4 nvarchar(50),
def5 nvarchar(50),
)
--alter table productsourcestock alter column Imagefile nvarchar(500)




/*
***************����*****************
*/
if exists(select * from sysobjects where name='productstock')
drop table productstock
create table productstock(
Id	int identity(1,1) primary key,		--����(����)
Scode	nvarchar(16),		--����
Bcode	nvarchar(16),		--����1
Bcode2	nvarchar(20),		--����2
Descript	nvarchar(60),	--Ӣ������
Cdescript	nvarchar(60),	--��������
Unit	nvarchar(6),		--��λ
Currency	nvarchar(3),	--����
Cat	nvarchar(5),			--Ʒ��
Cat1	nvarchar(5),		--����
Cat2	nvarchar(5),		--���
Clolor	nvarchar(20),		--��ɫ
Size	nvarchar(20),		--�ߴ�
Style	nvarchar(16),		--���
Pricea	numeric(13,2),		--�۸�1
Priceb	numeric(13,2),		--�۸�2
Pricec	numeric(13,2),		--�۸�3
Priced	numeric(13,2),		--�۸�4
Pricee	numeric(13,2),		--�۸�5
Disca	numeric(6,2),		--�ۿ�1
Discb	numeric(6,2),		--�ۿ�2
Discc	numeric(6,2),		--�ۿ�3
Discd	numeric(6,2),		--�ۿ�4
Disce	numeric(6,2),		--�ۿ�5
Vencode	nvarchar(10),		--��Ӧ�̱��(���)
Model	nvarchar(30),		--�ͺ�
Rolevel	int,				--Ԥ�����
Roamt	int,				--���ٶ�����
Stopsales	int,			--ͣ�ۿ��
Loc	nvarchar(5),			--����
Balance	int,				--��Ӧ�̿��
Lastgrnd	datetime,			--�ջ�����(����Ʒ)
Imagefile	nvarchar(500),			--��ƷͼƬ
			
PrevStock	int,		--�ϴο��
Def2	nvarchar(50),		---�Ƿ�Ϊ�д�Ʒ
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50),		--Ĭ��5
Def6	nvarchar(50),		--Ĭ��6
Def7	nvarchar(50),		--Ĭ��7
Def8	nvarchar(50),		--Ĭ��8
Def9	nvarchar(50),		--Ĭ��9
Def10	nvarchar(50),		--Ĭ��10
Def11	nvarchar(50),		--Ĭ��11
)
go


/*
***************�ⲿ����*****************
*/
if exists(select * from sysobjects where name='outsideProduct')
drop table outsideProduct
create table outsideProduct(
Id	int identity(1,1) primary key,		--����(����)
Scode	nvarchar(16),		--����
Bcode	nvarchar(16),		--����1
Bcode2	nvarchar(20),		--����2
Descript	nvarchar(60),	--Ӣ������
Cdescript	nvarchar(60),	--��������
Unit	nvarchar(6),		--��λ
Currency	nvarchar(3),	--����
Cat	nvarchar(5),			--Ʒ��
Cat1	nvarchar(5),		--����
Cat2	nvarchar(5),		--���
Clolor	nvarchar(20),		--��ɫ
Size	nvarchar(20),		--�ߴ�
Style	nvarchar(16),		--���
Pricea	numeric(13,2),		--�۸�1
Priceb	numeric(13,2),		--�۸�2
Pricec	numeric(13,2),		--�۸�3
Priced	numeric(13,2),		--�۸�4
Pricee	numeric(13,2),		--�۸�5
Disca	numeric(6,2),		--�ۿ�1
Discb	numeric(6,2),		--�ۿ�2
Discc	numeric(6,2),		--�ۿ�3
Discd	numeric(6,2),		--�ۿ�4
Disce	numeric(6,2),		--�ۿ�5
Vencode	nvarchar(10),		--��Ӧ�̱��(���)
Model	nvarchar(30),		--�ͺ�
Rolevel	int,				--Ԥ�����
Roamt	int,				--���ٶ�����
Stopsales	int,			--ͣ�ۿ��
Loc	nvarchar(5),			--����
Balance	int,				--��Ӧ�̿��
Lastgrnd	datetime,		--�ջ�����(����Ʒ)
Imagefile	nvarchar(500),	--��ƷͼƬ
			
PrevStock	int,			--�ϴο��
ShowState int ,				--��ʾ״̬ 1:��ʾ 0������ʾ
Def2	nvarchar(50),		--Ĭ��2 
Def3	nvarchar(50),		--Ĭ��3 
Def4	nvarchar(50),		--Ĭ��4 --�˻����
Def5	nvarchar(50),		--Ĭ��5 --������
Def6	nvarchar(50),		--Ĭ��6 --����ʱ��
Def7	nvarchar(50),		--Ĭ��7 --1   �Ѿ��޸ļ۸�   
Def8	nvarchar(50),		--Ĭ��8 --1   �Ѿ��޸� ���
Def9	nvarchar(50),		--Ĭ��9
Def10	nvarchar(50),		--Ĭ��10
Def11	nvarchar(50),		--Ĭ��11
)
go

/*
***************ѡ���ⲿ����*****************
*/
if exists(select * from sysobjects where name='SoCaloutsideProduct')
drop table SoCaloutsideProduct
create table SoCaloutsideProduct(
Id	int identity(1,1) primary key,		--����(����)
Scode	nvarchar(16),		--����
Bcode	nvarchar(16),		--����1
Bcode2	nvarchar(20),		--����2
Descript	nvarchar(60),	--Ӣ������
Cdescript	nvarchar(60),	--��������
Unit	nvarchar(6),		--��λ
Currency	nvarchar(3),	--����
Cat	nvarchar(5),			--Ʒ��
Cat1	nvarchar(5),		--����
Cat2	nvarchar(5),		--���
Clolor	nvarchar(20),		--��ɫ
Size	nvarchar(20),		--�ߴ�
Style	nvarchar(16),		--���
Pricea	numeric(13,2),		--�۸�1
Priceb	numeric(13,2),		--�۸�2
Pricec	numeric(13,2),		--�۸�3
Priced	numeric(13,2),		--�۸�4
Pricee	numeric(13,2),		--�۸�5
Disca	numeric(6,2),		--�ۿ�1
Discb	numeric(6,2),		--�ۿ�2
Discc	numeric(6,2),		--�ۿ�3
Discd	numeric(6,2),		--�ۿ�4
Disce	numeric(6,2),		--�ۿ�5
Vencode	nvarchar(10),		--��Ӧ�̱��(���)
Model	nvarchar(30),		--�ͺ�
Rolevel	int,				--Ԥ�����
Roamt	int,				--���ٶ�����
Stopsales	int,			--ͣ�ۿ��
Loc	nvarchar(5),			--����
Balance	int,				--��Ӧ�̿��
Lastgrnd	datetime,		--�ջ�����(����Ʒ)
Imagefile	nvarchar(500),	--��ƷͼƬ
			
PrevStock	int,			--�ϴο��
ShowState int ,				--��ʾ״̬ 1:��ʾ 0������ʾ
Def2	nvarchar(50),		--Ĭ��2 
Def3	nvarchar(50),		--Ĭ��3 
Def4	nvarchar(50),		--Ĭ��4 --�˻����
Def5	nvarchar(50),		--Ĭ��5 --������
Def6	nvarchar(50),		--Ĭ��6 --����ʱ��
Def7	nvarchar(50),		--Ĭ��7 --1   �Ѿ��޸ļ۸�   
Def8	nvarchar(50),		--Ĭ��8 --1   �Ѿ��޸� ���
Def9	nvarchar(50),		--Ĭ��9
Def10	nvarchar(50),		--Ĭ��10
Def11	nvarchar(50),		--Ĭ��11
)
go

/*
***************��Ʒ��*****************
*/
if exists(select * from sysobjects where name='product')
drop table product
create table product(
Id	int,		--����(����)
Scode	nvarchar(16),		--����
Bcode	nvarchar(16),		--����1
Bcode2	nvarchar(20),		--����2
Descript	nvarchar(60),	--Ӣ������
Cdescript	nvarchar(60),	--��������
Unit	nvarchar(6),		--��λ
Currency	nvarchar(3),	--����
Cat	nvarchar(5),			--Ʒ��
Cat1	nvarchar(5),		--����
Cat2	nvarchar(5),		--���
Clolor	nvarchar(20),		--��ɫ
Size	nvarchar(20),		--�ߴ�
Style	nvarchar(16),		--���
Pricea	numeric(13,2),		--�۸�1
Priceb	numeric(13,2),		--�۸�2
Pricec	numeric(13,2),		--�۸�3
Priced	numeric(13,2),		--�۸�4
Pricee	numeric(13,2),		--�۸�5
Disca	numeric(6,2),		--�ۿ�1
Discb	numeric(6,2),		--�ۿ�2
Discc	numeric(6,2),		--�ۿ�3
Discd	numeric(6,2),		--�ۿ�4
Disce	numeric(6,2),		--�ۿ�5
Vencode	nvarchar(10),		--��Ӧ�̱��(���)
Model	nvarchar(30),		--�ͺ�
Rolevel	int,				--Ԥ�����
Roamt	int,				--���ٶ�����
Stopsales	int,			--ͣ�ۿ��
Loc	nvarchar(5),			--����
Balance	int,				--��Ӧ�̿��
Lastgrnd	datetime,		--�ջ�����(����Ʒ)
Imagefile	nvarchar(500),	--��ƷͼƬ
			
Def1	nvarchar(50),	    --ͼƬ����ϴ�ʱ��
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--���ۿ��
ciqProductId nvarchar(128), --��Ʒ��Ϣͼ  1.�ϸ� 2.δ��� 3.���ϸ�
ciqSpec nvarchar(200),      --����ͺ�
ciqHSNo nvarchar(200),      --��ƷHS����
ciqAssemCountry nvarchar(200), --ԭ����/����
ciqProductNo int  identity(1,1)  primary key ,--��Ʒ��ţ���������EntGoodsNo�ֶΣ�����С��6
Def9	nvarchar(50),		--ͼƬ�Ƿ�ϸ���4��״̬  ��Ϊ��ͼ0.δ���1.�ϸ�2.���ϸ�
Def10	nvarchar(50),		--������
Def11	nvarchar(50),		--��Ʒ���ر������ 0.δ�� 1.��� 2.����ͨ�� 3.����ʧ��
Def12	nvarchar(50),		--�̼���Ʒ��Ϣ���� 1.�ϸ� 0.���ϸ�
Def13	nvarchar(50),		--������Ʒ��Ϣ���� 1.�ϸ� 0.���ϸ�
QtyUnit nvarchar(50),       --������λ	��Ʒ������/�����ļ�����λ
Def15 nvarchar(50),         --ë��
Def16 nvarchar(50),         --����
Def17 nvarchar(200),        --��Ʒ����
)
go
/*�ֻ���*/
if exists(select * from sysobjects where name='SoCalProduct')  
drop table SoCalProduct
create table SoCalProduct(
Id	int identity(1,1) primary key,		--����(����)
Scode	nvarchar(16),		--����
Bcode	nvarchar(16),		--����1
Bcode2	nvarchar(20),		--����2
Descript	nvarchar(60),	--Ӣ������
Cdescript	nvarchar(60),	--��������
Unit	nvarchar(6),		--��λ
Currency	nvarchar(3),	--����
Cat	        nvarchar(5),	--Ʒ��
Cat1	nvarchar(5),		--����
Cat2	nvarchar(5),		--���
Clolor	nvarchar(20),		--��ɫ
Size	nvarchar(20),		--�ߴ�
Style	nvarchar(16),		--���
Pricea	numeric(13,2),		--�۸�1
Priceb	numeric(13,2),		--�۸�2
Pricec	numeric(13,2),		--�۸�3
Priced	numeric(13,2),		--�۸�4
Pricee	numeric(13,2),		--�۸�5
Disca	numeric(6,2),		--�ۿ�1
Discb	numeric(6,2),		--�ۿ�2
Discc	numeric(6,2),		--�ۿ�3
Discd	numeric(6,2),		--�ۿ�4
Disce	numeric(6,2),		--�ۿ�5
Vencode	nvarchar(10),		--��Ӧ�̱��(���)
Model	nvarchar(30),		--�ͺ�
Rolevel	int,				--Ԥ�����
Roamt	int,				--���ٶ�����
Stopsales	int,			--ͣ�ۿ��
Loc	nvarchar(5),			--����
Balance	int,				--��Ӧ�̿��
Lastgrnd	datetime,		--�ջ�����(����Ʒ)
Imagefile	nvarchar(500),	--��ƷͼƬ		
Def1	nvarchar(50),	    --Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4 nvarchar(128),         --Ĭ��4
Def5 nvarchar(200),         --Ĭ��5
Def6 nvarchar(200),         --Ĭ��6
Def7 nvarchar(200),         --Ĭ��7
Def8 nvarchar(200),         --Ĭ��8
Def9	nvarchar(50),		--Ĭ��9
Def10	nvarchar(50),		--Ĭ��10
Def11	nvarchar(50),		--Ĭ��11
)
go

/*
***************���������*****************
*/
if exists(select * from sysobjects where name='styledescript')
drop table styledescript
create table styledescript(
Id	int identity(1,1) primary key,		--����(����)
style nvarchar(50) ,					--���
productDescript	nvarchar(4000) ,		--��Ʒ����
stylePicSrc	nvarchar(500) ,				--��Ʒ����ͼ
isQualified int,                        --����ͼ�Ƿ�ϸ�
UserId	int,		                    --������(�����
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go


/*
***************���ͼƬ��*****************
*/
if exists(select * from sysobjects where name='stylepic')
drop table stylepic
create table stylepic(
Id	int identity(1,1) primary key,		--����(����)
style nvarchar(50) ,		 --���
stylePicSrc	nvarchar(500) , --��Ʒ��ͼ
isQualified int,             --ͼƬ�Ƿ�ϸ�5�����£��ǲ��ϸ�5�����Ϻϸ񡿣�5-10�ţ�
stylePicState int,			 --ͼƬ״̬
UserId	int,		                    --������(�����
			
Def1	nvarchar(50),		--1.��ͨͼ 2.��ͼ 
Def2	nvarchar(50),		--1.��ͨͼ 2.��ͼ
Def3	nvarchar(50),		--1.��ͨͼ 2.����ͼ
Def4	nvarchar(50),		--1.ʹ���� 2.��ɾ��
Def5	nvarchar(50)		--ͼƬ����
)
go



/*
***************����ͼƬ��*****************
*/
if exists(select * from sysobjects where name='scodepic')
drop table scodepic
create table scodepic(
Id	int identity(1,1) primary key,		--����(����)
scode nvarchar(50) ,					--����
scodePicSrc	nvarchar(500) ,				--��ƷͼƬ
scodePicType	int ,					--ͼƬ�������ͼΪ1����ɫͼ�����ǳߴ�ͼ��Ϊ0��
isQualified int,                        --ͼƬ�Ƿ�ϸ�
scodePicState int,						--ͼƬ״̬
UserId	int,		                    --������(�����
			
Def1	nvarchar(50),					--1.��ͨͼ 2.��ͼ 
Def2	nvarchar(50),					--Ĭ��2
Def3	nvarchar(50),					--Ĭ��3
Def4	nvarchar(50),					--1.ʹ���� 2.��ɾ��
Def5	nvarchar(50)					--ͼƬ����
)
go





/*
***************�������ͱ�*****************
*/
if exists(select * from sysobjects where name='shoptype')
drop table shoptype
create table shoptype(
Id	int identity(1,1) primary key,		                --����
ShoptypeName	nvarchar(20),							--�����������
ShoptypeIndex int,										--����
UserId	int,											--������(���)
			
Def1	nvarchar(50),									--Ĭ��1
Def2	nvarchar(50),									--Ĭ��2
Def3	nvarchar(50),									--Ĭ��3
Def4	nvarchar(50),									--Ĭ��4
Def5	nvarchar(50)									--Ĭ��5
)
go










/*
***************���̱�*****************
*/
if exists(select * from sysobjects where name='shop')
drop table shop
create table shop(
Id	int identity(1,1) primary key,		                --����
ShopName	nvarchar(20),								--��������
ShoptypeId	int foreign key references shoptype(id),	--������������
ShopState	int,										--����״̬
ShopManageId	int foreign key references users(id),	--���̹����ˣ������
ShopIdex int,											--����
UserId	int,											--������(���)
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go





/*
***************������Ʒ��*****************
*/
if exists(select * from sysobjects where name='shopproduct')
drop table shopproduct
create table shopproduct(
Id	int identity(1,1) primary key,		--����(����)
Scode	nvarchar(16),		--����
Bcode	nvarchar(16),		--����1
Bcode2	nvarchar(20),		--����2
Descript	nvarchar(60),	--Ӣ������
Cdescript	nvarchar(60),	--��������
Unit	nvarchar(6),		--��λ
Currency	nvarchar(3),	--����
Cat	nvarchar(5),			--Ʒ��
Cat1	nvarchar(5),		--����
Cat2	nvarchar(5),		--���
Clolor	nvarchar(20),		--��ɫ
Size	nvarchar(20),		--�ߴ�
Style	nvarchar(16),		--���
Pricea	numeric(13,2),		--�۸�1
Priceb	numeric(13,2),		--�۸�2
Pricec	numeric(13,2),		--�۸�3
Priced	numeric(13,2),		--�۸�4
Pricee	numeric(13,2),		--�۸�5
Disca	numeric(6,2),		--�ۿ�1
Discb	numeric(6,2),		--�ۿ�2
Discc	numeric(6,2),		--�ۿ�3
Discd	numeric(6,2),		--�ۿ�4
Disce	numeric(6,2),		--�ۿ�5
Model	nvarchar(30),		--�ͺ�
Rolevel	int,				--Ԥ�����
ShopId	int foreign key references shop(id),		--����(���)
Balance1	int,			--���̿��(���)
Balance2	int,			--���ۿ��
Balance3	int,			--�˻����
Lastgrnd	datetime,		--�ջ�����
Imagefile	nvarchar(500),	--��ƷͼƬ
UserId	int,				--������(�����	
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50),		--Ĭ��5
Def6	nvarchar(50),		--Ĭ��6
Def7	nvarchar(50),		--Ĭ��7
Def8	nvarchar(50),		--Ĭ��8
Def9	nvarchar(50),		--Ĭ��9
Def10	nvarchar(50),		--Ĭ��10
Def11	nvarchar(50),		--Ĭ��11
)
go





/*
***************������*****************
*/
if exists(select * from sysobjects where name='porder')
drop table porder
create table porder(
OrderId	nvarchar(20) primary key,		--�������(����)--tid
--ProductId	nvarchar(20),		--��ƷID��ָ�Ա��ϣ�--num_iid
ShopName	nvarchar(20),		--��������--seller_nick
OrderPrice	numeric(10,2),		--�۸�--price
OrderTime	datetime,		--�µ�ʱ��--created
OrderPayTime	datetime,		--����ʱ��--pay_time
OrderEditTime datetime,         --�޸�ʱ��--modified
OrderSendTime	datetime,		--����ʱ��--consign_time
OrderSucessTime	datetime,		--���׳ɹ�ʱ��--end_time
OrderNick	nvarchar(20),		--�ǳ�--buyer_nick
PayState int,           --֧��״̬--status֧������״̬(0δ֧����1��֧��)
OrderState	int,		--����״̬(����)--status
OrderState1	int,		--����״̬(Դͷ)--status
--CustomServerId	int foreign key references users(id),		--�����ͷ��������
CustomServerId	int,		--�����ͷ��������
UserId	int,		--�����ˣ������

			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go
--state��
--//TRADE_NO_CREATE_PAY(û�д���֧��������) ---0
--//WAIT_BUYER_PAY(�ȴ���Ҹ���)  ---1
--//WAIT_SELLER_SEND_GOODS(�ȴ����ҷ���,��:����Ѹ���)  ---2
--//SELLER_CONSIGNED_PART�����Ҳ��ַ�����  ---3
--//WAIT_BUYER_CONFIRM_GOODS(�ȴ����ȷ���ջ�,��:�����ѷ���)  ---4
--//TRADE_BUYER_SIGNED(�����ǩ��,��������ר��)  ---5
--//TRADE_FINISHED(���׳ɹ�)  ---6
--//TRADE_CLOSED(���׹ر�)  ---7
--//TRADE_CLOSED_BY_TAOBAO(���ױ��Ա��ر�)  ---8
--//ALL_WAIT_PAY(������WAIT_BUYER_PAY��TRADE_NO_CREATE_PAY)  ---9
--//ALL_CLOSED(������TRADE_CLOSED��TRADE_CLOSED_BY_TAOBAO)  ---10
--//WAIT_PRE_AUTH_CONFIRM(��0Ԫ����Լ��) ---11
--//12  �˿�
--//����  ---100

--alter table porder add PayState int
--alter table porder add OrderEditTime datetime
--drop table porder
--drop table OrderDetails
--drop table OrderSplit
--drop table OrderProperty
--drop table OrderExpress
--drop table CustomAndOrder



/*
***************���������*****************
*/
if exists(select * from sysobjects where name='OrderDetails')
drop table OrderDetails
create table OrderDetails(
--Id	int identity(1,1),		                --����
OrderId	nvarchar(20) foreign key references porder(OrderId),		--������ţ������
OrderChildenId	nvarchar(20) primary key,		--�Ӷ������(����) ---oid
ProductId	nvarchar(20),		--��ƷID��ָ�Ա��ϣ�---num_iid
DetailsName	nvarchar(100),		--��Ʒ���� ---title
OrderScode	nvarchar(50),		--����--sku_id
OrderColor	nvarchar(100),		--��ɫ--sku_properties_name
OrderImg	nvarchar(500),		--��ƷͼƬ--pic_path
DetailsSum	int,		--��Ʒ����--num
DetailsPrice	numeric(10,2),		--��Ʒ�۸�--payment
OrderTime	datetime,		--�µ�ʱ��
OrderPayTime	datetime,		--����ʱ��
OrderSendTime	datetime,		--����ʱ��--consign_time
OrderSucessTime	datetime,		--���׳ɹ�ʱ��--end_time

			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go





/*
***************������ֱ�*****************
*/
if exists(select * from sysobjects where name='OrderSplit')
drop table OrderSplit
create table OrderSplit(
Id	int identity(1,1) primary key,		                --����
OrderChildenId	nvarchar(20) foreign key references OrderDetails(OrderChildenId),		--�Ӷ�����ţ������
OrderSplitId	nvarchar(20) foreign key references OrderDetails(OrderChildenId),		--�Ӷ�����ֱ�ţ������

			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go






/*
***************�������Ա�*****************
*/
if exists(select * from sysobjects where name='OrderProperty')
drop table OrderProperty
create table OrderProperty(
Id	int identity(1,1) primary key,		                --����
OrderChildenId	nvarchar(20) foreign key references OrderDetails(OrderChildenId),		--�����ӱ�ţ������
OrderType	nvarchar(20),		--��������
OrderForm	nvarchar(20),		--������Դ
OrderMethod	nvarchar(20),		--���۷�ʽ
OrderActivePrice	numeric(10,2),		--����ã��۵㣩
OrderOtherPrice	numeric(10,2),		--��������
ProductFormId	int,		--��������Դͷ�������
OrderRemark1	nvarchar(20),		--������ע����
OrderRemark	nvarchar(500),		--������ע
OrderIsSplit	int,		--�����Ƿ��Ѳ�֣��Ѳ�ֵĶ�������ʾ����ʾ��ֺ�Ķ�����

			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go
/*������־��Ϣ��**/
if exists(select * from sysobjects where name='ApiRequestMsg')
drop table ApiRequestMsg

create table ApiRequestMsg( -- API������־��Ϣ��
Id int identity(1,1) primary key,-- ����
userId nvarchar(20),-- ��¼��
RqIp nvarchar(20),-- ��½ip��ַ
RqApi nvarchar(200),-- ����api�ӿ�
RqSuccess int,-- �����Ƿ�ɹ�(1���ɹ� 2��ʧ��)
RqDetails nvarchar(500),-- �������飨ʧ����ϸ��Ϣ���ɹ���Ϣ��
RqTime datetime,-- ����ʱ��
Def1 nvarchar(50),
Def2 nvarchar(50),
Def3 nvarchar(50),
Def4 nvarchar(50),
Def5 nvarchar(50)
)
go
/*���ݻ�ȡ�쳣��Ϣ��**/
if exists(select * from sysobjects where name='ApiDataGetException')
drop table ApiDataGetException

create table ApiDataGetException-- APi ���ݻ�ȡ�쳣��Ϣ��
(
ID int identity(1,1) primary key,-- ����
ExceptionMess nvarchar(500),-- �쳣��Ϣ
ExceptionUrl nvarchar(200),-- �쳣·��
ExceptionDetails nvarchar(500),-- �쳣����
ExceptionTime datetime,-- ʱ��
Def1 nvarchar(50),
Def2 nvarchar(50),
Def3 nvarchar(50),
Def4 nvarchar(50),
Def5 nvarchar(50)
)
go




/*
***************����������*****************
*/
if exists(select * from sysobjects where name='OrderExpress')
drop table OrderExpress
create table OrderExpress(
Id	int identity(1,1),		                --����
OrderId	nvarchar(20) foreign key references porder(OrderId),		--������ţ������
OrderChildenId	nvarchar(20) foreign key references OrderDetails(OrderChildenId),		--�����ӱ�ţ������
ExpressNo	nvarchar(20) primary key,		--��ݵ���(����)
ExpressName	nvarchar(20),		--��ݹ�˾
ExpressPrice	nvarchar(20),		--�˷�
ExpressFollow	nvarchar(20),		--���׷��
CustomName	nvarchar(20),		--�ռ�������
CustomPhone	nvarchar(20),		--�ռ��˵绰
CustomAddress	nvarchar(20),		--�ռ���ַ
CustomCity1	nvarchar(20),		--�ռ���ʡ��
CustomCity2	nvarchar(20),		--�ռ��˳���
CustomCity3	nvarchar(20),		--�ռ��˵���

			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go





/*
***************�ͻ���*****************
*/
if exists(select * from sysobjects where name='custom')
drop table custom
create table custom(
Id	int identity(1,1) primary key,		                --����
customName	nvarchar(20),		--�ͻ�����
customPhone	nvarchar(20),		--�ͻ��绰
customSex	int,		--�ͻ��Ա�
customAge	int,		--�ͻ�����
customCity	nvarchar(20),		--�ͻ�����
customAddress	nvarchar(200),		--�ͻ���ַ
customIndex	int,		--����
UserId	int,		--�����ˣ������
CustomServiceId	int foreign key references users(id),		--�����ͷ�(���)
CustomTime	datetime,		--����ʱ��


			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go




/*
***************�ͻ����Ա�*****************
*/
if exists(select * from sysobjects where name='CustomProperty')
drop table CustomProperty
create table CustomProperty(
Id	int identity(1,1) primary key,		                --����
CustomId	int foreign key references custom(id),		--�ͻ����(���)
CustomLevel	nvarchar(5),		--�ͻ��ȼ�
CustomBuyCount	int,		--�ͻ��������
CustomBuyAmount	numeric(10,2),		--�ͻ�������
CustomLoveBrand	nvarchar(50),		--�ͻ�ϲ��Ʒ��
CustomServiceComment	nvarchar(500),		--�ͷ�����
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go




/*
***************�ͻ��붩����ϵ��*****************
*/
if exists(select * from sysobjects where name='CustomAndOrder')
drop table CustomAndOrder
create table CustomAndOrder(
Id	int identity(1,1) primary key,		                --����
CustomId	int foreign key references custom(id),		--�ͻ����(���)
OrderId	nvarchar(20) foreign key references porder(OrderId),		--�������(���)
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go



/*
***************������˰�۸��Լ�˰�ʱ�*****************
*/
if exists(select * from sysobjects where name='Tarifftab')
drop table Tarifftab
create table Tarifftab(
Id	        int identity(1,1) primary key,      --����(����)
TariffNo        nvarchar(50),		            --˰��
TariffName	nvarchar(50),		            --Ʒ�������
Unit      	nvarchar(50),		            --��λ
DutiableValue	nvarchar(50),		            --��˰�۸�
Tariff	        float,		                    --˰��
ConditionOne	nvarchar(50),		            --�̼�������
ConditionTwo	nvarchar(50),		            --���ؼ������
TariffIndex	int,		                    --����
UserId  	int,		                    --������(�����
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go

/*
***************����˰�Ź�ϵ��*****************
*/
if exists(select * from sysobjects where name='TypeIdToTariffNo')
drop table TypeIdToTariffNo
create table TypeIdToTariffNo(
Id	        int identity(1,1) primary key,      --����(����)
TypeNo          nvarchar(50),		            --���
TariffNo	nvarchar(50),		            --˰��		            
NoIndex  	int,		                    --����
UserId  	int,		                    --������(�����
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go

if exists(select * from sysobjects where name='HSInfomation')---HS�����Լ������Ϣ

drop table HSInfomation
create table HSInfomation(
Id int identity(1,1) primary key,--����(����)
HSNumber nvarchar(50) ,		--HS����
TypeName nvarchar(500) ,	--��Ʒ����
tariff nvarchar(50) ,		--��ݹ�˰��    			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go

/*
***************���excel��·��*****************
*/
if exists(select * from sysobjects where name='excelFilePath')
drop table excelFilePath
create table excelFilePath(
Id	int identity(1,1) primary key,      --����(����) 
FilePath	nvarchar(100) ,	         	--���excel·��
tableIndex int,                         --����
UserId	int,		                    --������(�����			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go


/*

***************api������*****************
*/
create table apiOrder
(
	orderId nvarchar(20) primary key,
	realName nvarchar(10),
	provinceId nvarchar(10), --ʡID
	cityId nvarchar(10),--=����ID
	district nvarchar(10),--=����ID
	buyNameAddress nvarchar(200),--=��ַ��Ϣ
	postcode nvarchar(10),--=��������
	phone nvarchar(20),--=�ֻ�����
	orderMsg nvarchar(200),-- =������ע
	orderStatus int,--=����״̬ ����ֵ�� OrderStatus�ֶ�ֵ 1 ��ȷ�� 2 ȷ�� 3 ������ 4 ���� 5 �ջ������׳ɹ��� 11 �˵� 12 ȡ������
	itemPrice money,--=��Ʒ�ܼ�
	deliveryPrice money,--=��ݷ���
	favorablePrice money,--=�Żݷ���
	taxPrice money,--=˰��
	orderPrice money,--=��������Ʒ�ܼ�+��ݷ���-�Żݷ���+˰�ѣ�
	paidPrice money,--=ʵ��֧�����
	isPay int,--=0��δ֧����1����֧��
	payTime datetime,--=֧��ʱ��
	payOuterId nvarchar(100),--=֧����ˮ��
	createTime datetime,--=��������ʱ��
	
	invoiceType nvarchar(10),--=��Ʊ����
	invoiceTitle nvarchar(500),--=��Ʊ̧ͷ

	def1 nvarchar(20),--���֤����
	def2 nvarchar(20),--�Ƿ��ѷ��乩Ӧ��
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20)
)
go
/*
***************������Ϣ��*****************
*/
CREATE TABLE [dbo].[apiOrder1](
	orderId nvarchar(20) primary key,
	realName nvarchar(10),
	provinceId nvarchar(10), --ʡID
	cityId nvarchar(10),--=����ID
	district nvarchar(10),--=����ID
	buyNameAddress nvarchar(200),--=��ַ��Ϣ
	postcode nvarchar(10),--=��������
	phone nvarchar(20),--=�ֻ�����
	orderMsg nvarchar(200),-- =������ע
	orderStatus int,--=����״̬ ����ֵ�� OrderStatus�ֶ�ֵ 1 ��ȷ�� 2 ȷ�� 3 ʧ�� 4 ���� 5 �ջ� 11 �˵�
	itemPrice money,--=��Ʒ�ܼ�
	deliveryPrice money,--=��ݷ���
	favorablePrice money,--=�Żݷ���
	taxPrice money,--=˰��
	orderPrice money,--=��������Ʒ�ܼ�+��ݷ���-�Żݷ���+˰�ѣ�
	paidPrice money,--=ʵ��֧�����
	isPay int,--=0��δ֧����1����֧��
	payTime datetime,--=֧��ʱ��
	payOuterId nvarchar(100),--=֧����ˮ��
	createTime datetime,--=��������ʱ��
	
	invoiceType nvarchar(10),--=��Ʊ����
	invoiceTitle nvarchar(500),--=��Ʊ̧ͷ

	def1 nvarchar(20),--�����Ϣ
	def2 nvarchar(20),--�Ƿ��ѷ��乩Ӧ��
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20)
	)
GO
--drop table apiOrder


/*
***************api���������*****************
*/
create table apiOrderDetails(
	orderId nvarchar(20) foreign key references apiOrder(orderId),
	detailsOrderId nvarchar(20) primary key,
	detailsScode nvarchar(20),--=��Ʒscode
	detailsColor nvarchar(20), --��ɫ
	detailsImg nvarchar(500), --ͼƬ
	detailsItemPrice money,--=��Ʒ�ۼ�
	detailsTaxPrice money,--=˰�ѽ��
	detailsSaleCount int,--=��������
	detailsDeliveryPrice money,--=��ݷ���
	detailsStatus int,--����״̬
	
	detailsTime datetime, --�µ�ʱ��
	detailsPayTime datetime, --����ʱ��
	detailsEditTime datetime, --�༭ʱ��
	detailsSendTime datetime, --����ʱ��
	detailsSucessTime datetime, --�ɽ�ʱ��
	
	def1 nvarchar(20),--��ַ
	def2 nvarchar(20),
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20)
)
go
/*
***************��������*****************
*/


CREATE TABLE [dbo].[apiOrderDetails1](
	[orderId] [nchar](20) NOT NULL,--������
	[productScode] [nvarchar](50) NOT NULL,-- ����
	[orderNum] [int] NOT NULL,--����
	[orderCreateTime] [datetime] NOT NULL,--ʱ��
	[detailsStatus] [int] NOT NULL,
	[UserName] [nvarchar](20) NOT NULL,--������
	[reserved] [int] NOT NULL,--״̬
 CONSTRAINT [PK_apiOrderDetails1] PRIMARY KEY CLUSTERED 
(
	[orderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
--select * from  apiOrderDetails


/*
***************api����֧�������*****************
*/
create table apiOrderPayDetails(
	orderId  nvarchar(20) foreign key references apiOrder(orderId),
	payMentName nvarchar(20),--=֧����ʽ
	payPlatform nvarchar(20),--=֧��ƽ̨��ios/andriod/html5��
	payId nvarchar(20),--=֧���ڲ���ˮ��
	payOuterId nvarchar(20),--=֧���ⲿ��ˮ��
	payPrice money,--=֧�����
	payTime datetime,--=֧��ʱ��
	sellerAccount nvarchar(20),--=����֧���˺�
	buyerAccount nvarchar(20),--=���֧���˺�
	
	def1 nvarchar(20),
	def2 nvarchar(20),
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20)
)
go


/*
***************api������Żݱ�*****************
*/
create table apiOrderDiscount
(
	orderId  nvarchar(20) foreign key references apiOrder(orderId),
	dicountType int,--=�Ż�����
	dicountAmount money,--=�Żݽ��
	remark nvarchar(500),--=�Żݱ�ע
	
	def1 nvarchar(20),
	def2 nvarchar(20),
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20)
)
go


/*
***************appԴ������(��Ӧ�̷���)*****************
*/
create table apiSendOrder
(
	orderId  nvarchar(20) foreign key references apiOrder(orderId),--����
	detailsOrderId nvarchar(20) foreign key references apiOrderDetails(detailsOrderId),--�Ӷ���
	newOrderId  nvarchar(20) primary key ,--�¶���ID
	newScode nvarchar(20),--����
	newColor nvarchar(20),--��ɫ
	newSize nvarchar(20),--����
	newImg nvarchar(500),--ͼƬ
	newSaleCount int ,--����
	newStatus int,--״̬(������ǰ״̬��1Ϊ��ȷ�ϣ�2Ϊȷ�ϣ�3Ϊ��������4Ϊ������5���׳ɹ���6ͨ���쳣��7��ͨ�سɹ���11�˻���12ȡ��)
	createTime datetime,--����ʱ��
	editTime datetime,--����ʱ��
	showStatus int,--��ʾ״̬(0����ʾ��1��ʾ)
	sendSource nvarchar(20),--���ڹ�Ӧ��
	
	def1 nvarchar(20), --�̼챨��״̬(0ʧ�ܣ�1�ɹ���2δ�ϴ�)
	def2 nvarchar(20), --���ر���״̬(0ʧ�ܣ�1�ɹ���2δ�ϴ�)
	def3 nvarchar(20), --�����״̬(0ʧ�ܣ�1�ɹ���2δ�ϴ�)
	def4 nvarchar(20),
	def5 nvarchar(20)
)
go

/*
***************apiԴ������ݱ�(��Ӧ�̷���)*****************
*/
create table apiOrderExpress
(
	orderId  nvarchar(20) foreign key references apiOrder(orderId),--����
	detailsOrderId  nvarchar(20) foreign key references apiOrderDetails(detailsOrderId),--����
	newOrderId  nvarchar(20) foreign key references apiSendOrder(newOrderId) ,--�¶���ID
	
	expressId nvarchar(20),--��ݱ��
	expressNo nvarchar(20),--��ݵ���
	
	sourceCity1 nvarchar(20),--���͵�
	sourceCity2 nvarchar(20),--���͵�
	sourceCity3 nvarchar(20),--���͵�
	sourceAddress nvarchar(500),--���͵�
	sourcePhone nvarchar(20) default(0755-25182899),--�����˵绰(����)
	
	customCity1 nvarchar(20),--���յ�
	customCity2 nvarchar(20),--���յ�
	customCity3 nvarchar(20),--���յ�
	customAddress nvarchar(500),--���յ�
	
	productPrice money,--��Ʒ�۸�
	supportValuePrice money, --����
	
	
	def1 nvarchar(20),
	def2 nvarchar(20),
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20)
)
go



/*
***************��ݱ�*****************
*/
create table apiExpress
(
	expressId nvarchar(20), --��ݱ��
	expressName  nvarchar(20), --�������
	expressPrincipal nvarchar(20), --������
	expressPhone nvarchar(20), --�绰
	expressAddress nvarchar(200), --��ݹ�˾��ַ
	
	def1 nvarchar(20),
	def2 nvarchar(20),
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20)
)
go

			

/*��ѯ�����ֶ�����*/
select name from sys.syscolumns where id =object_id('tableFiledPerssion')


/*��tableFiledPerssion���в���table������table��ȼ�Ĭ��Ϊ0*/
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

/*��tableFiledPerssion���в���table�ֶΣ�table�ֶεȼ�Ϊtable����ID*/
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
***************������λ�����غ��̼������ͬ��ֻ�в��ֲ�ͬ�����Ǻ��ص����ݱ��̼�࣬�����Ժ��ؼ�����λ��Ϊ������λ��*****************
*/
create table apiUnit
(
	unitNo nvarchar(20), --��λ���
	unitName  nvarchar(20), --��λ����
	
	def1 nvarchar(20),
	def2 nvarchar(20),
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20)
)
go


/*
***************��Ʒ���ر���*****************
*/
create table CiqProductDetails
(
	ciqProductId nvarchar(128) not null,  --��Ʒ������(���128�ַ�,�羳������ҵϵͳ�е�Ψһ����ֶ�)
	ciqRemark nvarchar(1000),--��Ʒ��ע
	ciqScode nvarchar(50) primary key not null ,--����
	ciqScodeName nvarchar(50) not null,--��Ʒ����
	ciqSpec nvarchar(200) not null,--����ͺ�
	ciqHSNo nvarchar(200) not null,--��ƷHS����
	ciqAssemCountry nvarchar(200) default('yidali') not null, --ԭ����/����
	ciqProductNo int identity(1,1),--��Ʒ��ţ���������EntGoodsNo�ֶΣ�����С��6
	
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
***************�Ա����̻*****************
*/
create table activeShop(
	acId int identity(1,1) primary key,
	acName nvarchar(200), --�����
	actimePrepareStart datetime default(getdate()), --�Ԥ�ȿ�ʼʱ��
	actimePrepareEnd  datetime default(getdate()), --�Ԥ�Ƚ���ʱ��
	acTimestart datetime default(getdate()), --���ʼʱ��
	acTimeend datetime default(getdate()), --�����ʱ��
	acCreateTime datetime default(getdate()), --����ʱ��
	
	def1 nvarchar(200),
	def2 nvarchar(200),
	def3 nvarchar(200),
	def4 nvarchar(200),
	def5 nvarchar(200)
)
go
select * from activeShop



/*
***************�Ա����̻��Ʒ*****************
*/
create table activeShopOrder(
	asoId int identity(1,1) primary key,
	acId int foreign key references activeShop(acId),		--���̻ID(���)
	asoScode nvarchar(200), --����
	asoCreateTime  datetime default(getdate()), --��Ʒ����ʱ��
	
	def1 nvarchar(200),
	def2 nvarchar(200),
	def3 nvarchar(200),
	def4 nvarchar(200),
	def5 nvarchar(200)
)
go
select * from activeShopOrder



/*
***************������������*****************
*/
create table orderComments
(
	ocId int identity(1,1) primary key,
	ocOrderId nvarchar(20),--����
	ocScode nvarchar(200),--����
	ocComment nvarchar(500),--����
	ocRemark nvarchar(500),--��ע
	ocBanner nvarchar(10),--����
	ocPostPrice money,--�ʷ�
	ocOtherPrice money,--�����۸�
	
	def1 nvarchar(200),
	def2 nvarchar(200),
	def3 nvarchar(200),
	def4 nvarchar(200),
	def5 nvarchar(200)
)
go


/*
***************֧�����ؽ��*****************
*/
create table payCustomsResult
(
	OrderChildId nvarchar(20) primary key, --�Ӷ���ID

	--��������
	isSuccess nvarchar(2) not null,--�����Ƿ�ɹ������Ƿ�ɹ�������ɹ�������ҵ����ɹ���(T����ɹ�,F����ʧ��)
	sign_type nvarchar(5),--ǩ����ʽ(DSA��RSA��MD5 ����ֵ��ѡ�������д)
	sign_qm nvarchar(50),--ǩ��(��μ���7 ǩ�����ơ���)
	error nvarchar(500),--�������(����ɹ�ʱ�������ڱ�����������ʧ��ʱ��������Ϊ������룬�μ���8.2 ��������롱�͡�8.3 ϵͳ�����롱)
	
	--ҵ�����
	result_code nvarchar(32) not null, --��Ӧ��(��������Ӧ�롣SUCCESS���ɹ�,FAIL��ʧ��)
	trade_no nvarchar(64),--֧�������׺�(�ý�����֧����ϵͳ�еĽ�����ˮ�š�� 64 λ��)
	alipay_declare_no nvarchar(64),--֧����������ˮ��(֧����������ˮ��)
	detail_error_code nvarchar(48),--��ϸ������(�Է�����Ӧ�����ԭ��˵������μ���8.1 ҵ������롱���� result_code ��Ӧ��ΪSUCCESS ʱ�������ظò���)
	detail_error_des nvarchar(64),--��ϸ��������(����ϸ�������������˵������μ���8.1 ҵ������롱�еġ����塱���� result_code ��Ӧ��ΪSUCCESS ʱ�������ظò�����)

	def1 nvarchar(50),
	def2 nvarchar(50),
	def3 nvarchar(50),
	def4 nvarchar(50),
	def5 nvarchar(50),
)
go


/*
***************�������ؽ��*****************
*/

--�̼챨��״̬(0ʧ�ܣ�1�ɹ���2δ�ϴ�,3�ϴ��ɹ�)
--���ر���״̬(0ʧ�ܣ�1�ɹ���2δ�ϴ�,3�ϴ��ɹ�)
--�����״̬(0ʧ�ܣ�1�ɹ���2δ�ϴ�,3�ϴ��ɹ�)
create table orderCustomsResult
(
	SJOrgOrderChildId nvarchar(20)  primary key, --�Ӷ���ID

	--�̼�
	--SJOrgMessageID nvarchar(50),--ԭ���ͱ����б��ı�ţ����50�ַ�
	SJOrgReturnTime datetime,--��ִʱ��	��ʽ��YYYY-MM-DD HH:MI:SS  --OrgSendTime
	SJOrgSendTime datetime,--����ʱ��	��ʽ��YYYYMMDDHHMISS  --OrgRecTime
	SJOrgStatus nvarchar(20),	--�걨״̬	�걨״̬������Ϊ2���ַ���10���ѽ��գ��ȴ���ˣ�20���걨ʧ��
	SJOrgNotes nvarchar(1000),--�걨������Ϣ	���걨ʧ�ܣ���Ŵ�����Ϣ����󳤶�1000���ַ���
	SJstatus nvarchar(1), --����״̬(0ʧ�ܣ�1�ɹ���2δ�ϴ�,3�ϴ��ɹ�)

	--����
	--HGOldMessageId nvarchar(30), --��Ӧԭ���ĵı��
	HGSendDate datetime, --���ķ���ʱ��
	HGReturnDate datetime, --��ִ����ʱ��
	HGReturnCode nvarchar(5),--����״̬(c01�ɹ�)
	HGReturnInfo nvarchar(255),--��ִ˵��
	HGAttachedFlag nvarchar(2),--0�������ݸ�����1�������ݸ�����=0�������ݸ�������ִ�����У���ҵ�����ݣ�=1�������ݸ�������ִ�����У���ҵ�����ݣ�
	HGstatus nvarchar(1), --����״̬(0ʧ�ܣ�1�ɹ���2δ�ϴ�,3�ϴ��ɹ�)

	--����
	BBCask int,--1�ɹ���0ʧ��
	BBCSendDate datetime, --���ķ���ʱ��
	BBCReturnData datetime, --��ִʱ��
	BBCmessage nvarchar(200), --������Ϣ
	BBCorderCode nvarchar(200), --oms�Ķ�����
	BBCerrorMessage nvarchar(1000), --������ϸ
	BBCstatus nvarchar(10), --����״̬(0ʧ�ܣ�1�ɹ�)


	def1 nvarchar(50), --�̼챨������(������׺)
	def2 nvarchar(50), --���ر�������(������׺)
	def3 nvarchar(50),
	def4 nvarchar(50),
	def5 nvarchar(50),
)
go


/*
***************��Ʒ���ؽ��*****************
*/
create table productCustomsResult
(
	productScode nvarchar(20) primary key,  --����

	--�̼�
	SJOrgReturnTime datetime,--��ִʱ��	��ʽ��YYYY-MM-DD HH:MI:SS  --OrgSendTime
	SJOrgSendTime datetime,--����ʱ��	��ʽ��YYYYMMDDHHMISS  --OrgRecTime
	SJOrgStatus nvarchar(20),	--�걨״̬	�걨״̬������Ϊ2���ַ���10���ѽ��գ��ȴ���ˣ�20���걨ʧ��
	SJOrgNotes nvarchar(1000),--�걨������Ϣ	���걨ʧ�ܣ���Ŵ�����Ϣ����󳤶�1000���ַ���
	SJstatus nvarchar(1), --����״̬(0ʧ�ܣ�1�ɹ���2δ�ϴ�,3�ϴ��ɹ�)
	--�̼����
	CIQGoodsNO nvarchar(50), --	��Ʒ������	���50�ַ�,��Ʒ���ͨ��ʱ����һ����Ʒ�����š�
	RegStatus  nvarchar(10), --ICIP��ִ״̬ ���4�ַ� 10:ͨ��;20:��ͨ��
	RegNotes nvarchar(256), --ICIP��ִ��Ϣ	���256�ַ�

	--����



	--����
	BBCask nvarchar(1), -- 1: �ɹ� 0: ʧ��
	BBCSendDate datetime, --���ķ���ʱ��
	BBCReturnData datetime, --��ִʱ��
	BBCmessage nvarchar(500), --ϵͳ���ص���Ϣ
	BBCskuNo nvarchar(50), --�ɹ�������Ʒ��Ĳ�ƷSku����
	BBCerrorMessage nvarchar(500), --������Ϣ

	def1 nvarchar(50),--��������(������׺)
	def2 nvarchar(50),
	def3 nvarchar(50),
	def4 nvarchar(50),
	def5 nvarchar(50)
)
go


--//�����,Ĭ��Ϊ0
--alter table sysQuestionType(����) add stSon(����) int default(0)
--//ɾ����
--alter table usertable(����) drop column ExtendPPCount(����)
--//�޸��е�����
--alter table usertable(����) alter column ExtendPPCount(����) nvarchar(50)(����)
--//����ⲿ���������ExamScore��Ϊ�����ExamMessageΪ��������
--alter table ExamScore add constraint fk_message_score foreign key(messageId)  references ExamMessage(messageId)

/*
***************�޸�apiorder��def1���Ƿ��ѷ��乩Ӧ��(20150702)*****************
*/



/*
***************��Ʒ���ؽ��*****************
*/
create table customsCountry
(
	id int identity(1,1) primary key,--id
	countryName nvarchar(50),--��������
	countryNo nvarchar(20),--���ұ��
	
	def1 nvarchar(20),
	def2 nvarchar(20),
	def3 nvarchar(20),
	def4 nvarchar(20),
	def5 nvarchar(20),
)
go

select * from customsCountry


/*
***************ϵͳ��־*****************
*/
if exists(select * from sysobjects where name='systemErrorLog')
drop table systemErrorLog
create table systemErrorLog
(
	id int primary key,                             --���
	errorMsg nvarchar(1000),                        --�쳣��Ϣ
	errorObject nvarchar(1000),                     --�쳣����
	errorStackTrace nvarchar(1000),                 --���ö�ջ
	errorMethod nvarchar(1000),                     --��������
	errorMenu nvarchar(50),                         --�쳣���ڲ˵�
	errorFunc nvarchar(50),                         --�쳣���ڹ���
	errorUser nvarchar(50),                         --�쳣������
	errorIP nvarchar(50),                           --�쳣���������ڵ�ַ
	createdatetime datetime default(getdate()),     --�쳣ʱ��

	Def1	nvarchar(50),		     --Ĭ��1
	Def2	nvarchar(50),		     --Ĭ��2
	Def3	nvarchar(50),		     --Ĭ��3
	Def4	nvarchar(50),		     --Ĭ��4
	Def5	nvarchar(50)		     --Ĭ��5
)
go


/*
***************������־*****************
*/
if exists(select * from sysobjects where name='operatingLog')
drop table operatingLog
create table operatingLog
(
	id int primary key,                              --���
	operatingMenu nvarchar(50),                      --�������ڲ˵�
	operatingFunc nvarchar(50),                      --�������ڹ���
	operatingUser nvarchar(50),                      --������
	operatingIP nvarchar(50),                        --���������ڵ�ַ
	operatingDate datetime default(getdate()),       --����ʱ��

	Def1	nvarchar(50),		     --Ĭ��1
	Def2	nvarchar(50),		     --Ĭ��2
	Def3	nvarchar(50),		     --Ĭ��3
	Def4	nvarchar(50),		     --Ĭ��4
	Def5	nvarchar(50)		     --Ĭ��5
)
go






/*end*********************************************************lcg***********************************************************/






/**********************************************************sqy***********************************************************/.

/*
***************�Ա�Ʒ�Ʊ�*****************
*/    
if exists(select * from sysobjects where name='TBBrand')
drop table TBBrand
create table TBBrand(
Id int identity(1,1) primary key,--����(����)
TBBrandName nvarchar(30) ,	         --�Ա�Ʒ����
vid nvarchar(1000) ,		     --�Ա�vid		
Def1	nvarchar(50),		     --Ĭ��1
Def2	nvarchar(50),		     --Ĭ��2
Def3	nvarchar(50),		     --Ĭ��3
Def4	nvarchar(50),		     --Ĭ��4
Def5	nvarchar(50)		     --Ĭ��5
)
go

/*
***************�Ա�����*****************
*/
  if exists(select * from sysobjects where name='TBProducttype')
drop table TBProducttype
create table TBProducttype(
Id int identity(1,1) primary key,--����(����)
TBtypeName nvarchar(30) ,	     --�Ա������
cid nvarchar(1000) ,		     --�Ա�cid	
Def1	nvarchar(50),		     --Ĭ��1
Def2	nvarchar(50),		     --Ĭ��2
Def3	nvarchar(50),		     --Ĭ��3
Def4	nvarchar(50),		     --Ĭ��4
Def5	nvarchar(50)		     --Ĭ��5
)
go

/*
***************�Ա����Ա�*****************
*/
  if exists(select * from sysobjects where name='TBProductProperty')
drop table TBProductProperty
create table TBProductProperty(
Id int identity(1,1) primary key,--����(����)
TBPropertyName nvarchar(30) ,	     --�Ա�������
vid nvarchar(1000) ,		     --�Ա�vid
parent_cid nvarchar(1000) ,	     --�Ա�cid				
Def1	nvarchar(50),		     --Ĭ��1
Def2	nvarchar(50),		     --Ĭ��2
Def3	nvarchar(50),		     --Ĭ��3
Def4	nvarchar(50),		     --Ĭ��4
Def5	nvarchar(50)		     --Ĭ��5
)
go

/*
***************�Ա�����ֵ��*****************
*/
  if exists(select * from sysobjects where name='TBProductPropertyValue')
drop table TBProductPropertyValue
create table TBProductPropertyValue(
Id int identity(1,1) primary key,--����(����)
TBPropertyValue nvarchar(30) ,	     --�Ա�����ֵ
vid nvarchar(1000) ,		     --�Ա�����ֵvid	
parent_vid nvarchar(1000) ,          --�Ա�����vid
Def1	nvarchar(50),		     --Ĭ��1
Def2	nvarchar(50),		     --Ĭ��2
Def3	nvarchar(50),		     --Ĭ��3
Def4	nvarchar(50),		     --Ĭ��4
Def5	nvarchar(50)		     --Ĭ��5
)
go

/*
***************��Ӧ��Ʒ�ƶ�Ӧ��*****************
*/
if exists(select * from sysobjects where name='BrandVen')
drop table BrandVen
create table BrandVen(
Id	int identity(1,1) primary key,		--����(����)
BrandName	nvarchar(20),		--Ʒ������
BrandAbridge	nvarchar(20),		--Ʒ��������д
BrandCode	nvarchar(20),		--���Ʊ���
BrandNameVen	nvarchar(20),		--��Ӧ��Ʒ������
Vencode	nvarchar(20),			--Ʒ������Դ
BrandIndex	int,		--����
UserId	int,		--������(�����
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50),		--Ĭ��5
)
go

/*
***************��Ӧ������*****************
*/
if exists(select * from sysobjects where name='producttypeVen')
drop table producttypeVen
create table producttypeVen(
Id	int identity(1,1) primary key,		--����(����)
BigId	Int	foreign key references productbigtype(id),	--�����ID�������
TypeName	nvarchar(20),		--�������
TypeNo	nvarchar(20),		--������
TypeNameVen	nvarchar(20),		--��Ӧ�����
Vencode	nvarchar(20),		--��Ӧ��
typeIndex	int,		--����
UserId	int,		--������(�����

Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50),		--Ĭ��5
)
go

/*
***************������ע��Ϣ��*****************
*/
if exists(select * from sysobjects where name='apiOrderRemark')
drop table apiOrderRemark
create table apiOrderRemark(
Id int identity(1,1) primary key,--����(����)
OrderId nvarchar(30) ,	--��������
Remark nvarchar(max) ,		--��ע��Ϣ
Edittime nvarchar(20),      --����ʱ��	
UserId int,		            --������(�����		
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)

/*
***************��Ʒ���ؽ��*****************
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



/***************������Ϣ��******************/
if exists(select * from sysobjects where name='TradeInfo')
drop table TradeInfo
create table TradeInfo(
Id	int identity(1,1) primary key,		--����(����)
OrderId	nvarchar(50),					--�������
SellPrice	nvarchar(50),				--�ɽ��ܽ��
SaleStates	nvarchar(50),				--�����Ƿ�ɹ� 1.�Ѹ��� 2.δ���� 3.���׳ɹ� 4.���׹ر�(3��4��Ҫ�ж�ͨ��Productinfo)
Evaluate	nvarchar(50),				--�Ƿ�����
ServiceId	nvarchar(50),				--�Ӵ��ͷ�
ServiceRemark	nvarchar(max),			--�ͷ���ע
OrderTime	datetime,				    --�µ�����
UserId int,								--������
			
Def1	nvarchar(50),					--Ĭ��1
Def2	nvarchar(50),					--Ĭ��2
Def3	nvarchar(50),					--Ĭ��3
Def4	nvarchar(50),					--Ĭ��4
Def5	nvarchar(50),					--Ĭ��5
)
go


/***************�����ͻ���Ϣ��******************/
if exists(select * from sysobjects where name='CustomerInfo')
drop table CustomerInfo
create table CustomerInfo(
Id	int identity(1,1) primary key,		--����(����)
OrderId	nvarchar(50),					--�������
CustomerId	nvarchar(50),				--�ͻ�Id
Shop	nvarchar(50),					--����/ƽ̨
Contactperson	nvarchar(50),			--��ϵ��
Telephone	nvarchar(50),				--�绰
Phone	nvarchar(50),					--�ֻ���
Weixin	nvarchar(50),					--΢��
QQNo	nvarchar(50),					--QQ
Provinces	nvarchar(50),				--ʡ
City	nvarchar(50),					--��
CusAddress	nvarchar(max),				--������ַ	
Payment	nvarchar(50),					--֧��ƽ̨
Account	nvarchar(50),					--֧���˺�
Def1	nvarchar(50),					--Ĭ��1
Def2	nvarchar(50),					--Ĭ��2
Def3	nvarchar(50),					--Ĭ��3
Def4	nvarchar(50),					--Ĭ��4
Def5	nvarchar(50),					--Ĭ��5
)
go


/***************��Ʒ��Ϣ��******************/
if exists(select * from sysobjects where name='ProductInfo')
drop table ProductInfo
create table ProductInfo(
Id	int identity(1,1) primary key,		--����(����)
OrderId	nvarchar(50),					--�������
Scode	nvarchar(50),					--��Ʒ���
Brand	nvarchar(50),					--Ʒ�� -��Ч
Color	nvarchar(50),					--��ɫ -��Ч
TypeNo	nvarchar(50),					--��� -��Ч
Imagefile	nvarchar(200),				--��Ʒʾͼ -��Ч
Size	nvarchar(50),					--���� -��Ч
Number	nvarchar(50),					--����
ProDetails	nvarchar(max),				--��Ʒ����
ProLink	nvarchar(50),					--��Ʒ����
DeliveryAttri	nvarchar(50),			--��������
LastOrderId	nvarchar(50),				--�ϴζ�����
SellPrice	nvarchar(50),				--�ɽ����
Warehouse	nvarchar(50),				--������
UserId int,								--������

Def1	nvarchar(50),					--����״̬1.δ���� 2.�ѷ���  3.�˻���  4.�˻����  5.�˿���  6.�˿����   7.���׳ɹ�
Def2	nvarchar(50),					--ϵͳ����
Def3	datetime,						--Ĭ��3--������ʱ��
Def4	nvarchar(50),					--Ĭ��4
Def5	nvarchar(50),					--Ĭ��5
)
go


/***************�˻���Ϣ******************/
if exists(select * from sysobjects where name='RetProductInfo')
drop table RetProductInfo
create table RetProductInfo(
Id	int identity(1,1) primary key,		--����(����)
OrderId	nvarchar(50),					--�������
RetPrice	nvarchar(50),				--�˿���
Express	nvarchar(50),					--��ݹ�˾
ExpressNo	nvarchar(50),				--��ݵ���
RetDetails	nvarchar(max),				--�˻�˵��
RetType	nvarchar(50),					--�˻������� 1.�˿� 2.�˻� 5.�˻�(δ����)
Receiver 	nvarchar(50),				--�ջ���	
ServiceId	nvarchar(50),				--����ͷ�
RetAccount	nvarchar(50),				--�˿��˺�
UserId int,								--������ 
		
Def1	nvarchar(50),					--�˻�״̬ 1.�����˿� 2.�����˻�3.�˿�ɹ� 4.�˻��ɹ� 5.�����˻�(δ����) 6.�˻�����
Def2	nvarchar(50),					--��Ʒ���
Def3	nvarchar(50),					--�˻����� 1.���������� 2.��ɫ�� 3.����
Def4	datetime,						--Ĭ��4--������ʱ��
Def5	nvarchar(50),					--Ĭ��5
)
go



/***************������¼******************/
if exists(select * from sysobjects where name='ShipmentRecord')
drop table ShipmentRecord
create table ShipmentRecord(
Id	int identity(1,1) primary key,		--����(����)
OrderId	nvarchar(50),					--�������
ExPrice	nvarchar(50),					--�������
SendTime	datetime,					--����ʱ��
Express	nvarchar(50),					--��ݹ�˾
ExpressNo		nvarchar(50),			--��ݵ���
YFHKD	nvarchar(50),					--�˷�HKD
YFRMB	nvarchar(50),					--�˷�RMB
RetRemark	nvarchar(max),				--�˻�˵��
SendPerson	nvarchar(50),				--������
SendType	nvarchar(50),				--�������� 1.�¶��� 
SendStatus	nvarchar(50),				--����״̬ 1.�ѷ��� 2.�����ر�
UserId int,								--������
			
Def1	nvarchar(50),					--Ĭ��1
Def2	nvarchar(50),					--��Ʒ���
Def3	nvarchar(50),					--Ĭ��3
Def4	nvarchar(50),					--Ĭ��4
Def5	nvarchar(50),					--Ĭ��5
)
go






/***************�˻�����******************/
if exists(select * from sysobjects where name='RetBalance')
drop table RetBalance
create table RetBalance(
Id	int identity(1,1) primary key,		--����(����)
OrderId	nvarchar(50),					--�������
Scode	nvarchar(50),					--��Ʒ���
Brand	nvarchar(50),					--Ʒ�� -��Ч
Color	nvarchar(50),					--��ɫ -��Ч
TypeNo	nvarchar(50),					--��� -��Ч
Imagefile	nvarchar(200),				--��Ʒʾͼ -��Ч
Size	nvarchar(50),					--���� -��Ч
Price	nvarchar(50),					--���׼�
Number	nvarchar(50),					--����
QuaLevel	nvarchar(50),				--�����ȼ�
RetTime	datetime,						--�ջ�ʱ��
ProDetails	nvarchar(max),				--��Ʒ����
ProLink	nvarchar(50),					--��Ʒ����
RetNum	nvarchar(50),					--�˻�����
LastOrderId	nvarchar(50),				--�ϴζ�����
SellPrice	nvarchar(50),				--�ɽ����
Heidui	nvarchar(50),					--�˶�
ExBalance	nvarchar(50),				--�˶�ת��
UserId int,								--������

Def1	nvarchar(50),					--Ĭ��1
Def2	nvarchar(50),					--Ĭ��2
Def3	nvarchar(50),					--Ĭ��3
Def4	nvarchar(50),					--Ĭ��4
Def5	nvarchar(50),					--Ĭ��5
)
go



/***************������¼��******************/
if exists(select * from sysobjects where name='OperationRecord')
drop table OperationRecord
create table OperationRecord(
Id	int identity(1,1) primary key,		--����(����)
OrderId	nvarchar(50),					--�������
OperaTable	nvarchar(50),				--�������
OperaType	nvarchar(50),				--��������
OperaTime	datetime,					--����ʱ��
UserId int,								--������
Def1	nvarchar(50),					--Ĭ��1
Def2	nvarchar(50),					--Ĭ��2
Def3	nvarchar(50),					--Ĭ��3
Def4	nvarchar(50),					--Ĭ��4
Def5	nvarchar(50),					--Ĭ��5
)


/***************�ͻ���Ϣ��******************/
if exists(select * from sysobjects where name='custom')
drop table custom
create table custom(
Id	int identity(1,1) primary key,		--����(����)
CustomerId	nvarchar(50),				--�ͻ�Id
Shop	nvarchar(50),					--����/ƽ̨
Contactperson	nvarchar(50),			--��ϵ��
Sex				int ,					--�Ա� 0.Ů 1.��
Age				int,					--����
Birthday		nvarchar(50),			--����
IDNumber		nvarchar(50),			--���֤
Telephone	nvarchar(50),				--�绰
Phone	nvarchar(50),					--�ֻ���
Weixin	nvarchar(50),					--΢��
QQNo	nvarchar(50),					--QQ
CustomerLevel	nvarchar(50),				--�û��ȼ�
Remark	nvarchar(max),					--��ע
CustomerServiceId  int,					--�ͷ�Id
UserId		int,						--������
Def1	nvarchar(50),					--Ĭ��1
Def2	nvarchar(50),					--Ĭ��2
Def3	nvarchar(50),					--Ĭ��3
Def4	nvarchar(50),					--Ĭ��4
Def5	nvarchar(50),					--Ĭ��5
)
go


/***************�ͻ���ַ��******************/
if exists(select * from sysobjects where name='customAddress')
drop table customAddress
create table customAddress(
Id	int identity(1,1) primary key,		--����(����)
CustomerId	nvarchar(50),				--�ͻ�Id
Provinces	nvarchar(50),				--ʡ
City	nvarchar(50),					--��
District nvarchar(50),					--��/��
CusAddress	nvarchar(max),				--�ջ���ַ
CustomerServiceId  int,					--�ͷ�Id
UserId		int,						--������	
Def1	nvarchar(50),					--Ĭ��1
Def2	nvarchar(50),					--Ĭ��2
Def3	nvarchar(50),					--Ĭ��3
Def4	nvarchar(50),					--Ĭ��4
Def5	nvarchar(50),					--Ĭ��5
)
go

/*
***************��Ӧ���ڶ�Ӧ��*****************
*/
if exists(select * from sysobjects where name='SeasonVen')
drop table SeasonVen
create table SeasonVen(
Id	int identity(1,1) primary key,		--����(����)
Cat1	nvarchar(20),		--Ʒ������
Cat1Ven	nvarchar(20),		--��Ӧ��Ʒ������
Vencode	nvarchar(20),			--Ʒ������Դ
BrandIndex	int,		--����
UserId	int,		--������
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50),		--Ĭ��5
)
go


/*
***************���ڱ�*****************
*/
if exists(select * from sysobjects where name='Season')
drop table Season
create table Season(
Id	int identity(1,1) primary key,		--����(����)
Cat1	nvarchar(20),		--Ʒ������
Cat1Index	int,		--����
UserId	int,		--������
			
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50),		--Ĭ��5
)
go


/*
***************������*****************
*/
if exists(select * from sysobjects where name='AreaTable')
drop table AreaTable
create table AreaTable(
Id	int identity(1,1) primary key,		--����(����)
Region_id	int,				--����Id
Parent_id	int,				--�̳�Id
Region_name	nvarchar(50),		--��������
Post_code	int,				--�ʱ�
Update_time datetime,			--����ʱ��
Def1	nvarchar(50),			--Ĭ��1
Def2	nvarchar(50),			--Ĭ��2
Def3	nvarchar(50),			--Ĭ��3
Def4	nvarchar(50),			--Ĭ��4
Def5	nvarchar(50),			--Ĭ��5
)
go


/*
***************�ͻ����û���*****************
*/
if exists(select * from sysobjects where name='ClientLogin')
drop table ClientLogin
create table ClientLogin(
Id	int identity(1,1) primary key,		--����(����)
userName	nvarchar(20),		--�ʺ�
userPwd	nvarchar(20),		--����
sourceId	int,		--��Ӧ��
			
Def1	nvarchar(50),		--����
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50),		--Ĭ��5
)
go



/**********************************************************rwh***********************************************************/















/**********************************************************fj***********************************************************/
/*
***************�д�Ʒ��ע*****************
*/
if exists(select * from sysobjects where name='DefectiveRemark')
drop table DefectiveRemark
create table DefectiveRemark(
Id	int identity(1,1) primary key,		--����(����)
Scode	nvarchar(16),		--����
ScodeMarKer nvarchar(50),	--�д�Ʒ���
Bcode	nvarchar(16),		--����1
Bcode2	nvarchar(20),		--����2
Descript	nvarchar(60),	--Ӣ������
Cdescript	nvarchar(60),	--��������
Unit	nvarchar(6),		--��λ
Currency	nvarchar(3),	--����
Cat	nvarchar(5),			--Ʒ��
Cat1	nvarchar(5),		--����
Cat2	nvarchar(5),		--���
Clolor	nvarchar(20),		--��ɫ
Size	nvarchar(20),		--�ߴ�
Style	nvarchar(16),		--���
Pricea	numeric(13,2),		--�۸�1
Priceb	numeric(13,2),		--�۸�2
Pricec	numeric(13,2),		--�۸�3
Priced	numeric(13,2),		--�۸�4
Pricee	numeric(13,2),		--�۸�5
Disca	numeric(6,2),		--�ۿ�1
Discb	numeric(6,2),		--�ۿ�2
Discc	numeric(6,2),		--�ۿ�3
Discd	numeric(6,2),		--�ۿ�4
Disce	numeric(6,2),		--�ۿ�5
Vencode	nvarchar(10),		--��Ӧ�̱��(���)
Model	nvarchar(30),		--�ͺ�
Loc	nvarchar(5),			--����
Balance	int,				--����
Lastgrnd	datetime,		--�ջ�����(����Ʒ)
Imagefile	nvarchar(500),	--��ƷͼƬ
ProductRemark nvarchar(1000),--���˵��
Def1	int,				--�ϴο��
Def2	nvarchar(50),		---�Ƿ�Ϊ�д�Ʒ
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50),		--Ĭ��5
Def6	nvarchar(50),		--Ĭ��6
Def7	nvarchar(50),		--Ĭ��7
Def8	nvarchar(50),		--Ĭ��8
Def9	nvarchar(50),		--Ĭ��9
Def10	nvarchar(50),		--Ĭ��10
Def11	nvarchar(50),		--Ĭ��11
)
go
/*�ͻ��˼������� ������־*/
if exists(select * from sysobjects where name='ClientError')
drop table ClientError
create table ClientError(
Id	int identity(1,1) primary key,		                --����(����)
ErrorMsg	nvarchar(20),								--������Ϣ
ErrorTime	nvarchar(20) not null ,						--������ʱ��
ErrorClient	nvarchar(150) not null,						--��������ĳ���
ErrorDetails	nvarchar(150) not null,					--��������
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go

/*����ͻ���*/
if exists(select * from sysobjects where name='ApplicationsClient')
drop table ApplicationsClient
create table ApplicationsClient(
Id int identity(1,1) primary key,--����(����)
ClientMark nvarchar(500) ,	--�ͻ���˵��
ClientName nvarchar(100) ,	--�ͻ�������	
Def1	nvarchar(50),		--Ĭ��1
Def2	nvarchar(50),		--Ĭ��2
Def3	nvarchar(50),		--Ĭ��3
Def4	nvarchar(50),		--Ĭ��4
Def5	nvarchar(50)		--Ĭ��5
)
go


/*
***************�Ա���Ŷ�ӦSKU����*****************
*/
if exists(select * from sysobjects where name='TbSkuScode')
drop table TbSkuScode
create table TbSkuScode(
Id int identity(1,1) primary key,--����(����)
TbRemarkId nvarchar(30) ,	--�Ա���Ʒ���
TbStyle nvarchar(50),		--�Ա����
TbSkuId nvarchar(1000) ,	--�Ա�SKUID
TbImage nvarchar(500),		--�Ա�ͼƬ		
Scode	nvarchar(50),		--����
Color	nvarchar(50),		--��ɫ
Balance	nvarchar(50),		--���
Price	nvarchar(50),		--�۸�
SaleStatus nvarchar(20),	--����״̬   -- onsale ������ ---instock�ֿ���
CreateTime	nvarchar(50),	--����ʱ��
Def1 nvarchar(50),
Def2 nvarchar(50),
Def3 nvarchar(50),
Def4 nvarchar(50),
Def5 nvarchar(50),
Def6 nvarchar(50)
)
go









































