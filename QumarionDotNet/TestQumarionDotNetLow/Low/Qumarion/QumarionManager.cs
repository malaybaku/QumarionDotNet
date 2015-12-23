using Microsoft.VisualStudio.TestTools.UnitTesting;

using Baku.Quma.Low;

namespace TestQumarionDotNet.Low
{
    [TestClass]
    public class QumarionManagerTest
    {
        [TestMethod]
        public void LowQManager_初期化()
        {
            QumarionManager.Initialize();
            Assert.IsTrue(QumarionManager.Initialized);

            //終了テストは廃止: 連続するテストの途中でExit呼び出すのはリスクが高い
            //QumarionManager.Exit();
            //Assert.IsFalse(QumarionManager.Initialized);
        }

        [TestMethod]
        public void LowQManager_デバイス取得()
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
        public void LowQManager_バージョン確認()
        {
            QumarionManager.Initialize();
            QumarionManager.GetLibraryVersion();
            QumarionManager.Exit();
        }
    }
}
