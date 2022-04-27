using DoctorAppointment.Service.Appointments.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DoctorAppointment.RestAPI.Controller
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentService _service;

        public AppointmentsController(AppointmentService appoinmentService)
        {
            _service = appoinmentService;
        }

        [HttpPost]
        public void MakeAppointment(CreateAppointmentDto dto)
        {
            _service.MakeAppointment(dto);
        }

        [HttpGet]
        public List<GetAppointmentDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpPut("id")]
        public void Update(UpdateAppointmentDto dto, [FromRoute] int id)
        {
            _service.Update(dto, id);
        }

        [HttpDelete("id")]
        public void Delete([FromRoute]int id)
        {
            _service.Delete(id);
        }


    }
}
