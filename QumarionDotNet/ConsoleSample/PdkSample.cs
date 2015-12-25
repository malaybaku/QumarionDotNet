using System;
using System.Linq;
using System.IO;

using Baku.Quma;
using Baku.Quma.Pdk;
using Baku.Quma.Pdk.Api;
using System.Threading;

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

                //検証: rootボーンが隠れているというウワサがあるので確認。
                //var names = QmPdk.Character.GetName(standardModel.ModelHandle, -1);

                var mat = standardModel.Bones[StandardPSBones.Hips].InitialLocalMatrix;
                Console.WriteLine(mat);

                standardModel.AttachQumarion(qumarion);
                qumarion.EnableAccelerometer = true;
                standardModel.AccelerometerMode = AccelerometerMode.Relative;
                standardModel.AccelerometerRestrictMode = AccelerometerRestrictMode.None;

                for (int i = 0;;i++)
                {
                    standardModel.Update();
                    //Console.WriteLine("update..");
                    Thread.Sleep(500);
                    //Console.WriteLine("local mat");
                    var rotateMat = standardModel.Bones[StandardPSBones.Hips].LocalMatrix;
                    Console.WriteLine(rotateMat);
                }

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
