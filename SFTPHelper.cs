using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.IO;

namespace SampleFluentFTP
{
    public class SFTPHelper : IDisposable
    {
        private SftpClient sftpClient;
        private Func<HostKeyEventArgs, bool> _sshIdentification;
        private string _hostKey;
        public SFTPHelper(string host, string username, string password,string hostKey = null, Func<HostKeyEventArgs, bool> SshIdentification = null) 
        {
            try
            {
                _hostKey = hostKey;
                _sshIdentification = SshIdentification;
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
                sftpClient.HostKeyReceived += Client_HostKeyReceived;

                LogHelper.Info("File Uploaded Successfully.");
            }
        }

        private void Client_HostKeyReceived(object sender, HostKeyEventArgs e)
        {
            if (_sshIdentification != null)
            {
                if (_hostKey == null)
                    LogHelper.Info("SSH Host key is null");

                e.CanTrust = true;
            }
            else 
            {
                e.CanTrust = true;
            }
        }
        public void Dispose()
        {
            sftpClient?.Dispose();
        }
    }
}
