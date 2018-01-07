
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
