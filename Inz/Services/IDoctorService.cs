using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using OneOf;
using OneOf.Types;

namespace Inz.Services
{
    public interface IDoctorService
    {
        public Task<OneOf<Doctor, DatabaseException>> InsertDoctorAsync(DoctorDTO doctorDTO);

        public Task<OneOf<Doctor, NotFound, DatabaseException>> UpdateDoctorAsync(UpdateDoctorDTO updateDoctorDTO);
    }
}
