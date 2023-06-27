using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using OneOf;

namespace Inz.Services
{
    public interface IDoctorService
    {
        public Task<OneOf<Doctor, DatabaseException>> InsertNewDoctorAsync(DoctorDTO doctorDTO);
    }
}
