using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Service.Appointments.AppointmentExceptions;
using DoctorAppointment.Service.Appointments.Contract;

namespace DoctorAppointment.Service.Appointments
{
    public class AppointmentAppService : AppointmentService
    {
        private readonly AppointmentRipository _appointmentRipository;
        private readonly UnitOfWork _unitOfWork;

        public AppointmentAppService(AppointmentRipository appointmentRipository, UnitOfWork unitOfWork)
        {
            _appointmentRipository = appointmentRipository;
            _unitOfWork = unitOfWork;
        }

        public void MakeAppointment(CreateAppointmentDto dto)
        {
            var countOfappointment = _appointmentRipository.Count(dto.DoctorId, dto.Date);
            if(countOfappointment >= 5)
            {
                throw new AppointmentOfDoctorIsFullException();
            }

            var appointment = new Appointment
            {
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                Date = dto.Date,
            };

            _appointmentRipository.MakeAppointment(appointment);
            _unitOfWork.Commit();
        }
    }
}
