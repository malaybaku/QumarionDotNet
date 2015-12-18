using Microsoft.VisualStudio.TestTools.UnitTesting;

using Baku.Quma;
using Baku.Quma.Low;

namespace TestQumarionDotNet
{
    /// <summary>
    /// 状態更新の関数をテストします。
    /// </summary>
    [TestClass]
    public class LowUpdate
    {
        static readonly QumaTypes TargetType = QumaTypes.Software;

        [TestMethod]
        public void Low_通常更新1()
        {
            using (var context = QumaActiveDeviceContext.Create(TargetType))
            {
                QmLow.Update.UpdateBuffer(context.QumaHandle);
            }
        }
        [TestMethod]
        public void Low_通常更新2()
        {
            using (var context = QumaActiveDeviceContext.Create(TargetType))
            {
                var res = QmLow.Update.TryUpdateBuffer(context.QumaHandle);
                Assert.AreEqual(QumaLowResponse.OK, res);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(QumaException))]
        public void Low_デバイス更新1_正常系で例外投げ()
        {
            using (var context = QumaActiveDeviceContext.Create(TargetType))
            {
                QmLow.Update.UpdateQumaHandle(context.QumaHandle);
            }
        }
        [TestMethod]
        public void Low_デバイス更新2_正常系でエラーコード()
        {
            using (var context = QumaActiveDeviceContext.Create(TargetType))
            {
                var res = QmLow.Update.TryUpdateQumaHandle(context.QumaHandle);
                Assert.AreNotEqual(QumaLowResponse.OK, res);
            }
        }

    }
}
