using System;
using System.Threading;

using Baku.Quma.Low;

namespace ConsoleSample
{
    class LowSample
    {
        //Low APIを使ったサンプルコードです。
        public static void LowSampleMain()
        {
            bool hardwareExists = QumarionManager.CheckConnectionToHardware();
            if(hardwareExists)
            {
                Console.WriteLine("Found Qumarion hardware.");
            }
            else
            {
                Console.WriteLine("Could not find Qumarion hardware: simulator will be used.");
            }

            //GetDefaultDevice関数では実機のQumarionがある場合それを、ない場合シミュレータをロードする
            var device = QumarionManager.GetDefaultDevice();
            //注意: デバイスの仕様でロード後ある程度待たないとアップデート処理が通らない
            Thread.Sleep(1000);

            for(int i = 0;i < 10; i++)
            {
                Console.WriteLine("press ENTER to check Head Yaw angle..");
                Console.ReadLine();

                //センサーのデータ更新
                device.Update();
                float angle = device.Sensors[Sensors.Head_Y].Angle;
                Console.WriteLine("Head Yaw(deg): {0}", angle);
            }

            QumarionManager.Exit();
        }
    }
}
