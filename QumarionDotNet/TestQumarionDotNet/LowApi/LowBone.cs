using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Baku.Quma.Low.Api;
using Baku.Quma.Low;

namespace TestQumarionDotNet
{
    /// <summary>
    /// ボーン関連のテストです。
    /// 全身についてはラッパクラスQumarionDevice/QumarionBoneで
    /// テストする方が効率的なため、ここではルートボーンを中心としたテストのみを行います。
    /// </summary>
    [TestClass]
    public class LowBone
    {
        //このクラスのテストで用いるQumaのデバイス種類を指定します。
        public static readonly QumaTypes TargetType = QumaTypes.Software;

        [TestMethod]
        public void Low_ルートボーン取得()
        {
            using (var context = QumaActiveDeviceContext.Create(TargetType))
            {
                var boneHandle = QmLow.Bone.GetRootBone(context.QumaHandle);
                Assert.AreNotEqual(IntPtr.Zero, boneHandle.Handle);
            }
        }

        [TestMethod]
        public void Low_ボーン名取得()
        {
            using (var context = QumaRootBoneContext.Create(TargetType))
            {
                string name = QmLow.Bone.GetBoneName(context.RootBoneHandle);
                //弱い検証: とりあえず名前入ってればOKというケース
                //Assert.IsFalse(string.IsNullOrWhiteSpace(name));
                //強いテスト
                Assert.AreEqual("Root", name);
            }
        }

        [TestMethod]
        public void Low_子ボーン名取得Try_正常系()
        {
            using (var context = QumaRootBoneContext.Create(TargetType))
            {
                BoneHandle result;
                bool success = QmLow.Bone.TryGetBoneByName(context.RootBoneHandle, "Head", out result);
                Assert.IsTrue(success);
                Assert.IsNotNull(result);
            }
        }
        [TestMethod]
        public void Low_子ボーン名取得Try_異常系()
        {
            using (var context = QumaRootBoneContext.Create(TargetType))
            {
                BoneHandle result;
                bool success = QmLow.Bone.TryGetBoneByName(context.RootBoneHandle, "SomeWrongBoneName", out result);
                Assert.IsFalse(success);
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void Low_子ボーン名取得_正常系()
        {
            using (var context = QumaRootBoneContext.Create(TargetType))
            {
                BoneHandle result = QmLow.Bone.GetBoneByName(context.RootBoneHandle, "Head");
                Assert.IsNotNull(result);
                Assert.AreNotEqual(IntPtr.Zero, result.Handle);
            }
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Low_子ボーン名取得_異常系()
        {
            using (var context = QumaRootBoneContext.Create(TargetType))
            {
                BoneHandle result = QmLow.Bone.GetBoneByName(context.RootBoneHandle, "SomeWrongBoneName");
            }
        }

        [TestMethod]
        public void Low_子ボーンの個数取得()
        {
            using (var context = QumaRootBoneContext.Create(TargetType))
            {
                int childCount = QmLow.Bone.GetChildCount(context.RootBoneHandle);
                //ソフトウェアで確認した範囲ではWaist_V(脚側)とWaist_H(胸側)が子要素
                Assert.AreEqual(2, childCount);
            }
        }

        [TestMethod]
        public void Low_子ボーンのインデクスベース取得_正常系()
        {
            using (var context = QumaRootBoneContext.Create(TargetType))
            {
                BoneHandle child = QmLow.Bone.GetChildBone(context.RootBoneHandle, 0);
                Assert.AreNotEqual(IntPtr.Zero, child.Handle);
            }
        }

        [TestMethod]
        public void Low_子ボーンのインデクスベース取得_異常系()
        {
            using (var context = QumaRootBoneContext.Create(TargetType))
            {
                BoneHandle child = QmLow.Bone.GetChildBone(context.RootBoneHandle, 10);
                //NOTE: ぬるぽ以前に例外飛んでくる可能性もあるよねコレ。
                Assert.AreEqual(IntPtr.Zero, child.Handle);
            }
        }

        [TestMethod]
        public void Low_子ボーンの一覧取得()
        {
            using (var context = QumaRootBoneContext.Create(TargetType))
            {
                int count = QmLow.Bone.GetChildCount(context.RootBoneHandle);
                BoneHandle[] childs = QmLow.Bone.GetChildBones(context.RootBoneHandle);

                Assert.AreEqual(count, childs.Length);
            }
        }

        //NOTE: 有効な値を検証するにはUpdateBuffer関数を組み合わせた検証が必要
        [TestMethod]
        public void Low_ボーンの位置取得_関数呼び出しのみ()
        {
            using (var context = QumaRootBoneContext.Create(TargetType))
            {
                Vector3 pos = QmLow.Bone.GetBonePosition(context.RootBoneHandle);
            }
        }

        [TestMethod]
        public void Low_ボーンの行列取得_関数呼び出しのみ()
        {
            using (var context = QumaRootBoneContext.Create(TargetType))
            {
                Matrix4 mat = QmLow.Bone.ComputeBoneMatrix(
                    context.QumaHandle,
                    context.RootBoneHandle
                    );
            }
        }

    
    }
}
