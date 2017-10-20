using System.Collections.Generic;

namespace DBA.FreshdeskSharp.Models
{
    public abstract class FreshdeskSearchResults<T>
    {
        public FreshdeskSearchResults()
        {
            Results = new List<T>();
        }

        public List<T> Results { get; }
        public int Total { get; set; }
    }
}