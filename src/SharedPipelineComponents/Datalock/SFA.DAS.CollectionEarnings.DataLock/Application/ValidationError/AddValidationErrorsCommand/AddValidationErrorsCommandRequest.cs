using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand
{
    public class AddValidationErrorsCommandRequest : IRequest
    {
        public ValidationError[] ValidationErrors { get; set; }
    }
}