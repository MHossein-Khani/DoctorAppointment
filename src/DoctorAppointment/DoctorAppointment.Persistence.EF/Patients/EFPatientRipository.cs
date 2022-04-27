using DoctorAppointment.Entities;
using DoctorAppointment.Service.Patients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Persistence.EF.Patients
{
    public class EFPatientRipository : PatientRipository
    {
        private readonly ApplicationDbContext _dbContext;

        public EFPatientRipository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Patient patient)
        {
            _dbContext.Patients.Add(patient);
        }

        public void Delete(int id)
        {
            var patient = FindById(id);

            _dbContext.Patients.Remove(patient);
        }

        public Patient FindById(int id)
        {
            return _dbContext.Patients.Find(id);
        }

        public List<GetPatientDto> GetAll()
        {
            return _dbContext.Patients.Select(p => new GetPatientDto
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                NationalCode = p.NationalCode,
            }).ToList();
        }

        public bool IsExistNationalCode(string nationalCode)
        {
            return _dbContext.Patients.Any(_ => _.NationalCode == nationalCode);
        }
    }
}
