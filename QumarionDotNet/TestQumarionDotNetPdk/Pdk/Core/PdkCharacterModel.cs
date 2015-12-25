using Baku.Quma.Pdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace TestQumarionDotNet.Pdk
{
    [TestClass]
    public class PdkCharacterModelTest
    {
        [TestMethod]
        public void Pdk高水準_デバイス必須_標準モデルロードとプロパティチェック()
        {
            using (var model = PdkManager.CreateStandardModelPS())
            {
                Assert.IsNull(model.AttachedQumarion);

                model.AttachQumarion(PdkManager.GetDefaultQumarion());
                Assert.IsNotNull(model.AttachedQumarion);

                model.AccelerometerMode = AccelerometerMode.Direct;
                Assert.AreEqual(AccelerometerMode.Direct, model.AccelerometerMode);
                model.AccelerometerMode = AccelerometerMode.Relative;
                Assert.AreEqual(AccelerometerMode.Relative, model.AccelerometerMode);
            }
        }

        [TestMethod]
        public void Pdk高水準_デバイス必須_標準モデルのUpdate処理呼び出し()
        {
            //NOTE: ユーザプログラマが行う最小処理もこんな感じ
            using (var model = PdkManager.CreateStandardModelPS())
            {
                model.AttachQumarion(PdkManager.GetDefaultQumarion());

                model.Update();

                var boneMatrices = model
                    .Bones
                    .Select(kvp => kvp.Value.LocalMatrix)
                    .ToArray();

                var boneValues = boneMatrices.Select(bm => bm.GetValues()).ToArray();

            }
        }
    }
}
