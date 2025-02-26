using Renci.SshNet;
using System;
using System.IO;

namespace SampleFluentFTP
{
    public class SFTPHelper : IDisposable
    {
        private SftpClient sftpClient;
        public SFTPHelper(string host, string username, string password) 
        {
            try
            {
                sftpClient = new SftpClient(host, username, password);
            }
            catch (Exception e) {
                LogHelper.Error(e.Message,e);
            }
            
        }

        public void UploadFile(string inputFilePath,string ftpFilePath)
        {
            if (!File.Exists(inputFilePath))
            {
                LogHelper.Info("File Not Found :" + inputFilePath);
                return;
            }

            LogHelper.Info("Input File :" + inputFilePath);

            string ftpFile = Path.Combine(ftpFilePath, Path.GetFileName(inputFilePath));


            using (FileStream fs = File.OpenRead(inputFilePath))
            {
                LogHelper.Info("File Size :" + fs.Length);
                LogHelper.Info("Upload File Path :" + ftpFilePath);
                sftpClient.UploadFile(fs, ftpFile);
                sftpClient.ServerIdentificationReceived += Client_ServerIdentificationReceived;

                LogHelper.Info("File Uploaded Successfully.");
            }
        }

        private static void Client_ServerIdentificationReceived(object sender, Renci.SshNet.Common.SshIdentificationEventArgs e)
        {
            
        }

        public void Dispose()
        {
            sftpClient?.Dispose();
        }
    }
}
