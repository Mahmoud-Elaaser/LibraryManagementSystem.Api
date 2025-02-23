using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Application.Services
{
    public class HtmlResponseService : IHtmlResponseService
    {
        private readonly string _templatePath;

        public HtmlResponseService()
        {
            _templatePath = "Templates/EmailConfirmation.html";
        }

        public ContentResult CreateHtmlResponse(string message, bool isSuccess)
        {
            var html = $@"
<!DOCTYPE html>
<html>
<head>
    <title>Email Confirmation</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
            background-color: #f5f5f5;
        }}
        .container {{
            text-align: center;
            padding: 40px;
            background-color: white;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            max-width: 500px;
        }}
        .icon {{
            font-size: 48px;
            margin-bottom: 20px;
        }}
        .message {{
            color: {(isSuccess ? "#2da44e" : "#cb2431")};
            margin-bottom: 20px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='icon'>{(isSuccess ? "✓" : "×")}</div>
        <div class='message'>{message}</div>
    </div>
</body>
</html>";

            return new ContentResult
            {
                Content = html,
                ContentType = "text/html",
                StatusCode = isSuccess ? 200 : 400
            };
        }
    }
}
