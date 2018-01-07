

/*--------------------- Object:  StoredProcedure [dbo].[ConfirmConsularAppointment]  ---------------------*/

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

/*----------------------------------  Store Procesdures ----------------------------------*/