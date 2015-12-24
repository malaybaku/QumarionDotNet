using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Baku.Quma.Pdk;
using Baku.Quma.Pdk.Api;

namespace TestQumarionDotNet.Pdk
{
    [TestClass]
    public class PdkQumarion
    {

        [TestMethod]
        public void Pdk_Qumarionデバイス総数取得()
        {
            QmPdk.BaseOperation.Initialize();
            int count = QmPdk.Quma.GetDeviceCount();
            //一般に成り立つのはこの程度なので精査は諦める
            Assert.IsTrue(count >= 0);
        }

        [TestMethod]
        public void Pdk_デバイス必須_デフォルトデバイス取得()
        {
            var quma = LoadQumaHandle();
        }

        [TestMethod]
        public void Pdk_デバイス必須_アタッチ済みモデル数チェック()
        {
            var quma = LoadQumaHandle();
            int count = QmPdk.Quma.GetAttachedModelCount(quma);
            Assert.IsTrue(count >= 0);
        }

        [TestMethod]
        public void Pdk_デバイス必須_デバイス名取得()
        {
            var quma = LoadQumaHandle();
            string deviceName = QmPdk.Quma.GetDeviceName(quma);
            Assert.IsFalse(string.IsNullOrEmpty(deviceName));
        }

        [TestMethod]
        public void Pdk_デバイス必須_デバイスのイネーブル設定()
        {
            var quma = LoadQumaHandle();
            QmPdk.Quma.SetEnableQuma(quma, false);
            bool isEnabled = QmPdk.Quma.GetEnableQuma(quma);
            Assert.AreEqual(false, isEnabled);

            QmPdk.Quma.SetEnableQuma(quma, true);
            isEnabled = QmPdk.Quma.GetEnableQuma(quma);
            Assert.AreEqual(true, isEnabled);

        }

        [TestMethod]
        public void Pdk_デバイス必須_デバイス加速度センサ設定()
        {
            var quma = LoadQumaHandle();
            QmPdk.Quma.SetEnableAccelerometer(quma, true);
            QmPdk.Quma.SetEnableAccelerometer(quma, false);
        }

        [TestMethod]
        public void Pdk_デバイス必須_ボタン状態_オフ_取得()
        {
            var quma = LoadQumaHandle();
            var button = QmPdk.Quma.GetButtonState(quma);
            Assert.AreEqual(QumaButtonState.Up, button);
        }

        [TestMethod]
        public void Pdk_デバイス必須_ポーズ変化チェック_関数呼び出しのみ()
        {
            //NOTE: CheckIsPoseChanged関数は見た感じだと戻り値がイマイチ信用できない気がする
            var quma = LoadQumaHandle();
            QmPdk.Quma.CheckIsPoseChanged(quma);
        }

        [TestMethod]
        public void Pdk_デバイス必須_デバイス状態確認()
        {
            var quma = LoadQumaHandle();
            QmPdk.Quma.CheckDeviceValidity(quma);
        }

        [TestMethod]
        public void Pdk_デバイス必須_センサー状態確認()
        {
            var quma = LoadQumaHandle();
            var state = QmPdk.Quma.CheckDeviceSensors(quma);
            Assert.IsTrue(state.IsOk);
        }

        static QumaHandle LoadQumaHandle()
        {
            QmPdk.BaseOperation.Initialize();
            if (QmPdk.Quma.GetDeviceCount() == 0)
            {
                throw new InvalidOperationException("Qumarion is not connected to this machine");
            }

            return QmPdk.Quma.GetHandle(0);
        }

    }

}
