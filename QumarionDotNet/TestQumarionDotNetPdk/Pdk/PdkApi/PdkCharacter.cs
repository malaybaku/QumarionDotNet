using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baku.Quma.Pdk.Api;
using Baku.Quma.Pdk;

namespace TestQumarionDotNet.Pdk
{
    [TestClass]
    public class PdkCharacter
    {
        [TestMethod]
        public void Pdk_標準PSボーン取得および解放()
        {
            QmPdk.BaseOperation.Initialize();
            var indexedModelHandle = QmPdk.Character.CreateStandardModelPS();
            Assert.AreNotEqual(IntPtr.Zero, indexedModelHandle.ModelHandle);
            Assert.AreEqual(66, indexedModelHandle.Indexes.Length);

            QmPdk.Character.Destroy(indexedModelHandle.ModelHandle);
        }

        [TestMethod]
        public void Pdk_モデル総数チェック()
        {
            QmPdk.BaseOperation.Initialize();
            int modelCountBeforeCreate = QmPdk.Character.GetNumOfHandle();

            var indexedModelHandle = QmPdk.Character.CreateStandardModelPS();

            int modelCount = QmPdk.Character.GetNumOfHandle();

            Assert.AreEqual(modelCount, modelCountBeforeCreate + 1);

            QmPdk.Character.Destroy(indexedModelHandle.ModelHandle);

            modelCount = QmPdk.Character.GetNumOfHandle();

            Assert.AreEqual(modelCount, modelCountBeforeCreate);
        }

        [TestMethod]
        public void Pdk_標準ボーンのボーン名取得()
        {
            QmPdk.BaseOperation.Initialize();
            var indexedModelHandle = QmPdk.Character.CreateStandardModelPS();
            var names = QmPdk.Character.GetName(indexedModelHandle.ModelHandle, 0);
            Assert.AreEqual("hips_bb_", names.Name);

            QmPdk.Character.Destroy(indexedModelHandle.ModelHandle);
        }


        //NOTE: キャラモデルのラッパークラス側でほぼ直接検証するので
        //      以下の関数はテストしない
        //QmPdk.Character.SetLocalMatrix
        //QmPdk.Character.GetLocalMatrix


        //NOTE: QUMARION SDKのサンプルで使用されておらず、
        //      本ラッパーでもそんなに重視していないため以下の関数をテストしない
        //QmPdk.Character.SetRotate
        //QmPdk.Character.GetRotate
        //QmPdk.Character.MemorizeInitialPose
        //QmPdk.Character.RecallInitialPose


    }
}
