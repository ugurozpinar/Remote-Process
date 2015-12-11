USE [UGUR]
GO
/****** Object:  Table [dbo].[Computers]    Script Date: 12/11/2015 22:48:56 ******/
DROP TABLE [dbo].[Computers]
GO
/****** Object:  Table [dbo].[task_user]    Script Date: 12/11/2015 22:48:56 ******/
DROP TABLE [dbo].[task_user]
GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 12/11/2015 22:48:56 ******/
ALTER TABLE [dbo].[Tasks] DROP CONSTRAINT [DF_Tasks_inserted_at]
GO
DROP TABLE [dbo].[Tasks]
GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 12/11/2015 22:48:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[process] [nvarchar](50) NULL,
	[param1] [nvarchar](50) NULL,
	[param2] [nvarchar](50) NULL,
	[delay] [int] NULL,
	[message] [nvarchar](255) NULL,
	[inserted_at] [datetime] NULL CONSTRAINT [DF_Tasks_inserted_at]  DEFAULT (getdate()),
	[keep] [tinyint] NULL,
 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[task_user]    Script Date: 12/11/2015 22:48:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[task_user](
	[cid] [int] NOT NULL,
	[tid] [int] NOT NULL,
	[result] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Computers]    Script Date: 12/11/2015 22:48:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Computers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[mac] [nvarchar](50) NULL,
	[ip] [nvarchar](50) NULL,
	[username] [nvarchar](50) NULL,
 CONSTRAINT [PK_Computers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
