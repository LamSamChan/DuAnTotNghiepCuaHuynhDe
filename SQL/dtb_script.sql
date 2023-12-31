/*Create Database [HUYNHDE_DATN_2023]
go

use HUYNHDE_DATN_2023
GO*/

/****** Object:  Table [dbo].[ContractCoordinate]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractCoordinate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FieldName] [nvarchar](50) NOT NULL,
	[X] [real] NOT NULL,
	[Y] [real] NOT NULL,
	[SignaturePage] [int] NOT NULL,
	[TContractID] [int] NOT NULL,
 CONSTRAINT [PK_ContractCoordinate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [uniqueidentifier] NOT NULL,
	[BuisinessName] [nvarchar](100) NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Position] [nvarchar](100) NULL,
	[DateOfBirth] [datetime2](7) NOT NULL,
	[Gender] [int] NOT NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Identification] [varchar](20) NOT NULL,
	[IssuedDate] [datetime2](7) NOT NULL,
	[IssuedPlace] [nvarchar](50) NOT NULL,
	[PowerOfAttorneyNum] [nvarchar](30) NULL,
	[DatePOA] [datetime2](7) NULL,
	[WhoPOA] [nvarchar](50) NULL,
	[BuisinessNumber] [nvarchar](50) NULL,
	[BNDate] [datetime2](7) NULL,
	[BNPlace] [nvarchar](50) NULL,
	[Nationality] [nvarchar](50) NOT NULL,
	[BankAccount] [varchar](50) NULL,
	[BankName] [varchar](50) NULL,
	[TaxIDNumber] [varchar](50) NULL,
	[Address] [nvarchar](255) NOT NULL,
	[FAX] [varchar](50) NULL,
	[ChargeNoticeAddress] [nvarchar](255) NOT NULL,
	[BillingAddress] [nvarchar](255) NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[Note] [nvarchar](255) NULL,
	[SerialPFX] [nvarchar](450) NULL,
	[typeofCustomer] [nvarchar](max) NOT NULL,
	[DateAdded] [datetime2](7) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DoneContract]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DoneContract](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateDone] [datetime2](7) NOT NULL,
	[DConTractName] [nvarchar](100) NOT NULL,
	[DContractFile] [nvarchar](max) NULL,
	[IsInEffect] [bit] NOT NULL,
	[InstallationAddress] [nvarchar](max) NOT NULL,
	[EmployeeCreatedId] [uniqueidentifier] NULL,
	[DirectorSignedId] [uniqueidentifier] NULL,
	[CustomerId] [uniqueidentifier] NULL,
	[TOS_ID] [int] NOT NULL,
	[DoneMinuteId] [int] NULL,
	[Base64File] [nvarchar](max) NULL,
	[DateUnEffect] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_DoneContract] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DoneMinute]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DoneMinute](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateDone] [datetime2](7) NOT NULL,
	[MinuteName] [nvarchar](100) NOT NULL,
	[MinuteFile] [nvarchar](250) NULL,
	[EmployeeId] [uniqueidentifier] NOT NULL,
	[Base64File] [nvarchar](max) NULL,
 CONSTRAINT [PK_DoneMinute] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[Id] [uniqueidentifier] NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[DateOfBirth] [datetime2](7) NOT NULL,
	[Gender] [int] NOT NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL,
	[Identification] [varchar](20) NOT NULL,
	[Address] [nvarchar](255) NOT NULL,
	[Image] [nvarchar](500) NULL,
	[Password] [varchar](50) NULL,
	[IsLocked] [bit] NOT NULL,
	[Note] [nvarchar](255) NULL,
	[IsFirstLogin] [bit] NOT NULL,
	[SerialPFX] [nvarchar](450) NULL,
	[RoleID] [int] NOT NULL,
	[PositionID] [int] NOT NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InstallationDevice]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InstallationDevice](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeviceName] [nvarchar](100) NOT NULL,
	[DeviceStatus] [nvarchar](50) NOT NULL,
	[DeviceQuantity] [nvarchar](50) NOT NULL,
	[TOS_ID] [int] NOT NULL,
 CONSTRAINT [PK_InstallationDevice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InstallationRequirement]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InstallationRequirement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateCreated] [datetime2](7) NOT NULL,
	[MinuteName] [nvarchar](100) NOT NULL,
	[DoneContractId] [int] NOT NULL,
	[InstallationAddress] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_InstallationRequirement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MinuteCoordinate]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MinuteCoordinate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FieldName] [nvarchar](50) NOT NULL,
	[X] [real] NOT NULL,
	[Y] [real] NOT NULL,
	[SignaturePage] [int] NOT NULL,
	[TMinuteID] [int] NOT NULL,
 CONSTRAINT [PK_MinuteCoordinate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OperationHistoryCus]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OperationHistoryCus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[OperationName] [nvarchar](255) NOT NULL,
	[CustomerID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_OperationHistoryCus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OperationHistoryEmp]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OperationHistoryEmp](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[OperationName] [nvarchar](255) NOT NULL,
	[EmployeeID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_OperationHistoryEmp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PendingContract]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PendingContract](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateCreated] [datetime2](7) NOT NULL,
	[PContractName] [nvarchar](100) NOT NULL,
	[PContractFile] [nvarchar](250) NOT NULL,
	[IsDirector] [bit] NOT NULL,
	[IsCustomer] [bit] NOT NULL,
	[IsRefuse] [bit] NOT NULL,
	[Reason] [nvarchar](max) NULL,
	[InstallationAddress] [nvarchar](max) NOT NULL,
	[EmployeeCreatedId] [uniqueidentifier] NOT NULL,
	[DirectorSignedId] [uniqueidentifier] NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[TOS_ID] [int] NOT NULL,
	[Base64File] [nvarchar](max) NULL,
 CONSTRAINT [PK_PendingContract] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PendingMinute]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PendingMinute](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateCreated] [datetime2](7) NOT NULL,
	[MinuteName] [nvarchar](100) NOT NULL,
	[IsIntallation] [bit] NOT NULL,
	[IsCustomer] [bit] NOT NULL,
	[MinuteFile] [nvarchar](250) NOT NULL,
	[EmployeeId] [uniqueidentifier] NOT NULL,
	[DoneContractId] [int] NOT NULL,
	[Base64File] [nvarchar](max) NULL,
	[InstallationAddress] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_PendingMinute] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PFXCertificate]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PFXCertificate](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[FilePath] [nvarchar](max) NULL,
	[Issuer] [nvarchar](max) NULL,
	[Subject] [nvarchar](max) NULL,
	[ValidFrom] [datetime2](7) NOT NULL,
	[ValidUntil] [datetime2](7) NOT NULL,
	[IsEmployee] [bit] NOT NULL,
	[ImageSignature1] [nvarchar](500) NULL,
	[ImageSignature2] [nvarchar](500) NULL,
	[ImageSignature3] [nvarchar](500) NULL,
	[ImageSignature4] [nvarchar](500) NULL,
	[ImageSignature5] [nvarchar](500) NULL,
	[DefaultImageSignature] [nvarchar](500) NULL,
 CONSTRAINT [PK_PFXCertificate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Position]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Position](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PositionName] [nvarchar](50) NOT NULL,
	[isHidden] [bit] NOT NULL,
 CONSTRAINT [PK_Position] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[isHidden] [bit] NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stamp]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stamp](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateUpdated] [datetime2](7) NOT NULL,
	[StampPath] [nvarchar](250) NULL,
 CONSTRAINT [PK_Stamp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TemplateContract]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TemplateContract](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateAdded] [datetime2](7) NOT NULL,
	[TContractName] [nvarchar](100) NOT NULL,
	[TContractFile] [nvarchar](250) NULL,
	[jsonDirectorZone] [nvarchar](50) NULL,
	[jsonCustomerZone] [nvarchar](50) NULL,
	[Base64File] [nvarchar](max) NULL,
 CONSTRAINT [PK_TemplateContract] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TemplateMinute]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TemplateMinute](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateAdded] [datetime2](7) NOT NULL,
	[TMinuteName] [nvarchar](100) NOT NULL,
	[TMinuteFile] [nvarchar](250) NOT NULL,
	[jsonIntallationZone] [nvarchar](50) NULL,
	[jsonCustomerZone] [nvarchar](50) NULL,
	[Base64File] [nvarchar](max) NULL,
 CONSTRAINT [PK_TemplateMinute] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypeOfService]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeOfService](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateAdded] [datetime2](7) NULL,
	[DateUpdated] [datetime2](7) NULL,
	[ServiceName] [nvarchar](100) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[PerTime] [nvarchar](100) NOT NULL,
	[isHidden] [bit] NOT NULL,
	[templateContractID] [int] NOT NULL,
	[templateMinuteID] [int] NOT NULL,
 CONSTRAINT [PK_TypeOfService] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Customer] ADD  CONSTRAINT [DF__Customer__typeof__70DDC3D8]  DEFAULT (N'') FOR [typeofCustomer]
GO
ALTER TABLE [dbo].[DoneContract] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [DateUnEffect]
GO
ALTER TABLE [dbo].[InstallationRequirement] ADD  DEFAULT (N'') FOR [InstallationAddress]
GO
ALTER TABLE [dbo].[PendingMinute] ADD  DEFAULT (N'') FOR [InstallationAddress]
GO
ALTER TABLE [dbo].[ContractCoordinate]  WITH CHECK ADD  CONSTRAINT [FK_ContractCoordinate_TemplateContract_TContractID] FOREIGN KEY([TContractID])
REFERENCES [dbo].[TemplateContract] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ContractCoordinate] CHECK CONSTRAINT [FK_ContractCoordinate_TemplateContract_TContractID]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_PFXCertificate_SerialPFX] FOREIGN KEY([SerialPFX])
REFERENCES [dbo].[PFXCertificate] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_PFXCertificate_SerialPFX]
GO
ALTER TABLE [dbo].[DoneContract]  WITH CHECK ADD  CONSTRAINT [FK_DoneContract_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[DoneContract] CHECK CONSTRAINT [FK_DoneContract_Customer_CustomerId]
GO
ALTER TABLE [dbo].[DoneContract]  WITH CHECK ADD  CONSTRAINT [FK_DoneContract_DoneMinute_DoneMinuteId] FOREIGN KEY([DoneMinuteId])
REFERENCES [dbo].[DoneMinute] ([Id])
GO
ALTER TABLE [dbo].[DoneContract] CHECK CONSTRAINT [FK_DoneContract_DoneMinute_DoneMinuteId]
GO
ALTER TABLE [dbo].[DoneContract]  WITH CHECK ADD  CONSTRAINT [FK_DoneContract_Employee_EmployeeCreatedId] FOREIGN KEY([EmployeeCreatedId])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[DoneContract] CHECK CONSTRAINT [FK_DoneContract_Employee_EmployeeCreatedId]
GO
ALTER TABLE [dbo].[DoneContract]  WITH CHECK ADD  CONSTRAINT [FK_DoneContract_TypeOfService_TOS_ID] FOREIGN KEY([TOS_ID])
REFERENCES [dbo].[TypeOfService] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DoneContract] CHECK CONSTRAINT [FK_DoneContract_TypeOfService_TOS_ID]
GO
ALTER TABLE [dbo].[DoneMinute]  WITH CHECK ADD  CONSTRAINT [FK_DoneMinute_Employee_EmployeeId] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DoneMinute] CHECK CONSTRAINT [FK_DoneMinute_Employee_EmployeeId]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_PFXCertificate_SerialPFX] FOREIGN KEY([SerialPFX])
REFERENCES [dbo].[PFXCertificate] ([Id])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_PFXCertificate_SerialPFX]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Position_PositionID] FOREIGN KEY([PositionID])
REFERENCES [dbo].[Position] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Position_PositionID]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Role_RoleID] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Role_RoleID]
GO
ALTER TABLE [dbo].[InstallationDevice]  WITH CHECK ADD  CONSTRAINT [FK_InstallationDevice_TypeOfService_TOS_ID] FOREIGN KEY([TOS_ID])
REFERENCES [dbo].[TypeOfService] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InstallationDevice] CHECK CONSTRAINT [FK_InstallationDevice_TypeOfService_TOS_ID]
GO
ALTER TABLE [dbo].[InstallationRequirement]  WITH CHECK ADD  CONSTRAINT [FK_InstallationRequirement_DoneContract_DoneContractId] FOREIGN KEY([DoneContractId])
REFERENCES [dbo].[DoneContract] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InstallationRequirement] CHECK CONSTRAINT [FK_InstallationRequirement_DoneContract_DoneContractId]
GO
ALTER TABLE [dbo].[MinuteCoordinate]  WITH CHECK ADD  CONSTRAINT [FK_MinuteCoordinate_TemplateMinute_TMinuteID] FOREIGN KEY([TMinuteID])
REFERENCES [dbo].[TemplateMinute] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MinuteCoordinate] CHECK CONSTRAINT [FK_MinuteCoordinate_TemplateMinute_TMinuteID]
GO
ALTER TABLE [dbo].[OperationHistoryCus]  WITH CHECK ADD  CONSTRAINT [FK_OperationHistoryCus_Customer_CustomerID] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[OperationHistoryCus] CHECK CONSTRAINT [FK_OperationHistoryCus_Customer_CustomerID]
GO
ALTER TABLE [dbo].[OperationHistoryEmp]  WITH CHECK ADD  CONSTRAINT [FK_OperationHistoryEmp_Employee_EmployeeID] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[OperationHistoryEmp] CHECK CONSTRAINT [FK_OperationHistoryEmp_Employee_EmployeeID]
GO
ALTER TABLE [dbo].[PendingContract]  WITH CHECK ADD  CONSTRAINT [FK_PendingContract_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PendingContract] CHECK CONSTRAINT [FK_PendingContract_Customer_CustomerId]
GO
ALTER TABLE [dbo].[PendingContract]  WITH CHECK ADD  CONSTRAINT [FK_PendingContract_Employee_EmployeeCreatedId] FOREIGN KEY([EmployeeCreatedId])
REFERENCES [dbo].[Employee] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PendingContract] CHECK CONSTRAINT [FK_PendingContract_Employee_EmployeeCreatedId]
GO
ALTER TABLE [dbo].[PendingContract]  WITH CHECK ADD  CONSTRAINT [FK_PendingContract_TypeOfService_TOS_ID] FOREIGN KEY([TOS_ID])
REFERENCES [dbo].[TypeOfService] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PendingContract] CHECK CONSTRAINT [FK_PendingContract_TypeOfService_TOS_ID]
GO
ALTER TABLE [dbo].[PendingMinute]  WITH CHECK ADD  CONSTRAINT [FK_PendingMinute_DoneContract_DoneContractId] FOREIGN KEY([DoneContractId])
REFERENCES [dbo].[DoneContract] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PendingMinute] CHECK CONSTRAINT [FK_PendingMinute_DoneContract_DoneContractId]
GO
ALTER TABLE [dbo].[PendingMinute]  WITH CHECK ADD  CONSTRAINT [FK_PendingMinute_Employee_EmployeeId] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[PendingMinute] CHECK CONSTRAINT [FK_PendingMinute_Employee_EmployeeId]
GO
ALTER TABLE [dbo].[TypeOfService]  WITH CHECK ADD  CONSTRAINT [FK_TypeOfService_TemplateContract_templateContractID] FOREIGN KEY([templateContractID])
REFERENCES [dbo].[TemplateContract] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TypeOfService] CHECK CONSTRAINT [FK_TypeOfService_TemplateContract_templateContractID]
GO
ALTER TABLE [dbo].[TypeOfService]  WITH CHECK ADD  CONSTRAINT [FK_TypeOfService_TemplateMinute_templateMinuteID] FOREIGN KEY([templateMinuteID])
REFERENCES [dbo].[TemplateMinute] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TypeOfService] CHECK CONSTRAINT [FK_TypeOfService_TemplateMinute_templateMinuteID]
GO
/****** Object:  StoredProcedure [dbo].[CountCustomersByTypeAndDate]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CountCustomersByTypeAndDate]
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        MIN(FORMAT(DateAdded, 'dd/MM/yyyy')) AS [Date],
        COUNT(Id) AS [TotalCustomers],
        SUM(CASE WHEN typeofCustomer = N'Doanh nghiệp' THEN 1 ELSE 0 END) AS [EnterpriseCustomers],
        SUM(CASE WHEN typeofCustomer = N'Cá nhân' THEN 1 ELSE 0 END) AS [IndividualCustomers]
    FROM
        Customer
    WHERE
        CAST(DateAdded AS DATE) BETWEEN @StartDate AND @EndDate
    GROUP BY
         FORMAT(DateAdded, 'dd/MM/yyyy')
	ORDER BY
        MIN(DateAdded); -- Sắp xếp theo ngày tăng dần
END;
GO
/****** Object:  StoredProcedure [dbo].[CountCustomersByWeekAndMonth]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CountCustomersByWeekAndMonth]
    @MonthYear NVARCHAR(7) -- Ví dụ: '2023-09'
AS
BEGIN
    SELECT
        ROW_NUMBER() OVER (ORDER BY DATEPART(ISO_WEEK, DateAdded)) AS [Date], -- Sử dụng ROW_NUMBER()
        COUNT(Id) AS [TotalCustomers],
        SUM(CASE WHEN typeofCustomer = N'Doanh nghiệp' THEN 1 ELSE 0 END) AS [EnterpriseCustomers],
        SUM(CASE WHEN typeofCustomer = N'Cá nhân' THEN 1 ELSE 0 END) AS [IndividualCustomers]
    FROM
        Customer
    WHERE
        FORMAT(DateAdded, 'yyyy-MM') = @MonthYear
    GROUP BY
        DATEPART(ISO_WEEK, DateAdded)
    ORDER BY
        [Date]; -- Sắp xếp theo số thứ tự của tuần
END;
GO
/****** Object:  StoredProcedure [dbo].[CountDContractMonthsInLastSixMonths]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CountDContractMonthsInLastSixMonths]
    @ReferenceMonth DATE -- Thay đổi kiểu dữ liệu của tham số này
AS
BEGIN
    SELECT
        FORMAT(DateDone, 'yyyy-MM') AS [Date],
        COUNT(Id) AS [Totals]
    FROM
        DoneContract
    WHERE
        DateDone BETWEEN DATEADD(MONTH, -5, @ReferenceMonth) AND DATEADD(DAY, -1, DATEADD(MONTH, 1, @ReferenceMonth))
    GROUP BY
        FORMAT(DateDone, 'yyyy-MM')
    ORDER BY
        FORMAT(DateDone, 'yyyy-MM'); -- Sắp xếp theo tháng tăng dần
END;
GO
/****** Object:  StoredProcedure [dbo].[CountDoneContractByWeekAndMonth]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CountDoneContractByWeekAndMonth]
    @MonthYear NVARCHAR(7) -- Ví dụ: '2023-09'
AS
BEGIN
    SELECT
        ROW_NUMBER() OVER (ORDER BY DATEPART(ISO_WEEK, DateDone)) AS [Date], -- Sử dụng ROW_NUMBER()
        COUNT(Id) AS [Totals]
    FROM
        DoneContract
    WHERE
        FORMAT(DateDone, 'yyyy-MM') = @MonthYear
    GROUP BY
        DATEPART(ISO_WEEK, DateDone)
    ORDER BY
        [Date]; -- Sắp xếp theo số thứ tự của tuần
END;
GO
/****** Object:  StoredProcedure [dbo].[CountMonthsInLastSixMonths]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CountMonthsInLastSixMonths]
    @ReferenceMonth DATE -- Thay đổi kiểu dữ liệu của tham số này
AS
BEGIN
    SELECT
        FORMAT(DateAdded, 'yyyy-MM') AS [Date],
        COUNT(Id) AS [TotalCustomers],
        SUM(CASE WHEN typeofCustomer = N'Doanh nghiệp' THEN 1 ELSE 0 END) AS [EnterpriseCustomers],
        SUM(CASE WHEN typeofCustomer = N'Cá nhân' THEN 1 ELSE 0 END) AS [IndividualCustomers]
    FROM
        Customer
    WHERE
        DateAdded BETWEEN DATEADD(MONTH, -5, @ReferenceMonth) AND DATEADD(DAY, -1, DATEADD(MONTH, 1, @ReferenceMonth))
    GROUP BY
        FORMAT(DateAdded, 'yyyy-MM')
    ORDER BY
        FORMAT(DateAdded, 'yyyy-MM'); -- Sắp xếp theo tháng tăng dần
END;
GO
/****** Object:  StoredProcedure [dbo].[CountPContractMonthsInLastSixMonths]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CountPContractMonthsInLastSixMonths]
    @ReferenceMonth DATE -- Thay đổi kiểu dữ liệu của tham số này
AS
BEGIN
    SELECT
        FORMAT(DateCreated, 'yyyy-MM') AS [Date],
        COUNT(Id) AS [Totals]
    FROM
        PendingContract
    WHERE
        DateCreated BETWEEN DATEADD(MONTH, -5, @ReferenceMonth) AND DATEADD(DAY, -1, DATEADD(MONTH, 1, @ReferenceMonth))
    GROUP BY
        FORMAT(DateCreated, 'yyyy-MM')
    ORDER BY
        FORMAT(DateCreated, 'yyyy-MM'); -- Sắp xếp theo tháng tăng dần
END;
GO
/****** Object:  StoredProcedure [dbo].[CountPContractWaitCusMonthsInLastSixMonths]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CountPContractWaitCusMonthsInLastSixMonths]
    @ReferenceMonth DATE -- Thay đổi kiểu dữ liệu của tham số này
AS
BEGIN
    SELECT
        FORMAT(DateCreated, 'yyyy-MM') AS [Date],
        COUNT(Id) AS [Totals]
    FROM
        PendingContract
    WHERE
        (DateCreated BETWEEN DATEADD(MONTH, -5, @ReferenceMonth) AND DATEADD(DAY, -1, DATEADD(MONTH, 1, @ReferenceMonth))) AND
        (IsCustomer = 0) AND
        (IsRefuse <> 1 OR IsRefuse IS NULL) 
    GROUP BY
        FORMAT(DateCreated, 'yyyy-MM')
    ORDER BY
        FORMAT(DateCreated, 'yyyy-MM'); -- Sắp xếp theo tháng tăng dần
END;
GO
/****** Object:  StoredProcedure [dbo].[CountPendingContractByWeekAndMonth]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CountPendingContractByWeekAndMonth]
    @MonthYear NVARCHAR(7) -- Ví dụ: '2023-09'
AS
BEGIN
    SELECT
        ROW_NUMBER() OVER (ORDER BY DATEPART(ISO_WEEK, DateCreated)) AS [Date], -- Sử dụng ROW_NUMBER()
        COUNT(Id) AS [Totals]
    FROM
        PendingContract
    WHERE
        FORMAT(DateCreated, 'yyyy-MM') = @MonthYear
    GROUP BY
        DATEPART(ISO_WEEK, DateCreated)
    ORDER BY
        [Date]; -- Sắp xếp theo số thứ tự của tuần
END;
GO
/****** Object:  StoredProcedure [dbo].[CountPendingContractWaitCusByWeekAndMonth]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CountPendingContractWaitCusByWeekAndMonth]
    @MonthYear NVARCHAR(7) -- Ví dụ: '2023-09'
AS
BEGIN
    SELECT
        ROW_NUMBER() OVER (ORDER BY DATEPART(ISO_WEEK, DateCreated)) AS [Date], -- Sử dụng ROW_NUMBER()
        COUNT(Id) AS [Totals]
    FROM
        PendingContract
    WHERE
        (FORMAT(DateCreated, 'yyyy-MM') = @MonthYear) AND
        (IsCustomer = 0) AND
        (IsRefuse <> 1 OR IsRefuse IS NULL) 
    GROUP BY
        DATEPART(ISO_WEEK, DateCreated)
    ORDER BY
        [Date]; -- Sắp xếp theo số thứ tự của tuần
END;
GO
/****** Object:  StoredProcedure [dbo].[CountUnEffectContractByWeekAndMonth]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CountUnEffectContractByWeekAndMonth]
    @MonthYear NVARCHAR(7) -- Ví dụ: '2023-09'
AS
BEGIN
    SELECT
        ROW_NUMBER() OVER (ORDER BY DATEPART(ISO_WEEK, DateUnEffect)) AS [Date], -- Sử dụng ROW_NUMBER()
        COUNT(Id) AS [Totals]
    FROM
        DoneContract
    WHERE
        FORMAT(DateUnEffect, 'yyyy-MM') = @MonthYear
    GROUP BY
        DATEPART(ISO_WEEK, DateUnEffect)
    ORDER BY
        [Date]; -- Sắp xếp theo số thứ tự của tuần
END;
GO
/****** Object:  StoredProcedure [dbo].[CountUnEffectContractMonthsInLastSixMonths]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CountUnEffectContractMonthsInLastSixMonths]
    @ReferenceMonth DATE -- Thay đổi kiểu dữ liệu của tham số này
AS
BEGIN
    SELECT
        FORMAT(DateUnEffect, 'yyyy-MM') AS [Date],
        COUNT(Id) AS [Totals]
    FROM
        DoneContract
    WHERE
        DateDone BETWEEN DATEADD(MONTH, -5, @ReferenceMonth) AND DATEADD(DAY, -1, DATEADD(MONTH, 1, @ReferenceMonth))
    GROUP BY
        FORMAT(DateUnEffect, 'yyyy-MM')
    ORDER BY
        FORMAT(DateUnEffect, 'yyyy-MM'); -- Sắp xếp theo tháng tăng dần
END;
GO
/****** Object:  StoredProcedure [dbo].[SumDoneContractByDate]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SumDoneContractByDate]
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        MIN(FORMAT(DateDone, 'dd/MM/yyyy')) AS [Date],
        COUNT(Id) AS [Totals]
    FROM
        DoneContract
    WHERE
        DateDone BETWEEN @StartDate AND @EndDate
    GROUP BY
        FORMAT(DateDone, 'dd/MM/yyyy')
	ORDER BY
        MIN(DateDone);
END;
GO
/****** Object:  StoredProcedure [dbo].[SumPendingContractByDate]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SumPendingContractByDate]
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        MIN(FORMAT(DateCreated, 'dd/MM/yyyy')) AS [Date],
        COUNT(Id) AS [Totals]
    FROM
        PendingContract
    WHERE
        DateCreated BETWEEN @StartDate AND @EndDate
    GROUP BY
        FORMAT(DateCreated, 'dd/MM/yyyy')
	ORDER BY
        MIN(DateCreated);
END;
GO
/****** Object:  StoredProcedure [dbo].[SumPendingContractWaitCusByDate]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SumPendingContractWaitCusByDate]
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        MIN(FORMAT(DateCreated, 'dd/MM/yyyy')) AS [Date],
        COUNT(Id) AS [Totals]
    FROM
        PendingContract
    WHERE
        (DateCreated BETWEEN @StartDate AND @EndDate) AND
        (IsCustomer = 0) AND
        (IsRefuse <> 1 OR IsRefuse IS NULL) 
    GROUP BY
        FORMAT(DateCreated, 'dd/MM/yyyy')
	ORDER BY
        MIN(DateCreated);
END;
GO
/****** Object:  StoredProcedure [dbo].[SumUnEffectContractByDate]    Script Date: 12/16/2023 9:58:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SumUnEffectContractByDate]
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        MIN(FORMAT(DateUnEffect, 'dd/MM/yyyy')) AS [Date],
        COUNT(Id) AS [Totals]
    FROM
        DoneContract
    WHERE
        DateDone BETWEEN @StartDate AND @EndDate
    GROUP BY
        FORMAT(DateUnEffect, 'dd/MM/yyyy')
	ORDER BY
        MIN(DateUnEffect);
END;
GO
