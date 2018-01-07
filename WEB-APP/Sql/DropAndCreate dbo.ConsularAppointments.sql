
/****** Object: Table [dbo].[ConsularAppointments] Script Date: 1/4/2018 4:21:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[ConsularAppointments];


GO
CREATE TABLE [dbo].[ConsularAppointments] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [FormSubmissionDate] DATETIME       NOT NULL,
    [AppointmentDate]    DATETIME       NOT NULL,
    [AppointmentType]    INT            NOT NULL,
    [QueueNumber]        INT            NOT NULL,
    [Name]               NVARCHAR (128) NOT NULL,
    [Gender]             NVARCHAR (1)   NOT NULL,
    [DateOfBirth]        DATETIME       NOT NULL,
    [PlaceOfBirth]       NVARCHAR (128) NOT NULL,
    [Nationality]        NVARCHAR (2)   NULL,
    [NRIC_No]            NVARCHAR (128) NOT NULL,
    [PassportNumber]     NVARCHAR (128) NOT NULL,
    [PassportIssuedDate] DATETIME       NOT NULL,
    [ConsulateLocation]  NVARCHAR (128) NULL,
    [StayPermitNumber]   NVARCHAR (128) NULL,
    [StayType]           INT            NOT NULL,
    [EmployerName]       NVARCHAR (128) NULL,
    [Occupation]         NVARCHAR (128) NULL,
    [ContactAddr1]       NVARCHAR (512) NOT NULL,
    [ContactAddr2]       NVARCHAR (512) NULL,
    [ContactPhone]       NVARCHAR (128) NOT NULL,
    [ContactEmail]       NVARCHAR (128) NOT NULL,
    [HomeAddr1]          NVARCHAR (512) NOT NULL,
    [HomeAddr2]          NVARCHAR (512) NULL,
    [HomePhone]          NVARCHAR (128) NULL,
    [AppointmentStatus]  INT            NOT NULL,
    [ActivationCode]     NVARCHAR (512) NOT NULL,
    [Note]               NVARCHAR (MAX) NULL
);


