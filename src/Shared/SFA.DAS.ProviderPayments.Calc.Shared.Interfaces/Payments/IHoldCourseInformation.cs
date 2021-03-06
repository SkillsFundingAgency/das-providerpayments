﻿using SFA.DAS.Payments.DCFS.Domain;
namespace SFA.DAS.ProviderPayments.Calc.Shared.Interfaces.Payments
{
    public interface IHoldCourseInformation
    {
        int StandardCode { get; }
        int ProgrammeType { get; }
        int PathwayCode { get; }
        int FrameworkCode { get; }
        ApprenticeshipContractType ApprenticeshipContractType { get; }
    }
}
