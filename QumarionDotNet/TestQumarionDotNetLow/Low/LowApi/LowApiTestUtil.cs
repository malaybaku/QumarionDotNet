using System;
using System.Linq;

using Baku.Quma.Low;
using Baku.Quma.Low.Api;
using System.Threading;

namespace TestQumarionDotNet.Low
{
    public static class QumaTestContextSetting
    {
        /// <summary>
        /// 実行コンテキストの終了時にExit処理を行うかどうかの設定です。
        /// NOTE: trueにすると連続でテストするときに不安定化の原因になるっぽい
        /// </summary>
        public readonly static bool ExitEachTimeWhenContextDisposed = false;
    }

    /// <summary>
    /// using文を利用してQumaの実行コンテキストを生成します。
    /// </summary>
    public class QumaContext : IDisposable
    {
        public QumaContext()
        {
            QmLow.BaseOperation.Initialize();
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
        public QumaActiveDeviceContext()
        {
            _qumaContext = new QumaContext();

            var ids = QmLow.Device.EnumerateQumaIDs();
            var hardwareId = ids.FirstOrDefault(id => id.QumaType == QumaTypes.HardwareAsai);

            var qumaHandle = QmLow.Device.GetQumaHandle(hardwareId);
            QmLow.Device.ActivateQuma(qumaHandle);
            //NOTE: 時間を空けないとUpdateやらなんやらの処理がまともに通らないっぽい？
            Thread.Sleep(1000);

            QumaHandle = qumaHandle;
        }

        private readonly QumaContext _qumaContext;

        public QumaHandle QumaHandle { get; }

        public void Dispose()
        {
            QmLow.Device.DeleteQumaHandle(QumaHandle);
            _qumaContext.Dispose();
        }
    }

    /// <summary>
    /// アクティブ化されたデバイスハンドルとルートボーンのハンドルを持ったコンテキストを生成します。
    /// </summary>
    public class QumaRootBoneContext : IDisposable
    {
        public QumaRootBoneContext()
        {
            _qumaActiveDeviceContext = new QumaActiveDeviceContext();

            RootBoneHandle = QmLow.Bone.GetRootBone(QumaHandle);
        }

        private readonly QumaActiveDeviceContext _qumaActiveDeviceContext;

        public QumaHandle QumaHandle => _qumaActiveDeviceContext.QumaHandle;
        public BoneHandle RootBoneHandle { get; }

        public void Dispose() => _qumaActiveDeviceContext.Dispose();
    }

    /// <summary>
    /// アクティブ化されたデバイスおよび腰センサのハンドルを持ったコンテキストを生成します。
    /// </summary>
    public class QumaWaistSensorContext : IDisposable
    {
        public QumaWaistSensorContext()
        {
            _qumaRootBoneContext = new QumaRootBoneContext();

            var waistBoneHandle = QmLow.Bone.GetChildBone(_qumaRootBoneContext.RootBoneHandle, 0);
            WaistSensorHandle = QmLow.Sensors.GetSensor(waistBoneHandle, 0);
        }

        private readonly QumaRootBoneContext _qumaRootBoneContext;
        public QumaHandle QumaHandle => _qumaRootBoneContext.QumaHandle;
        public SensorHandle WaistSensorHandle { get; }

        public void Dispose() => _qumaRootBoneContext.Dispose();
    }

}
