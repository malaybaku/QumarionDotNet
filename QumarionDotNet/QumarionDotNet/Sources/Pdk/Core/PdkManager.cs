using Baku.Quma.Pdk.Api;

namespace Baku.Quma.Pdk
{
    /// <summary>
    /// 高水準側APIのインスタンス生成処理を定義します。
    /// APIで必要となる初期化処理の実行を保障するため、
    /// ライブラリの初期化を定義します。
    /// このクラスから初期化を行う事で主要クラスの関数が利用可能になります。
    /// </summary>
    public static class PdkManager
    {
        /// <summary>ライブラリが初期化済みであるかどうかを取得します。</summary>
        public static bool Initialized => QmPdk.BaseOperation.Initialized;

        /// <summary>ライブラリを初期化します。</summary>
        public static void Initialize() => QmPdk.BaseOperation.Initialize();

        /// <summary>Qumaデバイス側にポーズ変更がない場合でもポーズのコピーを行うかどうかを設定します。</summary>
        public static bool ForceCopyPose
        {
            set
            {
                Initialize();
                QmPdk.BaseOperation.SetForcedCopyPose(value);
            }
        }

        /// <summary>あらかじめ定義されている標準人型ボーンをロードします。</summary>
        /// <returns>ライブラリで定義された標準人型ボーン</returns>
        public static CharacterModel CreateStandardModelPS()
        {
            Initialize();
            return StandardCharacterModel.CreateStandardModelPS();
        }

        /// <summary>マシンに接続されているQumarionデバイスの総数を取得します。</summary>
        public static int ConnectedDeviceCount
        {
            get
            {
                Initialize();
                return Qumarion.ConnectedDeviceCount;
            }
        }

        /// <summary>
        /// 利用可能なQumarionの一つを取得します。複数のQumarionからデバイス選んで接続したい場合
        /// <see cref="GetQumarionByIndex(int)"/>の使用を検討してください。
        /// </summary>
        /// <returns>Qumarionデバイス(一つも見つからなければ<see cref="InvalidOperationException"/>がスローされます)</returns>
        /// <exception cref="InvalidOperationException" />
        public static Qumarion GetDefaultDevice() => GetQumarionByIndex(0);

        /// <summary>
        /// デバイスをインデックス指定によって取得します。
        /// </summary>
        /// <param name="index">インデックス。値は0以上、かつ(<see cref="GetConnectedDeviceCount"/>の戻り値 -1)以下である必要があります。</param>
        /// <returns>指定されたQumarionデバイス</returns>
        public static Qumarion GetQumarionByIndex(int index)
        {
            Initialize();
            return Qumarion.GetQumarionByIndex(index);
        }

    }
}
