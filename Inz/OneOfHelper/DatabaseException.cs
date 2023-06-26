using Inz.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using System.Data.Common;

namespace Inz.OneOfHelper
{
    public class DatabaseException
    {
        public string _message;

        public DatabaseException(Exception exception) {
            OneOfException(exception);
            _message = exception.Message;
        }

        private static OneOf<DbUpdateException, DbUpdateConcurrencyException, NotSupportedException, InvalidOperationException> OneOfException(Exception exception)
        {
            if (exception is DbUpdateException)
            {
                return new DbUpdateException();
            }
            else if (exception is DbUpdateConcurrencyException)
            {
                return new DbUpdateConcurrencyException();
            }
            else if (exception is NotSupportedException)
            {
                return new NotSupportedException();
            }
            else
            {
                return new InvalidOperationException();
            }
        }
    }
}
