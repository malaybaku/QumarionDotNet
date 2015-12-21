using System.Runtime.InteropServices;

using Baku.Quma.Low.Api;

namespace Baku.Quma.Low
{
    /// <summary>デバイスの種類とIDを含むデバイス情報を表します。</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct QumaId
    {
        /// <summary>デバイスの種類です。</summary>
        public QumaTypes QumaType;
        /// <summary>デバイスのIDです。</summary>
        public int Id;
    }

    /// <summary>APIが列挙したデバイスを受け取るための構造体を表します。</summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct QumaIds
    {
        /// <summary>APIが列挙したデバイスを受け取るための配列です。</summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = QmLow.ApiConstants.ID_BUFFER_SIZE)]
        public QumaId[] Ids;
    }
}
