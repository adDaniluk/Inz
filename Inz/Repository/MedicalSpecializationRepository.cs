﻿using Inz.Context;
using Inz.Model;
using Inz.OneOfHelper;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Inz.Repository
{
    public class MedicalSpecializationRepository : IMedicalSpecializationRepository
    {
        private readonly DbContextApi _dbContextApi;
        public MedicalSpecializationRepository(DbContextApi dbContextApi)
        {
            _dbContextApi = dbContextApi;
        }

        public async Task<OneOf<IList<MedicalSpecialization>, DatabaseExceptionResponse>> GetMedicalSpecializationAsync(List<int> specializationsIds)
        {
            try
            {
                var medicalSpecializationsList = await _dbContextApi.MedicalSpecializations.Where(x => specializationsIds.Contains(x.Id)).ToListAsync();

                return medicalSpecializationsList;
            }
            catch (Exception exception)
            {
                return new DatabaseExceptionResponse(exception);
            }
        }
    }
}
