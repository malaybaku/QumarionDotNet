
namespace Baku.Quma.Pdk
{
    /// <summary>モデルのハンドルを表します。</summary>
    public class ModelHandle
    {
        internal ModelHandle(int handle)
        {
            Handle = handle;
        }
        public int Handle { get; }

        /// <summary>不正なハンドル値(未設定であることを表す)</summary>
        public const int ModelHandleError = -1;

    }

    /// <summary>QUMAデバイスのハンドルを表します。</summary>
    public class QumaHandle
    {
        internal QumaHandle(int handle)
        {
            Handle = handle;
        }
        public int Handle { get; }

        /// <summary>不正なQUMAデバイスハンドル値(未設定であることを表す)</summary>
        public const int QumaHandleError = -1;
    }
}
