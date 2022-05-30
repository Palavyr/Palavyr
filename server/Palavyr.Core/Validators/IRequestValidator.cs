using System.Threading.Tasks;
using MediatR;

namespace Palavyr.Core.Validators
{
    public interface IRequestValidator<TEvent, TR> where TEvent : IRequest<TR>
    {
        Task Validate(TEvent request);
    }
}