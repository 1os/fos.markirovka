using Microsoft.Win32;
using System.Diagnostics;
using System.Globalization;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.IO;
using System.Net;
using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Imaging;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Приложение обработчик печати этикеток честный знак");


        if (args.Length > 0 && args[0].IndexOf("fosmarkirovka:") == 0)
        {
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
                        using (var client = new WebClient())
                        {
                            client.DownloadFile(url, filepath);
                        }
                        break;
                }
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using (PrintDocument pd = new PrintDocument())
                {
                    var sizes = media.Split("x");
                    pd.DefaultPageSettings.PrinterResolution.X = 600;
                    pd.DefaultPageSettings.PrinterResolution.Y = 600;
                    pd.DefaultPageSettings.Color = false;
                    pd.DefaultPageSettings.PaperSize = new PaperSize(media,
                    (int)(int.Parse(sizes[0]) * (1.0 / 25.4) * 100.0),
                    (int)(int.Parse(sizes[1]) * (1.0 / 25.4) * 100.0));
                    pd.PrinterSettings.PrinterName = printer;
                    pd.PrinterSettings.DefaultPageSettings.PrinterResolution.X = 600;
                    pd.PrinterSettings.DefaultPageSettings.PrinterResolution.Y = 600;
                    pd.PrinterSettings.DefaultPageSettings.Color = false;
                    pd.PrintPage += (object o, PrintPageEventArgs e) =>
                    {
                        e.PageSettings.PrinterResolution.X = 600;
                        e.PageSettings.PrinterResolution.Y = 600;
                        e.PageSettings.Color = false;
                        e.Graphics.PageUnit = GraphicsUnit.Millimeter;
                        e.Graphics.PageScale = 1f;

                        Bitmap img = new Bitmap(filepath);
                        var bmp1bpp = img.Clone(new Rectangle(0, 0, img.Width, img.Height), PixelFormat.Format1bppIndexed);

                        e.Graphics.Clear(Color.White);
                        e.Graphics.DrawImage(img, 0, 0, float.Parse(sizes[0]), float.Parse(sizes[1]));
                        img.Dispose();
                    };
                    pd.Print();

                    Console.WriteLine("Отправили на печать этикетку");
                }
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                filename = "lpr";
                arguments = string.Format("-o fit-to-page -o media=Custom.{0}mm -d \"{1}\" {2}", media, printer, filepath);
                Console.WriteLine(filename + " " + arguments);
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
                }
                Process.Start("notify-send", "-a \"1ОС.Маркировка\" -i /snap/fos-markirovka/current/meta/gui/icon.png  \"Отправили на печать этикетку\"");
                Console.WriteLine("Отправили на печать этикетку");
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                filename = "lpr";
                arguments = string.Format("-o fit-to-page -o media=Custom.{0}mm -d \"{1}\" {2}", media, printer, filepath);

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
                Process.Start("notify-send", "-a \"1ОС.Маркировка\" -i /snap/fos-markirovka/current/meta/gui/icon.png  \"Успешно настроили приложение для работы Linux.\"");
                Console.WriteLine("Успешно настроили приложение для работы Linux.");
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Console.WriteLine("Успешно настроили приложение для работы OSX.");
            }
        }
    }
}