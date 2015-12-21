
namespace Baku.Quma.Low
{
    /// <summary>接続してるQumaの種類を表します。</summary>
    public enum QumaTypes
    {
        Software = 0, //ソフトウェアエミュレータ
        HardwareFirst = 1, //初号機
        HardwarePrototype = 2, //弐号機
        HardwareCustom = 3, //プロトタイプQuma
        HardwareAsai = 4, //Qumarion(基本コレ?)
    }

    /// <summary>成功/失敗が返ってくるタイプのコード一覧です。</summary>
    public enum QumaLowResponse
    {
        OK = 1,                  //正常
        ERR = -1,                //不明なエラー
        INFO_CHANGED = -2,       //Qumaのデバイス情報が更新されています
        INFO_NOT_ACTIVATED = -3, //Qumaがアクティベートされていません
        INFO_NO_HISTORY = -4,    //ヒストリバッファーに情報がありません.
        CRYPTLIB_INIT_FAILED = -5,                       //内部で使用されている暗号化ライブラリの初期化に失敗しました.
        BUTTON_NOT_EXIST = -101,                         //Qumaにボタンが存在しません（未使用）
        QUMAARM_NOT_CONNECTED = -301,                    //Qumaアームが接続されていません（未使用）
        ERROR_CUSTOMSET_FAILD_FILE_OPEN = -1000,         //Qumaカスタム設定ファイルのオープンに失敗しました
        ERROR_CUSTOMSET_FAILD_FILE_IS_NOT_EXIST = -1001, //Qumaカスタム設定ファイルが存在しません
        ERROR_CUSTOMSET_FORMAT_ERROR = -1010,            //Qumaカスタム設定ファイルのフォーマットが異なります
        ERROR_NOT_INITIALIZED = -10000,                  //Qumaライブラリが初期化されていません
    }

    /// <summary>ライブラリで用いる座標系を表します。デフォルトでは左手系が用いられます。</summary>
    public enum CoordinateSystem
    {
        /// <summary>左手系座標</summary>
        LeftHand,
        /// <summary>右手系座標</summary>
        RightHand
    }

    /// <summary>ライブラリが用いる回転方向を表します。デフォルトでは右回転が正として扱われます。</summary>
    public enum RotateDirection
    {
        /// <summary>右回転を正とする</summary>
        Right,
        /// <summary>左回転を正とする</summary>
        Left
    }

    /// <summary>センサーの状態を表します。</summary>
    public enum SensorStates
    {
        /// <summary>正常動作</summary>
        OK = 0,
        /// <summary>接続できていない</summary>
        NotConnected = -1,
        /// <summary>CPUと接続できていない</summary>
        NotCpuConnected = -2,
        /// <summary>ボードと接続できていない</summary>
        NotOptionBoardConnected = -3,
        /// <summary>外れ値を検出した</summary>
        Outlier = -4,
        /// <summary>想定外のエラー</summary>
        UnknownError = -100
    }

    /// <summary>ボタンの状態を表します。</summary>
    public enum ButtonState
    {
        /// <summary>ボタンが押されていない状態</summary>
        Up = 0,
        /// <summary>ボタンが押されている状態</summary>
        Down = 1
    }

    /// <summary>Qumaで利用可能なボタンの一覧です。現在MainButtonのみが存在します。</summary>
    public enum ButtonType
    {
        /// <summary>Qumarionのメインボタン。現時点では唯一のボタンです。</summary>
        MainButton
    }

}
