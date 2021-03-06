/****** Object:  Table [dbo].[ActorMaster]    Script Date: 6/30/2018 5:12:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ActorMaster](
	[ActorId] [bigint] IDENTITY(1,1) NOT NULL,
	[ActorName] [varchar](100) NULL,
	[Sex] [char](1) NULL,
	[DOB] [datetime] NULL,
	[Bio] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_ActorMaster] PRIMARY KEY CLUSTERED 
(
	[ActorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ActorMovieMap]    Script Date: 6/30/2018 5:12:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActorMovieMap](
	[ActorMovieMapId] [bigint] IDENTITY(1,1) NOT NULL,
	[ActorId] [bigint] NULL,
	[MovieId] [bigint] NULL,
 CONSTRAINT [PK_ActorMovieMap] PRIMARY KEY CLUSTERED 
(
	[ActorMovieMapId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ErrorLog]    Script Date: 6/30/2018 5:12:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ErrorLog](
	[ErrorId] [bigint] IDENTITY(1,1) NOT NULL,
	[ErrorMsg] [varchar](1500) NULL,
	[CreadtedOn] [datetime] NULL,
 CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED 
(
	[ErrorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MovieMaster]    Script Date: 6/30/2018 5:12:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MovieMaster](
	[MovieId] [bigint] IDENTITY(1,1) NOT NULL,
	[MovieName] [varchar](75) NULL,
	[ReleaseYear] [datetime] NULL,
	[Plot] [nvarchar](max) NULL,
	[Poster] [varchar](100) NULL,
	[ProducerId] [bigint] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_MovieMaster] PRIMARY KEY CLUSTERED 
(
	[MovieId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProducerMaster]    Script Date: 6/30/2018 5:12:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProducerMaster](
	[ProducerId] [bigint] IDENTITY(1,1) NOT NULL,
	[ProducerName] [varchar](100) NULL,
	[Sex] [char](1) NULL,
	[DOB] [datetime] NULL,
	[Bio] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_ProducerMaster] PRIMARY KEY CLUSTERED 
(
	[ProducerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[ActorMaster] ADD  CONSTRAINT [DF_ActorMaster_Sex]  DEFAULT ('M') FOR [Sex]
GO
ALTER TABLE [dbo].[ActorMaster] ADD  CONSTRAINT [DF_ActorMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_CreadtedOn]  DEFAULT (getdate()) FOR [CreadtedOn]
GO
ALTER TABLE [dbo].[MovieMaster] ADD  CONSTRAINT [DF_MovieMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[ProducerMaster] ADD  CONSTRAINT [DF_ProducerMaster_Sex]  DEFAULT ('M') FOR [Sex]
GO
ALTER TABLE [dbo].[ProducerMaster] ADD  CONSTRAINT [DF_ProducerMaster_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'(''M = Male, F = Female'')' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActorMaster', @level2type=N'COLUMN',@level2name=N'Sex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M = Male, F = Female' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProducerMaster', @level2type=N'COLUMN',@level2name=N'Sex'
GO

GO
/****** Object:  StoredProcedure [dbo].[USP_AddActor]    Script Date: 6/30/2018 5:12:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_AddActor]
@pActorName VARCHAR(100) = NULL,
@pSEX CHAR(1) = NULL,
@pDOB DATETIME = NULL,
@pBio NVARCHAR(MAX) = NULL

AS
BEGIN


	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM ActorMaster WHERE ActorName = @pActorName)
			BEGIN
				BEGIN TRAN
				INSERT INTO ActorMaster(ActorName, Sex, DOB, Bio)
				VALUES(@pActorName, @pSEX, @pDOB, @pBio)			
				
				COMMIT TRAN
				Select 1, ('Actor has been added successfully') AS ActionResult -- RECORD SAVED SUCCESSFULLY
			END
		ELSE
			BEGIN
				Select -1, ('Actor name already exists') AS ActionResult
			END
		
	END TRY
	BEGIN CATCH
		SELECT -100 ,ERROR_MESSAGE() AS ActionResult -- ERROR DURING INSERTING DATA
		ROLLBACK TRAN
	END CATCH


END
GO
/****** Object:  StoredProcedure [dbo].[USP_AddExceptionErrorLogData]    Script Date: 6/30/2018 5:12:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[USP_AddExceptionErrorLogData]
	-- Add the parameters for the stored procedure here
	@pMessage VARCHAR(1500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	BEGIN TRY		
		BEGIN TRAN		
		INSERT INTO ErrorLog (ErrorMsg) VALUES (@pMessage)

		COMMIT TRAN
		SELECT 1		

	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
		SELECT -100, ERROR_MESSAGE() AS ErrMsg
	END CATCH	
	
END

GO
/****** Object:  StoredProcedure [dbo].[USP_AddMovie]    Script Date: 6/30/2018 5:12:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_AddMovie]
@pMovieName VARCHAR(75) = NULL,
@pPlot NVARCHAR(MAX) = NULL,
@pPoster VARCHAR(100) = NULL,
@pReleaseYear DATETIME = NULL,
@pProducerId BIGINT =  NULL,
@pActorList VARCHAR(3000) = NULL
AS
BEGIN
DECLARE @MovieId int
DECLARE @hActor int

	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM MovieMaster WHERE MovieName = @pMovieName AND YEAR(ReleaseYear) = YEAR(@pReleaseYear))
			BEGIN
				BEGIN TRAN
				INSERT INTO MovieMaster(MovieName, Plot, Poster, ReleaseYear, ProducerId)
				VALUES(@pMovieName, @pPlot, @pPoster, @pReleaseYear, @pProducerId)

				SELECT @MovieId = @@identity
					
				IF(@MovieId > 0)
				BEGIN	
					--Prepare input values as an XML documnet
					exec sp_xml_preparedocument @hActor OUTPUT, @pActorList
			   
					-- Insert statements for procedure here						
					INSERT INTO ActorMovieMap (MovieId, ActorId) 
					SELECT @MovieId, ActorId FROM OPENXML (@hActor, '/NewDataSet/ActorList',2)
					WITH (ActorId varchar(10))							
						
					EXEC sp_xml_removedocument @hActor
				END				
				
				COMMIT TRAN
				Select 1, ('Movie has been added successfully') AS ActionResult -- RECORD SAVED SUCCESSFULLY
			END
		ELSE
			BEGIN
				Select -1, ('Movie name with same release year already exists') AS ActionResult
			END
		
	END TRY
	BEGIN CATCH
		SELECT -100 ,ERROR_MESSAGE() AS ActionResult -- ERROR DURING INSERTING DATA
		ROLLBACK TRAN
	END CATCH


END
GO
/****** Object:  StoredProcedure [dbo].[USP_AddProducer]    Script Date: 6/30/2018 5:12:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_AddProducer]
@pProducerName VARCHAR(100) = NULL,
@pSEX CHAR(1) = NULL,
@pDOB DATETIME = NULL,
@pBio NVARCHAR(MAX) = NULL

AS
BEGIN


	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM ProducerMaster WHERE ProducerName = @pProducerName)
			BEGIN
				BEGIN TRAN
				INSERT INTO ProducerMaster(ProducerName, Sex, DOB, Bio)
				VALUES(@pProducerName, @pSEX, @pDOB, @pBio)			
				
				COMMIT TRAN
				Select 1, ('Producer has been added successfully') AS ActionResult -- RECORD SAVED SUCCESSFULLY
			END
		ELSE
			BEGIN
				Select -1, ('Producer name already exists') AS ActionResult
			END
		
	END TRY
	BEGIN CATCH
		SELECT -100 ,ERROR_MESSAGE() AS ActionResult -- ERROR DURING INSERTING DATA
		ROLLBACK TRAN
	END CATCH


END
GO
/****** Object:  StoredProcedure [dbo].[USP_GetActorList]    Script Date: 6/30/2018 5:12:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_GetActorList]
@pActorId BIGINT = NULL
AS
BEGIN
			
	SELECT ActorId, ActorName, Sex, DOB, Bio FROM ActorMaster
	WHERE ActorId = ISNULL(@pActorId, ActorId)
	ORDER BY ActorName 
END
GO
/****** Object:  StoredProcedure [dbo].[USP_GetMovieList]    Script Date: 6/30/2018 5:12:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC [dbo].[USP_GetMovieList]
CREATE PROCEDURE [dbo].[USP_GetMovieList]
@pMovieId BIGINT = NULL
AS
BEGIN
			
	SELECT MM.MovieId, MM.MovieName, CONVERT(VARCHAR(10), MM.ReleaseYear, 103) AS ReleaseYear, YEAR(MM.ReleaseYear) AS MovieReleaseYear, MM.Plot, MM.Poster, PM.ProducerId, PM.ProducerName, ROW_NUMBER() OVER (ORDER BY MM.CreatedOn DESC)  AS RowNum,
	(SELECT SUBSTRING((SELECT ',' +CAST(AM.ActorId AS VARCHAR(150))FROM ActorMovieMap AS AM WHERE AM.MovieId = MM.MovieId ORDER BY MM.MovieId DESC FOR XML PATH('')),2,2000)) AS strActorId
	FROM MovieMaster AS MM
	INNER JOIN ProducerMaster AS PM ON MM.ProducerId = PM.ProducerId 
	WHERE MM.MovieId = ISNULL(@pMovieId, MM.MovieId)
	ORDER BY MM.CreatedOn DESC

END
GO
/****** Object:  StoredProcedure [dbo].[USP_GetProducerList]    Script Date: 6/30/2018 5:12:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_GetProducerList]
@pProducerId BIGINT = NULL
AS
BEGIN
			
	SELECT ProducerId, ProducerName, Sex, DOB, Bio FROM ProducerMaster
	WHERE ProducerId = ISNULL(@pProducerId, ProducerId)
	ORDER BY ProducerName 
END
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateMovie]    Script Date: 6/30/2018 5:12:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_UpdateMovie]
	-- Add the parameters for the stored procedure here
	@pMovieId BIGINT =  NULL,
	@pMovieName VARCHAR(75) = NULL,
	@pPlot NVARCHAR(MAX) = NULL,
	@pPoster VARCHAR(100) = NULL,
	@pReleaseYear DATETIME = NULL,
	@pProducerId BIGINT =  NULL,
	@pActorList VARCHAR(3000) = NULL
	

AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @MovieId int
	DECLARE @hActor int

	BEGIN TRY
		IF EXISTS(SELECT 1 FROM MovieMaster WHERE MovieId = @pMovieId)
			BEGIN					
				BEGIN TRAN			
					
					IF NOT EXISTS(SELECT 1 FROM MovieMaster WHERE MovieName = @pMovieName AND YEAR(ReleaseYear) = YEAR(@pReleaseYear) AND MovieId != @pMovieId)
					BEGIN
						UPDATE MovieMaster 
						SET MovieName = @pMovieName, 
							Plot = @pPlot, 
							Poster = @pPoster, 
							ReleaseYear = @pReleaseYear,
							ProducerId = @pProducerId,											
							ModifiedOn = GETDATE() 
						WHERE MovieId = @pMovieId		

						IF EXISTS(SELECT 1 FROM ActorMovieMap WHERE MovieId = @pMovieId)
							BEGIN	
								--DELETE MAP TABLE DATA RESPECTIVE MOVIE ID
								DELETE ActorMovieMap WHERE MovieId = @pMovieId
							END	

							--Prepare input values as an XML documnet
							exec sp_xml_preparedocument @hActor OUTPUT, @pActorList
			   
							-- Insert statements for procedure here						
							INSERT INTO ActorMovieMap (MovieId, ActorId) 
							SELECT @pMovieId, ActorId FROM OPENXML (@hActor, '/NewDataSet/ActorList',2)
							WITH (ActorId varchar(10))							
						
							EXEC sp_xml_removedocument @hActor
					END
							
					
				COMMIT TRAN
				SELECT 1
			END
	END TRY
	BEGIN CATCH
		SELECT -100 ,ERROR_MESSAGE() AS ActionResult -- ERROR DURING INSERTING DATA
		ROLLBACK TRAN
	END CATCH

	SET NOCOUNT OFF;
	RETURN

END
GO
