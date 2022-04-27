using DoctorAppointment.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Service.Appointments.Contract
{
    public interface AppointmentRipository
    {
        int Count(int doctorId, DateTime dateTime);

        void MakeAppointment(Appointment appointment);
    }
}
