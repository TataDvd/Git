using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Tempo2012.EntityFramework
{
    public class Logger
    {
        private static Logger instance = null;
        private static string sLogFilePath;

        protected Logger()
        {
            sLogFilePath = AppDomain.CurrentDomain.BaseDirectory;
        }

        public static Logger Instance()
        {
            if (instance == null)
            {
                instance = new Logger();
            }
            return instance;
        }
        public string SetLoggingDir
        {
            set { sLogFilePath = value; }
        }
        #region WriteLogError
        /// <summary>
        /// Write an error Log in File
        /// </summary>
        /// <param name="errorMessage"></param>
        public void WriteLogError(string errorMessage,string fromMethod)
        {
            try
            {
                string err;
                string path = Path.Combine(sLogFilePath,string.Format("tempoerrorlog_{0}-{1}-{2}",DateTime.Now.Day,DateTime.Now.Month,DateTime.Now.Year) + ".txt");
                if (!File.Exists(path))
                {
                    File.Create(path).Close();
                }
                using (StreamWriter w = File.AppendText(path))
                {
                    w.WriteLine("\r\nLog Entry : ");
                    w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    err = string.Format("Error Message from {0}: {1}", errorMessage, fromMethod);
                    w.WriteLine(err);
                    w.WriteLine("__________________________");
                    w.Flush();
                    w.Close();
                }
                MessageBoxWrapper.Show(err,"Моля обадете се на администратора на програмата");
            }
            catch (Exception ex)
            {
                //WriteLogError(ex.Message);
            }

        }

        #endregion
    }
}
