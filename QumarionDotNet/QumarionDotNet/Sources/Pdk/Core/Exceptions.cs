using System;

namespace Baku.Quma.Pdk
{

    /// <summary>APIの呼び出しに失敗した事を示す例外を表します。</summary>
    public class QmPdkException : Exception
    {
        internal QmPdkException(QmErrorCode errorCode)
        {
            ErrorCode = errorCode;
        }

        /// <summary>エラーの内容をエラーコードとして取得します。</summary>
        public QmErrorCode ErrorCode { get; }
    }

}
