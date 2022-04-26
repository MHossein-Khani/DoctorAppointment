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
        bool IsExistNationalCode(string nationalCode);

        bool IsRepeatNationalCode(string NationalCode, int id);

        void Create(Doctor doctor);

        List<GetDoctorDto> GetAll();

        Doctor FindById(int id);

        void Delete(int id);
    }

}

