using System;

namespace Baku.Quma
{
    /// <summary>Qumaのデバイスハンドルを表します。</summary>
    public sealed class QumaHandle
    {
        /// <summary>実際のハンドルを表すポインタでインスタンスを初期化します。</summary>
        /// <param name="handle">APIがデバイスハンドルとして渡すポインタ</param>
        internal QumaHandle(IntPtr handle)
        {
            Handle = handle;
        }

        /// <summary>ハンドルがAPIの関数にアクセスする際に用いるポインタを取得します。</summary>
        public IntPtr Handle { get; }
    }

    /// <summary>ボーンのハンドルを表します。</summary>
    public sealed class BoneHandle
    {
        /// <summary>実際のハンドルを表すポインタでインスタンスを初期化します。</summary>
        /// <param name="handle">APIがボーンのハンドルとして渡すポインタ</param>
        internal BoneHandle(IntPtr handle)
        {
            Handle = handle;
        }

        /// <summary>ボーンに関するAPI関数へアクセスする際に用いるポインタを取得します。</summary>
        public IntPtr Handle { get; }
    }

    /// <summary>センサーのハンドルを表します。</summary>
    public sealed class SensorHandle
    {
        /// <summary>実際のハンドルを表すポインタでインスタンスを初期化します。</summary>
        /// <param name="handle">APIがセンサーのハンドルとして渡すポインタ</param>
        internal SensorHandle(IntPtr handle)
        {
            Handle = handle;
        }

        /// <summary>センサーに関するAPI関数へアクセスする際に用いるポインタを取得します。</summary>
        public IntPtr Handle { get; }
    }

}
