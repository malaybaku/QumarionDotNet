using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Baku.Quma.Pdk;

namespace TestQumarionDotNet.Pdk
{
    [TestClass]
    public class PdkQumarionTest
    {
        [TestMethod]
        public void Pdk_デバイス必須_デフォルトデバイス取得とプロパティ()
        {
            var device = PdkManager.GetDefaultDevice();

            Assert.AreNotEqual(QumaHandle.QumaHandleError, device.QumaHandle.Handle);

            Assert.AreEqual(0, device.AttachedModelCount);

            device.Enable = false;
            Assert.AreEqual(false, device.Enable);
            device.Enable = true;
            Assert.AreEqual(true, device.Enable);

            device.EnableAccelerometer = true;
            device.EnableAccelerometer = false;

            Assert.AreEqual(QumaButtonState.Up, device.ButtonState);
        }

        [TestMethod]
        public void Pdk_デバイス必須_デバイスのチェック関数()
        {
            var device = PdkManager.GetDefaultDevice();
            device.CheckDeviceValidity();
            device.CheckPoseChanged();
            var sensorState = device.CheckDeviceSensors();
            Assert.AreEqual(true, sensorState.IsOk);
        }
    }
}
