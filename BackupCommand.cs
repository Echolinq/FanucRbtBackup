using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;
using FluentFTP;
using FluentFTP.Rules;

namespace FanucRbtBackup;

public class BackupCommand : Command<BackupCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Hostname or IPv4 Address of Robot")]
        [CommandArgument(0, "<Hostname>")]
        public string Hostname { get; init; }

        [Description("The destination folder for the robot backup. If not provided, the current working directory will be used.")]
        [CommandArgument(0, "[Hostname]")]
        [DefaultValue("")]
        public string DestinationDirectory { get; init; }

        [Description("Specifies a file extension filter for the backup. Only files with the specified extension will be backed up")]
        [CommandOption("-f|--filter")]
        [DefaultValue("")]
        public string fileExtension { get; init; }
    }

    public override ValidationResult Validate([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        if (!System.IO.Directory.Exists(settings.DestinationDirectory))
        {
            return ValidationResult.Error($"Path not found - {settings.DestinationDirectory}");
        }

        return base.Validate(context, settings);
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        string DestDir;
        if (settings.DestinationDirectory == null)
        {
            DestDir = Environment.CurrentDirectory;
        }
        else
        {
            DestDir = settings.DestinationDirectory;
        }

        var rule = new Rule("[red]Fanuc Robot Backup Tool[/]");
        AnsiConsole.Write(rule);

        AnsiConsole.Progress()
            .Columns(new ProgressColumn[]
            {
                    new TaskDescriptionColumn(),    // Task description
                    new ProgressBarColumn(),        // Progress bar
                    new PercentageColumn(),         // Percentage
                    new ElapsedTimeColumn(),        // Elapsed Time 
                    new SpinnerColumn(),            // Spinner
            })
            .Start(ctx =>
            {
                var OvarallUpload = ctx.AddTask("[green]Upload Progress[/]");
                string fileExt = settings.fileExtension.Replace(".", "");
                var rules = new List<FtpRule>{
                    new FtpFileExtensionRule(fileExt == "" ? false : true, new List<string>{ $"{fileExt}" })
                };
                int prevIndx = -1;
                using (var client = new FtpClient(settings.Hostname, "NONE", ""))
                {
                    client.Connect();
                    client.Config.DownloadDataType = FtpDataType.Binary;
                    client.DownloadDirectory(
                        @$"{DestDir}", @"/md:/",
                        FtpFolderSyncMode.Update,
                        FtpLocalExists.Overwrite,
                        FtpVerify.None,
                        rules,
                        (x =>
                            {
                                if (prevIndx != x.FileIndex)
                                {
                                    AnsiConsole.MarkupLine("[grey]Uploading File: [/] " + $"[blue]{System.IO.Path.GetFileName(x.LocalPath)}[/]");
                                    OvarallUpload.Value = ((double)x.FileIndex / (double)(x.FileCount - 1)) * 100;
                                    prevIndx = x.FileIndex;
                                }
                            }
                        )
                    );
                    client.Disconnect();
                }
            });
        return 0;
    }
}