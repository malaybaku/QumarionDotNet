using System;
using System.Collections.Generic;
using System.Linq;

using Baku.Quma.Low;

namespace Baku.Quma
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
            device.Bones = Enum.GetValues(typeof(Bones))
                .Cast<Bones>()
                .ToDictionary(
                    b => b,
                    b => QumarionDeviceMap.GetBone(device, b)
                    );

            device.Sensors = Enum.GetValues(typeof(Sensors))
                .Cast<Sensors>()
                .ToDictionary(
                    s => s,
                    s => QumarionDeviceMap.GetSensor(device, s)
                    );

            return device;
        }

        /// <summary>一覧化されたボーンを取得します。</summary>
        public IReadOnlyDictionary<Bones, Bone> Bones { get; private set; }

        /// <summary>一覧化されたセンサーを取得します。</summary>
        public IReadOnlyDictionary<Sensors, Sensor> Sensors { get; private set; }

    }
}
