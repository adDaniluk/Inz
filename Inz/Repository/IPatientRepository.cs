using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using OneOf;
using OneOf.Types;

namespace Inz.Repository
{
    public interface IPatientRepository
    {
        public Task InsertPatientAsync(Patient patient);
        public Task<OneOf<Patient, DatabaseException>> SaveChangesAsync();
        public Task<OneOf<Patient, NotFound, DatabaseException>> ValidateAndUpdatePatientAsyc(UpdatePatientDTO updatePatientDTO);
    }
}
