﻿using System.Threading.Tasks;

namespace SFA.DAS.ProviderPayments.Application.Validation
{
    public interface IValidator<T>
    {
        Task<ValidationResult> ValidateAsync(T item);
    }
}
