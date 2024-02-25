create table Users
(
	[Id] int  primary key identity,
	[Name] varchar(100),
	[Surname] varchar(100),
	[Age] int,
	[Balance] decimal
)



create table Transactions
(
	[Id] int  primary key identity,
	[Amount] decimal,
	[Description] varchar(100)
)

create table Logs
(
	[Id] int primary key identity,
    [UserId] int,
    [Url] nvarchar(max),
    [MethodType] nvarchar(max),
    [StatusCode] int,
    [RequestBody] nvarchar(max),
    [ResponseBody] nvarchar(max)
)