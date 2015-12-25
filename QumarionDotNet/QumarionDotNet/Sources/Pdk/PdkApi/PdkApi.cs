using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Baku.Quma.Pdk.Api
{
    /// <summary>QmPdkDll.hに定義されている関数をラップします。</summary>
    public static class QmPdk
    {
        private const string DllName86 = DllImportSetting.DllName86;
        private const string DllName64 = DllImportSetting.DllName64;
        private static readonly bool Is64bit = DllImportSetting.Is64bit;

        private const int NodeNameBufferLength = 256;
        private const int NodeOriginalNameBufferLength = 256;
        private const int DeviceNameBufferLength = 256;
        private const int SensorsCheckMessageBufferLength = 1024;

        private static void ThrowIfError(QmErrorCode errorCode)
        {
            if(errorCode != QmErrorCode.NoError)
            {
                throw new QmPdkException(errorCode);
            }
        }


        /// <summary>初期化と終了、各種処理</summary>
        public static class BaseOperation
        {
            #region privateなDllImport

            //NOTE: Unity向けに単一のDLLを提供するため、Any CPUビルドで実行時ビット数を見て
            //アンマネージライブラリの実体を使い分けるようにしてる。
            //暗黒感の高い書き方なので普通はきちんとターゲットをx86とかx64に固定すること

            #region GetVersionStr
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkGetVersionStr@@YAHPADPAH@Z")]
            private static extern QmErrorCode GetVersionStr86([MarshalAs(UnmanagedType.LPStr)]StringBuilder versionStr, ref int versionStrSize);

            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkGetVersionStr@@YAHPEADPEAH@Z")]
            private static extern QmErrorCode GetVersionStr64([MarshalAs(UnmanagedType.LPStr)]StringBuilder versionStr, ref int versionStrSize);

            private static QmErrorCode GetVersionStr(StringBuilder versionStr, ref int versionStrSize)
                => Is64bit ?
                    GetVersionStr64(versionStr, ref versionStrSize) :
                    GetVersionStr86(versionStr, ref versionStrSize);
            #endregion

            #region TryInit
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkInit@@YAHXZ")]
            private static extern QmErrorCode TryInit86();

            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkInit@@YAHXZ")]
            private static extern QmErrorCode TryInit64();

            private static QmErrorCode TryInit() => Is64bit ? TryInit64() : TryInit86();
            #endregion

            #region TryFinal
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkFinal@@YAHXZ")]
            private static extern QmErrorCode TryFinal86();

            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkFinal@@YAHXZ")]
            private static extern QmErrorCode TryFinal64();

            private static QmErrorCode TryFinal() => Is64bit ? TryFinal64() : TryFinal86();
            #endregion

            #region TryDisconnect
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkDisconnect@@YAHXZ")]
            private static extern QmErrorCode TryDisconnect86();

            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkDisconnect@@YAHXZ")]
            private static extern QmErrorCode TryDisconnect64();

            private static QmErrorCode TryDisconnect() => Is64bit ? TryDisconnect64() : TryDisconnect86();
            #endregion

            #region TrySetForcedCopyPose

            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkSetForcedCopyPose@@YAHH@Z")]
            private static extern QmErrorCode TrySetForcedCopyPose86([MarshalAs(UnmanagedType.U1)]bool isEnabled);

            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkSetForcedCopyPose@@YAHH@Z")]
            private static extern QmErrorCode TrySetForcedCopyPose64([MarshalAs(UnmanagedType.U1)]bool isEnabled);

            private static QmErrorCode TrySetForcedCopyPose(bool isEnabled)
                => Is64bit ? 
                TrySetForcedCopyPose64(isEnabled) : 
                TrySetForcedCopyPose86(isEnabled);

            #endregion

            #region CopyPose
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCopyPose@@YAHH@Z")]
            private static extern QmErrorCode CopyPose86(int modelHandle);

            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCopyPose@@YAHH@Z")]
            private static extern QmErrorCode CopyPose64(int modelHandle);

            private static QmErrorCode CopyPose(int modelHandle)
                => Is64bit ? 
                CopyPose64(modelHandle) : 
                CopyPose86(modelHandle);
            #endregion

            #region CalibratePose
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCalibratePose@@YAHH@Z")]
            private static extern QmErrorCode CalibratePose86(int modelHandle);

            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCalibratePose@@YAHH@Z")]
            private static extern QmErrorCode CalibratePose64(int modelHandle);

            private static QmErrorCode CalibratePose(int modelHandle)
                => Is64bit ?
                CalibratePose64(modelHandle) :
                CalibratePose86(modelHandle);
            #endregion

            #region SaveCalibrationDataFile
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkSaveCalibrationDataFile@@YAHHPBD@Z")]
            private static extern QmErrorCode SaveCalibrationDataFile86(int modelHandle, [MarshalAs(UnmanagedType.LPWStr)]string filepath);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkSaveCalibrationDataFile@@YAHHPEBD@Z")]
            private static extern QmErrorCode SaveCalibrationDataFile64(int modelHandle, [MarshalAs(UnmanagedType.LPWStr)]string filepath);

            private static QmErrorCode SaveCalibrationDataFile(int modelHandle, string filepath)
                => Is64bit ?
                SaveCalibrationDataFile64(modelHandle, filepath) :
                SaveCalibrationDataFile86(modelHandle, filepath);
            #endregion

            #region SaveCalibrationDataMem
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkSaveCalibrationDataMem@@YAHHPADPAH@Z")]
            private static extern QmErrorCode SaveCalibrationDataMem86(int modelHandle, ref byte[] outdData, ref int outDataSize);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkSaveCalibrationDataMem@@YAHHPEADPEAH@Z")]
            private static extern QmErrorCode SaveCalibrationDataMem64(int modelHandle, ref byte[] outdData, ref int outDataSize);
            private static QmErrorCode SaveCalibrationDataMem(int modelHandle, ref byte[] outdData, ref int outDataSize)
                => Is64bit ?
                SaveCalibrationDataMem64(modelHandle, ref outdData, ref outDataSize) :
                SaveCalibrationDataMem86(modelHandle, ref outdData, ref outDataSize);
            #endregion

            #region LoadCalibrationDataFile
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkLoadCalibrationDataFile@@YAHHPBD@Z")]
            private static extern QmErrorCode LoadCalibrationDataFile86(int modelHandle, [MarshalAs(UnmanagedType.LPWStr)]string filepath);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkLoadCalibrationDataFile@@YAHHPEBD@Z")]
            private static extern QmErrorCode LoadCalibrationDataFile64(int modelHandle, [MarshalAs(UnmanagedType.LPWStr)]string filepath);
            private static QmErrorCode LoadCalibrationDataFile(int modelHandle, string filepath)
                => Is64bit ?
                LoadCalibrationDataFile64(modelHandle, filepath) :
                LoadCalibrationDataFile86(modelHandle, filepath);
            #endregion

            #region LoadCalibrationDataMem
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkLoadCalibrationDataMem@@YAHHPBDH@Z")]
            private static extern QmErrorCode LoadCalibrationDataMem86(int modelHandle, IntPtr data, int dataSize);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkLoadCalibrationDataMem@@YAHHPEBDH@Z")]
            private static extern QmErrorCode LoadCalibrationDataMem64(int modelHandle, IntPtr data, int dataSize);
            private static QmErrorCode LoadCalibrationDataMem(int modelHandle, IntPtr data, int dataSize)
                => Is64bit ?
                LoadCalibrationDataMem64(modelHandle, data, dataSize) :
                LoadCalibrationDataMem86(modelHandle, data, dataSize);
            #endregion

            #endregion

            /// <summary>ライブラリのバージョン文字列を取得します。失敗した場合は空文字列を返します。</summary>
            /// <returns></returns>
            public static string GetVersionStr()
            {
                int strSize = 0;
                if(GetVersionStr(null, ref strSize) != QmErrorCode.NoError)
                {
                    return "";
                }

                var result = new StringBuilder(strSize);
                if (GetVersionStr(result, ref strSize) != QmErrorCode.NoError)
                {
                    return "";
                }

                return result.ToString();
            }

            private static object _initializedLock = new object();
            private static bool _initialized = false;
            /// <summary>ライブラリの初期化がプロセス中で一度以上行われたかどうかを取得します。</summary>
            public static bool Initialized
            {
                get
                {
                    lock (_initializedLock)
                    {
                        return _initialized;
                    }
                }
                set
                {
                    lock (_initializedLock)
                    {
                        _initialized = value;
                    }
                }
            }

            /// <summary>ライブラリの初期化です。アプリケーション開始時には必ず呼ばなければならず、複数回呼んでも構いません。</summary>
            public static void Initialize()
            {
                if(!Initialized)
                {
                    var res = TryInit();
                    if(res == QmErrorCode.NoError)
                    {
                        Initialized = true;
                    }
                    else
                    {
                        throw new QmPdkException(res);
                    }
                }
            }

            /// <summary>ライブラリの終了処理です。アプリケーション終了時に一度だけ呼んでください。</summary>
            public static void Final()
            {
                ThrowIfError(TryFinal());
            }

            /// <summary>
            /// ライブラリの切断処理です。アプリケーションの非アクティブ時に呼んでください
            /// (とラップ元のヘッダには載っているが、よく分からないなら使わないのが無難か。)
            /// </summary>
            public static void Disconnect() => ThrowIfError(TryDisconnect());

            /// <summary>
            /// ポーズの変更が無くても強制的にQmPdkCopyPoseを行うかを設定します。
            /// </summary>
            /// <param name="enable">強制的にコピーが行われるかどうか</param>
            public static void SetForcedCopyPose(bool enable) => ThrowIfError(TrySetForcedCopyPose(enable));

            /// <summary>
            /// 指定したモデルにQUMAのポーズを反映させます。
            /// 反映後に結果のマトリックスを取り出して利用してください。
            /// </summary>
            /// <param name="modelHandle">コピー先のモデルのハンドル</param>
            /// <returns>NoErrorの場合は成功</returns>
            public static void CopyPose(ModelHandle modelHandle) => ThrowIfError(CopyPose(modelHandle.Handle));

            /// <summary>
            /// ポーズをキャリブレートし、対象モデルのカレントポーズと現在のQUMAのポーズを対応付けます。
            /// </summary>
            /// <param name="modelHandle">キャリブレート対象のモデル</param>
            /// <returns>NoErrorの場合は成功</returns>
            public static void CalibratePose(ModelHandle modelHandle) => ThrowIfError(CalibratePose(modelHandle.Handle));

            /// <summary>キャリブレーション情報を指定したファイルにセーブします。</summary>
            /// <param name="modelHandle">セーブしたいモデル</param>
            /// <param name="filepath">セーブ先のファイルパス</param>
            /// <returns>NoErrorの場合は成功</returns>
            public static void SaveCalibrationDataFile(ModelHandle modelHandle, string filepath)
                => ThrowIfError(SaveCalibrationDataFile(modelHandle.Handle, filepath));

            /// <summary>キャリブレーション情報を指定したファイルからロードします。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <param name="filepath">ロード先のファイルパス</param>
            /// <returns>NoErrorの場合は成功</returns>
            public static void LoadCalibrationDataFile(ModelHandle modelHandle, string filepath)
                => ThrowIfError(LoadCalibrationDataFile(modelHandle.Handle, filepath));

            /// <summary>キャリブレーション情報をメモリデータとして取得します。</summary>
            /// <param name="modelHandle">セーブしたいモデル</param>
            /// <param name="result">セーブ結果を持っているバイト配列</param>
            /// <returns>NoErrorの場合は成功</returns>
            public static byte[] SaveCalibrationDataMem(ModelHandle modelHandle)
            {
                var result = new byte[0];
                int size = 0;

                ThrowIfError(SaveCalibrationDataMem(modelHandle.Handle, ref result, ref size));

                return result;    
            }

            /// <summary>キャリブレーション情報を指定したメモリデータからロードして適用します。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <param name="data">ロード先のバイトデータ</param>
            /// <returns>NoErrorの場合は成功</returns>
            public static void LoadCalibrationDataMem(ModelHandle modelHandle, byte[] data)
            {
                IntPtr dataPtr = Marshal.AllocHGlobal(data.Length);
                Marshal.Copy(data, 0, dataPtr, data.Length);

                var res = (LoadCalibrationDataMem(modelHandle.Handle, dataPtr, data.Length));

                Marshal.FreeHGlobal(dataPtr);

                ThrowIfError(res);
            }

        }

        /// <summary>キャラクターボーン関連</summary>
        public static class Character
        {
            #region privateなDllImport

            #region CreateStandardModelPS
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterCreateStandardModelPS@@YAHPAH00@Z")]
            private static extern QmErrorCode CreateStandardModelPS86(IntPtr modelHandle, ref int numberOfNode, IntPtr parentNodeIndexArray);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterCreateStandardModelPS@@YAHPEAH00@Z")]
            private static extern QmErrorCode CreateStandardModelPS64(IntPtr modelHandle, ref int numberOfNode, IntPtr parentNodeIndexArray);
            private static QmErrorCode CreateStandardModelPS(IntPtr modelHandle, ref int numberOfNode, IntPtr parentNodeIndexArray)
                => Is64bit ?
                CreateStandardModelPS64(modelHandle, ref numberOfNode, parentNodeIndexArray) :
                CreateStandardModelPS86(modelHandle, ref numberOfNode, parentNodeIndexArray);

            #endregion

            #region Create
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterCreate@@YAHPAHH0PAPBD@Z")]
            private static extern QmErrorCode Create86(ref int modelhandle, int numberOfNode, IntPtr parentNodeIndexArray, IntPtr nodeNameArray);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterCreate@@YAHPEAHH0PEAPEBD@Z")]
            private static extern QmErrorCode Create64(ref int modelhandle, int numberOfNode, IntPtr parentNodeIndexArray, IntPtr nodeNameArray);
            private static QmErrorCode Create(ref int modelhandle, int numberOfNode, IntPtr parentNodeIndexArray, IntPtr nodeNameArray)
                => Is64bit ?
                Create64(ref modelhandle, numberOfNode, parentNodeIndexArray, nodeNameArray) :
                Create86(ref modelhandle, numberOfNode, parentNodeIndexArray, nodeNameArray);
            //NOTE: Create関数は使いどころが少なそうなのでラップは放置中
            #endregion

            #region Destroy
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterDestroy@@YAHH@Z")]
            private static extern QmErrorCode Destroy86(int modelHandle);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterDestroy@@YAHH@Z")]
            private static extern QmErrorCode Destroy64(int modelHandle);
            private static QmErrorCode Destroy(int modelHandle) => Is64bit ? Destroy64(modelHandle) : Destroy86(modelHandle);
            #endregion

            #region GetNumOfHandle
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetNumOfHandle@@YAHPAH@Z")]
            private static extern QmErrorCode GetNumOfHandle86(out int num);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetNumOfHandle@@YAHPEAH@Z")]
            private static extern QmErrorCode GetNumOfHandle64(out int num);
            private static QmErrorCode GetNumOfHandle(out int num)
                => Is64bit ?
                GetNumOfHandle64(out num) :
                GetNumOfHandle86(out num);
            #endregion

            #region GetHandle
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetHandle@@YAHHPAH@Z")]
            private static extern QmErrorCode GetHandle86(int index, out int modelHandle);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetHandle@@YAHHPEAH@Z")]
            private static extern QmErrorCode GetHandle64(int index, out int modelHandle);
            private static QmErrorCode GetHandle(int index, out int modelHandle)
                => Is64bit ?
                GetHandle64(index, out modelHandle) :
                GetHandle86(index, out modelHandle);
            #endregion

            #region GetQumaHandle
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetQumaHandle@@YAHHPAH@Z")]
            private static extern QmErrorCode GetQumaHandle86(int modelHandle, out int qumaHandle);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetQumaHandle@@YAHHPEAH@Z")]
            private static extern QmErrorCode GetQumaHandle64(int modelHandle, out int qumaHandle);
            private static QmErrorCode GetQumaHandle(int modelHandle, out int qumaHandle)
                => Is64bit ?
                GetQumaHandle64(modelHandle, out qumaHandle) :
                GetQumaHandle86(modelHandle, out qumaHandle);
            #endregion

            #region SetLocalMatrix
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterSetLocalMatrix@@YAHHHQAY03M@Z")]
            private static extern QmErrorCode SetLocalMatrix86(int modelHandle, int nodeIndex, IntPtr localMatrix);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterSetLocalMatrix@@YAHHHQEAY03M@Z")]
            private static extern QmErrorCode SetLocalMatrix64(int modelHandle, int nodeIndex, IntPtr localMatrix);
            private static QmErrorCode SetLocalMatrix(int modelHandle, int nodeIndex, IntPtr localMatrix)
                => Is64bit ?
                SetLocalMatrix64(modelHandle, nodeIndex, localMatrix) :
                SetLocalMatrix86(modelHandle, nodeIndex, localMatrix);
            #endregion

            #region GetLocalMatrix
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetLocalMatrix@@YAHHHQAY03M@Z")]
            private static extern QmErrorCode GetLocalMatrix86(int modelHandle, int nodeIndex, IntPtr localMatrix);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetLocalMatrix@@YAHHHQEAY03M@Z")]
            private static extern QmErrorCode GetLocalMatrix64(int modelHandle, int nodeIndex, IntPtr localMatrix);
            private static QmErrorCode GetLocalMatrix(int modelHandle, int nodeIndex, IntPtr localMatrix)
                => Is64bit ?
                GetLocalMatrix64(modelHandle, nodeIndex, localMatrix) :
                GetLocalMatrix86(modelHandle, nodeIndex, localMatrix);
            #endregion

            #region SetRotate
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterSetRotate@@YAHHHQAY03M@Z")]
            private static extern QmErrorCode SetRotate86(int modelHandle, int nodeIndex, IntPtr rotateMatrix);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterSetRotate@@YAHHHQEAY03M@Z")]
            private static extern QmErrorCode SetRotate64(int modelHandle, int nodeIndex, IntPtr rotateMatrix);
            private static QmErrorCode SetRotate(int modelHandle, int nodeIndex, IntPtr rotateMatrix)
                => Is64bit ?
                SetRotate64(modelHandle, nodeIndex, rotateMatrix) :
                SetRotate86(modelHandle, nodeIndex, rotateMatrix);
            #endregion

            #region GetRotate
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetRotate@@YAHHHQAY03M@Z")]
            private static extern QmErrorCode GetRotate86(int modelHandle, int nodeIndex, IntPtr rotateMatrix);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetRotate@@YAHHHQEAY03M@Z")]
            private static extern QmErrorCode GetRotate64(int modelHandle, int nodeIndex, IntPtr rotateMatrix);
            private static QmErrorCode GetRotate(int modelHandle, int nodeIndex, IntPtr rotateMatrix)
                => Is64bit ?
                GetRotate64(modelHandle, nodeIndex, rotateMatrix) :
                GetRotate86(modelHandle, nodeIndex, rotateMatrix);
            #endregion

            #region GetName
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetName@@YAHHHPADH0H@Z")]
            private static extern QmErrorCode GetName86(
                int modelhandle, int nodeIndex,
                [MarshalAs(UnmanagedType.LPStr)]StringBuilder name, int sizeName,
                [MarshalAs(UnmanagedType.LPStr)]StringBuilder originalName, int sizeOriginalName
                );
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetName@@YAHHHPEADH0H@Z")]
            private static extern QmErrorCode GetName64(
                int modelhandle, int nodeIndex, 
                [MarshalAs(UnmanagedType.LPStr)]StringBuilder name, int sizeName, 
                [MarshalAs(UnmanagedType.LPStr)]StringBuilder originalName, int sizeOriginalName
                );
            private static QmErrorCode GetName(
                int modelhandle, int nodeIndex,
                StringBuilder name, int sizeName,
                StringBuilder originalName, int sizeOriginalName
                )
                => Is64bit ?
                GetName64(modelhandle, nodeIndex, name, sizeName, originalName, sizeOriginalName) :
                GetName86(modelhandle, nodeIndex, name, sizeName, originalName, sizeOriginalName);
            #endregion

            #region MemorizeInitialPose
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterMemorizeInitialPose@@YAHH@Z")]
            private static extern QmErrorCode MemorizeInitialPose86(int modelHandle);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterMemorizeInitialPose@@YAHH@Z")]
            private static extern QmErrorCode MemorizeInitialPose64(int modelHandle);
            private static QmErrorCode MemorizeInitialPose(int modelHandle)
                => Is64bit ?
                MemorizeInitialPose64(modelHandle) :
                MemorizeInitialPose86(modelHandle);
            #endregion

            #region RecallInitialPose
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterRecallInitialPose@@YAHH@Z")]
            private static extern QmErrorCode RecallInitialPose86(int modelHandle);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterRecallInitialPose@@YAHH@Z")]
            private static extern QmErrorCode RecallInitialPose64(int modelHandle);
            private static QmErrorCode RecallInitialPose(int modelHandle)
                => Is64bit ?
                RecallInitialPose64(modelHandle) :
                RecallInitialPose86(modelHandle);
            #endregion

            #region SetAccelerometerMode
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterSetAccelerometerMode@@YAHHH@Z")]
            private static extern QmErrorCode SetAccelerometerMode86(int modelHandle, [MarshalAs(UnmanagedType.I4)]AccelerometerMode mode);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterSetAccelerometerMode@@YAHHH@Z")]
            private static extern QmErrorCode SetAccelerometerMode64(int modelHandle, [MarshalAs(UnmanagedType.I4)]AccelerometerMode mode);
            private static QmErrorCode SetAccelerometerMode(int modelHandle, AccelerometerMode mode)
                => Is64bit ?
                SetAccelerometerMode64(modelHandle, mode) :
                SetAccelerometerMode86(modelHandle, mode);
            #endregion

            #region GetAccelerometerMode
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetAccelerometerMode@@YAHHPAH@Z")]
            private static extern QmErrorCode GetAccelerometerMode86(int modelHandle, [MarshalAs(UnmanagedType.I4)]out AccelerometerMode mode);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetAccelerometerMode@@YAHHPEAH@Z")]
            private static extern QmErrorCode GetAccelerometerMode64(int modelHandle, [MarshalAs(UnmanagedType.I4)]out AccelerometerMode mode);
            private static QmErrorCode GetAccelerometerMode(int modelHandle, out AccelerometerMode mode)
                => Is64bit ?
                GetAccelerometerMode64(modelHandle, out mode) :
                GetAccelerometerMode86(modelHandle, out mode);
            #endregion

            #region SetRestrictAccelerometerMode
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterSetRestrictAccelerometerMode@@YAHHH@Z")]
            private static extern QmErrorCode SetRestrictAccelerometerMode86(int modelHandle, AccelerometerRestrictMode mode);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterSetRestrictAccelerometerMode@@YAHHH@Z")]
            private static extern QmErrorCode SetRestrictAccelerometerMode64(int modelHandle, AccelerometerRestrictMode mode);
            private static QmErrorCode SetRestrictAccelerometerMode(int modelHandle, AccelerometerRestrictMode mode)
                => Is64bit ?
                SetRestrictAccelerometerMode64(modelHandle, mode) :
                SetRestrictAccelerometerMode86(modelHandle, mode);
            #endregion

            #region GetRestrictAccelerometerMode
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetRestrictAccelerometerMode@@YAHHPAH@Z")]
            private static extern QmErrorCode GetRestrictAccelerometerMode86(int modelHandle, out AccelerometerRestrictMode mode);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetRestrictAccelerometerMode@@YAHHPEAH@Z")]
            private static extern QmErrorCode GetRestrictAccelerometerMode64(int modelHandle, out AccelerometerRestrictMode mode);
            private static QmErrorCode GetRestrictAccelerometerMode(int modelHandle, out AccelerometerRestrictMode mode)
                => Is64bit ?
                GetRestrictAccelerometerMode64(modelHandle, out mode) :
                GetRestrictAccelerometerMode86(modelHandle, out mode);

            #endregion

            #endregion

            #region publicなラップ済関数

            /// <summary>ボーンの解放処理。<see cref="BaseOperation.Final"/>で解放できるので呼ばなくても問題はありません。</summary>
            /// <param name="modelHandle">解放したいモデル</param>
            public static void Destroy(ModelHandle modelHandle) => ThrowIfError(Destroy(modelHandle.Handle));

            /// <summary>標準モデルのキャラクターボーンを取得します。</summary>
            /// <returns>標準モデルのキャラクターボーン</returns>
            public static IndexedModelHandle CreateStandardModelPS()
            {
                int numberOfNode = 0;
                ThrowIfError(CreateStandardModelPS(IntPtr.Zero, ref numberOfNode, IntPtr.Zero));

                var modelHandlePtr = Marshal.AllocHGlobal(sizeof(int));
                var parentNodeIndexesPtr = Marshal.AllocHGlobal(sizeof(int) * numberOfNode);
                var parentNodeIndexes = new int[numberOfNode];

                var res = CreateStandardModelPS(modelHandlePtr, ref numberOfNode, parentNodeIndexesPtr);

                if (res != QmErrorCode.NoError)
                {
                    Marshal.FreeHGlobal(parentNodeIndexesPtr);
                    Marshal.FreeHGlobal(modelHandlePtr);
                    throw new QmPdkException(res);
                }

                int modelHandle = (int)Marshal.PtrToStructure(modelHandlePtr, typeof(int));

                Marshal.Copy(parentNodeIndexesPtr, parentNodeIndexes, 0, parentNodeIndexes.Length);
                Marshal.FreeHGlobal(parentNodeIndexesPtr);
                Marshal.FreeHGlobal(modelHandlePtr);

                return new IndexedModelHandle(
                    new ModelHandle(modelHandle),
                    parentNodeIndexes
                    );
            }

            /// <summary>ライブラリが内部的に保持するキャラクタボーンの総数を取得します。</summary>
            /// <returns>モデルの数</returns>
            public static int GetNumOfHandle()
            {
                int num = 0;
                ThrowIfError(GetNumOfHandle(out num));
                return num;
            }

            /// <summary>インデックス指定によりキャラクタボーンのハンドルを取得します。</summary>
            /// <param name="index">インデクス。指定できる範囲は0以上、(<see cref="GetNumOfHandle"/>の戻り値-1)以下</param>
            /// <returns>モデルのハンドル</returns>
            public static ModelHandle GetHandle(int index)
            {
                int modelHandle = 0;
                ThrowIfError(GetHandle(index, out modelHandle));
                return new ModelHandle(modelHandle);
            }

            /// <summary>モデルハンドルに関連付けられているQUMAデバイスのハンドルを取得します。</summary>
            /// <param name="modelHandle">確認したいモデル</param>
            /// <returns>モデルに関連付けられたデバイスのハンドル</returns>
            public static QumaHandle GetQumaHandle(ModelHandle modelHandle)
            {
                int qumaHandle = 0;
                ThrowIfError(GetQumaHandle(modelHandle.Handle, out qumaHandle));
                return new QumaHandle(qumaHandle);
            }

            /// <summary>ボーンのノードにローカル行列を設定します。ボーンの長さ情報が必要なため平行移動成分も必要なことに注意してください。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <param name="nodeIndex">適用先のノードのインデックス</param>
            /// <param name="matrix">適用するローカル行列</param>
            public static void SetLocalMatrix(ModelHandle modelHandle, int nodeIndex, Matrix4f matrix)
            {
                var matrixPtr = Marshal.AllocHGlobal(Marshal.SizeOf(matrix));

                Marshal.StructureToPtr(matrix, matrixPtr, false);
                var res = SetLocalMatrix(modelHandle.Handle, nodeIndex, matrixPtr);

                Marshal.FreeHGlobal(matrixPtr);

                ThrowIfError(res);
            }

            /// <summary>ボーンのノードのローカル行列を取得します。</summary>
            /// <param name="modelHandle">確認先のモデル</param>
            /// <param name="nodeIndex">確認先のノードのインデックス</param>
            /// <returns>適用されているローカル行列</returns>
            public static Matrix4f GetLocalMatrix(ModelHandle modelHandle, int nodeIndex)
            {
                var matrixPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Matrix4f)));

                var res = GetLocalMatrix(modelHandle.Handle, nodeIndex, matrixPtr);
                if(res != QmErrorCode.NoError)
                {
                    Marshal.FreeHGlobal(matrixPtr);
                    throw new QmPdkException(res);
                }

                Matrix4f result = (Matrix4f)Marshal.PtrToStructure(matrixPtr, typeof(Matrix4f));
                Marshal.FreeHGlobal(matrixPtr);
                return result;
            }

            /// <summary>ボーンのノードに回転行列を設定します。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <param name="nodeIndex">適用先のノードのインデックス</param>
            /// <param name="matrix">適用する回転行列</param>
            public static void SetRotate(ModelHandle modelHandle, int nodeIndex, Matrix4f matrix)
            {
                var matrixPtr = Marshal.AllocHGlobal(Marshal.SizeOf(matrix));

                Marshal.StructureToPtr(matrix, matrixPtr, false);
                var res = SetRotate(modelHandle.Handle, nodeIndex, matrixPtr);

                Marshal.FreeHGlobal(matrixPtr);

                ThrowIfError(res);
            }

            /// <summary>ボーンのノードの回転行列を取得します。</summary>
            /// <param name="modelHandle">確認先のモデル</param>
            /// <param name="nodeIndex">確認先のノードのインデックス</param>
            /// <param name="matrix">現在の回転行列</param>
            public static Matrix4f GetRotate(ModelHandle modelHandle, int nodeIndex)
            {
                var matrixPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Matrix4f)));

                var res = GetRotate(modelHandle.Handle, nodeIndex, matrixPtr);
                if (res != QmErrorCode.NoError)
                {
                    Marshal.FreeHGlobal(matrixPtr);
                    throw new QmPdkException(res);
                }

                Matrix4f result = (Matrix4f)Marshal.PtrToStructure(matrixPtr, typeof(Matrix4f));
                Marshal.FreeHGlobal(matrixPtr);
                return result;
            }

            /// <summary>ノードに関連付けられた名前を取得します。</summary>
            /// <param name="modelHandle">確認先のモデル</param>
            /// <param name="nodeIndex">確認先のノードのインデックス</param>
            /// <returns>ノードに関連付けられた名前</returns>
            public static NodeNames GetName(ModelHandle modelHandle, int nodeIndex)
            {
                var sbName = new StringBuilder(NodeNameBufferLength);
                var sbOriginalName = new StringBuilder(NodeOriginalNameBufferLength);

                ThrowIfError(GetName(
                    modelHandle.Handle, nodeIndex,
                    sbName, NodeNameBufferLength,
                    sbOriginalName, NodeOriginalNameBufferLength
                    ));

                return new NodeNames(
                    sbName.ToString(),
                    sbOriginalName.ToString()
                    );
            }

            /// <summary>現在のポーズをTポーズとして記憶</summary>
            /// <param name="modelHandle">記憶を行わせたいモデル</param>
            public static void MemorizeInitialPose(ModelHandle modelHandle)
            {
                ThrowIfError(MemorizeInitialPose(modelHandle.Handle));
            }

            /// <summary>キャラのTポーズを呼び出す。</summary>
            /// <param name="modelHandle">呼び出し処理を行うモデル</param>
            public static void RecallInitialPose(ModelHandle modelHandle)
            {
                ThrowIfError(RecallInitialPose(modelHandle.Handle));
            }

            /// <summary>ボーンの加速度センサーモードを設定します。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <param name="mode">モード</param>
            public static void SetAccelerometerMode(ModelHandle modelHandle, AccelerometerMode mode)
            {
                ThrowIfError(SetAccelerometerMode(modelHandle.Handle, mode));
            }

            /// <summary>ボーンの加速度センサーモードを取得します。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <returns>現在のモード</returns>
            public static AccelerometerMode GetAccelerometerMode(ModelHandle modelHandle)
            {
                AccelerometerMode mode;
                ThrowIfError(GetAccelerometerMode(modelHandle.Handle, out mode));
                return mode;
            }

            /// <summary>ボーンの加速度センサー回転制限モードを設定します。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <param name="mode">指定するモード</param>
            public static void SetRestrictAccelerometerMode(ModelHandle modelHandle, AccelerometerRestrictMode mode)
            {
                ThrowIfError(SetRestrictAccelerometerMode(modelHandle.Handle, mode));
            }

            /// <summary>ボーンの加速度センサー回転制限モードを取得します。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <returns>現在のモード</returns>
            public static AccelerometerRestrictMode GetRestrictAccelerometerMode(ModelHandle modelHandle)
            {
                AccelerometerRestrictMode mode;
                ThrowIfError(GetRestrictAccelerometerMode(modelHandle.Handle, out mode));
                return mode;
            }

            #endregion

        }

        /// <summary>QUMARIONデバイス関連</summary>
        public static class Quma
        {
            #region privateなDllImport

            #region GetNumOfHandle
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetNumOfHandle@@YAHPAH@Z")]
            private static extern QmErrorCode GetNumOfHandle86(out int num);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetNumOfHandle@@YAHPEAH@Z")]
            private static extern QmErrorCode GetNumOfHandle64(out int num);
            private static QmErrorCode GetNumOfHandle(out int num)
                => Is64bit ? GetNumOfHandle64(out num) : GetNumOfHandle86(out num);
            #endregion

            #region GetHandle
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetHandle@@YAHHPAH@Z")]
            private static extern QmErrorCode GetHandle86(int index, out int qumaHandle);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetHandle@@YAHHPEAH@Z")]
            private static extern QmErrorCode GetHandle64(int index, out int qumaHandle);
            private static QmErrorCode GetHandle(int index, out int qumaHandle)
                => Is64bit ? GetHandle64(index, out qumaHandle) : GetHandle86(index, out qumaHandle);
            #endregion

            #region GetNumOfAttachedModel
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetNumOfAttachedModel@@YAHHPAH@Z")]
            private static extern QmErrorCode GetNumOfAttachedModel86(int qumaHandle, out int num);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetNumOfAttachedModel@@YAHHPEAH@Z")]
            private static extern QmErrorCode GetNumOfAttachedModel64(int qumaHandle, out int num);
            private static QmErrorCode GetNumOfAttachedModel(int qumaHandle, out int num)
                => Is64bit ? GetNumOfAttachedModel64(qumaHandle, out num) : GetNumOfAttachedModel86(qumaHandle, out num);
            #endregion

            #region Enable
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkEnable@@YAHHH@Z")]
            private static extern QmErrorCode Enable86(int qumaHandle, int isEnable);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkEnable@@YAHHH@Z")]
            private static extern QmErrorCode Enable64(int qumaHandle, int isEnable);
            private static QmErrorCode Enable(int qumaHandle, int isEnable)
                => Is64bit ? Enable64(qumaHandle, isEnable) : Enable86(qumaHandle, isEnable);
            #endregion

            #region IsEnable
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkIsEnable@@YAHHPAH@Z")]
            private static extern QmErrorCode IsEnable86(int qumaHandle, out int isEnable);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkIsEnable@@YAHHPEAH@Z")]
            private static extern QmErrorCode IsEnable64(int qumaHandle, out int isEnable);
            private static QmErrorCode IsEnable(int qumaHandle, out int isEnable)
                => Is64bit ? IsEnable64(qumaHandle, out isEnable) : IsEnable86(qumaHandle, out isEnable);
            #endregion

            #region EnableAccelerometer
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkEnableAccelerometer@@YAHHH@Z")]
            private static extern QmErrorCode EnableAccelerometer86(int qumaHandle, int isEnable);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkEnableAccelerometer@@YAHHH@Z")]
            private static extern QmErrorCode EnableAccelerometer64(int qumaHandle, int isEnable);
            private static QmErrorCode EnableAccelerometer(int qumaHandle, int isEnable)
                => Is64bit ? EnableAccelerometer64(qumaHandle, isEnable) : EnableAccelerometer86(qumaHandle, isEnable);
            #endregion

            #region GetButtonState
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetButtonState@@YAHHHPAH@Z")]
            private static extern QmErrorCode GetButtonState86(int qumaHandle, QumaButton buttonType, out QumaButtonState state);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetButtonState@@YAHHHPEAH@Z")]
            private static extern QmErrorCode GetButtonState64(int qumaHandle, QumaButton buttonType, out QumaButtonState state);
            private static QmErrorCode GetButtonState(int qumaHandle, QumaButton buttonType, out QumaButtonState state)
                => Is64bit ?
                GetButtonState64(qumaHandle, buttonType, out state) :
                GetButtonState86(qumaHandle, buttonType, out state);
            #endregion

            #region AttachInitPoseModel
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaAttachInitPoseModel@@YAHHH@Z")]
            private static extern QmErrorCode AttachInitPoseModel86(int qumaHandle, int modelHandle);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaAttachInitPoseModel@@YAHHH@Z")]
            private static extern QmErrorCode AttachInitPoseModel64(int qumaHandle, int modelHandle);
            private static QmErrorCode AttachInitPoseModel(int qumaHandle, int modelHandle)
                => Is64bit ? AttachInitPoseModel64(qumaHandle, modelHandle) : AttachInitPoseModel86(qumaHandle, modelHandle);
            #endregion

            #region DetachModel
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaDetachModel@@YAHH@Z")]
            private static extern QmErrorCode DetachModel86(int modelHandle);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaDetachModel@@YAHH@Z")]
            private static extern QmErrorCode DetachModel64(int modelHandle);
            private static QmErrorCode DetachModel(int modelHandle)
                => Is64bit ? DetachModel64(modelHandle) : DetachModel86(modelHandle);
            #endregion

            #region IsPoseChanged
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaIsPoseChanged@@YAHHPAH@Z")]
            private static extern QmErrorCode IsPoseChanged86(int qumaHandle, out int isPoseChanged);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaIsPoseChanged@@YAHHPEAH@Z")]
            private static extern QmErrorCode IsPoseChanged64(int qumaHandle, out int isPoseChanged);
            private static QmErrorCode IsPoseChanged(int qumaHandle, out int isPoseChanged)
                => Is64bit ? IsPoseChanged64(qumaHandle, out isPoseChanged) : IsPoseChanged86(qumaHandle, out isPoseChanged);
            #endregion

            #region GetDeviceName
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetDeviceName@@YAHHPADPAH@Z")]
            private static extern QmErrorCode GetDeviceName86(int qumaHandle, [MarshalAs(UnmanagedType.LPStr)]StringBuilder name, out int size);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetDeviceName@@YAHHPEADPEAH@Z")]
            private static extern QmErrorCode GetDeviceName64(int qumaHandle, [MarshalAs(UnmanagedType.LPStr)]StringBuilder name, out int size);
            private static QmErrorCode GetDeviceName(int qumaHandle, StringBuilder name, out int size)
                => Is64bit ? GetDeviceName64(qumaHandle, name, out size) : GetDeviceName86(qumaHandle, name, out size);
            #endregion

            #region GetDeviceState
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetDeviceState@@YAHH@Z")]
            private static extern QmErrorCode GetDeviceState86(int qumaHandle);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetDeviceState@@YAHH@Z")]
            private static extern QmErrorCode GetDeviceState64(int qumaHandle);
            private static QmErrorCode GetDeviceState(int qumaHandle)
                => Is64bit ? GetDeviceState64(qumaHandle) : GetDeviceState86(qumaHandle);
            #endregion

            #region CheckDeviceSensors
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaCheckDeviceSensors@@YAHHPAHPAD0@Z")]
            private static extern QmErrorCode CheckDeviceSensors86(
                int qumaHandle,
                out int isOk,
                [MarshalAs(UnmanagedType.LPStr)]StringBuilder detailMessage,
                ref int detailMessageSize
                );
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaCheckDeviceSensors@@YAHHPEAHPEAD0@Z")]
            private static extern QmErrorCode CheckDeviceSensors64(
                int qumaHandle,
                out int isOk,
                [MarshalAs(UnmanagedType.LPStr)]StringBuilder detailMessage,
                ref int detailMessageSize
                );
            private static QmErrorCode CheckDeviceSensors(
                int qumaHandle, out int isOk, StringBuilder detailMessage, ref int detailMessageSize
                ) 
                => Is64bit ?
                CheckDeviceSensors64(qumaHandle, out isOk, detailMessage, ref detailMessageSize) :
                CheckDeviceSensors86(qumaHandle, out isOk, detailMessage, ref detailMessageSize);

            #endregion

            #endregion

            #region publicなラップ済み関数

            /// <summary>接続されているQUMAデバイスの総数を取得します。</summary>
            /// <returns>接続されているQUMAデバイスの総数</returns>
            public static int GetDeviceCount()
            {
                int num;
                ThrowIfError(GetNumOfHandle(out num));
                return num;
            }

            /// <summary>インデックスを指定してQUMAデバイスのハンドルを取得します。</summary>
            /// <param name="index">インデクス。指定可能な値は0以上、(<see cref="GetNumOfHandle"/>の戻り値-1)以下</param>
            /// <returns>QUMAデバイスのハンドル</returns>
            public static QumaHandle GetHandle(int index)
            {
                int qumaHandle;
                ThrowIfError(GetHandle(index, out qumaHandle));
                return new QumaHandle(qumaHandle);
            }

            /// <summary>QUMAデバイスに関連付けられたボーンの数を取得します。</summary>
            /// <param name="qumaHandle">確認先のQUMAデバイス</param>
            /// <returns>デバイスに関連付けられたボーンの数</returns>
            public static int GetAttachedModelCount(QumaHandle qumaHandle)
            {
                int num;
                ThrowIfError(GetNumOfAttachedModel(qumaHandle.Handle, out num));
                return num;
            }

            /// <summary>
            /// <para>QUMAデバイスのポーズコピー処理の有効/無効を設定します。</para>
            /// <para>ポーリングでポーズコピー処理を呼んでいる場合、この関数で切り替えを行うことで</para>
            /// <para>一次的にポーズコピーを無効にできます。</para>
            /// </summary>
            /// <param name="qumaHandle">適用先のデバイス</param>
            /// <param name="enable">デバイスを有効化するかどうか</param>
            public static void SetEnableQuma(QumaHandle qumaHandle, bool enable)
                => ThrowIfError(Enable(qumaHandle.Handle, Convert.ToInt32(enable)));

            /// <summary>QUMAデバイスが現在有効化されているかどうかを取得します。</summary>
            /// <param name="qumaHandle">確認先のデバイス</param>
            /// <returns>デバイスが有効化されているかどうか</returns>
            public static bool GetEnableQuma(QumaHandle qumaHandle)
            {
                int isEnable;
                ThrowIfError(IsEnable(qumaHandle.Handle, out isEnable));
                return Convert.ToBoolean(isEnable);
            }

            /// <summary>QUMAデバイスの加速度センサの有効/無効を設定します。</summary>
            /// <param name="qumaHandle">設定対象のデバイス</param>
            /// <param name="enable">有効化する場合は<see cref="true"/>、無効化する場合は<see cref="false"/></param>
            public static void SetEnableAccelerometer(QumaHandle qumaHandle, bool enable)
                => ThrowIfError(EnableAccelerometer(qumaHandle.Handle, Convert.ToInt32(enable)));

            /// <summary>デバイスのボタンの状態を取得します。
            /// <param name="qumaHandle">確認先のデバイス</param>
            /// <returns>現在のボタンの状態</returns>
            public static QumaButtonState GetButtonState(QumaHandle qumaHandle)
            {
                QumaButtonState state;
                ThrowIfError(GetButtonState(qumaHandle.Handle, QumaButton.MainButton, out state));
                return state;
            }

            /// <summary>
            /// <para>ボーンへデバイスを関連づけます。</para>
            /// <para>この関数を呼び出すことでQUMAドライバにモデルの初期化必要な情報が渡され、</para>
            /// <para>デバイスからモデルへのポーズ反映が可能になります。</para>
            /// </summary>
            /// <param name="qumaHandle">関連付けたいデバイス</param>
            /// <param name="modelHandle">関連付けたいモデル</param>
            public static void AttachInitPoseModel(QumaHandle qumaHandle, ModelHandle modelHandle)
                => ThrowIfError(AttachInitPoseModel(qumaHandle.Handle, modelHandle.Handle));

            /// <summary>モデルをデバイスから切断します。</summary>
            /// <param name="modelHandle">切断したいモデル</param>
            public static void DetachModel(ModelHandle modelHandle)
                => ThrowIfError(DetachModel(modelHandle.Handle));

            /// <summary>
            /// デバイスのポーズが前回から変更されたかを取得します。
            /// [CAUTION]現状(2015/12/22)ではこの関数の戻り値がイマイチ信用できないので
            /// 戻り値は使わず定期的に<see cref="BaseOperation.CopyPose"/>するのが良さそう
            /// </summary>
            /// <param name="qumaHandle">確認先のデバイス</param>
            /// <returns>ポーズが変更されているかどうか</returns>
            public static bool CheckIsPoseChanged(QumaHandle qumaHandle)
            {
                int isPoseChanged;
                ThrowIfError(IsPoseChanged(qumaHandle.Handle, out isPoseChanged));
                return Convert.ToBoolean(isPoseChanged);
            }

            /// <summary>QUMAデバイスの物理デバイス固有名を取得します。</summary>
            /// <param name="qumaHandle"></param>
            /// <returns></returns>
            public static string GetDeviceName(QumaHandle qumaHandle)
            {
                int size;
                ThrowIfError(GetDeviceName(qumaHandle.Handle, null, out size));

                var name = new StringBuilder(size);
                ThrowIfError(GetDeviceName(qumaHandle.Handle, name, out size));
                return name.ToString();
            }

            /// <summary>
            /// <para>QUMAデバイスが有効かどうかを確認し、USBが挿抜されるなどで無効化されている場合例外をスローします。</para> 
            /// <para>
            /// <see cref="QmErrorCode.DeviceUpdateBuffer"/>を持つ<see cref="QmPdkException"/>
            /// がスローされた場合、デバイスの再初期化が必要です。</para>
            /// </summary>
            /// <param name="qumaHandle">確認先のデバイス</param>
            public static void CheckDeviceValidity(QumaHandle qumaHandle)
                => ThrowIfError(GetDeviceState(qumaHandle.Handle));

            /// <summary>デバイスのセンサ状態を確認します。</summary>
            /// <param name="qumaHandle">確認したいデバイス</param>
            /// <returns>確認結果</returns>
            public static SensorsState CheckDeviceSensors(QumaHandle qumaHandle)
            {
                int isOk;
                int messageSize = 0;
                ThrowIfError(CheckDeviceSensors(qumaHandle.Handle, out isOk, null, ref messageSize));

                if(Convert.ToBoolean(isOk))
                {
                    return new SensorsState(true, "");
                }

                var message = new StringBuilder(messageSize);
                ThrowIfError(CheckDeviceSensors(qumaHandle.Handle, out isOk, message, ref messageSize));

                return new SensorsState(Convert.ToBoolean(isOk), message.ToString());
            }

            #endregion
        }

        /// <summary>Template(PS標準)ボーン関連</summary>
        public static class TemplateBone
        {

            #region privateなDllImport

            #region GetName
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetName@@YAHHHPADH@Z")]
            private static extern QmErrorCode GetName86(int modelHandle, int nodeIndex, [MarshalAs(UnmanagedType.LPStr)]StringBuilder name, int sizeName);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetName@@YAHHHPEADH@Z")]
            private static extern QmErrorCode GetName64(int modelHandle, int nodeIndex, [MarshalAs(UnmanagedType.LPStr)]StringBuilder name, int sizeName);
            private static QmErrorCode GetName(int modelHandle, int nodeIndex, StringBuilder name, int sizeName)
                => Is64bit ? GetName64(modelHandle, nodeIndex, name, sizeName) : GetName86(modelHandle, nodeIndex, name, sizeName);
            #endregion

            #region GetIdx
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetIdx@@YAHHPADPAH@Z")]
            private static extern QmErrorCode GetIdx86(int modelHandle, [MarshalAs(UnmanagedType.LPStr)]string name, out int index);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetIdx@@YAHHPEADPEAH@Z")]
            private static extern QmErrorCode GetIdx64(int modelHandle, [MarshalAs(UnmanagedType.LPStr)]string name, out int index);
            private static QmErrorCode GetIdx(int modelHandle, string name, out int index)
                => Is64bit ? GetIdx64(modelHandle, name, out index) : GetIdx86(modelHandle, name, out index);
            #endregion

            #region GetRootNodeIdx
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetRootNodeIdx@@YAHHPAH@Z")]
            private static extern QmErrorCode GetRootNodeIdx86(int modelHandle, out int rootIndex);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetRootNodeIdx@@YAHHPEAH@Z")]
            private static extern QmErrorCode GetRootNodeIdx64(int modelHandle, out int rootIndex);
            private static QmErrorCode GetRootNodeIdx(int modelHandle, out int rootIndex)
                => Is64bit ? GetRootNodeIdx64(modelHandle, out rootIndex) : GetRootNodeIdx86(modelHandle, out rootIndex);
            #endregion

            #region GetParentNodeIdx
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetParentNodeIdx@@YAHHHPAH@Z")]
            private static extern QmErrorCode GetParentNodeIdx86(int modelHandle, int nodeIndex, out int parentIndex);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetParentNodeIdx@@YAHHHPEAH@Z")]
            private static extern QmErrorCode GetParentNodeIdx64(int modelHandle, int nodeIndex, out int parentIndex);
            private static QmErrorCode GetParentNodeIdx(int modelHandle, int nodeIndex, out int parentIndex)
                => Is64bit ?
                GetParentNodeIdx64(modelHandle, nodeIndex, out parentIndex) :
                GetParentNodeIdx86(modelHandle, nodeIndex, out parentIndex);
            #endregion

            #region GetChildNodeIdx
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetChildNodeIdx@@YAHHHPAH0@Z")]
            private static extern QmErrorCode GetChildNodeIdx86(int modelHandle, int nodeIndex, IntPtr childIndex, ref int numberOfChildNode);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetChildNodeIdx@@YAHHHPEAH0@Z")]
            private static extern QmErrorCode GetChildNodeIdx64(int modelHandle, int nodeIndex, IntPtr childIndex, ref int numberOfChildNode);
            private static QmErrorCode GetChildNodeIdx(int modelHandle, int nodeIndex, IntPtr childIndex, ref int numberOfChildNode)
                => Is64bit ?
                GetChildNodeIdx64(modelHandle, nodeIndex, childIndex, ref numberOfChildNode) :
                GetChildNodeIdx86(modelHandle, nodeIndex, childIndex, ref numberOfChildNode);

            #endregion

            #endregion

            #region publicなラップ済み関数

            /// <summary>標準ボーンのノードインデックスから対応する名前を取得します。</summary>
            /// <param name="modelHandle">確認先のモデル</param>
            /// <param name="nodeIndex">確認先のインデックス</param>
            /// <returns>ボーンの名前。NiNinBaoriで識別子として用いられる</returns>
            public static string GetName(ModelHandle modelHandle, int nodeIndex)
            {
                var name = new StringBuilder(NodeNameBufferLength);
                ThrowIfError(GetName(modelHandle.Handle, nodeIndex, name, NodeNameBufferLength));
                return name.ToString();
            }

            /// <summary>標準ボーンのノード名から対応するインデックスを取得します。</summary>
            /// <param name="modelHandle">確認先のモデル</param>
            /// <param name="name">インデックスを確認したいボーンの名前</param>
            /// <returns>対応するインデックス</returns>
            public static int GetIndex(ModelHandle modelHandle, string name)
            {
                int nodeIndex;
                ThrowIfError(GetIdx(modelHandle.Handle, name, out nodeIndex));
                return nodeIndex;
            }

            /// <summary>標準ボーンのルートボーンのインデックスを取得します。</summary>
            /// <param name="modelHandle">確認先のモデル</param>
            /// <returns>ルートボーンのインデックス</returns>
            public static int GetRootNodeIndex(ModelHandle modelHandle)
            {
                int rootIndex;
                ThrowIfError(GetRootNodeIdx(modelHandle.Handle, out rootIndex));
                return rootIndex;
            }

            /// <summary>標準ボーンのノードインデックスを指定し、親要素のノードのインデックスを取得します。</summary>
            /// <param name="modelHandle">確認先のモデル</param>
            /// <param name="nodeIndex"></param>
            /// <returns></returns>
            public static int GetParentNodeIndex(ModelHandle modelHandle, int nodeIndex)
            {
                int parentIndex;
                ThrowIfError(GetParentNodeIdx(modelHandle.Handle, nodeIndex, out parentIndex));
                return parentIndex;
            }

            /// <summary>標準ボーンのノードインデックスを指定し、子ボーンのインデックス一覧を取得します。</summary>
            /// <param name="modelHandle">確認先のモデル</param>
            /// <param name="nodeIndex">確認したいノード</param>
            /// <returns>指定したノードの子ボーンのインデックス一覧</returns>
            public static int[] GetChildNodeIndex(ModelHandle modelHandle, int nodeIndex)
            {
                int numberOfChildNode = 0;
                ThrowIfError(GetChildNodeIdx(modelHandle.Handle, nodeIndex, IntPtr.Zero, ref numberOfChildNode));

                IntPtr indexes = Marshal.AllocHGlobal(sizeof(int) * numberOfChildNode);
                var result = new int[numberOfChildNode];

                ThrowIfError(GetChildNodeIdx(modelHandle.Handle, nodeIndex, indexes, ref numberOfChildNode));

                Marshal.Copy(indexes, result, 0, numberOfChildNode);

                Marshal.FreeHGlobal(indexes);
                return result;
            }

            #endregion

        }

        /// <summary>NNB関連</summary>
        public static class NiNinBaori
        {
            #region privateなDllImport

            #region IsValid
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbIsValid@@YAHHPAH@Z")]
            private static extern QmErrorCode IsValid86(int modelHandle, out int isValid);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbIsValid@@YAHHPEAH@Z")]
            private static extern QmErrorCode IsValid64(int modelHandle, out int isValid);
            private static QmErrorCode IsValid(int modelHandle, out int isValid)
                => Is64bit ? IsValid64(modelHandle, out isValid) : IsValid86(modelHandle, out isValid);
            #endregion

            #region LoadFromFile
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbLoadFromFile@@YAHHPBD0@Z")]
            private static extern QmErrorCode LoadFromFile86(int modelHandle, [MarshalAs(UnmanagedType.LPStr)]string nnbFilePath, [MarshalAs(UnmanagedType.LPWStr)]string templateFilePath);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbLoadFromFile@@YAHHPEBD0@Z")]
            private static extern QmErrorCode LoadFromFile64(int modelHandle, [MarshalAs(UnmanagedType.LPStr)]string nnbFilePath, [MarshalAs(UnmanagedType.LPWStr)]string templateFilePath);
            private static QmErrorCode LoadFromFile(int modelHandle, string nnbFilePath, string templateFilePath)
                => Is64bit ?
                LoadFromFile64(modelHandle, nnbFilePath, templateFilePath) :
                LoadFromFile86(modelHandle, nnbFilePath, templateFilePath);
            #endregion

            #region SaveToFile
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbSaveToFile@@YAHHHPBD@Z")]
            private static extern QmErrorCode SaveToFile86(int modelHandle, int withCalibrationData, [MarshalAs(UnmanagedType.LPWStr)]string filepath);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbSaveToFile@@YAHHHPEBD@Z")]
            private static extern QmErrorCode SaveToFile64(int modelHandle, int withCalibrationData, [MarshalAs(UnmanagedType.LPWStr)]string filepath);
            private static QmErrorCode SaveToFile(int modelHandle, int withCalibrationData, string filepath)
                => Is64bit ?
                SaveToFile64(modelHandle, withCalibrationData, filepath) :
                SaveToFile86(modelHandle, withCalibrationData, filepath);
            #endregion

            #region LoadFromMem
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbLoadFromMem@@YAHHPBDH@Z")]
            private static extern QmErrorCode LoadFromMem86(int modelHandle, IntPtr data, int dataSize);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbLoadFromMem@@YAHHPEBDH@Z")]
            private static extern QmErrorCode LoadFromMem64(int modelHandle, IntPtr data, int dataSize);
            private static QmErrorCode LoadFromMem(int modelHandle, IntPtr data, int dataSize)
                => Is64bit ? LoadFromMem(modelHandle, data, dataSize) : LoadFromMem86(modelHandle, data, dataSize);
            #endregion

            #region SaveToMem
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbSaveToMem@@YAHHHPADPAH@Z")]
            private static extern QmErrorCode SaveToMem86(int modelHandle, int withCalibrationData, IntPtr outData, ref int outDataSize);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbSaveToMem@@YAHHHPEADPEAH@Z")]
            private static extern QmErrorCode SaveToMem64(int modelHandle, int withCalibrationData, IntPtr outData, ref int outDataSize);
            private static QmErrorCode SaveToMem(int modelHandle, int withCalibrationData, IntPtr outData, ref int outDataSize)
                => Is64bit ?
                SaveToMem64(modelHandle, withCalibrationData, outData, ref outDataSize) :
                SaveToMem86(modelHandle, withCalibrationData, outData, ref outDataSize);
            #endregion

            #region CreateRootGroup
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbCreateRootGroup@@YAHHHHPAH@Z")]
            private static extern QmErrorCode CreateRootGroup86(int modelHandle, int charRootNodeIndex, int charNextNodeIndex, out int groupIdx);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbCreateRootGroup@@YAHHHHPEAH@Z")]
            private static extern QmErrorCode CreateRootGroup64(int modelHandle, int charRootNodeIndex, int charNextNodeIndex, out int groupIdx);
            private static QmErrorCode CreateRootGroup(int modelHandle, int charRootNodeIndex, int charNextNodeIndex, out int groupIdx)
                => Is64bit ?
                CreateRootGroup64(modelHandle, charRootNodeIndex, charNextNodeIndex, out groupIdx) :
                CreateRootGroup86(modelHandle, charRootNodeIndex, charNextNodeIndex, out groupIdx);
            #endregion

            #region AddGroup
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbAddGroup@@YAHHHHPAHH00@Z")]
            private static extern QmErrorCode AddGroup86(
                int modelHandle,
                int parentGroupIndex,
                int numberOfCharNodeIndexes,
                IntPtr charNodeIndexes,
                int numberOfTemplateNodeIndexes,
                IntPtr templateNodeIndexes,
                out int groupIndex
                );
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbAddGroup@@YAHHHHPEAHH00@Z")]
            private static extern QmErrorCode AddGroup64(
                int modelHandle, 
                int parentGroupIndex,
                int numberOfCharNodeIndexes,
                IntPtr charNodeIndexes,
                int numberOfTemplateNodeIndexes,
                IntPtr templateNodeIndexes,
                out int groupIndex
                );
            private static QmErrorCode AddGroup(
                int modelHandle,
                int parentGroupIndex,
                int numberOfCharNodeIndexes,
                IntPtr charNodeIndexes,
                int numberOfTemplateNodeIndexes,
                IntPtr templateNodeIndexes,
                out int groupIndex
                )
                => Is64bit ?
                AddGroup64(
                    modelHandle, parentGroupIndex,
                    numberOfCharNodeIndexes, charNodeIndexes,
                    numberOfTemplateNodeIndexes, templateNodeIndexes,
                    out groupIndex) :
                AddGroup86(
                    modelHandle, parentGroupIndex,
                    numberOfCharNodeIndexes, charNodeIndexes,
                    numberOfTemplateNodeIndexes, templateNodeIndexes,
                    out groupIndex);
            #endregion

            #region Apply
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbApply@@YAHH@Z")]
            private static extern QmErrorCode Apply86(int modelHandle);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbApply@@YAHH@Z")]
            private static extern QmErrorCode Apply64(int modelHandle);
            private static QmErrorCode Apply(int modelHandle)
                => Is64bit ? Apply64(modelHandle) : Apply86(modelHandle);
            #endregion

            #endregion

            #region publicなラップ済み関数

            /// <summary>NNBの情報を持っているかどうかを取得します。</summary>
            /// <param name="modelHandle">確認先のモデル</param>
            /// <returns>NNBの情報を持っているかどうか</returns>
            public static bool CheckIsValid(ModelHandle modelHandle)
            {
                int isValid;
                ThrowIfError(IsValid(modelHandle.Handle, out isValid));
                return Convert.ToBoolean(isValid);
            }

            /// <summary>ファイル(.nnb)へのパスを指定してNNB情報をロードします。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <param name="nnbFilePath">ロード元のファイルパス</param>
            public static void LoadFromFile(ModelHandle modelHandle, string nnbFilePath)
            {
                ThrowIfError(LoadFromFile(modelHandle.Handle, nnbFilePath, ""));
            }

            /// <summary>NNB情報をファイルに保存します。</summary>
            /// <param name="modelHandle">保存したいモデル</param>
            /// <param name="filepath">保存先のファイル名</param>
            /// <param name="withCalibrationData">キャリブレーション情報を保存先ファイルに含めるかどうか</param>
            public static void SaveToFile(ModelHandle modelHandle, string filepath, bool withCalibrationData)
            {
                ThrowIfError(SaveToFile(modelHandle.Handle, Convert.ToInt32(withCalibrationData), filepath));
            }

            /// <summary>メモリデータ上のNNB情報をモデルに適用します。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <param name="data">適用するNNBデータ</param>
            public static void LoadFromMem(ModelHandle modelHandle, byte[] data)
            {
                var dataPtr = Marshal.AllocHGlobal(data.Length);
                Marshal.Copy(data, 0, dataPtr, data.Length);

                LoadFromMem(modelHandle.Handle, dataPtr, data.Length);

                Marshal.FreeHGlobal(dataPtr);
            }

            /// <summary>モデルの現在のNNB情報をバイトデータとして取得します。</summary>
            /// <param name="modelHandle">取得元のモデル</param>
            /// <param name="withCalibrationData">キャリブレーション情報を含むかどうか</param>
            /// <returns>
            /// NNB情報(文字列化する場合<see cref="Encoding.UTF8"/>で<see cref="Encoding.GetString(byte[])"/>を使う)
            /// </returns>
            public static byte[] SaveToMem(ModelHandle modelHandle, bool withCalibrationData)
            {
                //TODO: なんとなくココまでのAPIのノリだとNULL代入でデータ長が拾えそうな予感するのでそう書いたが確証が無い。

                int outDataSize = 0;
                ThrowIfError(SaveToMem(modelHandle.Handle, Convert.ToInt32(withCalibrationData), IntPtr.Zero, ref outDataSize));

                IntPtr dataPtr = Marshal.AllocHGlobal(outDataSize);
                var res = SaveToMem(modelHandle.Handle, Convert.ToInt32(withCalibrationData), dataPtr, ref outDataSize);

                if (res != QmErrorCode.NoError)
                {
                    Marshal.FreeHGlobal(dataPtr);
                    throw new QmPdkException(res);
                }

                var result = new byte[outDataSize];
                Marshal.Copy(dataPtr, result, 0, outDataSize);
                Marshal.FreeHGlobal(dataPtr);
                return result;
            }

            /// <summary>ルートボーングループを作成します。</summary>
            /// <param name="modelHandle">作成するモデル</param>
            /// <param name="charRootNodeIndex">キャラクターのルートボーン(hips_bb_)のインデックス</param>
            /// <param name="charNextNodeIndex">キャラクターのルートボーンの子要素(spine_bb_)のインデックス</param>
            /// <returns>作成されたルートボーングループのグループインデックス</returns>
            public static int CreateRootGroup(ModelHandle modelHandle, int charRootNodeIndex, int charNextNodeIndex)
            {
                int groupIndex;
                ThrowIfError(CreateRootGroup(modelHandle.Handle, charRootNodeIndex, charNextNodeIndex, out groupIndex));
                return groupIndex;
            }

            /// <summary>ボーンの対応付けグループを設定します。</summary>
            /// <param name="modelHandle">適用するモデル</param>
            /// <param name="parentGroupIndex">親グループのインデックス</param>
            /// <param name="characterNodeIndexes">キャラクターボーン側のインデックス一覧</param>
            /// <param name="templateNodeIndexes">テンプレートボーン側のインデックス一覧</param>
            /// <returns>生成されたグループのインデックス</returns>
            public static int AddGroup(ModelHandle modelHandle, int parentGroupIndex, int[] characterNodeIndexes, int[] templateNodeIndexes)
            {
                int groupIndex;

                var characterNodeIndexesPtr = Marshal.AllocHGlobal(sizeof(int) * characterNodeIndexes.Length);
                var templateNodeIndexesPtr = Marshal.AllocHGlobal(sizeof(int) * templateNodeIndexes.Length);

                var res = AddGroup(
                    modelHandle.Handle,
                    parentGroupIndex,
                    characterNodeIndexes.Length,
                    characterNodeIndexesPtr,
                    templateNodeIndexes.Length,
                    templateNodeIndexesPtr,
                    out groupIndex
                    );

                Marshal.FreeHGlobal(characterNodeIndexesPtr);
                Marshal.FreeHGlobal(templateNodeIndexesPtr);

                ThrowIfError(res);

                return groupIndex;
            }

            /// <summary>設定されたNNBグループの情報に基づいてNNB情報を作成します。</summary>
            /// <param name="modelHandle">作成元のモデル</param>
            public static void Apply(ModelHandle modelHandle)
            {
                ThrowIfError(Apply(modelHandle.Handle));
            }

            #endregion
        }

        /// <summary>デバッグ関連</summary>
        public static class Debug
        {
            ///// <summary>デバッグ関数とだけ記載あり(詳細ないので触らないのが吉？)</summary>
            //[DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaDebugSetScriptForQumax@@YAHHPBD@Z")]
            //[DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaDebugSetScriptForQumax@@YAHHPEBD@Z")]
            //private static extern QmPdkErrorCode NotImplementedFunction39();


            /// <summary>デバッグ出力をオンにする関数らしい(サンプルコードまで確認したものの詳細不明)</summary>
            #region DebugFlags
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkDebugFlags@@YAHPBD@Z")]
            private static extern QmErrorCode DebugFlags86([MarshalAs(UnmanagedType.LPStr)]string flags);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkDebugFlags@@YAHPEBD@Z")]
            private static extern QmErrorCode DebugFlags64([MarshalAs(UnmanagedType.LPStr)]string flags);
            private static QmErrorCode DebugFlags(string flags)
                => Is64bit ? DebugFlags64(flags) : DebugFlags86(flags);
            #endregion

            /// <summary>
            /// <para>デバッグメッセージを有効化します。(※無効化方法は不明)</para>
            /// <para>本関数はSDKサンプルである"QmPdkSampleStandard.cpp"の使用例を基に実装しています</para>
            /// </summary>
            public static void SetDebugFlags()
            {
                string flags = new string(new char[] { (char)(0x01 | 0x02) });
                ThrowIfError(DebugFlags(flags));
            }

        }

    }

}
