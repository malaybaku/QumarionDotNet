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




    }
}
