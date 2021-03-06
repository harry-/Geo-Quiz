USE [master]
GO
/****** Object:  Database [geoquiz]    Script Date: 04.05.2018 10:30:49 ******/
CREATE DATABASE [geoquiz]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'geoquiz', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SERVERHARRY\MSSQL\DATA\geoquiz.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'geoquiz_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SERVERHARRY\MSSQL\DATA\geoquiz_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [geoquiz] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [geoquiz].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [geoquiz] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [geoquiz] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [geoquiz] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [geoquiz] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [geoquiz] SET ARITHABORT OFF 
GO
ALTER DATABASE [geoquiz] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [geoquiz] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [geoquiz] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [geoquiz] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [geoquiz] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [geoquiz] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [geoquiz] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [geoquiz] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [geoquiz] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [geoquiz] SET  DISABLE_BROKER 
GO
ALTER DATABASE [geoquiz] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [geoquiz] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [geoquiz] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [geoquiz] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [geoquiz] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [geoquiz] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [geoquiz] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [geoquiz] SET RECOVERY FULL 
GO
ALTER DATABASE [geoquiz] SET  MULTI_USER 
GO
ALTER DATABASE [geoquiz] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [geoquiz] SET DB_CHAINING OFF 
GO
ALTER DATABASE [geoquiz] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [geoquiz] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [geoquiz] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'geoquiz', N'ON'
GO
ALTER DATABASE [geoquiz] SET QUERY_STORE = OFF
GO
USE [geoquiz]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [geoquiz]
GO
/****** Object:  ApplicationRole [geogame]    Script Date: 04.05.2018 10:30:49 ******/
/* To avoid disclosure of passwords, the password is generated in script. */
declare @idx as int
declare @randomPwd as nvarchar(64)
declare @rnd as float
select @idx = 0
select @randomPwd = N''
select @rnd = rand((@@CPU_BUSY % 100) + ((@@IDLE % 100) * 100) + 
       (DATEPART(ss, GETDATE()) * 10000) + ((cast(DATEPART(ms, GETDATE()) as int) % 100) * 1000000))
while @idx < 64
begin
   select @randomPwd = @randomPwd + char((cast((@rnd * 83) as int) + 43))
   select @idx = @idx + 1
select @rnd = rand()
end
declare @statement nvarchar(4000)
select @statement = N'CREATE APPLICATION ROLE [geogame] WITH DEFAULT_SCHEMA = [dbo], ' + N'PASSWORD = N' + QUOTENAME(@randomPwd,'''')
EXEC dbo.sp_executesql @statement

GO
/****** Object:  User [geoquiz]    Script Date: 04.05.2018 10:30:49 ******/
CREATE USER [geoquiz] FOR LOGIN [geoquiz] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [geoquiz]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [geoquiz]
GO
/****** Object:  Table [dbo].[scoresheet]    Script Date: 04.05.2018 10:30:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scoresheet](
	[game] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](50) NULL,
	[questions] [int] NULL,
	[correct] [int] NULL,
 CONSTRAINT [PK_scoresheet] PRIMARY KEY CLUSTERED 
(
	[game] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[usersheet]    Script Date: 04.05.2018 10:30:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[usersheet](
	[username] [nvarchar](50) NOT NULL,
	[password] [nvarchar](200) NULL,
 CONSTRAINT [PK_usersheet] PRIMARY KEY CLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[scoresheet] ON 

INSERT [dbo].[scoresheet] ([game], [username], [questions], [correct]) VALUES (4, N'Harry', 3, 2)
INSERT [dbo].[scoresheet] ([game], [username], [questions], [correct]) VALUES (12, N'Harry', 0, 0)
INSERT [dbo].[scoresheet] ([game], [username], [questions], [correct]) VALUES (13, N'Harry', 1, 0)
INSERT [dbo].[scoresheet] ([game], [username], [questions], [correct]) VALUES (14, N'Wolfgang', 5, 4)
INSERT [dbo].[scoresheet] ([game], [username], [questions], [correct]) VALUES (18, N'Fabian', 25, 17)
INSERT [dbo].[scoresheet] ([game], [username], [questions], [correct]) VALUES (19, N'Fabian', 16, 8)
INSERT [dbo].[scoresheet] ([game], [username], [questions], [correct]) VALUES (21, N'Wolfgang', 10, 10)
INSERT [dbo].[scoresheet] ([game], [username], [questions], [correct]) VALUES (30, N'Wolfgang', 0, 0)
INSERT [dbo].[scoresheet] ([game], [username], [questions], [correct]) VALUES (31, N'Fabian', 0, 0)
INSERT [dbo].[scoresheet] ([game], [username], [questions], [correct]) VALUES (32, N'Harry', 0, 0)
INSERT [dbo].[scoresheet] ([game], [username], [questions], [correct]) VALUES (33, N'Harry', 1, 1)
INSERT [dbo].[scoresheet] ([game], [username], [questions], [correct]) VALUES (34, N'Harry', 0, 0)
INSERT [dbo].[scoresheet] ([game], [username], [questions], [correct]) VALUES (35, N'Harry', 0, 0)
INSERT [dbo].[scoresheet] ([game], [username], [questions], [correct]) VALUES (36, N'Harry', 1, 0)
SET IDENTITY_INSERT [dbo].[scoresheet] OFF
INSERT [dbo].[usersheet] ([username], [password]) VALUES (N'Fabian', N'Rfc2898DeriveBytes$100000$tBLn+VD1fP1gL5thAlkn/g==$GtfxAdTF+hliOvfryuQKHiYFSza5aJMec0ejImz9ylc=')
INSERT [dbo].[usersheet] ([username], [password]) VALUES (N'Harry', N'Rfc2898DeriveBytes$100000$yV0CdqmTg+7daD/FrYYJpw==$UVm2zscJNLvuIiVhoS5yv7JkGhM+UOJp7Jft3fPev54=')
INSERT [dbo].[usersheet] ([username], [password]) VALUES (N'Wolfgang', N'Rfc2898DeriveBytes$100000$KyQdzfstQsgLVixw6z633g==$IubFxo4ZodlltDczSKgXlKf4h6Fh8Wi3JIvr/86E9Q8=')
ALTER TABLE [dbo].[scoresheet]  WITH CHECK ADD  CONSTRAINT [FK_scoresheet_usersheet] FOREIGN KEY([username])
REFERENCES [dbo].[usersheet] ([username])
GO
ALTER TABLE [dbo].[scoresheet] CHECK CONSTRAINT [FK_scoresheet_usersheet]
GO
/****** Object:  StoredProcedure [dbo].[ADD_GAME]    Script Date: 04.05.2018 10:30:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ADD_GAME]
	-- Add the parameters for the stored procedure here
	@USERNAME nvarchar(50),
	@QUESTIONS int,
	@CORRECT int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into scoresheet (username, questions, correct) values (@USERNAME, @QUESTIONS, @CORRECT)
END

GO
/****** Object:  StoredProcedure [dbo].[CREATE_USER]    Script Date: 04.05.2018 10:30:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CREATE_USER]
	-- Add the parameters for the stored procedure here
	@USERNAME nvarchar(50),
	@PASSWORD nvarchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO usersheet (username, password) VALUES (@USERNAME, @PASSWORD)
END

GO
/****** Object:  StoredProcedure [dbo].[SEARCH_USER]    Script Date: 04.05.2018 10:30:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SEARCH_USER]
	-- Add the parameters for the stored procedure here
	@USERNAME nvarchar(50)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from usersheet where username = @USERNAME
END

GO
/****** Object:  StoredProcedure [dbo].[TOTAL_SCORE]    Script Date: 04.05.2018 10:30:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TOTAL_SCORE]
	-- Add the parameters for the stored procedure here
	@USERNAME nvarchar(50) 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select sum(questions), sum(correct) from scoresheet where username = @USERNAME
END

GO
USE [master]
GO
ALTER DATABASE [geoquiz] SET  READ_WRITE 
GO
