using Baku.Quma.Low;

namespace Baku.Quma
{
    /// <summary>単一の角度を測定するセンサーを表します。</summary>
    public sealed class Sensor
    {
        private Sensor(Bone bone, SensorHandle sensorHandle)
        {
            Device = bone.Device;
            Bone = bone;
            Handle = sensorHandle;

            Axis = QmLow.Sensors.GetSensorAxis(sensorHandle);
        }

        /// <summary>親であるボーンと対応するセンサのハンドルを基にセンサのインスタンスを生成します。</summary>
        /// <param name="bone">センサの親となるボーン</param>
        /// <param name="sensorHandle">センサの低レイヤハンドル</param>
        /// <returns>センサーのインスタンス</returns>
        public static Sensor Create(Bone bone, SensorHandle sensorHandle)
        {
            return new Sensor(bone, sensorHandle);
        }

        /// <summary>このセンサーが所属しているQumarionのデバイスを取得します。</summary>
        public GeneralizedQumarion Device { get; }

        /// <summary>このセンサーが所属しているボーンを取得します。</summary>
        public Bone Bone { get; }
        
        /// <summary>このセンサーの低レイヤハンドルを取得します。</summary>
        public SensorHandle Handle { get; }

        /// <summary>角度測定を行っている軸の方向を表します。</summary>
        public Vector3 Axis { get; private set; }

        /// <summary>センサーの状態を取得します。</summary>
        public SensorStates State { get; private set; }

        /// <summary>センサーから読み取った角度を取得します。</summary>
        public float Angle { get; private set; }

        /// <summary>センサー情報を最新のものに更新します。</summary>
        public void Update()
        {
            State = QmLow.Sensors.GetSensorState(Device.QumaHandle, Handle);

            float angle = 0.0f;
            if(QmLow.Sensors.TryComputeSensorAngle(Device.QumaHandle, Handle, ref angle) == QumaLowResponse.OK)
            {
                Angle = angle;
            }
        }

        public override string ToString()
        {
            return string.Format(
                "Sensor on Bone[{0}]: Axis=({1}, {2}, {3})",
                Bone.Name,
                Axis.X,
                Axis.Y,
                Axis.Z
                );
        }

    }
}
