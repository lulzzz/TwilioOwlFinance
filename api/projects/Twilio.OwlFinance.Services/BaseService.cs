using System;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Model;

namespace Twilio.OwlFinance.Services
{
    public abstract class BaseService
    {
        public readonly ILogger logger;

        protected ILogger Logger => logger;

        protected BaseService(ILogger logger)
        {
            this.logger = logger;
        }

        protected virtual T GetErrorModel<T>(string errorMessage) where T : ICanHaveError
        {
            var model = Activator.CreateInstance<T>();

            model.StatusCode = StatusCodes.ServerError;
            model.HasError = true;
            model.Message = errorMessage;

            return model;
        }

        protected virtual T GetErrorModel<T>(Exception ex) where T : ICanHaveError
        {
            var model = Activator.CreateInstance<T>();

            model.StatusCode = StatusCodes.ServerError;
            model.HasError = true;
            model.Message = ex.Message;

            return model;
        }

        protected virtual T GetNotFoundErrorModel<T>(Exception ex) where T : ICanHaveError
        {
            var model = Activator.CreateInstance<T>();

            model.StatusCode = StatusCodes.ItemNotFound;
            model.HasError = true;
            model.Message = ex.Message;

            return model;
        }

        protected virtual T GetNotAuthorizedErrorModel<T>(Exception ex) where T : ICanHaveError
        {
            var model = Activator.CreateInstance<T>();

            model.StatusCode = StatusCodes.NotAuthorized;
            model.HasError = true;
            model.Message = ex.Message;

            return model;
        }

        protected virtual T GetBadRequestErrorModel<T>(Exception ex) where T : ICanHaveError
        {
            var model = Activator.CreateInstance<T>();

            model.StatusCode = StatusCodes.BadRequest;
            model.HasError = true;
            model.Message = ex.Message;

            return model;
        }

        protected virtual T HandleErrorAndReturnStatus<T>(Exception ex) where T : ICanHaveError
        {
            this.logger.LogException(ex);
            return GetErrorModel<T>(ex);
        }
    }
}
