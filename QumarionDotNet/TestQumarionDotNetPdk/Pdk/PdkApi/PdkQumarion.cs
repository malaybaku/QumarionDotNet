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
            var quma = QumarionLoader.Load();
        }

        [TestMethod]
        public void Pdk_デバイス必須_アタッチ済みモデル数チェック()
        {
            var quma = QumarionLoader.Load();
            int count = QmPdk.Quma.GetAttachedModelCount(quma);
            Assert.IsTrue(count >= 0);
        }

        [TestMethod]
        public void Pdk_デバイス必須_デバイス名取得()
        {
            var quma = QumarionLoader.Load();
            string deviceName = QmPdk.Quma.GetDeviceName(quma);
            Assert.IsFalse(string.IsNullOrEmpty(deviceName));
        }

        [TestMethod]
        public void Pdk_デバイス必須_デバイスのイネーブル設定()
        {
            var quma = QumarionLoader.Load();
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
            var quma = QumarionLoader.Load();
            QmPdk.Quma.SetEnableAccelerometer(quma, true);
            QmPdk.Quma.SetEnableAccelerometer(quma, false);
        }

        [TestMethod]
        public void Pdk_デバイス必須_ボタン状態_オフ_取得()
        {
            var quma = QumarionLoader.Load();
            var button = QmPdk.Quma.GetButtonState(quma);
            Assert.AreEqual(QumaButtonState.Up, button);
        }

        [TestMethod]
        public void Pdk_デバイス必須_デバイス状態確認()
        {
            var quma = QumarionLoader.Load();
            QmPdk.Quma.CheckDeviceValidity(quma);
        }

        [TestMethod]
        public void Pdk_デバイス必須_センサー状態確認()
        {
            var quma = QumarionLoader.Load();
            var state = QmPdk.Quma.CheckDeviceSensors(quma);
            Assert.IsTrue(state.IsOk);
        }

        //NOTE: 以下の関数はCharacterModelと連携するのでこのソースファイルでは検証しない
        //QmPdk.Quma.AttachInitPoseModel
        //QmPdk.Quma.DetachModel
    }

    public static class QumarionLoader
    {
        public static QumaHandle Load()
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
