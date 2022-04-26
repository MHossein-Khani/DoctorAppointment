using DoctorAppointment.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Service.Doctors.Contracts
{
    public interface DoctorRipository 
    {
        bool IsExistNAtionalCode(string nationalCode);

        void Create(Doctor doctor);

        List<GetDoctorDto> GetAll();
    }

}

