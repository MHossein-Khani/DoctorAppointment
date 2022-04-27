using DoctorAppointment.Entities;
using DoctorAppointment.Service.Appointments.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Persistence.EF.Appointments
{
    public class EFAppointmentRipository : AppointmentRipository
    {
        private readonly ApplicationDbContext _dbContext;

        public EFAppointmentRipository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Count(int doctorId, DateTime dateTime)
        {
            return _dbContext.Appointments.Where(_ => _.DoctorId == doctorId &&
            _.Date == dateTime).Count();
        }

        public void Delete(int id)
        {
            var appointment = FindById(id);
            _dbContext.Appointments.Remove(appointment);
        }

        public Appointment FindById(int id)
        {
            return _dbContext.Appointments.Find(id);
        }

        public List<GetAppointmentDto> GetAll()
        {
            return _dbContext.Appointments.Select(_ => new GetAppointmentDto
            {
                DoctorId = _.DoctorId,
                PatientId = _.PatientId,
                Date = _.Date,
            }).ToList();
        }

        public void MakeAppointment(Appointment appointment)
        {
            _dbContext.Add(appointment);
        }
    }
}
