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

        List<GetDoctorDto> GetAll()
    }

    public class GetDoctorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string Field { get; set; }
    }
}
