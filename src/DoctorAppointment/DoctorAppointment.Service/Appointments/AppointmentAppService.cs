using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Service.Appointments.AppointmentExceptions;
using DoctorAppointment.Service.Appointments.Contract;
using System.Collections.Generic;

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

        public void Delete(int id)
        {
            var appointment = _appointmentRipository.FindById(id);
            if (appointment == null)
            {
                throw new AppointmentDoesNotSetException();
            };

            _appointmentRipository.Delete(id);
            _unitOfWork.Commit();
        }

        public List<GetAppointmentDto> GetAll()
        {
            return _appointmentRipository.GetAll();
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

        public void Update(UpdateAppointmentDto dto, int id)
        {
            var countOfappointment = _appointmentRipository.Count(dto.DoctorId, dto.Date);
            if (countOfappointment >= 5)
            {
                throw new AppointmentIsAlreadyExistException();
            }

            var appointmentOfDoctor = _appointmentRipository.FindById(id);
            if(appointmentOfDoctor == null)
            {
                throw new DoctorDoesNotExitForChangeTheDoctorException();
            }

            appointmentOfDoctor.DoctorId = dto.DoctorId;
            appointmentOfDoctor.Date = dto.Date;

            _unitOfWork.Commit();
        }
    }
}
