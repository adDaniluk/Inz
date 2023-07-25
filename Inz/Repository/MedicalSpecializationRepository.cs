using Inz.Context;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Inz.Repository
{
    public class MedicalSpecializationRepository : IMedicalSpecializationRepository
    {
        private DbContextApi _dbContextApi;
        public MedicalSpecializationRepository(DbContextApi dbContextApi)
        {
            _dbContextApi = dbContextApi;
        }

        public async Task<OneOf<IList<MedicalSpecialization>, NotFoundResponse, DatabaseExceptionResponse>> GetMedicalSpecializationAsync(List<int> specializationsIds)
        {
            try
            {
                var medicalSpecializationsList = await _dbContextApi.MedicalSpecializations.Where(x => specializationsIds.Contains(x.Id)).ToListAsync();

                if (medicalSpecializationsList.Any())
                {
                    return medicalSpecializationsList;
                }

                return new NotFoundResponse();
            }
            catch (Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }
    }
}
