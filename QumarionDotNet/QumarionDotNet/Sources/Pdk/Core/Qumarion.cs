using System;

using Baku.Quma.Pdk.Api;

namespace Baku.Quma.Pdk
{
    /// <summary>
    /// QUMARIONデバイスを表します。同名クラスである<see cref="Low.Qumarion"/>とは用途が大きく異なることに注意してください。
    /// </summary>
    public class Qumarion
    {
        private Qumarion(QumaHandle qumaHandle)
        {
            QumaHandle = qumaHandle;
            Name = QmPdk.Quma.GetDeviceName(QumaHandle);
        }

        /// <summary>ラップされたAPIにアクセスする際に用いるハンドルを取得します。</summary>
        public QumaHandle QumaHandle { get; }

        /// <summary>デバイスに対応する名前を取得します。</summary>
        public string Name { get; }

        /// <summary>このデバイスに関連付けられているモデルの総数</summary>
        public int AttachedModelCount => QmPdk.Quma.GetAttachedModelCount(QumaHandle);

        /// <summary>デバイスからモデルへのポーズコピーが有効であるかを取得、設定します。</summary>
        public bool Enable
        {
            get { return QmPdk.Quma.GetEnableQuma(QumaHandle); }
            set { QmPdk.Quma.SetEnableQuma(QumaHandle, value); }
        }

        /// <summary>加速度センサーが有効であるかを設定します。</summary>
        public bool EnableAccelerometer
        {
            set { QmPdk.Quma.SetEnableAccelerometer(QumaHandle, value); }
        }

        /// <summary>ボタンの状態を取得します。</summary>
        /// <returns>ボタンの状態</returns>
        public QumaButtonState ButtonState => QmPdk.Quma.GetButtonState(QumaHandle);

        /// <summary>
        /// ポーズが前回から変化したかどうかを取得します。
        /// [CAUTION]軽くテストした感じによると関数の戻り値がtrue/falseになる基準がイマイチ分からない
        /// </summary>
        /// <returns>ポーズが変化したかどうか</returns>
        public bool CheckPoseChanged() => QmPdk.Quma.CheckIsPoseChanged(QumaHandle);

        /// <summary>
        /// デバイスの状態を確認します。
        /// 正常系では何も起きず、異常系の場合は<see cref="QmPdkException"/>がスローされます。
        /// エラーコードが<see cref="QmErrorCode.DeviceUpdateBuffer"/>だった場合、USB挿抜などで
        /// 接続が外れているのでデバイスの再初期化が必要です。
        /// </summary>
        public void CheckDeviceValidity() => QmPdk.Quma.CheckDeviceValidity(QumaHandle);

        /// <summary>センサーの状態を取得します。</summary>
        /// <returns>センサーの状態</returns>
        public SensorsState CheckDeviceSensors() => QmPdk.Quma.CheckDeviceSensors(QumaHandle);

        /// <summary>マシンに接続されているQumarionデバイスの総数を取得します。</summary>
        public static int ConnectedDeviceCount => QmPdk.Quma.GetDeviceCount();

        /// <summary>
        /// 利用可能なQumarionの一つを取得します。複数のQumarionからデバイス選んで接続したい場合
        /// <see cref="GetQumarionByIndex(int)"/>の使用を検討してください。
        /// </summary>
        /// <returns>Qumarionデバイス(一つも見つからなければ<see cref="InvalidOperationException"/>がスローされます)</returns>
        /// <exception cref="InvalidOperationException" />
        internal static Qumarion GetDefaultDevice() => GetQumarionByIndex(0);

        /// <summary>
        /// デバイスをインデックス指定によって取得します。
        /// </summary>
        /// <param name="index">インデックス。値は0以上、かつ(<see cref="GetConnectedDeviceCount"/>の戻り値 -1)以下である必要があります。</param>
        /// <returns>指定されたQumarionデバイス</returns>
        internal static Qumarion GetQumarionByIndex(int index)
        {
            int qumaCount = ConnectedDeviceCount;
            if(qumaCount == 0)
            {
                throw new InvalidOperationException("Qumarion device was not found");
            }

            if (index < 0 || index >= qumaCount)
            {
                throw new IndexOutOfRangeException();
            }

            return new Qumarion(QmPdk.Quma.GetHandle(index));
        }

    }
}
