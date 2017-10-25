using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SFA.DAS.Payments.Automation.WebUI.ViewModels
{
    public class IlrSubmissionModel
    {
        [Required]
        public long Ukprn { get; set; }

        [Required]
        public string AcademicYear { get; set; }

        public int? ShiftMonth { get; set; }

        public int? ShiftYear { get; set; }

        [Required]
        public string Specs { get; set; }

    }
}