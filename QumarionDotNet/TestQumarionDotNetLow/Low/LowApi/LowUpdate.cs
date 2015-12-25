using Microsoft.VisualStudio.TestTools.UnitTesting;

using Baku.Quma.Low;
using Baku.Quma.Low.Api;

namespace TestQumarionDotNet.Low
{
    /// <summary>
    /// 状態更新の関数をテストします。
    /// </summary>
    [TestClass]
    public class LowUpdateTest
    {

        [TestMethod]
        public void Low_通常更新1()
        {
            using (var context = new QumaActiveDeviceContext())
            {
                QmLow.Update.UpdateBuffer(context.QumaHandle);
            }
        }
        [TestMethod]
        public void Low_通常更新2()
        {
            using (var context = new QumaActiveDeviceContext())
            {
                var res = QmLow.Update.TryUpdateBuffer(context.QumaHandle);
                Assert.AreEqual(QumaLowResponse.OK, res);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(QmLowException))]
        public void Low_デバイス更新1_正常系で例外投げ()
        {
            using (var context = new QumaActiveDeviceContext())
            {
                QmLow.Update.UpdateQumaHandle(context.QumaHandle);
            }
        }
        [TestMethod]
        public void Low_デバイス更新2_正常系でエラーコード()
        {
            using (var context = new QumaActiveDeviceContext())
            {
                var res = QmLow.Update.TryUpdateQumaHandle(context.QumaHandle);
                Assert.AreNotEqual(QumaLowResponse.OK, res);
            }
        }

    }
}
