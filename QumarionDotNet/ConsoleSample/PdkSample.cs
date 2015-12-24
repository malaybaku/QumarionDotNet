using System;
using System.Linq;
using System.IO;

using Baku.Quma;
using Baku.Quma.Pdk;
using Baku.Quma.Pdk.Api;

namespace ConsoleSample
{
    static class PdkSample
    {
        public static void PdkSampleMain()
        {
            if(!File.Exists(DllImportSetting.DllName86) || !File.Exists(DllImportSetting.DllName64))
            {
                Console.WriteLine(
                    $"ERROR: Library file'{DllImportSetting.DllName86}' or '{DllImportSetting.DllName64}' does not exist. Exit the program.");
                return;
            }

            try
            {
                //ここに何か書いてデバッグ実行してみよう！
                Console.WriteLine(IntPtr.Size);
                Console.WriteLine(QmPdk.BaseOperation.GetVersionStr());

                if (PdkManager.ConnectedDeviceCount == 0)
                {
                    throw new InvalidOperationException("Qumarion is not connected to this machine");
                }

                var standardModel = PdkManager.CreateStandardModelPS();
                var qumarion = PdkManager.GetDefaultQumarion();

                standardModel.AttachQumarion(qumarion);



            }
            catch (QmPdkException ex)
            {
                Console.WriteLine("ERROR: received error code = {0}, stack trace = {1}", ex.ErrorCode, ex.StackTrace);
            }
            finally
            {
                QmPdk.BaseOperation.Final();
            }
        }
    }
}
