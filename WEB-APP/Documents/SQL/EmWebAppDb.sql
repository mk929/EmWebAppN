
USE [EmWebAppDb]
GO 

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*-----------------       Object:  Table [dbo].[ConsularAppointments] ----------------------------------*/
 IF EXISTS ( SELECT  * 
				FROM    sys.objects
				WHERE   object_id = OBJECT_ID(N'[dbo].[ConsularAppointments]')
				AND type IN ( N'U' ) )
BEGIN
	DROP TABLE [dbo].[ConsularAppointments]
END
GO

CREATE TABLE [dbo].[ConsularAppointments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FormSubmissionDate] [datetime] NOT NULL,
	[AppointmentDate] [datetime] NOT NULL,
	[AppointmentType] [int] NOT NULL,
	[QueueNumber] [int] NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Gender] [nchar](1) NULL,
	[DateOfBirth] [datetime] NULL,
	[PlaceOfBirth] [nvarchar](128) NULL,
	[Nationality] [nchar](2) NULL,
	[NRIC_No] [nvarchar](128) NULL,
	[PassportNumber] [nvarchar](128) NULL,
	[PassportIssuedDate] [datetime] NULL,
	[ConsulateLocation] [nchar](2) NULL,
	[StayType] [int] NOT NULL,
	[StayPermitNumber] [nvarchar](128) NULL,
	[EmployerName] [nvarchar](128) NULL,
	[Occupation] [nvarchar](128) NULL,
	[ContactAddr1] [nvarchar](512) NULL,
	[ContactAddr2] [nvarchar](512) NULL,
	[ContactPhone] [nvarchar](128) NULL,
	[ContactEmail] [nvarchar](128) NOT NULL,
	[HomeAddr1] [nvarchar](512) NULL,
	[HomeAddr2] [nvarchar](512) NULL,
	[HomePhone] [nvarchar](128) NULL,
	[AppointmentStatus] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[ActivationCode] [nvarchar](256) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/*--------------------- Object:  StoredProcedure [dbo].[AddConsularAppointment]  ---------------------*/
 IF EXISTS ( SELECT  * 
    FROM    sys.objects
    WHERE   object_id = OBJECT_ID(N'[dbo].[AddConsularAppointment]')
			AND type IN ( N'P', N'PC' ) )
BEGIN
	DROP PROCEDURE [dbo].[AddConsularAppointment]
END
GO


CREATE PROCEDURE [dbo].[AddConsularAppointment]
(
    @AppointmentDate      DATETIME       ,
    @AppointmentType      INT		 ,
    @QueueNumber		  INT	 ,
    @Name                 NVARCHAR (128) ,
    @Gender               NCHAR (1)	 ,
    @DateOfBirth          DATETIME       ,
    @PlaceOfBirth         NVARCHAR (128) ,
    @Nationality          NCHAR (2) = 'MM' ,
    @NRIC_No              NVARCHAR (128) ,
    @PassportNumber       NVARCHAR (128) ,
    @PassportIssuedDate   DATETIME       ,
    @ConsulateLocation    NCHAR (2) = 'SG' ,
    @StayType		  INT,
    @StayPermitNumber	  NVARCHAR (128) ,
    @EmployerName	  NVARCHAR (128) = NULL,
    @Occupation		  NVARCHAR (128) = NULL,
    @ContactAddr1         NVARCHAR (512) ,
    @ContactAddr2         NVARCHAR (512) = NULL,
    @ContactPhone         NVARCHAR (128) ,
    @ContactEmail         NVARCHAR (128) ,
    @HomeAddr1            NVARCHAR (512) ,
    @HomeAddr2            NVARCHAR (512) = NULL,
    @HomePhone            NVARCHAR (128) = NULL,
    @AppointmentStatus    int  = 0,
    @Note                 NVARCHAR (MAX) = NULL,
    @ApplicationId        int output,
    @ActivationCode       NVARCHAR (256) output

)
AS	
BEGIN
	SET NOCOUNT ON;
SELECT @ActivationCode = convert(nvarchar(256),replace(NEWID(),N'-',N'')) 

INSERT INTO [dbo].[ConsularAppointments]
           ([FormSubmissionDate]
		   ,[AppointmentDate]
		   ,[AppointmentType]
           ,[QueueNumber]
           ,[Name]
           ,[Gender]
           ,[DateOfBirth]
           ,[PlaceOfBirth]
           ,[Nationality]
           ,[NRIC_No]
           ,[PassportNumber]
           ,[PassportIssuedDate]
           ,[ConsulateLocation]
		   ,[StayType]
           ,[StayPermitNumber]
		   ,[EmployerName]
		   ,[Occupation]
           ,[ContactAddr1]
           ,[ContactAddr2]
           ,[ContactPhone]
           ,[ContactEmail]
           ,[HomeAddr1]
           ,[HomeAddr2]
           ,[HomePhone]
           ,[AppointmentStatus]
           ,[ActivationCode]
           ,[Note])
     VALUES
           (GETDATE(),
		    @AppointmentDate  ,
		    @AppointmentType,
			@QueueNumber  , 
			@Name  ,
			@Gender  ,
			@DateOfBirth ,
			@PlaceOfBirth ,
			@Nationality   ,
			@NRIC_No   ,
			@PassportNumber ,
			@PassportIssuedDate,
			@ConsulateLocation,
			@StayType,
			@StayPermitNumber,
			@EmployerName,
			@Occupation,
			@ContactAddr1 ,
			@ContactAddr2  ,
			@ContactPhone ,
			@ContactEmail ,
			@HomeAddr1 ,
			@HomeAddr2 ,
			@HomePhone,
			@AppointmentStatus ,
			@ActivationCode,
			@Note  )

SELECT @ApplicationId = SCOPE_IDENTITY()
RETURN 0
END


GO

/*--------------------- Object:  StoredProcedure [dbo].[ConfirmConsularAppointment]  ---------------------*/

 IF EXISTS ( SELECT  * 
    FROM    sys.objects
    WHERE   object_id = OBJECT_ID(N'[dbo].[ConfirmConsularAppointment]')
			AND type IN ( N'P', N'PC' ) )
BEGIN
	DROP PROCEDURE [dbo].ConfirmConsularAppointment
END
GO

/****** Object:  StoredProcedure [dbo].[ConfirmConsularAppointment]    Script Date: 4/20/2017 10:38:52 PM ******/

CREATE PROCEDURE [dbo].[ConfirmConsularAppointment]
(
	@ApplicationID		  int,
	@ActivationCode		  nvarchar (256),
    	@Note			  nvarchar (MAX) = NULL,
    	@ConfirmedApptDate	  datetime output,
	@ConfirmedQueueNumber	  int output
	)
AS	
BEGIN
	SET NOCOUNT ON;
	declare @appointmentStatus as int

	select @ConfirmedApptDate = AppointmentDate
			, @ConfirmedQueueNumber = QueueNumber
			, @appointmentStatus = AppointmentStatus
		from ConsularAppointments
		where ID = @ApplicationID
			and ActivationCode = @ActivationCode

	if ( @@rowcount = 1 )
	begin
		if ( @appointmentStatus != 1 )  /* initial or canceled */
		begin
		select @ConfirmedQueueNumber =  max(QueueNumber) + 1
			from ConsularAppointments
			where AppointmentDate = @ConfirmedApptDate

		update ConsularAppointments
				set AppointmentStatus = 1, /* N'CONFIRMED', */
					QueueNumber = @ConfirmedQueueNumber
				where ID = @ApplicationID
					and ActivationCode = @ActivationCode
		end
	end
	else  /* ID and Code not found*/
	begin
			set @ConfirmedQueueNumber = 0
	end

	select * 
		from ConsularAppointments
		where ID = @ApplicationID
			and ActivationCode = @ActivationCode

RETURN 0

END


GO



/*--------------------- Object:  StoredProcedure [dbo].[GetActiveConsularApptsForAdmin]  ---------------------*/
 IF EXISTS ( SELECT  * 
    FROM    sys.objects
    WHERE   object_id = OBJECT_ID(N'[dbo].[GetActiveConsularApptsForAdmin]')
			AND type IN ( N'P', N'PC' ) )
BEGIN
	DROP PROCEDURE [dbo].GetActiveConsularApptsForAdmin
END
GO

/****** Object:  StoredProcedure [dbo].[GetActiveConsularApptsForAdmin]    Script Date: 4/20/2017 10:40:05 PM ******/

CREATE PROCEDURE [dbo].[GetActiveConsularApptsForAdmin]
(
	@AppointmentDate datetime = null,
	@PassportNumber   nvarchar (128) = null
	)
AS	
BEGIN
	SET NOCOUNT ON;

	if ( @AppointmentDate is null and @PassportNumber is null ) /* Admin query */
	begin
	/*
		select ID, [Name], [PassportNumber], [ContactEmail]
					, [AppointmentDate], [AppointmentType], [QueNumber]  */
		select *
			from ConsularAppointments
			where AppointmentDate >= convert(date, getdate())
			  and AppointmentStatus = 1 /* N'CONFIRMED', */
			  order by AppointmentDate, QueueNumber;
	end
	else if ( @AppointmentDate is null and @PassportNumber is not null )
	begin
		select *
			from ConsularAppointments
			where AppointmentDate >= convert(date, getdate())
				and AppointmentStatus = 1
				and [PassportNumber] like  @PassportNumber + '%'
			  order by AppointmentDate, QueueNumber;
	end
	else if ( @AppointmentDate is not null and @PassportNumber is null )
	begin
		select *
			from ConsularAppointments
			where AppointmentDate = @AppointmentDate
				and AppointmentStatus = 1
			  order by AppointmentDate, QueueNumber;
	end
	else /*  ( @AppointmentDate is not null and @PassportNumber is not null ) */
	begin
		select *
			from ConsularAppointments
			where AppointmentDate = @AppointmentDate
				and AppointmentStatus = 1
				and [PassportNumber] like   @PassportNumber + '%'
			  order by AppointmentDate, QueueNumber;
	end


RETURN 0

END


GO


/*--------------------- Object:  StoredProcedure [dbo].[GetActiveConsularApptsForUser]  ---------------------*/
 IF EXISTS ( SELECT  * 
    FROM    sys.objects
    WHERE   object_id = OBJECT_ID(N'[dbo].[GetActiveConsularApptsForUser]')
			AND type IN ( N'P', N'PC' ) )
BEGIN
	DROP PROCEDURE [dbo].GetActiveConsularApptsForUser
END
GO

/****** Object:  StoredProcedure [dbo].[GetActiveConsularApptsForUser]    Script Date: 4/20/2017 10:40:56 PM ******/

CREATE PROCEDURE [dbo].[GetActiveConsularApptsForUser]
(
	@PassportNumber   nvarchar (128),
	@Email  nvarchar (128)
	)
AS	
BEGIN
	SET NOCOUNT ON;
	select *
		from ConsularAppointments
		where PassportNumber = @PassportNumber
		and ContactEmail =  @Email
		and AppointmentDate >= convert(date, getdate())
			and AppointmentStatus = 1; 

RETURN 0

END



GO
/*----------------------------------  Store Procesdures ----------------------------------*/