
/*--------------------- Object:  StoredProcedure [dbo].[AddConsularAppointment]  ---------------------*/

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