using BotMarket2.Common;
using MudBlazor;
using static System.Net.HttpStatusCode;

namespace BotMarket2.Client.Shared
{
    public class ApiExceptionHandler(IDialogService dialogService)
    {
        private readonly IDialogService _dialogService = dialogService;
        public void HandleApiException(ApiException ex)
        {
            string message = GetExceptionMessage(ex);
            _dialogService.ShowMessageBox("Error", message, "OK");
        }

        public string GetExceptionMessage(ApiException ex)
        {
            string message;

            if (!string.IsNullOrEmpty(ex.Message))
                message = $"{ex.Message}";
            else
            {
                switch (ex.StatusCode)
                {
                    case NotFound:
                        message = "The resource is not found.";
                        break;
                    case InternalServerError:
                        message = "The request cannot be completed due to a server issue. Please try again later or contact support.";
                        break;
                    case Unauthorized:
                        message = "You are not authorized to read data.";
                        break;
                    case BadRequest:
                        message = "The request could not be completed due to a technical issue, please contact support.";
                        break;
                    case Forbidden:
                        message = "This request requires a higher permission level.";
                        break;
                    default:
                        message = "Unknown error. Please reach out for support.";
                        break;
                }
            }
            return message;
        }
    }
}
