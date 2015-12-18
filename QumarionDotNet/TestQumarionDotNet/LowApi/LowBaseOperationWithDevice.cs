using Microsoft.VisualStudio.TestTools.UnitTesting;

using Baku.Quma.Low;
using Baku.Quma;
using System;

namespace TestQumarionDotNet
{
    [TestClass]
    public class LowBaseOperationWithDevice
    {
        private static readonly QumaTypes TargetType = QumaTypes.Software;

        [TestMethod]
        public void Low_デバイスID取得()
        {
            using (var context = QumaActiveDeviceContext.Create(TargetType))
            {
                byte[] id = QmLow.BaseOperation.GetDeviceID(context.QumaHandle);
            }
        }

        [TestMethod]
        public void Low_デバイス名取得()
        {
            using (var context = QumaActiveDeviceContext.Create(TargetType))
            {
                string name = QmLow.BaseOperation.GetDeviceName(context.QumaHandle);
            }
        }

        //NOTE: APIの仕様上シミュレータQumaだとSetTimeoutがエラー、ハードウェアだと通る
        //ので、このテストでは関数のラッピングに成功してる事自体を確認している
        [TestMethod]
        [ExpectedException(typeof(QumaException))]
        public void Low_タイムアウト設定および設定解除_異常系()
        {
            using (var context = QumaActiveDeviceContext.Create(QumaTypes.Software))
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
            using (var context = QumaActiveDeviceContext.Create(TargetType))
            {
                var qumaHandle = context.QumaHandle;
                QmLow.BaseOperation.SetTimeout(qumaHandle, 0);
            }
        }

    }
}
