using System;

namespace Baku.Quma.Low
{
    /// <summary>QumaのAPI呼び出しが失敗した場合にスローされる例外です。</summary>
    public sealed class QumaException : Exception
    {
        /// <summary>APIからのエラーコードを用いてインスタンスを初期化します。</summary>
        /// <param name="response">APIが返してきたエラーコード</param>
        internal QumaException(QumaLowResponse response)
        {
            Response = response;
        }

        /// <summary>APIが返したエラーコードを取得します。</summary>
        public QumaLowResponse Response { get; }
    }
}
