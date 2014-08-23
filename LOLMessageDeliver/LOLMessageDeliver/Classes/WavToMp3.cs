using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;

namespace LOLMessageDeliver.Classes
{
    public class WavToMp3
    {
        public bool Run(string sourceFilePath, string targetFilePath, string decoderLocation)
        {
            bool result = true;
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;      // No Command Prompt window.
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.FileName = decoderLocation;
                psi.Arguments = "-b 48 --resample 22.05 -m j " + sourceFilePath + " " + targetFilePath;
                Process p = Process.Start(psi);
                p.WaitForExit();
                p.Close();
                p.Dispose();
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public byte[] ReadFile(string path)
        {
            FileStream fs = File.OpenRead(path);
            try
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                return bytes;
            }
            finally
            {
                fs.Close();
            }
        }
    }
}