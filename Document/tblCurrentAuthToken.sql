USE [SMSCreator]
GO

/****** Object:  Table [dbo].[tblCurrentAuthtoken]    Script Date: 09-06-2021 07:30:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblCurrentAuthtoken](
	[CurrentAuthtokenId] [int] IDENTITY(1,1) NOT NULL,
	[CurrentAuthTokenName] [nvarchar](400) NULL,
	[CurrentAuthToken] [nvarchar](400) NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CurrentAuthtokenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]
GO


