create table Currencies
(
	[Id] int  primary key identity,
	[Name] varchar(100),
	[Count] int,
	[Price] varchar(100)
)



create table Investors
(
	[Id] int  primary key identity,
	[Name] varchar(100),
	[Surname] varchar(100),
	[Age] int,
)


create table InvestorCurrencies
(
	[Id] int primary key,
	[CurrencyId] int,
	[InvestorId] int
)