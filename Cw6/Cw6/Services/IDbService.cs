using Cw6.Models;
using Cw6.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cw6.Models
{
    public interface IDbService
    {
        Task<IEnumerable<DoctorGetter>> GetDoctors();
        Task<bool> AddDoctor(Doctor doctorToAdd);
        Task<int> RemoveDoctor(int idDoctor);
        Task<bool> ModifyDoctor(DoctorGetter doctorToModify, int idDoctor);
        Task<IEnumerable<ReceptGetter>> GetPrescriptions(int idPresciption);



    }
}
