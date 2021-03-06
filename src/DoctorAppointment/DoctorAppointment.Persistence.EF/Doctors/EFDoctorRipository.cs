using DoctorAppointment.Entities;
using DoctorAppointment.Service.Doctors.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Persistence.EF.Doctors
{
    public class EFDoctorRipository : DoctorRipository
    {
        private readonly ApplicationDbContext _dbContext;

        public EFDoctorRipository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool IsExistNationalCode(string nationalCode)
        {
            return _dbContext.Doctors.Any(p => p.NationalCode == nationalCode);
        }
        public void Create(Doctor doctor)
        {
           _dbContext.Doctors.Add(doctor);
        }

        public List<GetDoctorDto> GetAll()
        {
           return _dbContext.Doctors.Select(p => new GetDoctorDto
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                NationalCode = p.NationalCode,
                Field = p.Field,
            }).ToList();
        }

        public Doctor FindById(int id)
        {
            return _dbContext.Doctors.Find(id);
        }

        public bool IsRepeatNationalCode(string NationalCode, int id)
        {
            return _dbContext.Doctors.Any(_ => _.NationalCode == NationalCode && _.Id != id);
        }

        public void Delete(int id)
        {
            var doctor = _dbContext.Doctors.Find(id);

            _dbContext.Doctors.Remove(doctor);
        }
    }
}
