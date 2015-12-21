using System.Linq;

using Baku.Quma.Low.Api;

namespace Baku.Quma.Low
{
    /// <summary>Qumarionのボーンのラッパークラスです。</summary>
    public sealed class Bone
    {
        private Bone(GeneralizedQumarion device, BoneHandle boneHandle)
        {
            Device = device;
            BoneHandle = boneHandle;

            Name = QmLow.Bone.GetBoneName(boneHandle);
            Position = QmLow.Bone.GetBonePosition(BoneHandle);
        }

        /// <summary>
        /// ボーンに対応するデバイスとハンドルを用いてインスタンスを生成します。
        /// </summary>
        /// <param name="device">ボーンが所属するデバイス</param>
        /// <param name="boneHandle">ボーンのハンドル</param>
        /// <returns>ボーンのインスタンス</returns>
        public static Bone Create(GeneralizedQumarion device, BoneHandle boneHandle)
        {
            var bone = new Bone(device, boneHandle);

            //再帰的に子ボーンを取得
            bone.ChildBones = QmLow.Bone
                .GetChildBones(bone.BoneHandle)
                .Select(cBoneHandle => Create(device, cBoneHandle))
                .ToArray();

            //センサーも同じよーなノリで取得
            bone.Sensors = QmLow.Sensors
                .GetSensors(bone.BoneHandle)
                .Select(sensorHandle => Sensor.Create(bone, sensorHandle))
                .ToArray();

            return bone;
        }

        /// <summary>このボーンが含まれるデバイスを取得します。</summary>
        public GeneralizedQumarion Device { get; }

        /// <summary>ボーンの低レイヤハンドルを取得します。</summary>
        public BoneHandle BoneHandle { get; }

        /// <summary>このボーンの名前を取得します。</summary>
        public string Name { get; }

        /// <summary>子ボーンを取得します。要素が無い場合は要素数0の配列を取得します。</summary>
        public Bone[] ChildBones { get; private set; }

        /// <summary>ボーンに割り当てられたセンサーを取得します。要素が無い場合は要素数0の配列を取得します。</summary>
        public Sensor[] Sensors { get; private set; }

        /// <summary>ボーンの位置を取得します。</summary>
        public Vector3 Position { get; private set; }

        public Matrix4 AttitudeMatrix { get; private set; }

        /// <summary>データが更新されている想定で新しい値を取得します。</summary>
        public void Update()
        {
            foreach(var childBone in ChildBones)
            {
                childBone.Update();
            }
            foreach(var sensor in Sensors)
            {
                sensor.Update();
            }

            AttitudeMatrix = QmLow.Bone.ComputeBoneMatrix(Device.QumaHandle, BoneHandle);
        }

        public override string ToString()
        {
            return string.Format(
                "Bone[{0}]: Child Count={1}, Sensor Count={2}",
                Name,
                ChildBones.Length,
                Sensors.Length
                );
        }
    }
}
