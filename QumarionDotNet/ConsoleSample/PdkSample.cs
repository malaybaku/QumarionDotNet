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

            QmPdk.BaseOperation.Initialize();
            try
            {
                //ここに何か書いてデバッグ実行してみよう！
                Console.WriteLine(IntPtr.Size);
                Console.WriteLine(QmPdk.BaseOperation.GetVersionStr());

                if (QmPdk.Quma.GetDeviceCount() == 0)
                {
                    throw new InvalidOperationException("Qumarion is not connected to this machine");
                }

                var modelHandle = QmPdk.Character.CreateStandardModelPS().ModelHandle;

                //var quma = PdkManager.GetDefaultDevice();
                //QmPdk.Quma.AttachInitPoseModel(quma.QumaHandle, modelHandle);

                QmPdk.Character.SetAccelerometerMode(modelHandle, AccelerometerMode.Direct);
                var currentMode = QmPdk.Character.GetAccelerometerMode(modelHandle);

                QmPdk.Character.SetAccelerometerMode(modelHandle, AccelerometerMode.Relative);
                currentMode = QmPdk.Character.GetAccelerometerMode(modelHandle);

                QmPdk.Character.Destroy(modelHandle);
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
