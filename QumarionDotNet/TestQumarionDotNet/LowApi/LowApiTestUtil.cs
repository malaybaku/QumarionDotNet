using System;
using System.Linq;

using Baku.Quma.Low;
using Baku.Quma.Low.Api;
using System.Threading;

namespace TestQumarionDotNet
{
    public static class QumaTestContextSetting
    {
        /// <summary>実行コンテキストの終了時にExit処理を行うかどうかの設定です。</summary>
        public readonly static bool ExitEachTimeWhenContextDisposed = false;
    }

    /// <summary>
    /// using文を利用してQumaの実行コンテキストを生成します。
    /// </summary>
    public class QumaContext : IDisposable
    {
        private QumaContext()
        {
            QmLow.BaseOperation.Initialize();
        }

        public static QumaContext Create()
        {
            return new QumaContext();
        }

        public void Dispose()
        {
            if(QumaTestContextSetting.ExitEachTimeWhenContextDisposed)
            {
                QmLow.BaseOperation.Exit();
            }
        }
    }

    /// <summary>
    /// アクティブ化されたデバイスハンドルを持ったコンテキストを生成します。
    /// </summary>
    public class QumaActiveDeviceContext : IDisposable
    {
        private QumaActiveDeviceContext(QumaHandle qumaHandle)
        {
            QumaHandle = qumaHandle;
        }

        public QumaHandle QumaHandle { get; }

        /// <summary>
        /// デバイスの種類を指定してデバイスつきのコンテキストを取得します。
        /// </summary>
        /// <param name="requiredType">使用するデバイスの種類</param>
        /// <returns></returns>
        public static QumaActiveDeviceContext Create(QumaTypes requiredType)
        {
            QmLow.BaseOperation.Initialize();

            var ids = QmLow.Device.EnumerateQumaIDs();
            var hardwareId = ids.FirstOrDefault(id => id.QumaType == requiredType);

            var qumaHandle = QmLow.Device.GetQumaHandle(hardwareId);
            QmLow.Device.ActivateQuma(qumaHandle);
            //NOTE: 時間を空けないとUpdateやらなんやらの処理がまともに通らないっぽい？
            Thread.Sleep(1000);

            return new QumaActiveDeviceContext(qumaHandle);
        }

        public void Dispose()
        {
            if (QumaTestContextSetting.ExitEachTimeWhenContextDisposed)
            {
                QmLow.BaseOperation.Exit();
            }
        }
    }

    /// <summary>
    /// アクティブ化されたデバイスハンドルとルートボーンのハンドルを持ったコンテキストを生成します。
    /// </summary>
    public class QumaRootBoneContext : IDisposable
    {
        private QumaRootBoneContext(QumaHandle qumaHandle, BoneHandle rootBoneHandle)
        {
            QumaHandle = qumaHandle;
            RootBoneHandle = rootBoneHandle;
        }

        public QumaHandle QumaHandle { get; }
        public BoneHandle RootBoneHandle { get; }

        /// <summary>
        /// デバイスの種類を指定してデバイスつきのコンテキストを取得します。
        /// </summary>
        /// <param name="requiredType">使用するデバイスの種類</param>
        /// <returns></returns>
        public static QumaRootBoneContext Create(QumaTypes requiredType)
        {
            QmLow.BaseOperation.Initialize();

            var ids = QmLow.Device.EnumerateQumaIDs();
            var hardwareId = ids.FirstOrDefault(id => id.QumaType == requiredType);

            var qumaHandle = QmLow.Device.GetQumaHandle(hardwareId);

            QmLow.Device.ActivateQuma(qumaHandle);

            var rootBoneHandle = QmLow.Bone.GetRootBone(qumaHandle);
            return new QumaRootBoneContext(qumaHandle, rootBoneHandle);
        }

        public void Dispose()
        {
            if (QumaTestContextSetting.ExitEachTimeWhenContextDisposed)
            {
                QmLow.BaseOperation.Exit();
            }
        }
    }

    /// <summary>
    /// アクティブ化されたデバイス、ルートボーン、腰ボーンおよび腰センサのハンドルを持ったコンテキストを生成します。
    /// </summary>
    public class QumaWaistSensorContext : IDisposable
    {
        private QumaWaistSensorContext(QumaHandle qumaHandle, SensorHandle sensorHandle)
        {
            QumaHandle = qumaHandle;
            WaistSensorHandle = sensorHandle;
        }

        public QumaHandle QumaHandle { get; }
        public SensorHandle WaistSensorHandle { get; }

        /// <summary>
        /// デバイスの種類を指定してコンテキストを取得します。
        /// </summary>
        /// <param name="requiredType">使用するデバイスの種類</param>
        /// <returns></returns>
        public static QumaWaistSensorContext Create(QumaTypes requiredType)
        {
            QmLow.BaseOperation.Initialize();

            var ids = QmLow.Device.EnumerateQumaIDs();
            var hardwareId = ids.FirstOrDefault(id => id.QumaType == requiredType);

            var qumaHandle = QmLow.Device.GetQumaHandle(hardwareId);

            QmLow.Device.ActivateQuma(qumaHandle);
            Thread.Sleep(1000);

            var rootBoneHandle = QmLow.Bone.GetRootBone(qumaHandle);

            var waistBoneHandle = QmLow.Bone.GetChildBone(rootBoneHandle, 0);
            var waistSensorHandle = QmLow.Sensors.GetSensor(waistBoneHandle, 0);


            return new QumaWaistSensorContext(qumaHandle, waistSensorHandle);
        }

        public void Dispose()
        {
            if(QumaTestContextSetting.ExitEachTimeWhenContextDisposed)
            {
                QmLow.BaseOperation.Exit();
            }
        }

    }

}
