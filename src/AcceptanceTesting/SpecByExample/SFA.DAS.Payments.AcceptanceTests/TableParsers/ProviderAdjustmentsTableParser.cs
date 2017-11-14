using System.Collections.Generic;

namespace SFA.DAS.Payments.AcceptanceTests.TableParsers
{
    class ProviderAdjustmentsTableParser
    {
        private Dictionary<string, string> SpecToDatabaseLookup = new Dictionary<string, string>
        {
            { "", "Audit Adjustments: 16-18 Levy Apprenticeships - Employer" },
                { "", "Audit Adjustments: 16-18 Levy Apprenticeships - Provider" },
                { "", "Audit Adjustments: 16-18 Levy Apprenticeships - Training" },
                { "", "Audit Adjustments: 16-18 Non-Levy Apprenticeships - Employer" },
                { "", "Audit Adjustments: 16-18 Non-Levy Apprenticeships - Provider" },
                { "", "Audit Adjustments: 16-18 Non-Levy Apprenticeships - Training" },
                { "", "Audit Adjustments: Adult Levy Apprenticeships - Employer" },
                { "", "Audit Adjustments: Adult Levy Apprenticeships - Provider" },
                { "", "Audit Adjustments: Adult Levy Apprenticeships - Training" },
                { "", "Audit Adjustments: Adult Non-Levy Apprenticeships - Employer" },
                { "", "Audit Adjustments: Adult Non-Levy Apprenticeships - Provider" },
                { "", "Audit Adjustments: Adult Non-Levy Apprenticeships - Training" },
                { "", "Authorised Claims: 16-18 Levy Apprenticeships - Employer" },
                { "", "Authorised Claims: 16-18 Levy Apprenticeships - Provider" },
                { "", "Authorised Claims: 16-18 Levy Apprenticeships - Training" },
                { "", "Authorised Claims: 16-18 Non-Levy Apprenticeships - Employer" },
                { "", "Authorised Claims: 16-18 Non-Levy Apprenticeships - Provider" },
                { "", "Authorised Claims: 16-18 Non-Levy Apprenticeships - Training" },
                { "", "Authorised Claims: Adult Levy Apprenticeships - Employer" },
                { "", "Authorised Claims: Adult Levy Apprenticeships - Provider" },
                { "", "Authorised Claims: Adult Levy Apprenticeships - Training" },
                { "", "Authorised Claims: Adult Non-Levy Apprenticeships - Employer" },
                { "", "Authorised Claims: Adult Non-Levy Apprenticeships - Provider" },
                { "", "Authorised Claims: Adult Non-Levy Apprenticeships - Training" },
                { "", "Excess Learning Support: 16-18 Levy Apprenticeships - Provider" },
                { "", "Excess Learning Support: 16-18 Non-Levy Apprenticeships - Provider" },
                { "", "Excess Learning Support: Adult Levy Apprenticeships - Provider" },
                { "", "Excess Learning Support: Adult Non-Levy Apprenticeships - Provider" },
                { "", "Exceptional Learning Support: 16-18 Levy Apprenticeships - Provider" },
                { "", "Exceptional Learning Support: 16-18 Non-Levy Apprenticeships - Provider" },
                { "", "Exceptional Learning Support: Adult Levy Apprenticeships - Provider" },
                { "", "Exceptional Learning Support: Adult Non-Levy Apprenticeships - Provider" },
        };
    }
}
