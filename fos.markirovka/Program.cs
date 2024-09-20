// See https://aka.ms/new-console-template for more information

using Microsoft.Win32;
using System.Diagnostics;
using System.Globalization;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.IO;

public class Program
{
    public static void handleOpenURL(string[] args)
    {
        File.WriteAllLines(Path.GetTempPath() + Guid.NewGuid().ToString() + ".txt", Environment.GetCommandLineArgs());
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("Приложение обработчик печати этикеток честный знак");


        if (args.Length > 0 && args[0].IndexOf("fosmarkirovka:") == 0)
        {
            //lp -o fit-to-page -o media=Custom.58x40mm -d Xprinter_XP_365B km.png
            var query = args[0].Split('?')[1];
            var parms = query.Split('&');
            var media = "";
            var printer = "";
            var url = "";
            var filename = "";
            var arguments = "";
            var filepath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".png";
            foreach (var parm in parms)
            {
                var toupl = parm.Split('=');
                switch (toupl[0])
                {
                    case "media":
                        media = HttpUtility.UrlDecode(toupl[1]);
                        break;
                    case "printer":
                        printer = HttpUtility.UrlDecode(toupl[1]);
                        break;
                    case "url":
                        url = HttpUtility.UrlDecode(toupl[1]);
                        File.WriteAllBytes(filepath, Convert.FromBase64String(url));
                        break;
                }
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                filename = "lp";
                arguments = string.Format("-o fit-to-page -o media=Custom.{1}mm -d '{2}' {3}", media, printer, filepath);

                Console.Write(filename + " " + arguments);
                using (Process process = new Process())
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.FileName = filename;
                    startInfo.Arguments = arguments;
                    process.StartInfo = startInfo;
                    process.Start();
                    do
                    {
                        if (!process.HasExited)
                        {
                            process.Refresh();
                        }
                    }
                    while (!process.WaitForExit(1000));
                    Console.WriteLine("Отправили на печать этикетку");
                }
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                filename = "lp";
                arguments = string.Format("-o fit-to-page -o media=Custom.{1}mm -d '{2}' {3}", media, printer, filepath);

                Console.Write(filename + " " + arguments);
                using (Process process = new Process())
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.FileName = filename;
                    startInfo.Arguments = arguments;
                    process.StartInfo = startInfo;
                    process.Start();
                    do
                    {
                        if (!process.HasExited)
                        {
                            process.Refresh();
                        }
                    }
                    while (!process.WaitForExit(1000));
                    Console.WriteLine("Отправили на печать этикетку");
                }
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                filename = "lp";
                arguments = string.Format("-o fit-to-page -o media=Custom.{1}mm -d '{2}' {3}", media, printer, filepath);

                Console.Write(filename + " " + arguments);
                using (Process process = new Process())
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.FileName = filename;
                    startInfo.Arguments = arguments;
                    process.StartInfo = startInfo;
                    process.Start();
                    do
                    {
                        if (!process.HasExited)
                        {
                            process.Refresh();
                        }
                    }
                    while (!process.WaitForExit(1000));
                    Console.WriteLine("Отправили на печать этикетку");
                }
            }
            Thread.Sleep(5);
            File.Delete(filepath);
        }
        else
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                RegistryKey key;
                key = Registry.ClassesRoot.CreateSubKey("fosmarkirovka");
                key.SetValue("", "URL: fosmarkirovka Protocol");
                key.SetValue("URL Protocol", "");

                key = key.CreateSubKey("shell");
                key = key.CreateSubKey("open");
                key = key.CreateSubKey("command");
                key.SetValue("", Process.GetCurrentProcess().MainModule.FileName + " \"%1\"");
                key.Close();

                Console.WriteLine("Успешно настроили приложение для работы Windows.");
                Console.ReadKey();
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Console.WriteLine("Успешно настроили приложение для работы Linux.");
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Console.WriteLine("Успешно настроили приложение для работы OSX.");
            }
        }
    }
}