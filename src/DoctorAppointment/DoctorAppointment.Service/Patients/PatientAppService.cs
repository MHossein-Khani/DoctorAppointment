using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Service.Patients.Contracts;
using DoctorAppointment.Service.Patients.PatientException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Service.Patients
{
    public class PatientAppService : PatientService
    {
        private readonly PatientRipository _patientRipository;
        private readonly UnitOfWork _unitOfWork;

        public PatientAppService(PatientRipository patientRipository, UnitOfWork unitOfWork)
        {
            _patientRipository = patientRipository;
            _unitOfWork = unitOfWork;
        }

        public void Create(CreatePatientDto dto)
        {
            var doctor = new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                NationalCode = dto.NationalCode,
            };

            var IsPatientExist = _patientRipository.
                IsExistNationalCode(dto.NationalCode);
            if (IsPatientExist)
            {
                throw new PatientIsAlreadyExistException();
            }

            _patientRipository.Create(doctor);
            _unitOfWork.Commit();
        }

        public List<GetPatientDto> GetAll()
        {
            return _patientRipository.GetAll();
        }

        public void Update(UpdatePatientDto dto, int id)
        {
            var patient = _patientRipository.FindById(id);

            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.NationalCode = dto.NationalCode;

            _unitOfWork.Commit();
        }
    }
}
