using Microsoft.VisualStudio.TestTools.UnitTesting;

using Baku.Quma.Low.Api;

namespace TestQumarionDotNet.Low
{
    /// <summary>デバイス取得まわりのテストです。</summary>
    [TestClass]
    public class LowDeviceTest
    {
        [TestMethod]
        public void Low_デバイス列挙()
        {
            using (var _ = new QumaContext())
            {
                var ids = QmLow.Device.EnumerateQumaIDs();
                Assert.IsTrue(ids.Length > 0);
            }
        }

        [TestMethod]
        public void Low_デバイス取得および有効化と無効化()
        {
            using (var _ = new QumaContext())
            {
                var ids = QmLow.Device.EnumerateQumaIDs();
                var qumaHandle = QmLow.Device.GetQumaHandle(ids[0]);
                QmLow.Device.ActivateQuma(qumaHandle);
                QmLow.Device.DeleteQumaHandle(qumaHandle);
            }
        }

    }

}
