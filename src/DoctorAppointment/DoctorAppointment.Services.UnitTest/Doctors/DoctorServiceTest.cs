using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Infrastructure.Test;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Persistence.EF.Doctors;
using DoctorAppointment.Service.Doctors;
using DoctorAppointment.Service.Doctors.Contracts;
using DoctorAppointment.Service.Doctors.DoctorExceptions;
using DoctorAppointment.Test.Tool;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public void Throw_Exception_if_DoctorIsAlreadyExistException_when_create_a_doctor()
        {
            var doctor = DoctorFactory.CreateDoctor();
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
            var doctors = Create_list_of_doctors();
            _dbContext.Manipulate(_ => _.AddRange(doctors));

            var expected = _sut.GetAll();
            expected.Should().HaveCount(2);
        }

        [Fact]
        public void Update_updates_a_doctor_properly()
        {
            var doctor = DoctorFactory.CreateDoctor();
            _dbContext.Manipulate(_ => _.Add(doctor));

            var dto = new UpdateDoctorDto
            {
                FirstName = "test",
                LastName = "test",
                NationalCode = "45",
                Field = "test"
            };

            _sut.Update(dto, doctor.Id);

            var expected = _dbContext.Doctors.FirstOrDefault();
            expected.FirstName.Should().Be(dto.FirstName);
            expected.LastName.Should().Be(dto.LastName);
            expected.NationalCode.Should().Be(dto.NationalCode);
            expected.Field.Should().Be(dto.Field);
        }

        [Fact]
        public void Throw_Exception_if_NationalCodeIsAlreadyExistException_when_update_a_doctor()
        {
            var doctor = DoctorFactory.CreateDoctor();
            _dbContext.Manipulate(_ => _.Add(doctor));

            var fakeDoctorId = 100;

            var dto = new UpdateDoctorDto
            {
                FirstName = "test",
                LastName = "test",
                NationalCode = doctor.NationalCode,
                Field = "test"
            };

            Action expected = () => _sut.Update(dto, fakeDoctorId);
            expected.Should().ThrowExactly<NationalCodeIsAlreadyExistException>();
        }

        [Fact]
        public void Throw_Exception_if_DoctorDoesNotExistException_when_update_a_doctor()
        {
            var doctor = DoctorFactory.CreateDoctor();
            _dbContext.Manipulate(_ => _.Add(doctor));

            var fakeDoctorId = 100;

            var dto = new UpdateDoctorDto
            {
                FirstName = "test",
                LastName = "test",
                NationalCode = doctor.NationalCode,
                Field = "test"
            };

            Action expected = () => _sut.Update(dto, fakeDoctorId);
        }

        [Fact]
        public void Delete_deletes_a_doctor_properly()
        {
            var doctor = DoctorFactory.CreateDoctor();
            _dbContext.Manipulate(_ => _.Add(doctor));

            _sut.Delete(doctor.Id);

            _dbContext.Doctors.Should().HaveCount(0);
        }

        [Fact]
        public void Throw_Exception_if_DoctorIsNotExistException_when_delete_a_doctor()
        {
            var doctor = DoctorFactory.CreateDoctor();
            _dbContext.Manipulate(_ => _.Add(doctor));

            var fakeDoctorId = 100;

            Action expected = () => _sut.Delete(fakeDoctorId);
            expected.Should().ThrowExactly<DoctorIsNotExistException>();
        }

        private List<Doctor> Create_list_of_doctors()
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
                     Field = "omomi"
                },
            };

            return doctors;
        }

    }

}
