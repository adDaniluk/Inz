using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.EntityFrameworkCore.Update.Internal;
using OneOf;
using OneOf.Types;

namespace Inz.Repository
{
    public interface IDoctorRepository
    {
        public Task InsertDoctorAsync(Doctor doctor);
        public Task<OneOf<Doctor, DatabaseException>> SaveChangesAsync();
        public Task<OneOf<Doctor, NotFound, DatabaseException>> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO);
    }
}
