/****** Object:  Table [dbo].[State]    Script Date: 04/15/2011 13:36:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[State]') AND type in (N'U'))
DROP TABLE [dbo].[State]
GO
/****** Object:  Table [dbo].[State]    Script Date: 04/15/2011 13:36:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[State]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[State](
	[Code] [char](2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Abbreviation] [char](2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Name] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Display] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[LastUpdated] [date] NULL
 CONSTRAINT [pk_d_state] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'01', N'AL', N'Alabama', N'Alabama')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'02', N'AK', N'Alaska', N'Alaska')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'04', N'AZ', N'Arizona', N'Arizona')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'05', N'AR', N'Arkansas', N'Arkansas')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'06', N'CA', N'California', N'California')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'08', N'CO', N'Colorado', N'Colorado')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'09', N'CT', N'Connecticut', N'Connecticut')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'10', N'DE', N'Delaware', N'Delaware')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'11', N'DC', N'District of Columbia', N'District of Columbia')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'12', N'FL', N'Florida', N'Florida')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'13', N'GA', N'Georgia', N'Georgia')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'15', N'HI', N'Hawaii', N'Hawaii')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'16', N'ID', N'Idaho', N'Idaho')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'17', N'IL', N'Illinois', N'Illinois')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'18', N'IN', N'Indiana', N'Indiana')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'19', N'IA', N'Iowa', N'Iowa')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'20', N'KS', N'Kansas', N'Kansas')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'21', N'KY', N'Kentucky', N'Kentucky')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'22', N'LA', N'Louisiana', N'Louisiana')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'23', N'ME', N'Maine', N'Maine')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'24', N'MD', N'Maryland', N'Maryland')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'25', N'MA', N'Massachusetts', N'Massachusetts')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'26', N'MI', N'Michigan', N'Michigan')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'27', N'MN', N'Minnesota', N'Minnesota')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'28', N'MS', N'Mississippi', N'Mississippi')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'29', N'MO', N'Missouri', N'Missouri')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'30', N'MT', N'Montana', N'Montana')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'31', N'NE', N'Nebraska', N'Nebraska')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'32', N'NV', N'Nevada', N'Nevada')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'33', N'NH', N'New Hampshire', N'New Hampshire')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'34', N'NJ', N'New Jersey', N'New Jersey')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'35', N'NM', N'New Mexico', N'New Mexico')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'36', N'NY', N'New York', N'New York')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'37', N'NC', N'North Carolina', N'North Carolina')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'38', N'ND', N'North Dakota', N'North Dakota')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'39', N'OH', N'Ohio', N'Ohio')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'40', N'OK', N'Oklahoma', N'Oklahoma')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'41', N'OR', N'Oregon', N'Oregon')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'42', N'PA', N'Pennsylvania', N'Pennsylvania')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'44', N'RI', N'Rhode Island', N'Rhode Island')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'45', N'SC', N'South Carolina', N'South Carolina')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'46', N'SD', N'South Dakota', N'South Dakota')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'47', N'TN', N'Tennessee', N'Tennessee')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'48', N'TX', N'Texas', N'Texas')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'49', N'UT', N'Utah', N'Utah')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'50', N'VT', N'Vermont', N'Vermont')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'51', N'VA', N'Virginia', N'Virginia')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'53', N'WA', N'Washington', N'Washington')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'54', N'WV', N'West Virginia', N'West Virginia')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'55', N'WI', N'Wisconsin', N'Wisconsin')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'56', N'WY', N'Wyoming', N'Wyoming')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'60', N'AS', N'American Samoa', N'American Samoa')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'64', N'FM', N'Federated States of Micronesia', N'Micronesia')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'66', N'GU', N'Guam', N'Guam')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'68', N'MH', N'Marshall Islands', N'Marshall Islands')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'69', N'MP', N'Northern Mariana Islands', N'Mariana Islands')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'70', N'PW', N'Palau', N'Palau')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'72', N'PR', N'Puerto Rico', N'Puerto Rico')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'74', N'UM', N'U.S. Minor Outlying Islands', N'Outlying Islands')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'78', N'VI', N'Virgin Islands of the U.S.', N'Virgin Islands')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'79', N'AB', N'Alberta', N'Alberta')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'80', N'BC', N'British Columbia', N'British Columbia')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'81', N'MB', N'Manitoba', N'Manitoba')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'82', N'NL', N'Newfoundland and Labrador', N'Newfoundland')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'83', N'NT', N'Northwest Territories', N'Northwest Territories')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'84', N'NS', N'Nunavut', N'Nunavut')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'85', N'ON', N'Ontario', N'Ontario')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'86', N'PE', N'Prince Edward Island', N'Prince Edward Island')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'87', N'QC', N'Quebec', N'Quebec')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'88', N'SK', N'Saskatchewan', N'Saskatchewan')
INSERT [dbo].[State] ([Code], [Abbreviation], [Name], [Display]) VALUES (N'89', N'YT', N'Yukon', N'Yukon')
go

if OBJECT_ID('Customer') is not null
	drop table Customer
go

create table Customer(
	[ID] int not null identity(1,1) primary key nonclustered,
	FirstName varchar(30) not null,
	MiddleInitial char(1) null,
	LastName varchar(30) not null
)
go

set identity_insert Customer on 
go

insert into Customer([ID], [FirstName], [LastName], [MiddleInitial])values(1, 'Chris', 'Carter', 'J')
insert into Customer([ID], [FirstName], [LastName], [MiddleInitial])values(2, 'Jimmy', 'Smith', null)
go

set identity_insert Customer off
go