using DoctorAppointment.Service.Patients.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DoctorAppointment.RestAPI.Controller
{
    [ApiController]
    [Route("api/patients")]
    public class PatientsController : ControllerBase
    {
        private readonly PatientService _patientService;

        public PatientsController(PatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost]
        public void Add(CreatePatientDto dto)
        {
            _patientService.Create(dto);
        }

        [HttpGet]
        public List<GetPatientDto> GetAll()
        {
            return _patientService.GetAll();
        }

        [HttpPut("{id}")]
        public void Update(UpdatePatientDto dto, [FromRoute] int id)
        {
            _patientService.Update(dto, id);
        }

        [HttpDelete("{id}")]
        public void Delete([FromRoute] int id)
        {
            _patientService.Delete(id);
        }
    }
}
