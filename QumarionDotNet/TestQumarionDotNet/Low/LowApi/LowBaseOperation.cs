using Microsoft.VisualStudio.TestTools.UnitTesting;

using Baku.Quma.Low.Api;
using Baku.Quma.Low;

namespace TestQumarionDotNet.Low
{

    /// <summary>ベース処理のうちデバイスの取得が不要なものをテストします。</summary>
    [TestClass]
    public class LowBaseOperationTest
    {
        [TestMethod]
        public void Low_初期化()
        {
            var res = QmLow.BaseOperation.Initialize();
            Assert.IsTrue(res == QumaLowResponse.OK);

            //終了テストは廃止: テストのラスト以外でExit関数を呼ぶとなんかリークしてそうなリスクがあるので
            //res = QmLow.BaseOperation.Exit();
            //Assert.IsTrue(res == QumaLowResponse.OK);
        }

        [TestMethod]
        public void Low_ライブラリバージョン確認()
        {
            using (QumaContext.Create())
            {
                string version = QmLow.BaseOperation.GetVersion();
                //若干適当なチェックだけど、まあ、いいでしょ別に。
                Assert.IsTrue(
                    version == "1.0.2.0b x86 QD Ver 2107" ||
                    version == "1.0.2.0b x64 QD Ver 2107"
                    );

                //テスト通らない時はコレでバージョンチェックして挙動確認
                //System.Windows.MessageBox.Show($"version = {version}");
            }
        }

        [TestMethod]
        public void Low_デバッグ出力設定()
        {
            using (QumaContext.Create())
            {
                QmLow.BaseOperation.SetDebugWrite(true);
                QmLow.BaseOperation.SetDebugWrite(false);
            }
        }

        [TestMethod]
        public void Low_座標系設定()
        {
            using (QumaContext.Create())
            {
                QmLow.BaseOperation.SetCoordinateSystem(CoordinateSystem.RightHand);
                QmLow.BaseOperation.SetCoordinateSystem(CoordinateSystem.LeftHand);
            }
        }

        [TestMethod]
        public void Low_回転方向設定()
        {
            using (QumaContext.Create())
            {
                QmLow.BaseOperation.SetRotateDirection(RotateDirection.Left);
                QmLow.BaseOperation.SetRotateDirection(RotateDirection.Right);
            }
        }
    }

}
