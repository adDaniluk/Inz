using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Inz.Repository;
using OneOf;
using System.Globalization;

namespace Inz.Services
{
    public class DoctorVisitService : IDoctorVisitService
    {
        private readonly IDoctorVisitRepository _doctorVisitRepository;
        private readonly ICalendarRepository _calendarRepository;
        private readonly IDoctorServiceRepository _doctorServiceRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly ILogger _logger;

        public DoctorVisitService(IDoctorVisitRepository doctorVisitRepository,
            ICalendarRepository calendarRepository,
            IDoctorServiceRepository doctorServiceRepository,
            IStatusRepository statusRepository,
            ILogger<IDoctorVisitService> logger)
        {
            _doctorVisitRepository = doctorVisitRepository;
            _calendarRepository = calendarRepository;
            _doctorServiceRepository = doctorServiceRepository;
            _statusRepository = statusRepository;
            _logger = logger;
        }

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> BookDoctorVisitAsync(DoctorVisitDTO doctorVisitDTO)
        {
            string log;

            int calendarId = doctorVisitDTO.CalendarId;
            int doctorServiceId = doctorVisitDTO.DoctorServiceId;
            int patientId = doctorVisitDTO.PatientId;

            var callbackCalendar = await _calendarRepository.GetCalendarAsync(calendarId);


            if (callbackCalendar.TryPickT1(out var dbErrorCalendar, out var calendarToUpdate))
            {
                log = $"Error on a database, see inner exception: {dbErrorCalendar.Exception.Message}";
                _logger.LogError(message: log);
                return dbErrorCalendar;
            }

            if(calendarToUpdate == null)
            {
                log = $"Calendar with {doctorVisitDTO.CalendarId} does not exist.";
                _logger.LogError(message: log);
                return new NotFoundResponse(log);
            }

            var callbackDoctorService = await _doctorServiceRepository.GetDoctorServiceAsync(calendarToUpdate.DoctorId, doctorServiceId);
            
            if(callbackDoctorService.TryPickT1(out var dbErrorDoctorService, out var doctorService))
            {
                log = $"Error on a database, see inner exception: {dbErrorDoctorService.Exception.Message}";
                _logger.LogError(message: log);
                return dbErrorDoctorService;
            }

            if (doctorService == null)
            {
                log = $"Service does not exist for picked doctor.";
                _logger.LogError(message: log);
                return new NotFoundResponse(log);
            }

            var callbackStatus = await _statusRepository.GetStatusAsync(StatusEnum.Reserved);

            if(callbackStatus.TryPickT1(out var dbErrorStatusCheck, out var status))
            {
                log = $"Error on a database, see inner exception: {dbErrorStatusCheck.Exception.Message}";
                _logger.LogError(message: log);
                return dbErrorStatusCheck;
            }

            if(status == null)
            {
                log = $"Status does not exist.";
                _logger.LogError(message: log);
                return new NotFoundResponse(log);
            }

            DoctorVisit doctorVisit = new()
            {
                Calendar = calendarToUpdate
            };

            calendarToUpdate.AlterTimestamp = DateTime.Now;
            calendarToUpdate.PatientId = patientId;
            calendarToUpdate.ServicePrice = doctorService.Price;
            calendarToUpdate.Status = status;

            var callbackCreateDoctorVisit = await _doctorVisitRepository.CreateDoctorVisitAsync(doctorVisit);

            if(callbackCreateDoctorVisit.IsT1)
            {
                log = $"Error on a database, see inner exception: {callbackCreateDoctorVisit.AsT1.Exception.Message}";
                _logger.LogError(message: log);
                return callbackCreateDoctorVisit.AsT1;
            }

            var callbackCalendarUpdate = await _calendarRepository.UpdateCalendarAsync(calendarToUpdate);

            if(callbackCalendarUpdate.TryPickT1(out var dbErrorCalendarUpdate, out var okResponse))
            {
                log = $"Error on a database, see inner exception: {dbErrorCalendarUpdate.Exception.Message}";
                _logger.LogError(message: log);
                return dbErrorCalendarUpdate;
            }

            log = "New visit has been booked.";
            _logger.LogInformation(message: log);
            okResponse.ResponseMessage = log;
            return okResponse;
        }

        public async Task<OneOf<DoctorVisit, DatabaseExceptionResponse>> GetDoctorVisitByIdAsync(int visitId)
        {
            throw new NotImplementedException();
        }

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> RemoveDoctorVisitAsync(int id, int patientId)
        {
            throw new NotImplementedException();
        }
    }
}
