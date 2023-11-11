using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading;

public class GoogleDriveService
{
    private readonly DriveService _driveService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public GoogleDriveService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;

        // Ініціалізація _driveService - об'єкта служби Google Drive API
        _driveService = InitializeDriveService();
    }

    public string UploadFile(string fileName, byte[] fileContent)
    {
        // Завантаження файлу на Google Drive
        var fileId = UploadFileToDrive(fileName, fileContent);

        // Повертаємо ідентифікатор завантаженого файлу
        return fileId;
    }

    private DriveService InitializeDriveService()
    {
        string projectId = "celtic-park-403708";

        string jsonPath = Path.Combine(_webHostEnvironment.ContentRootPath, "celtic-park-403708-fe8609d830d1.json");

        var credential = GoogleCredential.FromFile(jsonPath)
            .CreateScoped(new[] { DriveService.Scope.Drive });

        // Створення служби Google Drive API
        var driveService = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "CourseApp",
        });

        return driveService;
    }

    private string UploadFileToDrive(string fileName, byte[] fileContent)
    {
        var fileMetadata = new Google.Apis.Drive.v3.Data.File()
        {
            Name = fileName,
            MimeType = "application/octet-stream",
        };

        using (var stream = new MemoryStream(fileContent))
        {
            var request = _driveService.Files.Create(fileMetadata, stream, "application/octet-stream");
            request.Upload();

            return request.ResponseBody?.Id;
        }
    }
}
