using Inz.Context;

namespace Inz.Repository
{
    public class DoctorVisitRepository : IDoctorVisitRepository
    {
        private readonly DbContextApi _contextApi;
        public DoctorVisitRepository(DbContextApi contextApi)
        {
            _contextApi = contextApi;
        }
    }
}
