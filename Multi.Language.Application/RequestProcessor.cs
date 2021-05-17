using System;
using System.Threading.Tasks;
using App.Core;
using MediatR;

namespace Multi.Language.Application
{
    public sealed class RequestProcessor
    {
        private readonly IMediator _mediator;
        public HttpResult HttpResult;

        public RequestProcessor(IMediator mediator)
        {
            _mediator = mediator;
            HttpResult = new HttpResult();
        }
        public async Task<T> Execute<T>(IRequest<T> request)
        {
            try
            {
                var result = await _mediator.Send(request);

                HttpResult.Successful();

                return result;
            }
            catch (DomainException domainException)
            {
                HttpResult.Error(domainException.Message);
            }
            catch (Exception e)
            {
                HttpResult.Error("სისტემური შეცდომა. " + e.Message);
            }

            return default(T);
        }
    }
}
