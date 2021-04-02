using System;
using System.Threading.Tasks;
using App.Core;
using Multi.Language.Domain.SeedWork;

namespace Multi.Language.Application.Queries
{
    public sealed class QueryProcessor
    {
        private readonly IUnitOfWork _unitOfWork;

        public QueryProcessor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<T> Execute<T>(QueryBase<T> query)
        {
            try
            {
                query.HttpResult.Successful();

                query.Initialize(_unitOfWork);
                var result = await query.Execute();
                return result;
            }
            catch (DomainException domainException)
            {
                if (domainException.Level == ExceptionLevel.Error || domainException.Level == ExceptionLevel.Fatal || domainException.Level == ExceptionLevel.Warning)
                {
                    _unitOfWork.DetachChanges();
                    //await _unitOfWork.ExceptionLogRepository.AddAsync(new ExceptionLog(domainException.Message, "Ip", JsonService.Serialize(domainException)));
                    //await _unitOfWork.CompleteAsync();
                }

                query.HttpResult.Error(domainException.Message);
                return default(T);
            }
            catch (Exception ex)
            {
                _unitOfWork.DetachChanges();
                //await _unitOfWork.ExceptionLogRepository.AddAsync(new ExceptionLog(ex.Message, "Ip", JsonService.Serialize(ex)));
                //await _unitOfWork.CompleteAsync();

                query.HttpResult.Error("სისტემური შეცდომა. " + ex.Message);
                return default(T);
            }
            finally
            {
                query.Dispose();
            }
        }
    }
}
