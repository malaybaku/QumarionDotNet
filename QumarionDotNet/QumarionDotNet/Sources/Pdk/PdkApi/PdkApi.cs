using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Baku.Quma.Pdk.Api
{
    /// <summary>QmPdkDll.hに定義されている関数をラップします。</summary>
    public static class QmPdk
    {
        //NOTE: 理由がよく分からないがdebugのQmPdkDll.dllは呼べないらしい？
#if !T_X64
        public const string DllName = @"dll\release\x86\QmPdkDll.dll";
        //public const string DllName = @"dll\debug\x86\QmPdkDll.dll";
#else
        public const string DllName = @"dll\release\x64\QmPdkDll.dll";
        //public const string DllName = @"dll\debug\x64\QmPdkDll.dll";
#endif

        private const int NodeNameBufferLength = 256;
        private const int NodeOriginalNameBufferLength = 256;
        private const int DeviceNameBufferLength = 256;
        private const int SensorsCheckMessageBufferLength = 1024;

        private static void ThrowIfError(QmPdkErrorCode errorCode)
        {
            if(errorCode != QmPdkErrorCode.NoError)
            {
                throw new QmPdkException(errorCode);
            }
        }

        /// <summary>初期化と終了、各種処理</summary>
        public static class BaseOperation
        {
            #region privateなDllImport

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkGetVersionStr@@YAHPADPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkGetVersionStr@@YAHPEADPEAH@Z")]
#endif
            private static extern QmPdkErrorCode GetVersionStr([MarshalAs(UnmanagedType.LPWStr)]StringBuilder versionStr, ref int versionStrSize);

            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkInit@@YAHXZ")]
            private static extern QmPdkErrorCode TryInit();

            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkFinal@@YAHXZ")]
            private static extern QmPdkErrorCode TryFinal();

            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkDisconnect@@YAHXZ")]
            private static extern QmPdkErrorCode TryDisconnect();

            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkSetForcedCopyPose@@YAHH@Z")]
            private static extern QmPdkErrorCode TrySetForcedCopyPose([MarshalAs(UnmanagedType.I4)]bool isEnabled);


            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCopyPose@@YAHH@Z")]
            private static extern QmPdkErrorCode CopyPose(int modelHandle);

            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCalibratePose@@YAHH@Z")]
            private static extern QmPdkErrorCode CalibratePose(int modelHandle);


#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkSaveCalibrationDataFile@@YAHHPBD@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkSaveCalibrationDataFile@@YAHHPEBD@Z")]
#endif
            private static extern QmPdkErrorCode SaveCalibrationDataFile(int modelHandle, [MarshalAs(UnmanagedType.LPWStr)]string filepath);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkSaveCalibrationDataMem@@YAHHPADPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkSaveCalibrationDataMem@@YAHHPEADPEAH@Z")]
#endif
            private static extern QmPdkErrorCode SaveCalibrationDataMem(int modelHandle, ref byte[] outdData, ref int outDataSize);


#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkLoadCalibrationDataFile@@YAHHPBD@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkLoadCalibrationDataFile@@YAHHPEBD@Z")]
#endif
            private static extern QmPdkErrorCode LoadCalibrationDataFile(int modelHandle, [MarshalAs(UnmanagedType.LPWStr)]string filepath);


#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkLoadCalibrationDataMem@@YAHHPBDH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkLoadCalibrationDataMem@@YAHHPEBDH@Z")]
#endif
            private static extern QmPdkErrorCode LoadCalibrationDataMem(int modelHandle, IntPtr data, int dataSize);


            #endregion

            /// <summary>ライブラリのバージョン文字列を取得します。失敗した場合は空文字列を返します。</summary>
            /// <returns></returns>
            public static string GetVersionStr()
            {
                int strSize = 0;
                if(GetVersionStr(null, ref strSize) != QmPdkErrorCode.NoError)
                {
                    return "";
                }

                var result = new StringBuilder(strSize);
                if (GetVersionStr(result, ref strSize) != QmPdkErrorCode.NoError)
                {
                    return "";
                }

                return result.ToString();
            }

            /// <summary>
            /// ライブラリの初期化です。アプリケーション開始時に一度だけ呼んでください。
            /// </summary>
            public static void Init()
            {
                ThrowIfError(TryInit());
            }

            /// <summary>
            /// ライブラリの終了処理です。アプリケーション終了時に一度だけ呼んでください。
            /// </summary>
            public static void Final()
            {
                ThrowIfError(TryFinal());
            }

            /// <summary>
            /// ライブラリの切断処理です。アプリケーションの非アクティブ時に呼んでください
            /// (とラップ元のヘッダには載っているが、よく分からないなら使わないのが無難か。)
            /// </summary>
            public static void Disconnect()
            {
                ThrowIfError(TryDisconnect());
            }

            /// <summary>
            /// ポーズの変更が無くても強制的にQmPdkCopyPoseを行うかを設定します。
            /// </summary>
            /// <param name="enable">強制的にコピーが行われるかどうか</param>
            public static void SetForcedCopyPose(bool enable)
            {
                ThrowIfError(TrySetForcedCopyPose(enable));
            }


            /// <summary>
            /// 指定したモデルにQUMAのポーズを反映させます。
            /// 反映後に結果のマトリックスを取り出して利用してください。
            /// </summary>
            /// <param name="modelHandle">コピー先のモデルのハンドル</param>
            /// <returns>NoErrorの場合は成功</returns>
            public static void CopyPose(ModelHandle modelHandle)
            {
                ThrowIfError(CopyPose(modelHandle.ConstHandle));
            }

            /// <summary>
            /// ポーズをキャリブレートし、対象モデルのカレントポーズと現在のQUMAのポーズを対応付けます。
            /// </summary>
            /// <param name="modelHandle">キャリブレート対象のモデル</param>
            /// <returns>NoErrorの場合は成功</returns>
            public static void CalibratePose(ModelHandle modelHandle)
            {
                ThrowIfError(CalibratePose(modelHandle.ConstHandle));
            }

            /// <summary>キャリブレーション情報を指定したファイルにセーブします。</summary>
            /// <param name="modelHandle">セーブしたいモデル</param>
            /// <param name="filepath">セーブ先のファイルパス</param>
            /// <returns>NoErrorの場合は成功</returns>
            public static void SaveCalibrationDataFile(ModelHandle modelHandle, string filepath)
            {
                ThrowIfError(SaveCalibrationDataFile(modelHandle.ConstHandle, filepath));
            }

            /// <summary>キャリブレーション情報を指定したファイルからロードします。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <param name="filepath">ロード先のファイルパス</param>
            /// <returns>NoErrorの場合は成功</returns>
            public static void LoadCalibrationDataFile(ModelHandle modelHandle, string filepath)
            {
                ThrowIfError(LoadCalibrationDataFile(modelHandle.ConstHandle, filepath));
            }

            /// <summary>キャリブレーション情報をメモリデータとして取得します。</summary>
            /// <param name="modelHandle">セーブしたいモデル</param>
            /// <param name="result">セーブ結果を持っているバイト配列</param>
            /// <returns>NoErrorの場合は成功</returns>
            public static byte[] SaveCalibrationDataMem(ModelHandle modelHandle)
            {
                var result = new byte[0];
                int size = 0;

                ThrowIfError(SaveCalibrationDataMem(modelHandle.ConstHandle, ref result, ref size));

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

                var res = (LoadCalibrationDataMem(modelHandle.ConstHandle, dataPtr, data.Length));

                Marshal.FreeHGlobal(dataPtr);

                ThrowIfError(res);
            }

        }

        /// <summary>キャラクターボーン関連</summary>
        public static class Character
        {
            #region privateなDllImport

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterCreateStandardModelPS@@YAHPAH00@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterCreateStandardModelPS@@YAHPEAH00@Z")]
#endif
            private static extern QmPdkErrorCode CreateStandardModelPS(IntPtr modelHandle, IntPtr numberOfNode, IntPtr parentNodeIndexArray);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterCreate@@YAHPAHH0PAPBD@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterCreate@@YAHPEAHH0PEAPEBD@Z")]
#endif
            private static extern QmPdkErrorCode Create(ref int modelhandle, int numberOfNode, IntPtr parentNodeIndexArray, IntPtr nodeNameArray);
            //NOTE: Create関数は使いどころが少なそうなのでラップは放置中


            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterDestroy@@YAHH@Z")]
            private static extern QmPdkErrorCode Destroy(int modelHandle);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetNumOfHandle@@YAHPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetNumOfHandle@@YAHPEAH@Z")]
#endif
            private static extern QmPdkErrorCode GetNumOfHandle(out int num);


#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetHandle@@YAHHPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetHandle@@YAHHPEAH@Z")]
#endif
            private static extern QmPdkErrorCode GetHandle(int index, out int modelHandle);


#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetQumaHandle@@YAHHPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetQumaHandle@@YAHHPEAH@Z")]
#endif
            private static extern QmPdkErrorCode GetQumaHandle(int modelHandle, out int qumaHandle);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterSetLocalMatrix@@YAHHHQAY03M@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterSetLocalMatrix@@YAHHHQEAY03M@Z")]
#endif
            private static extern QmPdkErrorCode SetLocalMatrix(int modelHandle, int nodeIndex, IntPtr localMatrix);


#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetLocalMatrix@@YAHHHQAY03M@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetLocalMatrix@@YAHHHQEAY03M@Z")]
#endif
            private static extern QmPdkErrorCode GetLocalMatrix(int modelHandle, int nodeIndex, IntPtr localMatrix);


#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterSetRotate@@YAHHHQAY03M@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterSetRotate@@YAHHHQEAY03M@Z")]
#endif
            private static extern QmPdkErrorCode SetRotate(int modelHandle, int nodeIndex, IntPtr rotateMatrix);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetRotate@@YAHHHQAY03M@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetRotate@@YAHHHQEAY03M@Z")]
#endif
            private static extern QmPdkErrorCode GetRotate(int modelHandle, int nodeIndex, IntPtr rotateMatrix);


#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetName@@YAHHHPADH0H@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetName@@YAHHHPEADH0H@Z")]
#endif
            private static extern QmPdkErrorCode GetName(
                int modelhandle, int nodeIndex, 
                [MarshalAs(UnmanagedType.LPStr)]StringBuilder name, int sizeName, 
                [MarshalAs(UnmanagedType.LPStr)]StringBuilder originalName, int sizeOriginalName
                );

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterMemorizeInitialPose@@YAHH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterMemorizeInitialPose@@YAHH@Z")]
#endif
            private static extern QmPdkErrorCode MemorizeInitialPose(int modelHandle);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterRecallInitialPose@@YAHH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterRecallInitialPose@@YAHH@Z")]
#endif
            private static extern QmPdkErrorCode RecallInitialPose(int modelHandle);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterSetAccelerometerMode@@YAHHH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterSetAccelerometerMode@@YAHHH@Z")]
#endif
            private static extern QmPdkErrorCode SetAccelerometerMode(int modelHandle, QmPdkAccelerometerMode mode);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetAccelerometerMode@@YAHHPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetAccelerometerMode@@YAHHPEAH@Z")]
#endif
            private static extern QmPdkErrorCode GetAccelerometerMode(int modelHandle, out QmPdkAccelerometerMode mode);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterSetRestrictAccelerometerMode@@YAHHH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterSetRestrictAccelerometerMode@@YAHHH@Z")]
#endif
            private static extern QmPdkErrorCode SetRestrictAccelerometerMode(int modelHandle, QmPdkAccelerometerRestrictMode mode);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetRestrictAccelerometerMode@@YAHHPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkCharacterGetRestrictAccelerometerMode@@YAHHPEAH@Z")]
#endif
            private static extern QmPdkErrorCode GetRestrictAccelerometerMode(int modelHandle, out QmPdkAccelerometerRestrictMode mode);

            #endregion

            /// <summary>ボーンの解放処理。<see cref="BaseOperation.Final"/>で解放できるので呼ばなくても問題はありません。</summary>
            /// <param name="modelHandle">解放したいモデル</param>
            public static void Destroy(ModelHandle modelHandle)
            {
                ThrowIfError(Destroy(modelHandle.ConstHandle));
            }

            /// <summary>標準モデルのキャラクターボーンを取得します。</summary>
            /// <returns>標準モデルのキャラクターボーン</returns>
            public static IndexedModelHandle CreateStandardModelPS()
            {
                var numberOfNodePtr = Marshal.AllocHGlobal(sizeof(int));

                var res = CreateStandardModelPS(IntPtr.Zero, numberOfNodePtr, IntPtr.Zero);

                int numberOfNode = numberOfNodePtr.ToInt32();

                if (res != QmPdkErrorCode.NoError)
                {
                    Marshal.FreeHGlobal(numberOfNodePtr);
                    throw new QmPdkException(res);
                }

                var parentNodeIndexesPtr = Marshal.AllocHGlobal(sizeof(int) * numberOfNode);
                var parentNodeIndexes = new int[numberOfNode];

                var modelHandlePtr = Marshal.AllocHGlobal(sizeof(int));

                res = CreateStandardModelPS(modelHandlePtr, numberOfNodePtr, parentNodeIndexesPtr);
                Marshal.FreeHGlobal(numberOfNodePtr);

                if (res != QmPdkErrorCode.NoError)
                {
                    Marshal.FreeHGlobal(parentNodeIndexesPtr);
                    Marshal.FreeHGlobal(modelHandlePtr);
                    throw new QmPdkException(res);
                }


                Marshal.Copy(parentNodeIndexesPtr, parentNodeIndexes, 0, parentNodeIndexes.Length);
                Marshal.FreeHGlobal(parentNodeIndexesPtr);

                int modelHandle = modelHandlePtr.ToInt32();
                Marshal.FreeHGlobal(modelHandlePtr);

                return new IndexedModelHandle(
                    new ModelHandle(new IntPtr(modelHandle)),
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
                return new ModelHandle(new IntPtr(modelHandle));
            }

            /// <summary>モデルハンドルに関連付けられているQUMAデバイスのハンドルを取得します。</summary>
            /// <param name="modelHandle">確認したいモデル</param>
            /// <returns>モデルに関連付けられたデバイスのハンドル</returns>
            public static QumaHandle GetQumaHandle(ModelHandle modelHandle)
            {
                int qumaHandle = 0;
                ThrowIfError(GetQumaHandle(modelHandle.ConstHandle, out qumaHandle));
                return new QumaHandle(new IntPtr(qumaHandle));
            }

            /// <summary>ボーンのノードにローカル行列を設定します。ボーンの長さ情報が必要なため平行移動成分も必要なことに注意してください。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <param name="nodeIndex">適用先のノードのインデックス</param>
            /// <param name="matrix">適用するローカル行列</param>
            public static void SetLocalMatrix(ModelHandle modelHandle, int nodeIndex, Matrix44f matrix)
            {
                var matrixPtr = Marshal.AllocHGlobal(Marshal.SizeOf(matrix));

                Marshal.StructureToPtr(matrix, matrixPtr, false);
                var res = SetLocalMatrix(modelHandle.ConstHandle, nodeIndex, matrixPtr);

                Marshal.FreeHGlobal(matrixPtr);

                ThrowIfError(res);
            }

            /// <summary>ボーンのノードのローカル行列を取得します。</summary>
            /// <param name="modelHandle">確認先のモデル</param>
            /// <param name="nodeIndex">確認先のノードのインデックス</param>
            /// <returns>適用されているローカル行列</returns>
            public static Matrix44f GetLocalMatrix(ModelHandle modelHandle, int nodeIndex)
            {
                Matrix44f result = Matrix44f.Create();
                var matrixPtr = Marshal.AllocHGlobal(Marshal.SizeOf(result));

                var res = GetLocalMatrix(modelHandle.ConstHandle, nodeIndex, matrixPtr);
                if(res != QmPdkErrorCode.NoError)
                {
                    Marshal.FreeHGlobal(matrixPtr);
                    throw new QmPdkException(res);
                }

                Marshal.PtrToStructure(matrixPtr, result);
                Marshal.FreeHGlobal(matrixPtr);
                return result;
            }

            /// <summary>ボーンのノードに回転行列を設定します。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <param name="nodeIndex">適用先のノードのインデックス</param>
            /// <param name="matrix">適用する回転行列</param>
            public static void SetRotate(ModelHandle modelHandle, int nodeIndex, Matrix44f matrix)
            {
                var matrixPtr = Marshal.AllocHGlobal(Marshal.SizeOf(matrix));

                Marshal.StructureToPtr(matrix, matrixPtr, false);
                var res = SetRotate(modelHandle.ConstHandle, nodeIndex, matrixPtr);

                Marshal.FreeHGlobal(matrixPtr);

                ThrowIfError(res);
            }

            /// <summary>ボーンのノードの回転行列を取得します。</summary>
            /// <param name="modelHandle">確認先のモデル</param>
            /// <param name="nodeIndex">確認先のノードのインデックス</param>
            /// <param name="matrix">現在の回転行列</param>
            public static Matrix44f GetRotate(ModelHandle modelHandle, int nodeIndex)
            {
                Matrix44f result = Matrix44f.Create();
                var matrixPtr = Marshal.AllocHGlobal(Marshal.SizeOf(result));

                var res = GetRotate(modelHandle.ConstHandle, nodeIndex, matrixPtr);
                if (res != QmPdkErrorCode.NoError)
                {
                    Marshal.FreeHGlobal(matrixPtr);
                    throw new QmPdkException(res);
                }

                Marshal.PtrToStructure(matrixPtr, result);
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
                    modelHandle.ConstHandle, nodeIndex,
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
                ThrowIfError(MemorizeInitialPose(modelHandle.ConstHandle));
            }

            /// <summary>キャラのTポーズを呼び出す。</summary>
            /// <param name="modelHandle">呼び出し処理を行うモデル</param>
            public static void RecallInitialPose(ModelHandle modelHandle)
            {
                ThrowIfError(RecallInitialPose(modelHandle.ConstHandle));
            }

            /// <summary>ボーンの加速度センサーモードを設定します。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <param name="mode">モード</param>
            public static void SetAccelerometerMode(ModelHandle modelHandle, QmPdkAccelerometerMode mode)
            {
                ThrowIfError(SetAccelerometerMode(modelHandle.ConstHandle, mode));
            }

            /// <summary>ボーンの加速度センサーモードを取得します。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <returns>現在のモード</returns>
            public static QmPdkAccelerometerMode GetAccelerometerMode(ModelHandle modelHandle)
            {
                QmPdkAccelerometerMode mode;
                ThrowIfError(GetAccelerometerMode(modelHandle.ConstHandle, out mode));
                return mode;
            }

            /// <summary>ボーンの加速度センサー回転制限モードを設定します。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <param name="mode">指定するモード</param>
            public static void SetRestrictAccelerometerMode(ModelHandle modelHandle, QmPdkAccelerometerRestrictMode mode)
            {
                ThrowIfError(SetRestrictAccelerometerMode(modelHandle.ConstHandle, mode));
            }

            /// <summary>ボーンの加速度センサー回転制限モードを取得します。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <returns>現在のモード</returns>
            public static QmPdkAccelerometerRestrictMode GetRestrictAccelerometerMode(ModelHandle modelHandle)
            {
                QmPdkAccelerometerRestrictMode mode;
                ThrowIfError(GetRestrictAccelerometerMode(modelHandle.ConstHandle, out mode));
                return mode;
            }

        }

        /// <summary>QUMARIONデバイス関連</summary>
        public static class Quma
        {
            #region privateなDllImport

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetNumOfHandle@@YAHPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetNumOfHandle@@YAHPEAH@Z")]
#endif
            private static extern QmPdkErrorCode GetNumOfHandle(out int num);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetHandle@@YAHHPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetHandle@@YAHHPEAH@Z")]
#endif
            private static extern QmPdkErrorCode GetHandle(int index, out int qumaHandle);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetNumOfAttachedModel@@YAHHPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetNumOfAttachedModel@@YAHHPEAH@Z")]
#endif
            private static extern QmPdkErrorCode GetNumOfAttachedModel(int qumaHandle, out int num);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkEnable@@YAHHH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkEnable@@YAHHH@Z")]
#endif
            private static extern QmPdkErrorCode Enable(int qumaHandle, int isEnable);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkIsEnable@@YAHHPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkIsEnable@@YAHHPEAH@Z")]
#endif
            private static extern QmPdkErrorCode IsEnable(int qumaHandle, out int isEnable);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkEnableAccelerometer@@YAHHH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkEnableAccelerometer@@YAHHH@Z")]
#endif
            private static extern QmPdkErrorCode EnableAccelerometer(int qumaHandle, int isEnable);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetButtonState@@YAHHHPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetButtonState@@YAHHHPEAH@Z")]
#endif
            private static extern QmPdkErrorCode GetButtonState(int qumaHandle, QmPdkQumaButton buttonType, out QmPdkQumaButtonState state);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaAttachInitPoseModel@@YAHHH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaAttachInitPoseModel@@YAHHH@Z")]
#endif
            private static extern QmPdkErrorCode AttachInitPoseModel(int qumaHandle, int modelHandle);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaDetachModel@@YAHH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaDetachModel@@YAHH@Z")]
#endif
            private static extern QmPdkErrorCode DetachModel(int modelHandle);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaIsPoseChanged@@YAHHPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaIsPoseChanged@@YAHHPEAH@Z")]
#endif
            private static extern QmPdkErrorCode IsPoseChanged(int qumaHandle, out int isPoseChanged);


#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetDeviceName@@YAHHPADPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetDeviceName@@YAHHPEADPEAH@Z")]
#endif
            private static extern QmPdkErrorCode GetDeviceName(int qumaHandle, [MarshalAs(UnmanagedType.LPStr)]StringBuilder name, out int size);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetDeviceState@@YAHH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaGetDeviceState@@YAHH@Z")]
#endif
            private static extern QmPdkErrorCode GetDeviceState(int qumaHandle);


#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaCheckDeviceSensors@@YAHHPAHPAD0@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaCheckDeviceSensors@@YAHHPEAHPEAD0@Z")]
#endif
            private static extern QmPdkErrorCode CheckDeviceSensors(
                int qumaHandle,
                out int isOk,
                [MarshalAs(UnmanagedType.LPStr)]StringBuilder detailMessage,
                ref int detailMessageSize
                );

            /// <summary>デバッグ関数とだけ記載あり(詳細ないので触らないのが吉？)</summary>
#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaDebugSetScriptForQumax@@YAHHPBD@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkQumaDebugSetScriptForQumax@@YAHHPEBD@Z")]
#endif
            private static extern QmPdkErrorCode NotImplementedFunction39();


            /// <summary>デバッグ関数とだけ記載あり(詳細ないので触らないのが吉？)</summary>
#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkDebugFlags@@YAHPBD@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkDebugFlags@@YAHPEBD@Z")]
#endif
            private static extern QmPdkErrorCode DebugFlags([MarshalAs(UnmanagedType.LPStr)]string flags);

            #endregion

            #region publicなラップ済み関数

            /// <summary>接続されているQUMAデバイスの総数を取得します。</summary>
            /// <returns>接続されているQUMAデバイスの総数</returns>
            public static int GetNumOfHandle()
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
                return new QumaHandle(new IntPtr(qumaHandle));
            }

            /// <summary>QUMAデバイスに関連付けられたボーンの数を取得します。</summary>
            /// <param name="qumaHandle">確認先のQUMAデバイス</param>
            /// <returns>デバイスに関連付けられたボーンの数</returns>
            public static int GetNumOfAttachedModel(QumaHandle qumaHandle)
            {
                int num;
                ThrowIfError(GetNumOfAttachedModel(qumaHandle.ConstHandle, out num));
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
            {
                ThrowIfError(Enable(qumaHandle.ConstHandle, Convert.ToInt32(enable)));
            }

            /// <summary>QUMAデバイスが現在有効化されているかどうかを取得します。</summary>
            /// <param name="qumaHandle">確認先のデバイス</param>
            /// <returns>デバイスが有効化されているかどうか</returns>
            public static bool GetEnableQuma(QumaHandle qumaHandle)
            {
                int isEnable;
                ThrowIfError(IsEnable(qumaHandle.ConstHandle, out isEnable));
                return Convert.ToBoolean(isEnable);
            }

            /// <summary>QUMAデバイスの加速度センサの有効/無効を設定します。</summary>
            /// <param name="qumaHandle">設定対象のデバイス</param>
            /// <param name="enable">有効化する場合は<see cref="true"/>、無効化する場合は<see cref="false"/></param>
            public static void SetEnableAccelerometer(QumaHandle qumaHandle, bool enable)
            {
                ThrowIfError(EnableAccelerometer(qumaHandle.ConstHandle, Convert.ToInt32(enable)));
            }

            /// <summary>デバイスのボタンの状態を取得します。
            /// <param name="qumaHandle"></param>
            /// <returns></returns>
            public static QmPdkQumaButtonState GetButtonState(QumaHandle qumaHandle)
            {
                QmPdkQumaButtonState state;
                ThrowIfError(GetButtonState(qumaHandle.ConstHandle, QmPdkQumaButton.MainButton, out state));
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
            {
                ThrowIfError(AttachInitPoseModel(qumaHandle.ConstHandle, modelHandle.ConstHandle));
            }

            /// <summary>モデルをデバイスから切断します。</summary>
            /// <param name="modelHandle">切断したいモデル</param>
            public static void DetachModel(ModelHandle modelHandle)
            {
                ThrowIfError(DetachModel(modelHandle.ConstHandle));
            }

            /// <summary>デバイスのポーズが前回から変更されたかを取得します。</summary>
            /// <param name="qumaHandle">確認先のデバイス</param>
            /// <returns>ポーズが変更されているかどうか</returns>
            public static bool CheckIsPoseChanged(QumaHandle qumaHandle)
            {
                int isPoseChanged;
                ThrowIfError(IsPoseChanged(qumaHandle.ConstHandle, out isPoseChanged));
                return Convert.ToBoolean(isPoseChanged);
            }

            /// <summary>QUMAデバイスの物理デバイス固有名を取得します。</summary>
            /// <param name="qumaHandle"></param>
            /// <returns></returns>
            public static string GetDeviceName(QumaHandle qumaHandle)
            {
                var name = new StringBuilder(DeviceNameBufferLength);
                int placeHolder;
                ThrowIfError(GetDeviceName(qumaHandle.ConstHandle, name, out placeHolder));

                return name.ToString();
            }

            /// <summary>
            /// <para>QUMAデバイスが有効かどうかを確認し、USBが挿抜されるなどで無効化されている場合例外をスローします。</para> 
            /// <para>
            /// <see cref="QmPdkErrorCode.DeviceUpdateBuffer"/>を持つ<see cref="QmPdkException"/>
            /// がスローされた場合、デバイスの再初期化が必要です。</para>
            /// </summary>
            /// <param name="qumaHandle">確認先のデバイス</param>
            public static void CheckDeviceValidity(QumaHandle qumaHandle)
            {
                ThrowIfError(GetDeviceState(qumaHandle.ConstHandle));
            }

            /// <summary>デバイスのセンサ状態を確認します。</summary>
            /// <param name="qumaHandle">確認したいデバイス</param>
            /// <returns>確認結果</returns>
            public static SensorsState CheckDeviceSensors(QumaHandle qumaHandle)
            {
                int isOk;
                var message = new StringBuilder(SensorsCheckMessageBufferLength);
                int messageSize = SensorsCheckMessageBufferLength;
                ThrowIfError(CheckDeviceSensors(qumaHandle.ConstHandle, out isOk, message, ref messageSize));

                return new SensorsState(Convert.ToBoolean(isOk), message.ToString());
            }

            /// <summary>
            /// <para>デバッグメッセージを有効化します。(※無効化方法は不明)</para>
            /// <para>本関数はSDKサンプルである"QmPdkSampleStandard.cpp"の使用例を基に実装しています</para>
            /// </summary>
            public static void SetDebugFlags()
            {
                string flags = new string(new char[] { (char)(0x01 | 0x02) });
                ThrowIfError(DebugFlags(flags));
            }

            #endregion
        }

        /// <summary>Template(PS標準)ボーン関連</summary>
        public static class TemplateBone
        {

            #region privateなDllImport
#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetName@@YAHHHPADH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetName@@YAHHHPEADH@Z")]
#endif
            private static extern QmPdkErrorCode GetName(int modelHandle, int nodeIndex, [MarshalAs(UnmanagedType.LPStr)]StringBuilder name, int sizeName);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetIdx@@YAHHPADPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetIdx@@YAHHPEADPEAH@Z")]
#endif
            private static extern QmPdkErrorCode GetIdx(int modelHandle, [MarshalAs(UnmanagedType.LPStr)]string name, out int index);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetRootNodeIdx@@YAHHPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetRootNodeIdx@@YAHHPEAH@Z")]
#endif
            private static extern QmPdkErrorCode GetRootNodeIdx(int modelHandle, out int rootIndex);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetParentNodeIdx@@YAHHHPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetParentNodeIdx@@YAHHHPEAH@Z")]
#endif
            private static extern QmPdkErrorCode GetParentNodeIdx(int modelHandle, int nodeIndex, out int parentIndex);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetChildNodeIdx@@YAHHHPAH0@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkTemplateGetChildNodeIdx@@YAHHHPEAH0@Z")]
#endif
            private static extern QmPdkErrorCode GetChildNodeIdx(int modelHandle, int nodeIndex, IntPtr childIndex, ref int numberOfChildNode);
        
            #endregion

            #region publicなラップ済み関数

            /// <summary>標準ボーンのノードインデックスから対応する名前を取得します。</summary>
            /// <param name="modelHandle">確認先のモデル</param>
            /// <param name="nodeIndex">確認先のインデックス</param>
            /// <returns>ボーンの名前。NiNinBaoriで識別子として用いられる</returns>
            public static string GetName(ModelHandle modelHandle, int nodeIndex)
            {
                var name = new StringBuilder(NodeNameBufferLength);
                ThrowIfError(GetName(modelHandle.ConstHandle, nodeIndex, name, NodeNameBufferLength));
                return name.ToString();
            }

            /// <summary>標準ボーンのノード名から対応するインデックスを取得します。</summary>
            /// <param name="modelHandle">確認先のモデル</param>
            /// <param name="name">インデックスを確認したいボーンの名前</param>
            /// <returns>対応するインデックス</returns>
            public static int GetIndex(ModelHandle modelHandle, string name)
            {
                int nodeIndex;
                ThrowIfError(GetIdx(modelHandle.ConstHandle, name, out nodeIndex));
                return nodeIndex;
            }

            /// <summary>標準ボーンのルートボーンのインデックスを取得します。</summary>
            /// <param name="modelHandle">確認先のモデル</param>
            /// <returns>ルートボーンのインデックス</returns>
            public static int GetRootNodeIndex(ModelHandle modelHandle)
            {
                int rootIndex;
                ThrowIfError(GetRootNodeIdx(modelHandle.ConstHandle, out rootIndex));
                return rootIndex;
            }

            /// <summary>標準ボーンのノードインデックスを指定し、親要素のノードのインデックスを取得します。</summary>
            /// <param name="modelHandle">確認先のモデル</param>
            /// <param name="nodeIndex"></param>
            /// <returns></returns>
            public static int GetParentNodeIndex(ModelHandle modelHandle, int nodeIndex)
            {
                int parentIndex;
                ThrowIfError(GetParentNodeIdx(modelHandle.ConstHandle, nodeIndex, out parentIndex));
                return parentIndex;
            }

            /// <summary>標準ボーンのノードインデックスを指定し、子ボーンのインデックス一覧を取得します。</summary>
            /// <param name="modelHandle">確認先のモデル</param>
            /// <param name="nodeIndex">確認したいノード</param>
            /// <returns>指定したノードの子ボーンのインデックス一覧</returns>
            public static int[] GetChildNodeIndex(ModelHandle modelHandle, int nodeIndex)
            {
                int numberOfChildNode = 0;
                ThrowIfError(GetChildNodeIdx(modelHandle.ConstHandle, nodeIndex, IntPtr.Zero, ref numberOfChildNode));

                IntPtr indexes = Marshal.AllocHGlobal(sizeof(int) * numberOfChildNode);
                var result = new int[numberOfChildNode];

                ThrowIfError(GetChildNodeIdx(modelHandle.ConstHandle, nodeIndex, indexes, ref numberOfChildNode));

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

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbIsValid@@YAHHPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbIsValid@@YAHHPEAH@Z")]
#endif
            private static extern QmPdkErrorCode IsValid(int modelHandle, out int isValid);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbLoadFromFile@@YAHHPBD0@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbLoadFromFile@@YAHHPEBD0@Z")]
#endif
            private static extern QmPdkErrorCode LoadFromFile(int modelHandle, [MarshalAs(UnmanagedType.LPStr)]string nnbFilePath, [MarshalAs(UnmanagedType.LPWStr)]string templateFilePath);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbSaveToFile@@YAHHHPBD@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbSaveToFile@@YAHHHPEBD@Z")]
#endif
            private static extern QmPdkErrorCode SaveToFile(int modelHandle, int withCalibrationData, [MarshalAs(UnmanagedType.LPWStr)]string filepath);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbLoadFromMem@@YAHHPBDH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbLoadFromMem@@YAHHPEBDH@Z")]
#endif
            private static extern QmPdkErrorCode LoadFromMem(int modelHandle, IntPtr data, int dataSize);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbSaveToMem@@YAHHHPADPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbSaveToMem@@YAHHHPEADPEAH@Z")]
#endif
            private static extern QmPdkErrorCode SaveToMem(int modelHandle, int withCalibrationData, IntPtr outData, ref int outDataSize);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbCreateRootGroup@@YAHHHHPAH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbCreateRootGroup@@YAHHHHPEAH@Z")]
#endif
            private static extern QmPdkErrorCode CreateRootGroup(int modelHandle, int charRootNodeIndex, int charNextNodeIndex, out int groupIdx);

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbAddGroup@@YAHHHHPAHH00@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbAddGroup@@YAHHHHPEAHH00@Z")]
#endif
            private static extern QmPdkErrorCode AddGroup(
                int modelHandle, 
                int parentGroupIndex,
                int numberOfCharNodeIndexes,
                IntPtr charNodeIndexes,
                int numberOfTemplateNodeIndexes,
                IntPtr templateNodeIndexes,
                out int groupIndex
                );

#if !T_X64
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbApply@@YAHH@Z")]
#else
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmPdkNnbApply@@YAHH@Z")]
#endif
            private static extern QmPdkErrorCode Apply(int modelHandle);

            #endregion

            #region publicなラップ済み関数

            /// <summary>NNBの情報を持っているかどうかを取得します。</summary>
            /// <param name="modelHandle">確認先のモデル</param>
            /// <returns>NNBの情報を持っているかどうか</returns>
            public static bool CheckIsValid(ModelHandle modelHandle)
            {
                int isValid;
                ThrowIfError(IsValid(modelHandle.ConstHandle, out isValid));
                return Convert.ToBoolean(isValid);
            }

            /// <summary>ファイル(.nnb)へのパスを指定してNNB情報をロードします。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <param name="nnbFilePath">ロード元のファイルパス</param>
            public static void LoadFromFile(ModelHandle modelHandle, string nnbFilePath)
            {
                ThrowIfError(LoadFromFile(modelHandle.ConstHandle, nnbFilePath, ""));
            }

            /// <summary>NNB情報をファイルに保存します。</summary>
            /// <param name="modelHandle">保存したいモデル</param>
            /// <param name="filepath">保存先のファイル名</param>
            /// <param name="withCalibrationData">キャリブレーション情報を保存先ファイルに含めるかどうか</param>
            public static void SaveToFile(ModelHandle modelHandle, string filepath, bool withCalibrationData)
            {
                ThrowIfError(SaveToFile(modelHandle.ConstHandle, Convert.ToInt32(withCalibrationData), filepath));
            }

            /// <summary>メモリデータ上のNNB情報をモデルに適用します。</summary>
            /// <param name="modelHandle">適用先のモデル</param>
            /// <param name="data">適用するNNBデータ</param>
            public static void LoadFromMem(ModelHandle modelHandle, byte[] data)
            {
                var dataPtr = Marshal.AllocHGlobal(data.Length);
                Marshal.Copy(data, 0, dataPtr, data.Length);

                LoadFromMem(modelHandle.ConstHandle, dataPtr, data.Length);

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
                ThrowIfError(SaveToMem(modelHandle.ConstHandle, Convert.ToInt32(withCalibrationData), IntPtr.Zero, ref outDataSize));

                IntPtr dataPtr = Marshal.AllocHGlobal(outDataSize);
                var res = SaveToMem(modelHandle.ConstHandle, Convert.ToInt32(withCalibrationData), dataPtr, ref outDataSize);

                if (res != QmPdkErrorCode.NoError)
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
                ThrowIfError(CreateRootGroup(modelHandle.ConstHandle, charRootNodeIndex, charNextNodeIndex, out groupIndex));
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
                    modelHandle.ConstHandle,
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
                ThrowIfError(Apply(modelHandle.ConstHandle));
            }

            #endregion
        }

    }


    /// <summary>QmPdk APIのエラーコード一覧です。</summary>
    public enum QmPdkErrorCode
    {
        /// <summary>エラーなし</summary>
        NoError = 0,
        /// <summary>内部エラー</summary>
        Error = 1,
        /// <summary>メモリ確保エラー</summary>
        MemoryAlloc = 2, 
        /// <summary>引数不正(NULLなど)</summary>
        Parameter = 3,
        /// <summary>QUMAが正しく初期化されていない</summary>
        NoQuma = 10,
        /// <summary>QUMAのデバイスハンドルが不正</summary>
        InvalidDevice = 11,
        /// <summary>QUMAからデータが取得できない(USBが抜けた、など)</summary>
        DeviceUpdateBuffer = 12,
        /// <summary>デバイスを別プロセスで使用中</summary>
        DeviceOtherProcess = 13,
        /// <summary>モデルハンドルが不正</summary>
        InvalidModel = 20,
        /// <summary>templateモデルが不正</summary>
        InvalidModelTemplate = 21,
        /// <summary>デバイスにアタッチされていない</summary>
        NotAttachedToDevice = 22,
        /// <summary>templateモデルのノードが不正</summary>
        InvalidNodeTemplate = 26, 
        /// <summary>NNB情報が不正であるか、存在しない</summary>
        InvalidNNB = 30,
        /// <summary>マッピングダイアログが既に開かれている</summary>
        MappingDlgAlreadyOpen = 35,
        /// <summary>QUMA情報セーブエラー</summary>
        SaveQuma = 50,
        /// <summary>NNB情報セーブエラー</summary>
        SaveNNB = 51,
        /// <summary>ライセンスエラー</summary>
        License = 60,
        /// <summary>ライセンス有効期限切れ</summary>
        LicenseExpired = 61,
        /// <summary>ライセンスエラー無効なデバイス</summary>
        LicenseInvalidDevice = 62
    }

    /// <summary>デバイスのボタン一覧です。一つしかボタンはありません。</summary>
    public enum QmPdkQumaButton
    {
        /// <summary>QUMARIONの背面ボタン</summary>
        MainButton = 0
    }

    /// <summary>ボタンの状態を表します。</summary>
    public enum QmPdkQumaButtonState
    {
        /// <summary>ボタンが押されていない状態</summary>
        Up = 0,
        /// <summary>ボタンが押されている状態</summary>
        Down = 1
    }

    /// <summary>加速度センサを基にしたルートボーンマトリックス計算の方法一覧です。</summary>
    public enum QmPdkAccelerometerMode
    {
        /// <summary>事前のフィルタリング処理を行いません。</summary>
        Direct = 1,
        /// <summary>フィルタ処理を行います。</summary>
        Relative = 2
    }

    /// <summary>加速度センサ回転制限を設定します。</summary>
    public enum QmPdkAccelerometerRestrictMode
    {
        /// <summary>制限なし</summary>
        None = 0,
        /// <summary>X軸回転に制限</summary>
        AxisX = 1,
        /// <summary>Z軸回転に制限</summary>
        AxisZ = 2
    }

    /// <summary>ハンドル処理エラーに関する定数を定義します。</summary>
    public class QmPdkHandleErrorConstants
    {
        /// <summary>不正なQUMAデバイスハンドル(未設定であることを表す)</summary>
        public const int QumaHandleError = -1;
        /// <summary>不正なモデルハンドル(未設定であることを表す)</summary>
        public const int ModelHandleError = -1;
    }

    /// <summary>モデルのハンドルを表します。</summary>
    public class ModelHandle
    {
        internal ModelHandle(IntPtr handle)
        {
            Handle = handle;
        }
        public IntPtr Handle { get; }
        public int ConstHandle
        {
            get
            {
                return Handle.ToInt32();
            }
        }
    }

    /// <summary>QUMAデバイスのハンドルを表します。</summary>
    public class QumaHandle
    {
        internal QumaHandle(IntPtr handle)
        {
            Handle = handle;
        }
        public IntPtr Handle { get; }
        public int ConstHandle
        {
            get
            {
                return Handle.ToInt32();
            }
        }
    }

    public class QmPdkException : Exception
    {
        internal QmPdkException(QmPdkErrorCode errorCode)
        {
            ErrorCode = errorCode;
        }

        /// <summary>エラーの内容を取得します。</summary>
        public QmPdkErrorCode ErrorCode { get; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix44f
    {
        public static Matrix44f Create()
        {
            return new Matrix44f()
            {
                Values = new float[16]
            };
        }

        [MarshalAs(UnmanagedType.R4, SizeConst = 16)]
        public float[] Values;
    }

    public class IndexedModelHandle
    {
        internal IndexedModelHandle(ModelHandle modelHandle, int[] indexes)
        {
            ModelHandle = modelHandle;
            Indexes = indexes;
        }
    
        public int[] Indexes { get; }
        public ModelHandle ModelHandle { get; }
    }

    public class NodeNames
    {
        internal NodeNames(string name, string originalName)
        {
            Name = name;
            OriginalName = originalName;
        }

        /// <summary>ボーンの名前。NiNinBaori APIで内部的に使用される</summary>
        public string Name { get; }
        /// <summary>キャラ作成時にユーザが指定した名前。内部では利用されない</summary>
        public string OriginalName { get; }
    }

    public class SensorsState
    {
        internal SensorsState(bool isOk, string message)
        {
            IsOk = isOk;
            Message = message;
        }

        public bool IsOk { get; }

        public string Message { get; }
    }
}
