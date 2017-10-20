using System;
using System.Collections.Generic;
using System.Net;
using DBA.FreshdeskSharp.Models;

namespace DBA.FreshdeskSharp.Exceptions
{
    public class FreshdeskException : Exception
    {
        public FreshdeskException(FreshdeskErrorResponse errorResponse) : base(errorResponse.Description)
        {
            Errors = errorResponse.Errors;
            StatusCode = errorResponse.StatusCode;
        }

        public HttpStatusCode StatusCode { get; set; }
        public List<FreshdeskError> Errors { get; set; }
    }
}