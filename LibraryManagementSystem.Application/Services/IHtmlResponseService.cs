using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Application.Services
{
    public interface IHtmlResponseService
    {
        ContentResult CreateHtmlResponse(string message, bool isSuccess);
    }
}
