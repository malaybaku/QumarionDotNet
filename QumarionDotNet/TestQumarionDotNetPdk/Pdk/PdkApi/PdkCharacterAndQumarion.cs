using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baku.Quma.Pdk.Api;
using Baku.Quma.Pdk;

namespace TestQumarionDotNet.Pdk
{

    [TestClass]
    public class PdkCharacterAndQumarion
    {

        [TestMethod]
        public void Pdk_デバイス必須_キャラ加速度センサモード設定_正常系()
        {
            using (var context = new QumaAndModelContext())
            {
                var modelHandle = context.ModelHandle;

                QmPdk.Character.SetAccelerometerMode(modelHandle, AccelerometerMode.Direct);
                var currentMode = QmPdk.Character.GetAccelerometerMode(modelHandle);
                Assert.AreEqual(AccelerometerMode.Direct, currentMode);

                QmPdk.Character.SetAccelerometerMode(modelHandle, AccelerometerMode.Relative);
                currentMode = QmPdk.Character.GetAccelerometerMode(modelHandle);
                Assert.AreEqual(AccelerometerMode.Relative, currentMode);
            }
        }

        [TestMethod]
        public void Pdk_デバイス必須_キャラ加速度センサ制約モード設定_正常系()
        {
            using (var context = new QumaAndModelContext())
            {
                var modelHandle = context.ModelHandle;

                //いちおう他のテストに影響出にくいよう一周回して戻しておく
                QmPdk.Character.SetRestrictAccelerometerMode(modelHandle, AccelerometerRestrictMode.None);
                var currentMode = QmPdk.Character.GetRestrictAccelerometerMode(modelHandle);
                Assert.AreEqual(AccelerometerRestrictMode.None, currentMode);

                QmPdk.Character.SetRestrictAccelerometerMode(modelHandle, AccelerometerRestrictMode.AxisX);
                currentMode = QmPdk.Character.GetRestrictAccelerometerMode(modelHandle);
                Assert.AreEqual(AccelerometerRestrictMode.AxisX, currentMode);

                QmPdk.Character.SetRestrictAccelerometerMode(modelHandle, AccelerometerRestrictMode.AxisZ);
                currentMode = QmPdk.Character.GetRestrictAccelerometerMode(modelHandle);
                Assert.AreEqual(AccelerometerRestrictMode.AxisZ, currentMode);

                QmPdk.Character.SetRestrictAccelerometerMode(modelHandle, AccelerometerRestrictMode.None);
                currentMode = QmPdk.Character.GetRestrictAccelerometerMode(modelHandle);
                Assert.AreEqual(AccelerometerRestrictMode.None, currentMode);
            }
        }
    
        //デバイスをアタッチせずに加速度センサ設定を行った場合の想定挙動が下記の二つ。

        [TestMethod]
        public void Pdk_デバイス必須_キャラ加速度センサモード設定_異常系()
        {
            using (var context = new QumaAndModelContext(false))
            {
                var currentMode = QmPdk.Character.GetAccelerometerMode(context.ModelHandle);
                Assert.AreEqual(AccelerometerMode.NotAttached, currentMode);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(QmPdkException))]
        public void Pdk_デバイス必須_キャラ加速度センサ制約モード設定_異常系()
        {
            using (var context = new QumaAndModelContext(false))
            {
                var currentMode = QmPdk.Character.GetRestrictAccelerometerMode(context.ModelHandle);
            }
        }


    }

    public class QumaAndModelContext : IDisposable
    {
        public ModelHandle ModelHandle { get; }
        public QumaHandle QumaHandle { get; }

        public QumaAndModelContext(bool attachDevice = true)
        {
            QmPdk.BaseOperation.Initialize();

            ModelHandle = QmPdk.Character.CreateStandardModelPS().ModelHandle;
            QumaHandle = PdkApiTestUtil.LoadQumaHandle();

            if(attachDevice)
            {
                QmPdk.Quma.AttachInitPoseModel(QumaHandle, ModelHandle);
            }
        }

        public void Dispose()
        {
            QmPdk.Quma.DetachModel(ModelHandle);
            QmPdk.Character.Destroy(ModelHandle);
        }
    }
}
