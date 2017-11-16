using System;
using System.Collections.Generic;

namespace SFA.DAS.Payments.AcceptanceTests.TableParsers
{
    class ProviderAdjustmentsTableParser
    {
        public static string PaymentNameFromSpec(string specValue)
        {
            if (!SpecToDatabaseLookup.ContainsKey(specValue))
            {
                throw new ArgumentOutOfRangeException($"Could not find match for payment type: {specValue}");
            }
            return SpecToDatabaseLookup[specValue];
        }

        private static readonly Dictionary<string, string> SpecToDatabaseLookup = new Dictionary<string, string>
            (StringComparer.OrdinalIgnoreCase)
        {
            { "16-18 Levy Employer Audit Adjustments", "Audit Adjustments: 16-18 Levy Apprenticeships - Employer" },
            { "16-18 Levy Provider Audit Adjustments", "Audit Adjustments: 16-18 Levy Apprenticeships - Provider" },
            { "16-18 Levy Training Audit Adjustments", "Audit Adjustments: 16-18 Levy Apprenticeships - Training" },

            { "16-18 Non-Levy Employer Audit Adjustments", "Audit Adjustments: 16-18 Non-Levy Apprenticeships - Employer" },
            { "16-18 Non-Levy Provider Audit Adjustments", "Audit Adjustments: 16-18 Non-Levy Apprenticeships - Provider" },
            { "16-18 Non-Levy Training Audit Adjustments", "Audit Adjustments: 16-18 Non-Levy Apprenticeships - Training" },

            { "Adult Levy Employer Audit Adjustments", "Audit Adjustments: Adult Levy Apprenticeships - Employer" },
            { "Adult Levy Provider Audit Adjustments", "Audit Adjustments: Adult Levy Apprenticeships - Provider" },
            { "Adult Levy Training Audit Adjustments", "Audit Adjustments: Adult Levy Apprenticeships - Training" },

            { "Adult Non-Levy Employer Audit Adjustments", "Audit Adjustments: Adult Non-Levy Apprenticeships - Employer" },
            { "Adult Non-Levy Provider Audit Adjustments", "Audit Adjustments: Adult Non-Levy Apprenticeships - Provider" },
            { "Adult Non-Levy Training Audit Adjustments", "Audit Adjustments: Adult Non-Levy Apprenticeships - Training" },

            { "16-18 Levy Employer Authorised Claims", "Authorised Claims: 16-18 Levy Apprenticeships - Employer" },
            { "16-18 Levy Provider Authorised Claims", "Authorised Claims: 16-18 Levy Apprenticeships - Provider" },
            { "16-18 Levy Training Authorised Claims", "Authorised Claims: 16-18 Levy Apprenticeships - Training" },

            { "16-18 Non-Levy Employer Authorised Claims", "Authorised Claims: 16-18 Non-Levy Apprenticeships - Employer" },
            { "16-18 Non Levy Provider Authorised Claims", "Authorised Claims: 16-18 Non-Levy Apprenticeships - Provider" },
            { "16-18 Non Levy Training Authorised Claims", "Authorised Claims: 16-18 Non-Levy Apprenticeships - Training" },

            { "Adult Levy Employer Authorised Claims", "Authorised Claims: Adult Levy Apprenticeships - Employer" },
            { "Adult Levy Provider Authorised Claims", "Authorised Claims: Adult Levy Apprenticeships - Provider" },
            { "Adult Levy Training Authorised Claims", "Authorised Claims: Adult Levy Apprenticeships - Training" },

            { "Adult Non-Levy Employer Authorised Claims", "Authorised Claims: Adult Non-Levy Apprenticeships - Employer" },
            { "Adult Non-Levy Provider Authorised Claims", "Authorised Claims: Adult Non-Levy Apprenticeships - Provider" },
            { "Adult Non-Levy Training Authorised Claims", "Authorised Claims: Adult Non-Levy Apprenticeships - Training" },

            { "16-18 Levy Excess Learning Support", "Excess Learning Support: 16-18 Levy Apprenticeships - Provider" },
            { "16-18 Non-Levey Excess Learning Support", "Excess Learning Support: 16-18 Non-Levy Apprenticeships - Provider" },
            { "Adult Levy Excess Learning Support", "Excess Learning Support: Adult Levy Apprenticeships - Provider" },
            { "Adult Non-Levy Excess Learning Support", "Excess Learning Support: Adult Non-Levy Apprenticeships - Provider" },
            { "16-18 Levy Exceptional Learning Support", "Exceptional Learning Support: 16-18 Levy Apprenticeships - Provider" },
            { "16-18 Non-Levy Exceptional Learning Support", "Exceptional Learning Support: 16-18 Non-Levy Apprenticeships - Provider" },
            { "Adult Levy Exceptional Learning Support", "Exceptional Learning Support: Adult Levy Apprenticeships - Provider" },
            { "Adult Non-Levy Exceptional Learning Support", "Exceptional Learning Support: Adult Non-Levy Apprenticeships - Provider" },
        };
    }
}
