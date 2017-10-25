using System;

namespace IlrGeneratorApp.Services
{
    public class ProviderSelectedEventArgs : EventArgs
    {
        public long Ukprn { get; set; }
    }
}
