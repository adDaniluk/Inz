using Azure;
using Inz.DTOModel;
using Inz.Model;
using Inz.OneOfHelper;
using Inz.Repository;
using OneOf;
using OneOf.Types;

namespace Inz.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly ICalendarRepository _calendarRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly ILogger _logger;

        public CalendarService(ICalendarRepository calendarRepository,
            IDoctorRepository doctorRepository,
            IStatusRepository statusRepository,
            ILogger<ICalendarRepository> logger)
        {
            _calendarRepository = calendarRepository;
            _doctorRepository = doctorRepository;
            _statusRepository = statusRepository;
            _logger = logger;

        }

        public async Task<OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse>> InsertCalendarAsync(CalendarDTO calendarDTO)
        {
            string log;
            OneOf<OkResponse, NotFoundResponse, DatabaseExceptionResponse> responseHandler = new ();

            var callbackDoctor = await _doctorRepository.GetDoctorAsync(calendarDTO.DoctorId);

            callbackDoctor.Switch(
                callbackDoctor => { },
                notFound =>
                {
                    log = $"Doctor with id {calendarDTO.DoctorId} does not exist.";
                    _logger.LogInformation(log);
                    responseHandler = new NotFoundResponse(log);
                },
                dbException =>
                {
                    log = $"Error on a database, see inner exception: {dbException.Exception.Message}";
                    _logger.LogError(message: log);
                    responseHandler = dbException;
                });

            if (!callbackDoctor.IsT0)
            {
                return responseHandler;
            }

            var callbackDoctorsCalendar = await _calendarRepository.GetCalendarsByDoctorId(calendarDTO.DoctorId);

            if (callbackDoctorsCalendar.TryPickT0(out var calendars, out var dbException))
            {
                if (calendars.Any(x => x.Date.Date == calendarDTO.Date.Date
                            && calendarDTO.TimeBlockIds.Contains(x.TimeBlockId)))
                {
                    log = $"Cannot create new calendar block - the calendar already exist";
                    _logger.LogInformation(message: log);
                    return new NotFoundResponse(log);
                }


                var callbackGetStatus = await _statusRepository.GetStatus(StatusEnum.Open);
                List<Calendar> calendarsList = new List<Calendar>();

                callbackGetStatus.Switch(
                    status =>
                    {
                        calendarsList = calendarDTO.TimeBlockIds.Select(id => new Calendar
                        {
                            Date = calendarDTO.Date,
                            DoctorId = calendarDTO.DoctorId,
                            TimeBlockId = id,
                            Timestamp = DateTime.Now,
                            AlterTimestamp = DateTime.Now,
                            StatusId = status.Id
                        }).ToList();
                    },
                    notFound =>
                    {
                        log = $"'Open' status does not exist -> missing statues in database.";
                        _logger.LogInformation(message: log);
                        responseHandler = new NotFoundResponse(log);
                    },
                    dbException =>
                    {
                        log = $"Error on a database, see inner exception: {dbException.Exception.Message}";
                        _logger.LogError(message: log);
                        responseHandler = dbException;
                    }
                    );

                if (callbackGetStatus.IsT0 && calendarsList.Any())
                {

                    var callbackInsertCalendar = await _calendarRepository.InsertCalendar(calendarsList);

                    callbackInsertCalendar.Switch(
                        okResponse => {
                            log = $"New calendar(s) have been created";
                            _logger.LogInformation(log);
                            responseHandler = new OkResponse(log);
                        },
                        dbError => {
                            log = $"Error on a database, see inner exception: {dbError.Exception.Message}";
                            _logger.LogError(message: log);
                            responseHandler = dbError;
                        });
                }
                return responseHandler;
            }
            return responseHandler;
        }

        public async Task<OneOf<Calendar, NotFoundResponse, DatabaseExceptionResponse>> GetCalendarByIdAsync(int id)
        {
            OneOf<Calendar, NotFoundResponse, DatabaseExceptionResponse> responseHandler = new ();
            string log;

            var callbackGetCalendar = await _calendarRepository.GetCalendar(id);

            callbackGetCalendar.Switch
                (okResponse => responseHandler = okResponse,
                notFound => {
                    log = $"Calendar with Id:{id} does not exist.";
                    _logger.LogInformation(log);
                    responseHandler = new NotFoundResponse(log);
                },
                databaseException => {
                    log = $"Error on a database, see inner exception: {databaseException.Exception.Message}";
                    _logger.LogError(message: log);
                    responseHandler = databaseException;
                });

            return responseHandler;
        }

        public async Task<OneOf<List<Calendar>, NotFoundResponse, DatabaseExceptionResponse>> GetCalendarListByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            string log;

            if (startDate.Date > endDate.Date)
            {
                log = "Start date cannot be before end date.";    
                _logger.LogInformation(message: log);
                return new NotFoundResponse(log);
            }

            var callbackGetCalendarList = await _calendarRepository.GetCalendarListByDateRangeAsync(startDate, endDate);

            if(callbackGetCalendarList.TryPickT0(out var calendars, out var databaseException))
            {
                if (calendars.Count > 0)
                {
                    log = $"There are ${calendars.Count} calendars between {startDate.Date} and {endDate.Date}";
                    _logger.LogInformation(message: log);
                }

                return calendars;
            }
            else
            {
                log = $"Error on a database, see inner exception: {databaseException.Exception.Message}";
                _logger.LogError(message: log);
                return databaseException;
            }
        }
    }
}
