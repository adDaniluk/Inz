using Inz.DTOModel;
using Inz.Helpers;
using Inz.Model;
using Inz.OneOfHelper;
using Inz.Repository;
using OneOf;

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
                log = $"{LogHelper.DatabaseError}{dbErrorCalendar.Exception.Message}";
                _logger.LogError("{log}", log);
                return dbErrorCalendar;
            }

            if(calendarToUpdate == null)
            {
                log = $"Calendar with {doctorVisitDTO.CalendarId} does not exist.";
                _logger.LogError("{log}", log);
                return new NotFoundResponse(log);
            }

            var callbackStatusOpen = await _statusRepository.GetStatusAsync(StatusEnum.Free);

            if (callbackStatusOpen.TryPickT1(out var dbErrorStatusCheckOpen, out var statusOpen))
            {
                log = $"{LogHelper.DatabaseError}{dbErrorStatusCheckOpen.Exception.Message}";
                _logger.LogError("{log}", log);
                return dbErrorStatusCheckOpen;
            }

            if (statusOpen == null)
            {
                log = $"Status does not exist.";
                _logger.LogError("{log}", log);
                return new NotFoundResponse(log);
            }

            if(calendarToUpdate.Status != statusOpen)
            {
                log = $"The calendar is already booked.";
                _logger.LogInformation("{log}", log);
                return new NotFoundResponse(log);
            }

            var callbackDoctorService = await _doctorServiceRepository.GetDoctorServiceAsync(calendarToUpdate.DoctorId, doctorServiceId);
            
            if(callbackDoctorService.TryPickT1(out var dbErrorDoctorService, out var doctorService))
            {
                log = $"{LogHelper.DatabaseError}{dbErrorDoctorService.Exception.Message}";
                _logger.LogError("{log}", log);
                return dbErrorDoctorService;
            }

            if (doctorService == null)
            {
                log = $"Service does not exist for picked doctor.";
                _logger.LogError("{log}", log);
                return new NotFoundResponse(log);
            }

            var callbackStatusReserved = await _statusRepository.GetStatusAsync(StatusEnum.Reserved);

            if(callbackStatusReserved.TryPickT1(out var dbErrorStatusCheckReserved, out var statusReserved))
            {
                log = $"{LogHelper.DatabaseError}{dbErrorStatusCheckReserved.Exception.Message}";
                _logger.LogError("{log}", log);
                return dbErrorStatusCheckReserved;
            }

            if(statusReserved == null)
            {
                log = $"Status does not exist.";
                _logger.LogError("{log}", log);
                return new NotFoundResponse(log);
            }

            DoctorVisit doctorVisit = new()
            {
                Calendar = calendarToUpdate
            };

            calendarToUpdate.AlterTimestamp = DateTime.Now;
            calendarToUpdate.PatientId = patientId;
            calendarToUpdate.ServicePrice = doctorService.Price;
            calendarToUpdate.Status = statusReserved;

            var callbackCreateDoctorVisit = await _doctorVisitRepository.CreateDoctorVisitAsync(doctorVisit);

            if(callbackCreateDoctorVisit.IsT1)
            {
                log = $"{LogHelper.DatabaseError}{callbackCreateDoctorVisit.AsT1.Exception.Message}";
                _logger.LogError("{log}", log);
                return callbackCreateDoctorVisit.AsT1;
            }

            var callbackCalendarUpdate = await _calendarRepository.UpdateCalendarAsync(calendarToUpdate);

            if(callbackCalendarUpdate.TryPickT1(out var dbErrorCalendarUpdate, out var okResponse))
            {
                log = $"{LogHelper.DatabaseError}{dbErrorCalendarUpdate.Exception.Message}";
                _logger.LogError("{log}", log);
                return dbErrorCalendarUpdate;
            }

            log = "New visit has been booked.";
            _logger.LogInformation("{log}", log);
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
