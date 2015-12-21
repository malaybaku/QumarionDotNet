using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Baku.Quma.Pdk.Api;
using System.IO;

namespace ConsoleSample
{
    static class PdkSample
    {
        public static void PdkSampleMain()
        {
            if(!File.Exists(QmPdk.DllName))
            {
                Console.WriteLine("ERROR: Library file'{0}' does not exist. Exit the program.", QmPdk.DllName);
                return;
            }

            QmPdk.BaseOperation.Init();
            try
            {
                string version = QmPdk.BaseOperation.GetVersionStr();
                Console.WriteLine("Version is {0}", version);

            }
            finally
            {
                QmPdk.BaseOperation.Final();
            }
        }
    }
}
