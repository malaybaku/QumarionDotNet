using Microsoft.VisualStudio.TestTools.UnitTesting;

using Baku.Quma.Pdk.Api;

namespace TestQumarionDotNet.Pdk
{
    [TestClass]
    public class PdkBaseOperation
    {
        [TestMethod]
        public void Pdk_バージョン取得()
        {
            string version = QmPdk.BaseOperation.GetVersionStr();
            Assert.AreEqual("Version 1.1.0 RELEASE WIN32   Dec 10 2012 18:46:34", version);
        }

        [TestMethod]
        public void Pdk_初期化()
        {
            QmPdk.BaseOperation.Initialize();
            Assert.AreEqual(true, QmPdk.BaseOperation.Initialized);
        }

        [TestMethod]
        public void Pdk_デバッグフラグ立て()
        {
            //NOTE: 初期化より前にデバッグフラグを立てにいくとAccessViolationが飛んでくる
            QmPdk.BaseOperation.Initialize();
            QmPdk.Debug.SetDebugFlags();
        }

        [TestMethod]
        public void Pdk_強制コピー設定()
        {
            QmPdk.BaseOperation.Initialize();
            QmPdk.BaseOperation.SetForcedCopyPose(true);
            QmPdk.BaseOperation.SetForcedCopyPose(false);
        }

        

    }
}
