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
using System.Text;
using System.Threading.Tasks;
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
            var doctor = new DoctorBuilder()
                .WithAppointment(DateTime.Now.Date ,"as", "fr", "7878")
                .WithAppointment(DateTime.Now.Date, "gt", "nb", "6565")
                .WithAppointment(DateTime.Now.Date, "ld", "yv", "4030")
                .WithAppointment(DateTime.Now.Date, "mk", "qw", "6598")
                .WithAppointment(DateTime.Now.Date, "nt", "cw", "1245")
                .Build();
            _dbContext.Manipulate(_ => _.AddRange(doctor));

            var patient = PatientFactory.CreatePatient();
            _dbContext.Manipulate(_ => _.Add(patient));

            var dto = new CreateAppointmentDto
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                Date =DateTime.Now.Date
            };

            Action expected = () => _sut.MakeAppointment(dto);
            expected.Should().ThrowExactly<AppointmentOfDoctorIsFullException>();


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
                },

                new Patient
                {
                    FirstName = "as",
                    LastName = "bi",
                    NationalCode = "127",
                },

                new Patient
                {
                    FirstName = "tr",
                    LastName = "hg",
                    NationalCode = "4587",
                },

                new Patient
                {
                    FirstName = "lk",
                    LastName = "loi",
                    NationalCode = "7878",
                }
            };

            return patients;
        }


    }


    public class DoctorBuilder
    {
        private Doctor doctor;

        public DoctorBuilder()
        {
            doctor = new Doctor
            {
                FirstName = "Hossein",
                LastName = "Khani",
                NationalCode = "123456",
                Field =""
            };
        }

        public DoctorBuilder WithAppointment(DateTime date, string firstName,
            string lastName,string patientNationlCode)
        {
            doctor.Appointments.Add(new Appointment
            {
                Date = date,
                Patient = new Patient
                {
                    FirstName = firstName,
                    LastName = lastName,
                    NationalCode = patientNationlCode,
                }
            });

            return this;
        }

        public Doctor Build()
        {
            return doctor;
        }
    }

}
