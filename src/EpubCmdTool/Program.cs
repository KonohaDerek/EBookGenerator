// See https://aka.ms/new-console-template for more information
using System.CommandLine;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using EpubCmdTool.Models.Options;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

// init args
Option<string> GoogleDriverTokenOption, GoogleDriverPathOption;

var rootCommand = new RootCommand("epub grenter tool");
// add args
rootCommand.AddGoogleDriverOption(out GoogleDriverTokenOption, out GoogleDriverPathOption);
rootCommand.SetHandler(async (GoogleDriverOption) =>
{
    Console.WriteLine($"--drive-token = {GoogleDriverOption.Token}");
    Console.WriteLine($"--drive-path = {GoogleDriverOption.Path}");
    await ZhConverterAsync("優化");
    // Create EpubBook
    // Upload EpubBook To Google Drive
    // Create the service.
    string _credentialJson = string.Empty;
    using (FileStream fs = new FileStream("credential.json", FileMode.Open, FileAccess.Read))
    {
        using (StreamReader sr = new StreamReader(fs, Encoding.Default))
        {
            _credentialJson = sr.ReadToEnd();
        }
    }
    // Console.WriteLine(_credentialJson);
    GoogleCredential credential = GoogleCredential.FromJson(_credentialJson).CreateScoped(new[] {DriveService.Scope.Drive, DriveService.Scope.DriveFile });
    var service = new DriveService(new BaseClientService.Initializer
    {
        HttpClientInitializer = credential,
        ApplicationName = "epubserviceaccount",
    });

    // Run the request.
    Console.WriteLine("Executing a list request...");
   

    var result = await service.Files.List().ExecuteAsync();
    // Display the results.
    if (result.Files != null)
    {
        Console.WriteLine($"total files : {result.Files.Count}");
        foreach (var file in result.Files)
        {
            Console.WriteLine($"Id : {file.Id}, Name : {file.Name} , DriveId: {file.DriveId} , Kind : {file.Kind}, MimeType:{file.MimeType}");
            // Console.WriteLine(file.Id + " - " + file.Name+ " - " + file.GetType());
        }
    }else {
        Console.WriteLine("files is empty");
    }


}, new GoogleDriverBinder(GoogleDriverTokenOption, GoogleDriverPathOption)); // binder

await rootCommand.InvokeAsync(args);


async Task ZhConverterAsync(string text){
    var client = new HttpClient();
    var request = new HttpRequestMessage
    {
        Method = HttpMethod.Post,
        RequestUri = new Uri("http://api.zhconvert.org/convert"),
        Content = new StringContent(JsonSerializer.Serialize(new {
            text= text,
            converter = "Taiwan"
        }))
        {
            Headers =
            {
                ContentType = new MediaTypeHeaderValue("application/json")
            }
        }
    };
    using (var response = await client.SendAsync(request))
    {
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        Console.WriteLine(body);
    }
}