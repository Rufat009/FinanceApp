create database BinanceAppDb;

use BinanceAppDb;

create table Cryptocurrencies (
	
     [Id] int primary key identity,
     [Name] nvarchar(100),
     [Count] int,
     [Price] money

)