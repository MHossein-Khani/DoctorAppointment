using DoctorAppointment.Service.Doctors.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DoctorAppointment.RestAPI.Controller
{
    [ApiController]
    [Route("api/doctors")]
    public class DoctorsController : ControllerBase
    {
        private readonly DoctorService _service;

        public DoctorsController(DoctorService service)
        {
            _service = service;
        }

        public void AddDoctor(CreateDoctorDto dto)
        {
            _service.Create(dto);
        }

        [HttpGet]
        public List<GetDoctorDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpPut("{id}")]
        public void Update(UpdateDoctorDto dto, [FromRoute] int id)
        {
            _service.Update(dto, id);
        }

        [HttpDelete("{id}")]
        public void Delete([FromRoute] int id)
        {
            _service.Delete(id);
        }
    }
}

