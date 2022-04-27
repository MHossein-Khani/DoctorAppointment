using DoctorAppointment.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Service.Patients.Contracts
{
    public interface PatientService
    {
        void Create(CreatePatientDto dto);

        List<GetPatientDto> GetAll();

        void Update(UpdatePatientDto dto, int id);

        void Delete(int id);
    }
}
