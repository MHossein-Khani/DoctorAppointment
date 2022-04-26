using DoctorAppointment.Entities;
using System;

namespace DoctorAppointment.Test.Tool
{
    public static class DoctorFactory
    {
        public static Doctor CreateDoctor()
        {
            return new Doctor
            {
                FirstName = "Hossien",
                LastName = "Khani",
                NationalCode = "2281892125",
                Field = "Phsicology"
            };
        }
    }
}
