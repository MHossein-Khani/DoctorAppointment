using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Infrastructure.Test;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Persistence.EF.Patients;
using DoctorAppointment.Service.Patients;
using DoctorAppointment.Service.Patients.Contracts;
using DoctorAppointment.Service.Patients.PatientException;
using DoctorAppointment.Test.Tool;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DoctorAppointment.Services.UnitTest.Patients
{
    public class PatientServiceTest
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PatientRipository _patientRipository;
        private readonly UnitOfWork _unitOfWork;
        private readonly PatientService _sut;

        public PatientServiceTest()
        {
            _dbContext =
                new EFInMemoryDataBase()
                .CreateDataContext<ApplicationDbContext>();
            _unitOfWork = new EFUnitOfWork(_dbContext);
            _patientRipository = new EFPatientRipository(_dbContext);
            _sut = new PatientAppService(_patientRipository, _unitOfWork);
        }

        [Fact]
        public void Create_creates_patient_properly()
        {
            var dto = new CreatePatientDto
            {
                FirstName = "Hossein",
                LastName = "Khani",
                NationalCode = "2281892125",
            };

            _sut.Create(dto);
           
            _dbContext.Patients.Should().Contain(p => p.FirstName == dto.FirstName &&
            p.LastName == dto.LastName && 
            p.NationalCode == dto.NationalCode);
        }

        [Fact]
        public void Throw_Exception_if_PatientIsAlreadyExistException_when_creating_a_patient()
        {
            var patient = PatientFactory.CreatePatient();
            _dbContext.Manipulate(_ => _.Add(patient));
            var dto = new CreatePatientDto
            {
                FirstName = "Hossien",
                LastName = "Khani",
                NationalCode = patient.NationalCode,
            };

            Action expected = () => _sut.Create(dto);
           
            expected.Should().ThrowExactly<PatientIsAlreadyExistException>();
        }

        [Fact]
        public void GetAll_returns_all_patients_properly()
        {
            var patients = Create_list_of_patients();
            _dbContext.Manipulate(_ => _.AddRange(patients));

            var expected = _sut.GetAll();
            expected.Should().HaveCount(2);
        }

        [Fact]
        public void Update_updates_a_patient_properly()
        {
            var patient = PatientFactory.CreatePatient();
            _dbContext.Manipulate(_ => _.Add(patient));

            var dto = new UpdatePatientDto
            {
                FirstName = "test",
                LastName = "test",
                NationalCode = "test",
            };

            _sut.Update(dto, patient.Id);

            var expected = _dbContext.Patients.FirstOrDefault();
            expected.FirstName.Should().Be(dto.FirstName);
            expected.LastName.Should().Be(dto.LastName);
            expected.NationalCode.Should().Be(dto.NationalCode);
        }

        [Fact]
        public void Throw_Exception_if_PatientDoesNotExistException_when_updating_a_patient()
        {
            var patient = PatientFactory.CreatePatient();
            _dbContext.Manipulate(_ => _.Add(patient));

            var fakeID = 100;

            var dto = new UpdatePatientDto
            {
                FirstName = "test",
                LastName = "test",
                NationalCode = "1234"
            };

            Action expected  = () => _sut.Update(dto, fakeID);
            expected.Should().ThrowExactly<PatientDoesNotExistForUpdateException>();
        }

        [Fact]
        public void Delete_deletes_a_doctor_properly()
        {
            var patient = PatientFactory.CreatePatient();
            _dbContext.Manipulate(_ => _.Add(patient));

            _sut.Delete(patient.Id);

            _dbContext.Patients.Should().HaveCount(0);
        }

        [Fact]
        public void Throw_Exception_if_PatientDoesNotExistException_when_deleting_a_patient()
        {
            var patient = PatientFactory.CreatePatient();
            _dbContext.Manipulate(_ => _.Add(patient));

            var fakeId = 100;

            Action expected = () => _sut.Delete(fakeId);
            expected.Should().ThrowExactly<PatientDoesNotExistForDeleteException>();
        }

        public List<Patient> Create_list_of_patients()
        {
            var patients = new List<Patient>()
            {
                new Patient
                {
                    FirstName = "Hossien",
                    LastName = "Khani",
                    NationalCode = "2281892125",
                },

                new Patient
                {
                    FirstName = "Ahmad",
                    LastName = "Yagoubi",
                    NationalCode = "124",
                }
            };

            return patients;
        }
    }

}
