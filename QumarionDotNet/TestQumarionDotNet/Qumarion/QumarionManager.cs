using Microsoft.VisualStudio.TestTools.UnitTesting;

using Baku.Quma;

namespace QumarionCSTest
{
    [TestClass]
    public class QumarionManagerTest
    {
        [TestMethod]
        public void QManager_初期化と終了()
        {
            Assert.IsFalse(QumarionManager.Initialized);

            QumarionManager.Initialize();
            Assert.IsTrue(QumarionManager.Initialized);

            QumarionManager.Exit();
            Assert.IsFalse(QumarionManager.Initialized);
        }

        [TestMethod]
        public void QManager_デバイス取得()
        {
            bool hardwareExists = QumarionManager.CheckConnectionToHardware();
            var device = QumarionManager.GetDefaultDevice();

            //ハードが接続してる場合に優先的にハードを取得しているかどうかを一応チェック
            Assert.IsTrue(
                (hardwareExists && device.QumaType == QumaTypes.HardwareAsai) ||
                (!hardwareExists && device.QumaType == QumaTypes.Software)
                );

            QumarionManager.Exit();
        }

        [TestMethod]
        public void QManager_バージョン確認()
        {
            QumarionManager.Initialize();
            QumarionManager.GetLibraryVersion();
            QumarionManager.Exit();
        }
    }
}
