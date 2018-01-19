using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace StudentServisWebScraper.Api.Scraping
{

    [Serializable]
    public class ScrapingException : Exception
    {
        public ScrapingException() { }
        public ScrapingException(string message) : base(message) { }
        public ScrapingException(string message, Exception inner) : base(message, inner) { }
        protected ScrapingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
