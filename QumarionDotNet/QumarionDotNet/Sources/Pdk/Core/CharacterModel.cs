using System;

using Baku.Quma.Pdk.Api;

namespace Baku.Quma.Pdk
{
    /// <summary>外部ライブラリとの接点に用いるキャラクターモデルを表します。</summary>
    public class CharacterModel : IDisposable
    {
        protected CharacterModel(ModelHandle modelHandle, Bone root)
        {
            ModelHandle = modelHandle;
            Root = root;
        }

        /// <summary>APIとのやり取りに用いるハンドルを取得します。</summary>
        public ModelHandle ModelHandle { get; }

        /// <summary>ルートボーンを取得します。</summary>
        public Bone Root { get; }

        /// <summary>このデバイスに関連付けられたQumaデバイスを取得します。</summary>
        public Qumarion AttachedQumarion { get; private set; }

        /// <summary>加速度計の動作モードを取得、設定します。</summary>
        public AccelerometerMode AccelerometerMode
        {
            get { return QmPdk.Character.GetAccelerometerMode(ModelHandle); }
            set
            {
                if(value == AccelerometerMode.NotAttached)
                {
                    throw new ArgumentException(
                        $"{AccelerometerMode.NotAttached} cannot be used to set: use {AccelerometerMode.Direct} or {AccelerometerMode.Relative}"
                        );
                }
                QmPdk.Character.SetAccelerometerMode(ModelHandle, value);
            }
        }

        /// <summary>加速度センサーの制限モードを取得、設定します。</summary>
        public AccelerometerRestrictMode AccelerometerRestrictMode
        {
            get { return QmPdk.Character.GetRestrictAccelerometerMode(ModelHandle); }
            set { QmPdk.Character.SetRestrictAccelerometerMode(ModelHandle, value); }
        }

        /// <summary>
        /// 指定されたデバイスをこのモデルに関連づけます。
        /// 以前に関連付けられたQumaデバイスがある場合、古い方の接続は解除されます。
        /// </summary>
        /// <param name="qumarion">関連づけるQumaデバイス</param>
        public void AttachQumarion(Qumarion qumarion)
        {
            DetachQumarion();

            QmPdk.Quma.AttachInitPoseModel(qumarion.QumaHandle, ModelHandle);
            AttachedQumarion = qumarion;
        }

        /// <summary>Qumaデバイスから最新情報を取得し、モデルに適用します。</summary>
        public void Update()
        {
            QmPdk.BaseOperation.CopyPose(ModelHandle);
            //追加: 状態量としてワールド座標系を保持させる。
            Root.UpdateWorldMatrix();
        }

        /// <summary>現在のQumaデバイスとモデルのポーズを対応付けます。</summary>
        public void Calibrate() => QmPdk.BaseOperation.CalibratePose(ModelHandle);

        /// <summary>Qumaデバイスとの接続を解除します。</summary>
        public void DetachQumarion()
        {
            if(AttachedQumarion != null)
            {
                QmPdk.Quma.DetachModel(ModelHandle);
                AttachedQumarion = null;
            }
        }

        /// <summary>デバイスとの接続を切り、キャラモデルを解放します。</summary>
        public void Dispose()
        {
            DetachQumarion();
            QmPdk.Character.Destroy(ModelHandle);
        }


        /// <summary>[実装されていません]予め用意されたボーン構成からモデルを構成します。</summary>
        /// <returns>キャラクターモデル</returns>
        internal static CharacterModel CreateModel(ModelHandle modelHandle, Bone root)
        {
            throw new NotImplementedException();
        }


    }
}
