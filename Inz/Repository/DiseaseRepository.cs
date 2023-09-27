using Inz.Context;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Inz.Repository
{
    public class DiseaseRepository : IDiseaseRepository
    {
        private readonly DbContextApi _contextApi;

        public DiseaseRepository(DbContextApi dbContextApi)
        {
            _contextApi = dbContextApi;
        }

        public async Task<OneOf<IList<Disease>, DatabaseExceptionResponse>> GetDiseaseAsync(List<int> ids)
        {
            try
            {
                List<Disease> diseasesList = await _contextApi.Diseases.Where(x => ids.Contains(x.Id)).ToListAsync();
                return diseasesList;
            }
            catch(Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }
    }
}
