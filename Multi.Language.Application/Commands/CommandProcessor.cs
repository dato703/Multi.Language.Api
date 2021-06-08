using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Multi.Language.Domain.SeedWork;
using Multi.Language.Infrastructure.Authorization;

namespace Multi.Language.Application.Commands
{
    public class CommandProcessor
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorizationService _authorizationService;
        public CommandProcessor(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
        {
            _unitOfWork = unitOfWork;
            _authorizationService = authorizationService;
        }

        public async Task Execute(CommandBase command)
        {
            try
            {
                command.HttpResult.Successful();

                if (command.NeedTransaction)
                {
                    command.Initialize(_unitOfWork, _authorizationService);
                    await _unitOfWork.UseTransaction(async () =>
                    {
                        await ExecuteCommand(command);
                    });
                }
                else
                {
                    command.Initialize(_unitOfWork, _authorizationService);
                    await ExecuteCommand(command);
                }
            }
            catch (DomainException domainException)
            {
                command.HttpResult.Error(domainException.Message);

                if (domainException.Level == ExceptionLevel.Error || domainException.Level == ExceptionLevel.Fatal)
                {
                    _unitOfWork.DetachChanges();
                    //await _unitOfWork.ExceptionLogRepository.AddAsync(new ExceptionLog(domainException.Message, "Ip", JsonService.Serialize(domainException)));
                    //await _unitOfWork.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                command.HttpResult.Error(ex.Message);
                _unitOfWork.DetachChanges();
                //await _unitOfWork.ExceptionLogRepository.AddAsync(new ExceptionLog(ex.Message, "Ip", JsonService.Serialize(ex)));
                //await _unitOfWork.CompleteAsync();
            }
        }

        private async Task ExecuteCommand(CommandBase command)
        {
            bool saveFailed;
            const int tryLimit = 3;
            var tryCount = 0;
            IReadOnlyList<EntityEntry> entries = null;
            do
            {
                saveFailed = false;
                try
                {
                    await command.Execute();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    tryCount++;

                    entries = ex.Entries;
                    foreach (var entry in entries)
                    {
                        await entry.ReloadAsync();
                    }
                    _unitOfWork.DetachChanges();
                }

                if (tryCount >= tryLimit)
                {
                    throw new DomainException("ტექნიკური პრობლემა. სცადეთ მოგვიანებით.",
                        ExceptionLevel.Fatal, new
                        {
                            TryLimit = tryLimit,
                            TryCount = tryCount,
                            Entries = entries
                        });
                }
            } while (saveFailed);
        }


    }
}
