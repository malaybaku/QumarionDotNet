using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Baku.Quma.Low;

namespace TestQumarionDotNet
{
    [TestClass]
    public class QumarionTest
    {
        [TestMethod]
        public void Qumarion_ボーン列挙()
        {
            var device = QumarionManager.GetDefaultDevice();
            try
            {
                var bones = Enum
                    .GetValues(typeof(Bones))
                    .Cast<Bones>()
                    .Select(b => device.Bones[b]);
            }
            finally
            {
                if(QumaTestContextSetting.ExitEachTimeWhenContextDisposed)
                {
                    QumarionManager.Exit();
                }
            }
        }

        [TestMethod]
        public void Qumarion_センサ列挙()
        {
            var device = QumarionManager.GetDefaultDevice();
            try
            {
                var sensors = Enum
                    .GetValues(typeof(Sensors))
                    .Cast<Sensors>()
                    .Select(s => device.Sensors[s]);
            }
            finally
            {
                if (QumaTestContextSetting.ExitEachTimeWhenContextDisposed)
                {
                    QumarionManager.Exit();
                }
            }
        }

        [TestMethod]
        public void Qumarion_アップデート処理()
        {
            var device = QumarionManager.GetDefaultDevice();
            //アップデートは急に呼び出すと準備未完了で怒られることに注意
            Thread.Sleep(1000);

            Assert.AreEqual(QumaLowResponse.OK, device.TryUpdateSensors());
            Assert.AreEqual(QumaLowResponse.OK, device.TryUpdateButton());
            //デバイスが要更新状態でない通常ケースでは失敗
            Assert.AreNotEqual(QumaLowResponse.OK, device.TryUpdateDevice());

            if (QumaTestContextSetting.ExitEachTimeWhenContextDisposed)
            {
                QumarionManager.Exit();
            }
        }
    }
}
