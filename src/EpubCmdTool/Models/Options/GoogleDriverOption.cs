using System.CommandLine;
using System.CommandLine.Binding;
using System.ComponentModel;

namespace EpubCmdTool.Models.Options;


public static class GoogleDriverOption
{
    public static Command AddGoogleDriverOption(this Command cmd, out Option<string> GoogleDriverTokenOption , out Option<string> GoogleDriverPathOption)
    {
         GoogleDriverTokenOption = new Option<string>
            (name: "--drive-token",
            description: "An option set google-drive token.")
        { IsRequired = true };

         GoogleDriverPathOption = new Option<string>
            (name: "--drive-path",
            description: "An option set google-drive path.")
        { IsRequired = true };

        cmd.AddOption(GoogleDriverTokenOption);
        cmd.AddOption(GoogleDriverPathOption);
        return cmd;
    }
}

public class GoogleDriver
{
    public string? Token { get; set; }
    public string? Path { get; set; }
}

public class GoogleDriverBinder : BinderBase<GoogleDriver>
{
    private readonly Option<string> _tokenOption;
    private readonly Option<string> _pathOption;

    public GoogleDriverBinder(Option<string> tokenOption, Option<string> pathOption)
    {
        this._tokenOption = tokenOption;
        this._pathOption = pathOption;
    }

    protected override GoogleDriver GetBoundValue(BindingContext bindingContext)
    {
        return new GoogleDriver
        {
            Token = bindingContext.ParseResult.GetValueForOption(_tokenOption),
            Path = bindingContext.ParseResult.GetValueForOption(_pathOption)
        };
    }
}