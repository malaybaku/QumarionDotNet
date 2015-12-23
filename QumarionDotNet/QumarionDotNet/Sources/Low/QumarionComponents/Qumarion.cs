using System;
using System.Collections.Generic;
using System.Linq;

using Baku.Quma.Low.Api;

namespace Baku.Quma.Low
{
    /// <summary>事前知識をもとにボーンとセンサに名前を割り当てたQumarionデバイスを表します。</summary>
    public sealed class Qumarion : GeneralizedQumarion
    {
        /// <summary>継承元の初期化処理を行います。</summary>
        private Qumarion(QumaHandle qumaHandle, QumaId qumaId)
            : base(qumaHandle, qumaId)
        {
        }

        /// <summary>デバイスに対応するIDを用いてデバイスのインスタンスを生成します。</summary>
        /// <param name="qumaId">デバイスに対応するID</param>
        /// <returns>デバイスのインスタンス</returns>
        public static Qumarion LoadDeviceFromQumaId(QumaId qumaId)
        {
            var qumaHandle = QmLow.Device.GetQumaHandle(qumaId);
            if (qumaHandle.Handle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to get quma handle pointer");
            }

            var device = new Qumarion(qumaHandle, qumaId);
            QmLow.Device.ActivateQuma(device.QumaHandle);

            device.RootBone = Bone.Create(
                device,
                QmLow.Bone.GetRootBone(device.QumaHandle)
                );

            //以下がGeneralized版との違い: 事前知識を用いてボーンとセンサ一覧を得る
            device.Bones = new ReadOnlyDictionary<Low.Bones, Bone>(
                Enum.GetValues(typeof(Bones))
                    .Cast<Bones>()
                    .ToDictionary(
                        b => b,
                        b => QumarionDeviceMap.GetBone(device, b)
                        )
                    );

            device.Sensors = new ReadOnlyDictionary<Sensors, Sensor>(
                Enum.GetValues(typeof(Sensors))
                    .Cast<Sensors>()
                    .ToDictionary(
                        s => s,
                        s => QumarionDeviceMap.GetSensor(device, s)
                        )
                    );

            return device;
        }

        /// <summary>一覧化されたボーンを取得します。</summary>
        public ReadOnlyDictionary<Bones, Bone> Bones { get; private set; }

        /// <summary>一覧化されたセンサーを取得します。</summary>
        public ReadOnlyDictionary<Sensors, Sensor> Sensors { get; private set; }

    }

    ///// <summary>ボーン列挙体と実際のボーンを辞書的に関連づけたものを表します。</summary>
    //public class QumarionBones
    //{
    //    internal QumarionBones(Dictionary<Bones, Bone> bones)
    //    {
    //        _bones = bones;
    //    }

    //    private readonly Dictionary<Bones, Bone> _bones;

    //    /// <summary>ボーンの種類を指定して実際のセンサーを取得します。</summary>
    //    /// <param name="sensor">センサーの種類</param>
    //    /// <returns>対応するセンサー</returns>
    //    public Bone this[Bones bone] => _bones[bone];
    //}

    ///// <summary>センサー列挙体と実際のセンサーを辞書的に関連づけたものを表します。</summary>
    //public class QumarionSensors
    //{
    //    internal QumarionSensors(Dictionary<Sensors, Sensor> sensors)
    //    {
    //        _sensors = sensors;
    //    }

    //    private readonly Dictionary<Sensors, Sensor> _sensors;

    //    /// <summary>センサーの種類を指定して実際のセンサーを取得します。</summary>
    //    /// <param name="sensor">センサーの種類</param>
    //    /// <returns>対応するセンサー</returns>
    //    public Sensor this[Sensors sensor] => _sensors[sensor];

    //}

}
