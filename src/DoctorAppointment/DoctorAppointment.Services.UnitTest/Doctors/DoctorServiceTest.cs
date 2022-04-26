using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Infrastructure.Test;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Persistence.EF.Doctors;
using DoctorAppointment.Service.Doctors;
using DoctorAppointment.Service.Doctors.Contracts;
using DoctorAppointment.Service.Doctors.DoctorExceptions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace DoctorAppointment.Services.UnitTest.Doctors
{
    public class DoctorServiceTest
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DoctorRipository _doctorRipository;
        private readonly UnitOfWork _unitOfWork;
        private readonly DoctorService _sut;

        public DoctorServiceTest()
        {
            _dbContext =
                new EFInMemoryDataBase()
                .CreateDataContext<ApplicationDbContext>();
            _unitOfWork = new EFUnitOfWork(_dbContext);
            _doctorRipository = new EFDoctorRipository(_dbContext);
            _sut = new DoctorAppService(_doctorRipository, _unitOfWork);
        }

        [Fact]
        public void Create_creats_doctor_properly()
        {
            var dto = new CreateDoctorDto
            {
                FirstName = "Hossin",
                LastName = "Khani",
                NationalCode = "2281892123",
                Field = "Phsyco"
            };

            _sut.Create(dto);

            _dbContext.Doctors.Should().Contain(p => p.FirstName == dto.FirstName &&
            p.LastName == dto.LastName && 
            p.NationalCode == dto.NationalCode &&
            p.Field == dto.Field);
        }

        [Fact]
        public void Throw_Exception_if_DoctorIsAlreadyExistException_properly()
        {
            var doctor = new Doctor
            {
                FirstName = "Hossin",
                LastName = "Khani",
                NationalCode = "2281892123",
                Field = "Phsyco"
            };
            _dbContext.Manipulate(_ => _.Add(doctor));


            var dto = new CreateDoctorDto
            {
                FirstName = "Hossin",
                LastName = "Khani",
                NationalCode = doctor.NationalCode,
                Field = "Phsyco"
            };
            

            Action expected = () => _sut.Create(dto);
            expected.Should().ThrowExactly<DoctorIsAlreadyExistException>();
        }

        [Fact]
        public void GetAll_returns_all_doctors()
        {
            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    FirstName = "Hossin",
                LastName = "Khani",
                NationalCode = "2281892123",
                Field = "Phsyco"
                },

                new Doctor
                {
                    FirstName = "Ahmad",
                LastName = "yaghob",
                NationalCode = "1487932",
                Field = "omom"
                },
            };
            _dbContext.Manipulate(_ => _.AddRange(doctors));

            var expected = _sut.GetAll();
            expected.Should().HaveCount(2);
            
        }
        

    }



   
}
