using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Baku.Quma.Low;
using Baku.Quma.Low.Api;

namespace TestQumarionDotNet.Low
{

    /// <summary>
    /// センサーの関数をテストします。
    /// 全身のテストはラッパークラスで行う方が効率がよいので
    /// ここではルートボーンを中心に簡単なテストを行います。
    /// </summary>
    [TestClass]
    public class LowSensorTest
    {

        [TestMethod]
        public void Low_センサー個数取得()
        {
            using (var context = new QumaRootBoneContext())
            {
                //Rootにはセンサが無さそうなので腰っぽい場所まで手を伸ばして確認しに行く
                var waistBoneHandle = QmLow.Bone.GetChildBone(context.RootBoneHandle, 0);
                SensorHandle sensorHandle = QmLow.Sensors.GetSensor(waistBoneHandle, 0);


                int count = QmLow.Sensors.GetSensorCount(waistBoneHandle);
                //TODO: 個数については要確認
                Assert.AreEqual(2, count);
            }
        }

        [TestMethod]
        public void Low_センサーインデクス取得_正常系()
        {
            using (var context = new QumaRootBoneContext())
            {
                //Rootにはセンサが無さそうなので腰っぽい場所まで手を伸ばして確認しに行く
                var waistBoneHandle = QmLow.Bone.GetChildBone(context.RootBoneHandle, 0);
                SensorHandle sensorHandle = QmLow.Sensors.GetSensor(waistBoneHandle, 0);
                Assert.AreNotEqual(IntPtr.Zero, sensorHandle.Handle);
            }
        }
        [TestMethod]
        public void Low_センサーインデクス取得_異常系()
        {
            using (var context = new QumaRootBoneContext())
            {
                SensorHandle sensorHandle = QmLow.Sensors.GetSensor(context.RootBoneHandle, 10);
                Assert.AreEqual(IntPtr.Zero, sensorHandle.Handle);
            }
        }

        [TestMethod]
        public void Low_センサー一覧取得()
        {
            using (var context = new QumaRootBoneContext())
            {
                int count = QmLow.Sensors.GetSensorCount(context.RootBoneHandle);
                var sensors = QmLow.Sensors.GetSensors(context.RootBoneHandle);
                Assert.AreEqual(count, sensors.Length);
            }
        }

        [TestMethod]
        public void Low_センサーコンテキスト取得確認_テスト用のテスト()
        {
            using (var context = new QumaWaistSensorContext())
            {
                Assert.IsNotNull(context);
                Assert.IsNotNull(context.QumaHandle);
                Assert.IsNotNull(context.WaistSensorHandle);
            }
        }

        [TestMethod]
        public void Low_センサー回転軸取得()
        {
            using (var context = new QumaWaistSensorContext())
            {
                Vector3 axis = QmLow.Sensors.GetSensorAxis(context.WaistSensorHandle);
                //結果が零ベクトル以外で初期化されたことをチェックするだけ
                Assert.IsTrue(axis.X * axis.X + axis.Y * axis.Y + axis.Z * axis.Z > 0);
            }
        }

        [TestMethod]
        public void Low_センサー状態取得()
        {
            using (var context = new QumaWaistSensorContext())
            {
                SensorStates state = QmLow.Sensors.GetSensorState(context.QumaHandle, context.WaistSensorHandle);
                Assert.AreEqual(SensorStates.OK, state);
            }
        }

        [TestMethod]
        public void Low_センサー角度取得1()
        {
            using (var context = new QumaWaistSensorContext())
            {
                float angle = QmLow.Sensors.ComputeSensorAngle(context.QumaHandle, context.WaistSensorHandle);
            }
        }

        [TestMethod]
        public void Low_センサー角度取得2()
        {
            using (var context = new QumaWaistSensorContext())
            {
                float angle = 0.0f;
                var response = QmLow.Sensors.TryComputeSensorAngle(context.QumaHandle, context.WaistSensorHandle, ref angle);
                Assert.AreEqual(QumaLowResponse.OK, response);
            }
        }

        [TestMethod]
        public void Low_加速度計生取得()
        {
            using (var context = new QumaActiveDeviceContext())
            {
                Vector3 accel = QmLow.Sensors.GetAccelerometer(context.QumaHandle);
            }
        }

        [TestMethod]
        public void Low_加速度計フィルタ済み行列取得()
        {
            using (var context = new QumaActiveDeviceContext())
            {
                Matrix4 matrix = QmLow.Sensors.GetAccelerometerPoseMatrix(context.QumaHandle);
            }
        }



    }
}
