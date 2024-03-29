﻿using Inz.DTOModel;
using Inz.Helpers;
using Inz.Model;
using Inz.OneOfHelper;
using Inz.Repository;
using OneOf;

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

            var callbackDoctor = await _doctorRepository.GetDoctorByIdAsync(calendarDTO.DoctorId);

            callbackDoctor.Switch(
                callbackDoctor =>
                {
                    if (callbackDoctor == null)
                    {
                        log = $"Doctor with id {calendarDTO.DoctorId} does not exist.";
                        _logger.LogInformation("{log}", log);
                        responseHandler = new NotFoundResponse(log);
                    }
                },
                dbException =>
                {
                    log = $"{LogHelper.DatabaseError}{dbException.Exception.Message}";
                    _logger.LogError("{log}", log);
                    responseHandler = dbException;
                });

            if (!callbackDoctor.IsT0)
            {
                return responseHandler;
            }

            var callbackDoctorsCalendar = await _calendarRepository.GetCalendarsByDoctorIdAsync(calendarDTO.DoctorId);

            if (callbackDoctorsCalendar.TryPickT0(out var calendars, out var dbException))
            {
                if (calendars.Any(x => x.Date.Date == calendarDTO.Date.Date
                            && calendarDTO.TimeBlockIds.Contains(x.TimeBlockId)))
                {
                    log = $"Cannot create new calendar block - the calendar already exist";
                    _logger.LogInformation("{log}", log);
                    return new NotFoundResponse(log);
                }


                var callbackGetStatus = await _statusRepository.GetStatusAsync(StatusEnum.Free);
                List<Calendar> calendarsList = new();

                callbackGetStatus.Switch(
                    status =>
                    {
                        if (status != null)
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
                        }
                        else
                        {
                            log = $"'Open' status does not exist -> missing statuses in database.";
                            _logger.LogInformation("{log}", log);
                            responseHandler = new NotFoundResponse(log);
                        }
                    },
                    dbException =>
                    {
                        log = $"{LogHelper.DatabaseError}{dbException.Exception.Message}";
                        _logger.LogError("{log}", log);
                        responseHandler = dbException;
                    }
                    );

                if (callbackGetStatus.IsT0 && calendarsList.Any())
                {

                    var callbackInsertCalendar = await _calendarRepository.InsertCalendarAsync(calendarsList);

                    callbackInsertCalendar.Switch(
                        okResponse => {
                            log = $"New calendar(s) have been created";
                            _logger.LogInformation("{log}", log);
                            responseHandler = new OkResponse(log);
                        },
                        dbError => {
                            log = $"{LogHelper.DatabaseError}{dbError.Exception.Message}";
                            _logger.LogError("{log}", log);
                            responseHandler = dbError;
                        });
                }
                return responseHandler;
            }
            return responseHandler;
        }

        public async Task<OneOf<Calendar, NotFoundResponse, DatabaseExceptionResponse>> GetCalendarByIdAsync(int id)
        {
            OneOf<Calendar, NotFoundResponse, DatabaseExceptionResponse> responseHandler = new();
            string log;

            var callbackGetCalendar = await _calendarRepository.GetCalendarAsync(id);

            callbackGetCalendar.Switch
                (okResponse =>
                    {
                        if (okResponse != null)
                        {
                            responseHandler = okResponse;
                        }

                        log = $"Calendar with Id: {id} does not exist.";
                        _logger.LogInformation("{log}", log);
                        responseHandler = new NotFoundResponse(log);
                    },
                databaseException =>
                {
                    log = $"{LogHelper.DatabaseError}{databaseException.Exception.Message}";
                    _logger.LogError("{log}", log);
                    responseHandler = databaseException;
                });

            return responseHandler;
        }

        public async Task<OneOf<List<Calendar>, NotFoundResponse, DatabaseExceptionResponse>> GetCalendarListByDateRangeAsync(CalendarTimeframeDTO calendarTimeframeDTO)
        {
            string log;

            //if (startDate.Date > endDate.Date)
            //{
            //    log = "Start date cannot be before end date.";    
            //    _logger.LogInformation("{log}", log);
            //    return new NotFoundResponse(log);
            //}

            DateTime startDate = DateTime.ParseExact(calendarTimeframeDTO.StartDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(calendarTimeframeDTO.EndDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);



            var callbackGetCalendarList = await _calendarRepository.GetCalendarListByDateRangeAsync(startDate, endDate);

            if(callbackGetCalendarList.TryPickT0(out var calendars, out var databaseException))
            {
                if (calendars.Count > 0)
                {
                    log = $"There are ${calendars.Count} calendars between {startDate.Date} and {endDate.Date}";
                    _logger.LogInformation("{log}", log);
                }

                return calendars;
            }
            else
            {
                log = $"{LogHelper.DatabaseError}{databaseException.Exception.Message}";
                _logger.LogError("{log}", log);
                return databaseException;
            }
        }
    }
}
