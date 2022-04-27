using DoctorAppointment.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Test.Tool
{
    public static class PatientFactory
    {
        public static Patient CreatePatient()
        {
            return new Patient()
            {
                FirstName = "Hossien",
                LastName = "Khani",
                NationalCode = "2281892125",
            };
        }
    }
}
