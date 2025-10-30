using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocontrol_PPI_NET_9.Models.Mail
{
    public class EmailTemplate
    {
        public string Subject { get; set; }
        public string Title { get; set; }
        public string Greeting { get; set; }
        public string MainContent { get; set; }
        public string HighlightedContent { get; set; }
        public string Instructions { get; set; }
        public string FooterNotice { get; set; }
        public string CompanyName { get; set; }
        public string HighlightCode { get; set; }
        public string ExpirationDate { get; set; }
        public string Remitter { get; set; }
        public string Note { get; set; }
    }

}
