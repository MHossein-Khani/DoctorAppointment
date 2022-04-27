using DoctorAppointment.Service.Appointments.Contract;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.RestAPI.Controller
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentService _appoinmentService;

        public AppointmentsController(AppointmentService appoinmentService)
        {
            _appoinmentService = appoinmentService;
        }

        [HttpPost]
        public void MakeAppointment(CreateAppointmentDto dto)
        {
            _appoinmentService.MakeAppointment(dto);
        }
    }
}
