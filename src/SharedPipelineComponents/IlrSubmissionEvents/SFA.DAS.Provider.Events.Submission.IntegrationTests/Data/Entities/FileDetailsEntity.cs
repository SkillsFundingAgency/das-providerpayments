﻿using System;

namespace SFA.DAS.Provider.Events.Submission.IntegrationTests.Data.Entities
{
    public class FileDetailsEntity
    {
        public long Ukprn { get; set; }
        public string FileName { get; set; }
        public DateTime SubmittedTime { get; set; }
    }
}