CREATE TABLE [dbo].[Clients] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [Client_surname]    NVARCHAR (50) NOT NULL,
    [Client_name]       NVARCHAR (50) NOT NULL,
    [Client_patronymic] NVARCHAR (50) NOT NULL,
    [Phone_number]      NVARCHAR (50) NULL,
    [Email]             NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED ([Id] ASC)
);

insert into Clients(Client_surname,Client_name,Client_patronymic,Phone_number,Email) 
	values(N'Пономарёв',N'Иван',N'Валерьянович','+79515644485','valera99@gmail.com')
insert into Clients(Client_surname,Client_name,Client_patronymic,Phone_number,Email) 
	values(N'Звездаков',N'Пётр',N'Филатович','+79516485168','petya111@gmail.com')