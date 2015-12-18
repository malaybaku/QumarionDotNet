using Microsoft.VisualStudio.TestTools.UnitTesting;

using Baku.Quma;
using Baku.Quma.Low;

namespace TestQumarionDotNet
{
    /// <summary>
    /// ボタンの状態チェック関数をテストします。
    /// </summary>
    [TestClass]
    public class LowButton
    {
        [TestMethod]
        public void Low_ボタン状態確認1()
        {
            using (var context = QumaActiveDeviceContext.Create(QumaTypes.Software))
            {
                var buttonState = QmLow.Button.GetState(context.QumaHandle);
                Assert.AreEqual(ButtonState.Up, buttonState);
            }
        }

        [TestMethod]
        public void Low_ボタン状態確認2()
        {
            using (var context = QumaActiveDeviceContext.Create(QumaTypes.Software))
            {
                ButtonState buttonState;
                var response = QmLow.Button.TryGetState(context.QumaHandle, out buttonState);
                Assert.AreEqual(QumaLowResponse.OK, response);
                Assert.AreEqual(ButtonState.Up, buttonState);
            }
        }
    }
}
