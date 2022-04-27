using DoctorAppointment.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Service.Patients.Contracts
{
    public interface PatientRipository
    {
        bool IsExistNationalCode(string nationalCode);

        void Create(Patient patient);

        List<GetPatientDto> GetAll();

        Patient FindById(int id);

        void Delete(int id);

    }
}
