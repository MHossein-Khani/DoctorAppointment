using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Service.Doctors.Contracts;
using DoctorAppointment.Service.Doctors.DoctorExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Service.Doctors
{
    public class DoctorAppService : DoctorService
    {
        private DoctorRipository _doctorRipository;
        private UnitOfWork _unitOfWork;

        public DoctorAppService(DoctorRipository doctorRipository ,UnitOfWork unitOfWork)
        {
            _doctorRipository = doctorRipository;
            _unitOfWork = unitOfWork;
        }

        public void Create(CreateDoctorDto dto)
        {
            var doctor = new Doctor
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                NationalCode = dto.NationalCode,
                Field = dto.Field,
            };

            var isDoctorExist = _doctorRipository.
                IsExistNationalCode(dto.NationalCode);
            if (isDoctorExist)
            {
                throw new DoctorIsAlreadyExistException();
            }

            _doctorRipository.Create(doctor);
            _unitOfWork.Commit();
        }

        

        public List<GetDoctorDto> GetAll()
        {
           return _doctorRipository.GetAll();
        }

        public void Update(UpdateDoctorDto dto, int id)
        {
            var doctor = _doctorRipository.FindById(id);

            var isExistNationalCode = _doctorRipository.
                IsRepeatNationalCode(dto.NationalCode, id);
            if (isExistNationalCode)
            {
                throw new NationalCodeIsAlreadyExistException();
            }

            if(doctor == null)
            {
                throw new DoctorDoesNotExistException();
            }

            doctor.FirstName = dto.FirstName;
            doctor.LastName = dto.LastName;
            doctor.NationalCode = dto.NationalCode;
            doctor.Field = dto.Field;

            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var doctor = _doctorRipository.FindById(id);
            if (doctor == null)
            {
                throw new DoctorIsNotExistException();
            }

            _doctorRipository.Delete(id);
            _unitOfWork.Commit();
        }
    }
}
