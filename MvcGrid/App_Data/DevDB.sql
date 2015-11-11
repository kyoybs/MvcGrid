 

CREATE TABLE [dbo].[DevControlType](
	[ControlTypeId] [int] NOT NULL,
	[ControlTypeName] [varchar](100) NOT NULL,
	[Pattern] [varchar](500) NOT NULL,
	[Notes] [varchar](500) NULL,
 CONSTRAINT [PK_DevControlType] PRIMARY KEY CLUSTERED 
(
	[ControlTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

 
CREATE TABLE [dbo].[DevFieldInfo](
	[FieldId] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [varchar](100) NOT NULL,
	[FieldName] [varchar](100) NOT NULL,
	[FieldLabel] [varchar](500) NOT NULL,
	[EntityProperty] [varchar](100) NULL,
	[FieldIndex] [int] NOT NULL,
	[IsPK] [smallint] NOT NULL,
	[CanNull] [bit] NOT NULL,
	[DataType] [varchar](50) NOT NULL,
	[Length] [int] NOT NULL,
	[CategoryName] [varchar](200) NULL,
	[ControlTypeId] [int] NULL,
	[ControlIndex] [int] NULL,
	[Notes] [varchar](500) NULL,
 CONSTRAINT [PK_FieldInfo] PRIMARY KEY CLUSTERED 
(
	[FieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
 

ALTER TABLE [dbo].[DevFieldInfo] ADD  CONSTRAINT [DF_FieldInfo_CanNull]  DEFAULT ((0)) FOR [CanNull]
GO

ALTER TABLE [dbo].[DevFieldInfo] ADD  CONSTRAINT [DF_FieldInfo_DataType]  DEFAULT ('varchar') FOR [DataType]
GO

ALTER TABLE [dbo].[DevFieldInfo] ADD  CONSTRAINT [DF_FieldInfo_Length]  DEFAULT ((0)) FOR [Length]
GO

