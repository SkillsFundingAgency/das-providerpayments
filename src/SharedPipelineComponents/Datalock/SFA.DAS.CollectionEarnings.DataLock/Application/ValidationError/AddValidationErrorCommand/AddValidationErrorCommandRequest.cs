using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorCommand
{
    public class AddValidationErrorCommandRequest : IRequest
    {
        public ValidationError ValidationError { get; set; } 
    }
}