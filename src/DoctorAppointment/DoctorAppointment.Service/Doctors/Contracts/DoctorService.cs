using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Service.Doctors.Contracts
{
    public interface DoctorService
    {
        void Create(CreateDoctorDto dto);

        List<GetDoctorDto> GetAll();

        void Update(UpdateDoctorDto dto, int id);

        void Delete(int id);
    }

}
