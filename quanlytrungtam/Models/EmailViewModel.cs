using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace quanlytrungtam.Models
{
    public class EmailViewModel
    {
        [Required]
        public string EmailSubject { get; set; }

        public string EmailBody { get; set; }

        [Required]
        public string ReceiverEmailAddress { get; set; }
        public string SenderEmailAddress { get; set; }
        public string SenderPassword { get; set; }
        public string SmtpHostServer { get; set; }
        public int SmtpPort { get; set; }
    }
}