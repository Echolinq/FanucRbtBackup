# Fanuc Robot Backup Tool

This is a command-line tool for backing up files from a Fanuc robot. It uses the `Spectre.Console.Cli` library to handle command-line parsing and execution.

## Usage

To run the backup command, use the following syntax:

`FanucRbtBackup.exe <Hostname> [DestinationDirectory] [-f|--filter]`
  
- `<Hostname>`: The hostname or IPv4 address of the Fanuc robot.
- `[DestinationDirectory]` (optional): The destination folder for the robot backup. If not provided, the current working directory will be used.
- `-f|--filter` (optional): Specifies a file extension filter for the backup. Only files with the specified extension will be backed up.

## Example

 `FanucRbtBackup.exe 192.168.0.10 C:\RobotBackup -f .txt` 
  
In the above example, the tool will connect to the Fanuc robot at IP address 192.168.0.10 and backup all files with the .txt extension to the "C:\RobotBackup" directory.

## Progress Reporting

During the backup process, a progress bar will be displayed using the `Spectre.Console` library. The progress bar shows the overall upload progress, including the current task description, progress percentage, elapsed time, and a spinner for visual effect.

## Validation

The tool validates the provided destination directory before starting the backup process. If the directory does not exist, an error message will be displayed.

## Building from Source

To build the Fanuc Robot Backup Tool from source, follow these steps:

1. Clone the repository: `git clone https://github.com/Echolinq/FanucRbtBackup.git`
2. Navigate to the project directory: `cd FanucRbtBackup`
3. Build the project: `dotnet build`

Make sure you have the .NET Core SDK installed on your machine.

## Downloading Releases

You can also download pre-built releases of the Fanuc Robot Backup Tool from the [Releases](https://github.com/Echolinq/FanucRbtBackup/releases) section of the GitHub repository. Select the desired release and download the appropriate executable file for your platform.

## Dependencies

The tool depends on the following libraries:

- `Spectre.Console`: Used for command-line interface and progress reporting.
- `FluentFTP`: Used for FTP operations to download files from the Fanuc robot.

Make sure these libraries are included in the project and referenced properly.

## License

This code is provided under the [MIT License](https://opensource.org/licenses/MIT). Feel free to modify and use it according to your needs.