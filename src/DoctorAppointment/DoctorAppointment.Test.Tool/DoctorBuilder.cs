using DoctorAppointment.Entities;
using System;

namespace DoctorAppointment.Test.Tool
{
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
                Field = ""
            };
        }

        public DoctorBuilder WithAppointment(DateTime date, string firstName,
            string lastName, string patientNationlCode)
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
