using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Baku.Quma.Pdk;
using Baku.Quma.Pdk.Api;

namespace TestQumarionDotNet.Pdk
{
    [TestClass]
    public class PdkTemplateBone
    {
        [TestMethod]
        public void Pdk_テンプレ_ルートとルートの名前取得()
        {
            using (var context = new TemplateContext())
            {
                int rootIndex = QmPdk.TemplateBone.GetRootNodeIndex(context.ModelHandle);
                Assert.AreEqual(0, rootIndex);

                string rootName = QmPdk.TemplateBone.GetName(context.ModelHandle, rootIndex);
                Assert.AreEqual("hips_bb_", rootName);
            }
        }

        [TestMethod]
        public void Pdk_テンプレ_子ボーン取得()
        {
            using (var context = new TemplateContext())
            {
                int rootIndex = QmPdk.TemplateBone.GetRootNodeIndex(context.ModelHandle);
                int[] childs = QmPdk.TemplateBone.GetChildNodeIndex(context.ModelHandle, rootIndex);
                Assert.AreEqual(4, childs.Length);
                //NOTE: 若干やりすぎ感があるが、呼び出しサンプルも兼ねて。
                Assert.AreEqual(2, childs[0]);
                Assert.AreEqual(1, childs[1]);
                Assert.AreEqual(56, childs[2]);
                Assert.AreEqual(61, childs[3]);
                Assert.AreEqual("hips_end_bb_", QmPdk.TemplateBone.GetName(context.ModelHandle, childs[0]));
                Assert.AreEqual("spine_bb_", QmPdk.TemplateBone.GetName(context.ModelHandle, childs[1]));
                Assert.AreEqual("leftupleg_bb_", QmPdk.TemplateBone.GetName(context.ModelHandle, childs[2]));
                Assert.AreEqual("rightupleg_bb_", QmPdk.TemplateBone.GetName(context.ModelHandle, childs[3]));
            }
        }

        [TestMethod]
        public void Pdk_テンプレ_親ボーン取得()
        {
            using (var context = new TemplateContext())
            {
                int rootIndex = QmPdk.TemplateBone.GetRootNodeIndex(context.ModelHandle);
                int[] childs = QmPdk.TemplateBone.GetChildNodeIndex(context.ModelHandle, rootIndex);

                int rootIndexAsParent = QmPdk.TemplateBone.GetParentNodeIndex(context.ModelHandle, childs[0]);
                Assert.AreEqual(rootIndex, rootIndexAsParent);
            }
        }

    }

    /// <summary>テスト用にモデルハンドルの生成と削除をやってくれるコンテクスト</summary>
    public class TemplateContext : IDisposable
    {
        public TemplateContext()
        {
            QmPdk.BaseOperation.Initialize();

            ModelHandle = QmPdk.Character.CreateStandardModelPS().ModelHandle;
        }

        public ModelHandle ModelHandle { get; }

        public void Dispose()
        {
            QmPdk.Character.Destroy(ModelHandle);
        }
    }

}
