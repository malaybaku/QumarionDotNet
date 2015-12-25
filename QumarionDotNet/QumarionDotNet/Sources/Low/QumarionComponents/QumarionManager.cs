using System;
using System.Linq;

using Baku.Quma.Low.Api;

namespace Baku.Quma.Low
{
    /// <summary>
    /// Qumarionのライブラリ初期化、終了および既定デバイス取得といった基本処理を提供します。
    /// </summary>
    public static class QumarionManager
    {
        /// <summary>ライブラリが初期化されているかどうかを取得します。</summary>
        public static bool Initialized => QmLow.BaseOperation.Initialized;

        /// <summary>ライブラリの初期化を行います。</summary>
        public static void Initialize() => QmLow.BaseOperation.Initialize();

        /// <summary>デバイスのロードに用いる事が可能なID一覧を取得します。</summary>
        /// <returns>デバイスIDの一覧</returns>
        public static QumaId[] GetHardwareQumaIds()
        {
            Initialize();
            return QmLow.Device.EnumerateQumaIDs().Where(id => id.QumaType == QumaTypes.HardwareAsai).ToArray();
        }

        /// <summary>実機と接続状態にあるかどうかを取得します。</summary>
        /// <returns>実機が検出された場合true、されていない場合false</returns>
        public static bool CheckConnectionToHardware() => GetHardwareQumaIds().Any();

        /// <summary>
        /// 実機のQUMARIONかが接続されている場合は最初に見つかった実機を、
        /// ない場合はシミュレータデバイスを返します。
        /// </summary>
        /// <returns>実機があれば実機、なければシミュレータデバイス</returns>
        public static GeneralizedQumarion GetDefaultGeneralDevice()
        {
            var ids = GetHardwareQumaIds();
            if(ids.Length == 0)
            {
                throw new InvalidOperationException("No simulator/hardware were detected");
            }

            if(CheckConnectionToHardware())
            {
                return GeneralizedQumarion.LoadGeneralDeviceFromQumaId(ids.First(id => id.QumaType == QumaTypes.HardwareAsai));
            }
            else
            {
                return GeneralizedQumarion.LoadGeneralDeviceFromQumaId(ids[0]);
            }
        }

        /// <summary>
        /// 実機のQUMARIONかが接続されている場合は最初に見つかった実機を、
        /// ない場合はシミュレータデバイスを返します。
        /// </summary>
        /// <returns>実機があれば実機、なければシミュレータデバイス</returns>
        public static Qumarion GetDefaultDevice()
        {
            var ids = GetHardwareQumaIds();
            if (ids.Length == 0)
            {
                throw new InvalidOperationException("No simulator/hardware were detected");
            }
            else
            {
                return Qumarion.LoadDeviceFromQumaId(ids[0]);
            }
        }

        /// <summary>Idを指定してデバイスを取得します。複数のQUMARIONを接続している場合に使ってください。</summary>
        /// <param name="qumaId">デバイスを表すID。<see cref="GetHardwareQumaIds"/>を用いて利用可能なIDの一覧を取得できます。</param>
        /// <returns>取得されたデバイス</returns>
        public static Qumarion GetDeviceById(QumaId qumaId)
            => Qumarion.LoadDeviceFromQumaId(qumaId);

        /// <summary>デバイスへの接続を終了します。</summary>
        /// <param name="device">接続終了したいデバイス</param>
        public static void DeleteDevice(GeneralizedQumarion device)
            => QmLow.Device.DeleteQumaHandle(device.QumaHandle);

        public static string GetLibraryVersion()
        {
            Initialize();
            return QmLow.BaseOperation.GetVersion();
        }

        /// <summary>ライブラリが初期化済みの状態で呼び出された場合、終了処理を行います。</summary>
        public static void Exit()
        {
            if(Initialized)
            {
                QmLow.BaseOperation.Exit();
            }
        }

    }
}
