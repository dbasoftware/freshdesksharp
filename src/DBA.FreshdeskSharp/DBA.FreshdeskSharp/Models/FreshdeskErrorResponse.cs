using System.Collections.Generic;
using System.Net;

namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskErrorResponse
    {
        public string Description { get; set; }
        public List<FreshdeskError> Errors { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}