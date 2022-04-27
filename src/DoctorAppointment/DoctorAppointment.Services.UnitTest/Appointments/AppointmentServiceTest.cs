using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Infrastructure.Test;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Persistence.EF.Appointments;
using DoctorAppointment.Service.Appointments;
using DoctorAppointment.Service.Appointments.AppointmentExceptions;
using DoctorAppointment.Service.Appointments.Contract;
using DoctorAppointment.Test.Tool;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DoctorAppointment.Services.UnitTest.Appointments
{
    public class AppointmentServiceTest
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly AppointmentRipository _appointmentRipository;
        private readonly UnitOfWork _unitOfWork;
        private readonly AppointmentService _sut;

        public AppointmentServiceTest()
        {
            _dbContext =
                new EFInMemoryDataBase()
                .CreateDataContext<ApplicationDbContext>();
            _unitOfWork = new EFUnitOfWork(_dbContext);
            _appointmentRipository = new EFAppointmentRipository(_dbContext);
            _sut = new AppointmentAppService(_appointmentRipository, _unitOfWork);
        }

        [Fact]
        public void MakeAppointment_makes_a_appointment_for_doctor_and_patient()
        {
            var doctor = DoctorFactory.CreateDoctor();
            _dbContext.Manipulate(_ => _.Add(doctor));

            var patient = PatientFactory.CreatePatient();
            _dbContext.Manipulate(_ => _.Add(patient));

            var dto = new CreateAppointmentDto
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                Date = new DateTime(2022, 04, 27)
            };

            _sut.MakeAppointment(dto);

            _dbContext.Appointments.Should().Contain(_ => _.DoctorId == dto.DoctorId &&
            _.PatientId == dto.PatientId &&
            _.Date == dto.Date);
        }

        [Fact]
        public void Throw_Exception_if_AppointmentOfDoctorIsFullException_when_appointment_of_a_foctor_is_grater_than_five()
        {
            Doctor doctor = CreateDoctorWith5AppointmentInOneDay();
            _dbContext.Manipulate(_ => _.Add(doctor));
            var patient = PatientFactory.CreatePatient();
            _dbContext.Manipulate(_ => _.Add(patient));

            var dto = new CreateAppointmentDto
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                Date = DateTime.Now.Date
            };

            Action expected = () => _sut.MakeAppointment(dto);

            expected.Should().ThrowExactly<AppointmentOfDoctorIsFullException>();
        }


        [Fact]
        public void GetAll_returns_all_appointments_properly()
        {
            var appointments = Create_list_of_appointment();
            _dbContext.Manipulate(_ => _.AddRange(appointments));

            var expected = _sut.GetAll();
        }

        [Fact]
        public void Update_updates_appointment_of_patient_properly()
        {
            var doctor = DoctorFactory.CreateDoctor();
            _dbContext.Manipulate(_ => _.Add(doctor));

            var patient = PatientFactory.CreatePatient();
            _dbContext.Manipulate(_ => _.Add(patient));

            var doctor2 = DoctorFactory.CreateDoctor();
            _dbContext.Manipulate(_ => _.Add(doctor2));

            var appointment = new Appointment
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                Date = new DateTime(2022, 04, 27)
            };
            _dbContext.Manipulate(_ => _.Add(appointment));

            var dto = new UpdateAppointmentDto
            {
                DoctorId = doctor2.Id,
                PatientId = patient.Id,
                Date = new DateTime(2022, 04, 28)

            };

            _sut.Update(dto, doctor.Id);
            var expected = _dbContext.Appointments.FirstOrDefault();
            expected.DoctorId.Should().Be(dto.DoctorId);
            expected.PatientId.Should().Be(dto.PatientId);
            expected.Date.Should().Be(dto.Date);
        }
        
        [Fact]
        public void Throw_Exception_if_AppointmentIsAlreadyExistException_when_updating_a_appointment()
        {
            Doctor doctor = CreateDoctorWith5AppointmentInOneDay();
            _dbContext.Manipulate(_ => _.Add(doctor));

            var patient = PatientFactory.CreatePatient();
            _dbContext.Manipulate(_ => _.Add(patient));

            var dto = new UpdateAppointmentDto
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                Date = DateTime.Now.Date
            };

            Action expected = () => _sut.Update(dto, doctor.Id);

            expected.Should().ThrowExactly<AppointmentIsAlreadyExistException>();
        }

        [Fact]
        public void Throw_Exception_if_DoctorDoesNotExitForChangeTheDoctorException_when_patient_want_to_change_appointment()
        {

            var doctor = DoctorFactory.CreateDoctor();
            _dbContext.Manipulate(_ => _.Add(doctor));

            var patient = PatientFactory.CreatePatient();
            _dbContext.Manipulate(_ => _.Add(patient));

            var fakeId = 100;

            var appointment = new Appointment
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                Date = new DateTime(2022, 04, 27)
            };
            _dbContext.Manipulate(_ => _.Add(appointment));

            var dto = new UpdateAppointmentDto
            {
                DoctorId = fakeId,
                PatientId = patient.Id,
                Date = new DateTime(2022, 04, 28)
            };

            Action expected = () => _sut.Update(dto, dto.DoctorId);
            expected.Should().ThrowExactly<DoctorDoesNotExitForChangeTheDoctorException>();
        }

        [Fact]
        public void Delete_deletes_a_appointment_for_patient_properly()
        {
            var doctor = DoctorFactory.CreateDoctor();
            _dbContext.Manipulate(_ => _.Add(doctor));

            var patient = PatientFactory.CreatePatient();
            _dbContext.Manipulate(_ => _.Add(patient));

            var appointment = new Appointment
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                Date = new DateTime(2022, 04, 27)
            };
            _dbContext.Manipulate(_ => _.Add(appointment));

            _sut.Delete(appointment.Id);
            _dbContext.Appointments.Should().HaveCount(0);
        }

        [Fact]
        public void Throw_exception_if_AppointmentDoesNotSetException_when_deleting_a_appointment()
        {
            var doctor = DoctorFactory.CreateDoctor();
            _dbContext.Manipulate(_ => _.Add(doctor));

            var patient = PatientFactory.CreatePatient();
            _dbContext.Manipulate(_ => _.Add(patient));

            var fakeId = 100;

            Action expected = () => _sut.Delete(fakeId);
            expected.Should().ThrowExactly<AppointmentDoesNotSetException>();
        }


        public List<Appointment> Create_list_of_appointment()
        {
            var doctor = DoctorFactory.CreateDoctor();
            _dbContext.Manipulate(_ => _.Add(doctor));

            var patient = PatientFactory.CreatePatient();
            _dbContext.Manipulate(_ => _.Add(patient));

            var doctor2 = DoctorFactory.CreateDoctor();
            _dbContext.Manipulate(_ => _.Add(doctor2));

            var patient2 = PatientFactory.CreatePatient();
            _dbContext.Manipulate(_ => _.Add(patient2));

            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    DoctorId = doctor.Id,
                    PatientId = patient.Id,
                    Date = new DateTime(2022, 04, 25)
                },

                 new Appointment
                {
                    DoctorId = doctor2.Id,
                    PatientId = patient2.Id,
                    Date = new DateTime(2022, 04, 28)
                }
            };

            return appointments;
        }

        private static Doctor CreateDoctorWith5AppointmentInOneDay()
        {
            return new DoctorBuilder()
                .WithAppointment(DateTime.Now.Date, "as", "fr", "7878")
                .WithAppointment(DateTime.Now.Date, "gt", "nb", "6565")
                .WithAppointment(DateTime.Now.Date, "ld", "yv", "4030")
                .WithAppointment(DateTime.Now.Date, "mk", "qw", "6598")
                .WithAppointment(DateTime.Now.Date, "nt", "cw", "1245")
                .Build();
        }

    }

}
