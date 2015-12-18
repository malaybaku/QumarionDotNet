using System;

using Baku.Quma.Low;

namespace Baku.Quma
{
    /// <summary>デバイスのボーン/センサ構成を特に仮定しない、一般的なQumaのデバイスを表します。</summary>
    public class GeneralizedQumarion
    {
        /// <summary>デバイスを表すハンドルを用いてインスタンスを初期化します。</summary>
        /// <param name="qumaHandle">デバイスに対応するポインター</param>
        protected GeneralizedQumarion(QumaHandle qumaHandle, QumaId qumaId)
        {
            Id = qumaId.Id;
            QumaType = qumaId.QumaType;

            QumaHandle = qumaHandle;
            Name = QmLow.BaseOperation.GetDeviceName(qumaHandle);
            DeviceId = QmLow.BaseOperation.GetDeviceID(qumaHandle);
        }

        //NOTE: 何となくコンストラクタ内で子要素のコンストラクタに自分自身のインスタンス渡す構造がキモいので
        //ワンクッション置く目的でファクトリパターンを採用

        /// <summary>デバイスに対応するIDを用いてデバイスのインスタンスを生成します。</summary>
        /// <param name="qumaId">デバイスに対応するID</param>
        /// <returns>デバイスのインスタンス</returns>
        public static GeneralizedQumarion LoadGeneralDeviceFromQumaId(QumaId qumaId)
        {
            var qumaHandle = QmLow.Device.GetQumaHandle(qumaId);
            if (qumaHandle.Handle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to get quma handle pointer");
            }

            var qumarionDevice = new GeneralizedQumarion(qumaHandle, qumaId);
            QmLow.Device.ActivateQuma(qumarionDevice.QumaHandle);

            qumarionDevice.RootBone = Bone.Create(
                qumarionDevice, 
                QmLow.Bone.GetRootBone(qumarionDevice.QumaHandle)
                );

            return qumarionDevice;
        }

        /// <summary>デバイスの種類を取得します。</summary>
        public QumaTypes QumaType { get; }

        /// <summary>デバイスのIDを取得します。</summary>
        public int Id { get; }

        /// <summary>デバイスの名称を取得します。</summary>
        public string Name { get; }

        /// <summary>デバイスIDをバイト配列として取得します。</summary>
        public byte[] DeviceId { get; }

        /// <summary>デバイスの低レイヤハンドルを取得します。基本的に直接使わないでください。</summary>
        public QumaHandle QumaHandle { get; }

        /// <summary>デバイスIDを文字列として取得します。</summary>
        public string DeviceIdAsString
        {
            get { return BitConverter.ToString(DeviceId); }
        }

        /// <summary>ボタンの状態を取得します。</summary>
        public ButtonState ButtonState { get; private set; }

        /// <summary>ルート要素となるボーンを取得します。</summary>
        public Bone RootBone { get; protected set; }

        /// <summary>加速度計の生の読みを取得します。</summary>
        public Vector3 RawAccel { get; private set; }

        /// <summary>姿勢を表す行列を取得します。</summary>
        public Matrix4 PoseMatrix { get; private set; }

        /// <summary>デバイス全体の姿勢および加速度情報の更新を試みます。</summary>
        /// <returns>成功した場合OK、失敗の場合はそれ以外</returns>
        public QumaLowResponse TryUpdateAccelometer()
        {
            Vector3 rawAccel;
            var res1 = QmLow.Sensors.TryGetAccelerometer(QumaHandle, out rawAccel);
            if(res1 == QumaLowResponse.OK)
            {
                RawAccel = rawAccel;
            }

            Matrix4 poseMatrix;
            var res2 = QmLow.Sensors.TryGetAccelerometerPoseMatrix(QumaHandle, out poseMatrix);
            if(res2 == QumaLowResponse.OK)
            {
                PoseMatrix = poseMatrix;
            }

            if(res1 != QumaLowResponse.OK)
            {
                return res1;
            }
            else
            {
                return res2;
            }
        }

        /// <summary>センサー情報の更新を試みます。</summary>
        /// <returns>
        /// 成功した場合OK、失敗した場合はそれ以外
        /// (<see cref="QumaLowResponse.INFO_CHANGED"/>を受け取った場合<see cref="TryUpdateDevice"/>でデバイスを更新してください)
        /// </returns>
        public QumaLowResponse TryUpdateSensors()
        {
            var result = QmLow.Update.TryUpdateBuffer(QumaHandle);
            if (result == QumaLowResponse.OK)
            {
                RootBone.Update();
            }
            return result;
        }

        /// <summary>ボタン状態の更新を試みます。</summary>
        /// <returns>成功した場合OK、失敗した場合はそれ以外</returns>
        public QumaLowResponse TryUpdateButton()
        {
            ButtonState state;
            var result = QmLow.Button.TryGetState(QumaHandle, out state);
            if(result == QumaLowResponse.OK)
            {
                ButtonState = state;
            }
            return result;
        }

        /// <summary>
        /// デバイスが最新状態でなかった場合に呼び出し、デバイスを更新します。
        /// </summary>
        /// <returns>成功した場合OK, 失敗の場合はそれ以外。最新状態のデバイスに対して呼び出した場合もOK以外が返る場合があります。</returns>
        public QumaLowResponse TryUpdateDevice()
        {
            return QmLow.Update.TryUpdateQumaHandle(QumaHandle);
        }

        /// <summary>ボタン、角度センサ、加速度計の情報についてエラーを無視して更新を行います。</summary>
        public void Update()
        {
            TryUpdateAccelometer();
            TryUpdateSensors();
            TryUpdateButton();
        }
    }
}
