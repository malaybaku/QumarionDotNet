using Microsoft.VisualStudio.TestTools.UnitTesting;

using Baku.Quma.Low.Api;
using System;

namespace TestQumarionDotNet.Low
{
    [TestClass]
    public class LowBaseOperationWithDeviceTest
    {

        [TestMethod]
        public void Low_デバイスID取得()
        {
            using (var context = new QumaActiveDeviceContext())
            {
                byte[] id = QmLow.BaseOperation.GetDeviceID(context.QumaHandle);
            }
        }

        [TestMethod]
        public void Low_デバイス名取得()
        {
            using (var context = new QumaActiveDeviceContext())
            {
                string name = QmLow.BaseOperation.GetDeviceName(context.QumaHandle);
            }
        }

        [TestMethod]
        public void Low_タイムアウト設定および設定解除_異常系()
        {
            using (var context = new QumaActiveDeviceContext())
            {
                var qumaHandle = context.QumaHandle;
                QmLow.BaseOperation.SetTimeout(qumaHandle, 1000);
                QmLow.BaseOperation.DisableTimeout(qumaHandle);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Low_タイムアウト設定_異常系()
        {
            using (var context = new QumaActiveDeviceContext())
            {
                var qumaHandle = context.QumaHandle;
                QmLow.BaseOperation.SetTimeout(qumaHandle, 0);
            }
        }

    }
}
