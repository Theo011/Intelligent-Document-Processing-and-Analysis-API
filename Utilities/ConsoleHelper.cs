using Serilog;
using System.Runtime.InteropServices;

namespace Intelligent_Document_Processing_and_Analysis_API.Utilities;

public partial class ConsoleHelper
{
    private const uint ENABLE_EXTENDED_FLAGS = 0x0080;
    private const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
    private const int STD_INPUT_HANDLE = -10;

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    [LibraryImport("kernel32.dll")]
    private static partial IntPtr GetStdHandle(int nStdHandle);

    public static void DisableConsoleQuickEditMode()
    {
        try
        {
            IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);

            if (!GetConsoleMode(consoleHandle, out uint consoleMode))
            {
                int errorCode = Marshal.GetLastWin32Error();
                Log.Error("Failed to get console mode at class: {class}, method: {method}. Error code: {errorCode}", nameof(ConsoleHelper), nameof(DisableConsoleQuickEditMode), errorCode);
                return;
            }

            consoleMode &= ~ENABLE_QUICK_EDIT_MODE;
            consoleMode |= ENABLE_EXTENDED_FLAGS;

            if (!SetConsoleMode(consoleHandle, consoleMode))
            {
                int errorCode = Marshal.GetLastWin32Error();
                Log.Error("Failed to set console mode at class: {class}, method: {method}. Error code: {errorCode}", nameof(ConsoleHelper), nameof(DisableConsoleQuickEditMode), errorCode);
                return;
            }

            Log.Information("Console quick edit mode disabled successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(ConsoleHelper), nameof(DisableConsoleQuickEditMode));
        }
    }
}