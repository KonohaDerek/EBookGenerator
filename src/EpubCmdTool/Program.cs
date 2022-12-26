// See https://aka.ms/new-console-template for more information
using System.CommandLine;
using EpubCmdTool.Models.Options;

// init args
Option<string> GoogleDriverTokenOption, GoogleDriverPathOption;

var rootCommand = new RootCommand("Sample command-line app");
// add args
rootCommand.AddGoogleDriverOption(out GoogleDriverTokenOption,out GoogleDriverPathOption);
rootCommand.SetHandler((GoogleDriverOption) =>
{
    // Create EpubBook
    // Upload EpubBook To Google Drive
    Console.WriteLine("Hello world!");
    Console.WriteLine($"--drive-token = {GoogleDriverOption.Token}");
    Console.WriteLine($"--drive-path = {GoogleDriverOption.Path}");
}, new GoogleDriverBinder(GoogleDriverTokenOption, GoogleDriverPathOption)); // binder

await rootCommand.InvokeAsync(args);