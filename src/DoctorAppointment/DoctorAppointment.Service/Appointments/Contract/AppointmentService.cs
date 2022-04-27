using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Service.Appointments.Contract
{
    public interface AppointmentService
    {
        void MakeAppointment(CreateAppointmentDto dto);

        void Update(UpdateAppointmentDto dto, int id);

        void Delete(int id);

        List<GetAppointmentDto> GetAll();
    }
}
