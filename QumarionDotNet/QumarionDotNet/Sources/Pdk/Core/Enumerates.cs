
namespace Baku.Quma.Pdk
{

    /// <summary>QmPdk APIのエラーコード一覧です。</summary>
    public enum QmErrorCode
    {
        /// <summary>エラーなし</summary>
        NoError = 0,
        /// <summary>内部エラー</summary>
        Error = 1,
        /// <summary>メモリ確保エラー</summary>
        MemoryAlloc = 2,
        /// <summary>引数不正(NULLなど)</summary>
        Parameter = 3,
        /// <summary>QUMAが正しく初期化されていない</summary>
        NoQuma = 10,
        /// <summary>QUMAのデバイスハンドルが不正</summary>
        InvalidDevice = 11,
        /// <summary>QUMAからデータが取得できない(USBが抜けた、など)</summary>
        DeviceUpdateBuffer = 12,
        /// <summary>デバイスを別プロセスで使用中</summary>
        DeviceOtherProcess = 13,
        /// <summary>モデルハンドルが不正</summary>
        InvalidModel = 20,
        /// <summary>templateモデルが不正</summary>
        InvalidModelTemplate = 21,
        /// <summary>デバイスにアタッチされていない</summary>
        NotAttachedToDevice = 22,
        /// <summary>templateモデルのノードが不正</summary>
        InvalidNodeTemplate = 26,
        /// <summary>NNB情報が不正であるか、存在しない</summary>
        InvalidNNB = 30,
        /// <summary>マッピングダイアログが既に開かれている</summary>
        MappingDlgAlreadyOpen = 35,
        /// <summary>QUMA情報セーブエラー</summary>
        SaveQuma = 50,
        /// <summary>NNB情報セーブエラー</summary>
        SaveNNB = 51,
        /// <summary>ライセンスエラー</summary>
        License = 60,
        /// <summary>ライセンス有効期限切れ</summary>
        LicenseExpired = 61,
        /// <summary>ライセンスエラー無効なデバイス</summary>
        LicenseInvalidDevice = 62
    }

    /// <summary>ボタンの状態を表します。</summary>
    public enum QumaButtonState
    {
        /// <summary>ボタンが押されていない状態</summary>
        Up = 0,
        /// <summary>ボタンが押されている状態</summary>
        Down = 1
    }

    /// <summary>モデルが加速度センサーから姿勢を算出する方法の一覧です。</summary>
    public enum AccelerometerMode
    {
        /// <summary>モデルがデバイスにアタッチされていません。</summary>
        NotAttached = 0,
        /// <summary>事前のフィルタリング処理を行いません。</summary>
        Direct = 1,
        /// <summary>フィルタ処理を行います。</summary>
        Relative = 2
    }

    /// <summary>加速度センサ回転の制約モードの一覧です。</summary>
    public enum AccelerometerRestrictMode
    {
        /// <summary>制限なし</summary>
        None = 0,
        /// <summary>X軸回転に制限</summary>
        AxisX = 1,
        /// <summary>Z軸回転に制限</summary>
        AxisZ = 2
    }

    /// <summary>デバイスのボタン一覧です。実際にはボタンは一つのみです。</summary>
    internal enum QumaButton
    {
        /// <summary>QUMARIONの背面ボタン</summary>
        MainButton = 0
    }

}
