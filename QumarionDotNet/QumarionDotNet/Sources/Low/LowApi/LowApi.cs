using System;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace Baku.Quma.Low.Api
{
    /// <summary>
    /// <para>ネイティブAPIで"QmLow"から始まっているAPI関数のラッパをまとめたクラスです。</para>
    /// <para>全ての関数は接頭辞"QmLow"を省略したそれっぽい名前でラップされ、入れ子クラスのいずれかに配置されています。</para>
    /// <para>このクラスは拡張性のためアセンブリ外部にも公開されていますが、基本的にはユーザが直接使うことは想定されていません。</para>
    /// </summary>
    public static class QmLow
    {
        private const string DllName86 = DllImportSetting.DllName86;
        private const string DllName64 = DllImportSetting.DllName64;
        private static readonly bool Is64bit = DllImportSetting.Is64bit;

        /// <summary>APIで使われている定数</summary>
        public static class ApiConstants
        {
            /// <summary>APIが接続できるデバイス数の上限です。</summary>
            public const int DEVICE_ID_LENGTH = 12;
            /// <summary>APIバージョンを表す文字列のバッファ長です。</summary>
            public const int API_VERSION_STR_MAX_LENGTH = 64;
            /// <summary>デバイスIDの文字列バッファ長です。</summary>
            public const int ID_BUFFER_SIZE = 256;
            /// <summary>デバイス名の文字列バッファ長です。</summary>
            public const int QUMAHANDLE_NAME_SIZE = 128;
            /// <summary>ボーン名の文字列バッファ長です。</summary>
            public const int QUMABONE_NAME_NAX_SIZE = 128;

            /// <summary>タイムアウトを無効化する数値の一つです。</summary>
            public static readonly uint TIMEOUT_DISABLE_0 = 0;
            /// <summary>タイムアウトを無効化する数値の一つです。</summary>
            public static readonly uint TIMEOUT_DISABLE_1 = 0x7fffffff;
            /// <summary>タイムアウトを無効化する数値の一つです。</summary>
            public static readonly uint TIMEOUT_DISABLE_2 = 0xffffffff;

        }

        /// <summary>初期化、設定、終了のAPI</summary>
        public static class BaseOperation
        {
            #region privateなDllImport

            #region _Initialize
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowInitialize@@YAHXZ")]
            private static extern QumaLowResponse _Initialize86();
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowInitialize@@YAHXZ")]
            private static extern QumaLowResponse _Initialize64();
            private static QumaLowResponse _Initialize() => Is64bit ? _Initialize64() : _Initialize86();
            #endregion

            #region GetVersion
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetVersion@@YAXPA_W@Z")]
            private static extern void GetVersion86([MarshalAs(UnmanagedType.LPWStr, SizeConst = ApiConstants.API_VERSION_STR_MAX_LENGTH)]StringBuilder res);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetVersion@@YAXPEA_W@Z")]
            private static extern void GetVersion64([MarshalAs(UnmanagedType.LPWStr, SizeConst = ApiConstants.API_VERSION_STR_MAX_LENGTH)]StringBuilder res);
            private static void GetVersion(StringBuilder res)
            {
                if (Is64bit)
                {
                    GetVersion64(res);
                } else
                {
                    GetVersion86(res);
                }
            }
            #endregion

            #region GetDeviceID
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetDeviceID@@YAXPAXPAE@Z")]
            private static extern void GetDeviceID86(IntPtr qumaHandle, IntPtr result);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetDeviceID@@YAXPEAXPEAE@Z")]
            private static extern void GetDeviceID64(IntPtr qumaHandle, IntPtr result);
            private static void GetDeviceID(IntPtr qumaHandle, IntPtr result)
            {
                if(Is64bit)
                {
                    GetDeviceID64(qumaHandle, result);
                }
                else
                {
                    GetDeviceID86(qumaHandle, result);
                }
            }
            #endregion

            #region GetDeviceName
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetDeviceName@@YAXPAXPA_W@Z")]
            private static extern void GetDeviceName86(IntPtr qumaHandle, [MarshalAs(UnmanagedType.LPWStr, SizeConst = ApiConstants.QUMAHANDLE_NAME_SIZE)]StringBuilder result);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetDeviceName@@YAXPEAXPEA_W@Z")]
            private static extern void GetDeviceName64(IntPtr qumaHandle, [MarshalAs(UnmanagedType.LPWStr, SizeConst = ApiConstants.QUMAHANDLE_NAME_SIZE)]StringBuilder result);
            private static void GetDeviceName(IntPtr qumaHandle, StringBuilder result)
            {
                if (Is64bit)
                {
                    GetDeviceName64(qumaHandle, result);
                }
                else
                {
                    GetDeviceName86(qumaHandle, result);
                }
            }
            #endregion

            #region SetTimeout
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowSetTimeout@@YAHPAXI@Z")]
            private static extern QumaLowResponse SetTimeout86(IntPtr qumaHandle, uint timeout);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowSetTimeout@@YAHPEAXI@Z")]
            private static extern QumaLowResponse SetTimeout64(IntPtr qumaHandle, uint timeout);
            private static QumaLowResponse SetTimeout(IntPtr qumaHandle, uint timeout)
                => Is64bit ? SetTimeout64(qumaHandle, timeout) : SetTimeout86(qumaHandle, timeout);
            #endregion

            #region SetDebugWrite
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowSetDebugWrite@@YAX_N@Z")]
            private static extern void SetDebugWrite86(bool isEnable);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowSetDebugWrite@@YAX_N@Z")]
            private static extern void SetDebugWrite64(bool isEnable);
            #endregion

            #region SetCoordinateSystem
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowSetCoordinateSystem@@YAXW4QMLOW_COORDINATE_SYSTEM@@@Z")]
            private static extern void SetCoordinateSystem86(CoordinateSystem coordinateSystem);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowSetCoordinateSystem@@YAXW4QMLOW_COORDINATE_SYSTEM@@@Z")]
            private static extern void SetCoordinateSystem64(CoordinateSystem coordinateSystem);
            #endregion

            #region SetRotateDirection
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowSetRotateDirection@@YAXW4QMLOW_ROTATE_DIRECTION@@@Z")]
            private static extern void SetRotateDirection86(RotateDirection direction);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowSetRotateDirection@@YAXW4QMLOW_ROTATE_DIRECTION@@@Z")]
            private static extern void SetRotateDirection64(RotateDirection direction);
            #endregion

            #region Exit
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowExit@@YAHXZ")]
            private static extern QumaLowResponse Exit86();
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowExit@@YAHXZ")]
            private static extern QumaLowResponse Exit64();
            #endregion


            #endregion

            #region publicなラップ済関数

            private static object _initializedLock = new object();
            private static bool _initialized = false;
            /// <summary><see cref="Initialize"/>関数が一度以上呼ばれているかどうかを取得します。このプロパティはスレッドセーフです。</summary>
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

            /// <summary>
            /// Quma APIライブラリを初期化します。ライブラリの使用前に必ず呼び出してください。
            /// この関数は本来のAPIをプロセス中で1度しか呼ばないよう状態管理を行います。
            /// </summary>
            /// <returns>起動に成功した場合と2回目以降の呼び出しでは<see cref="QumaLowResponse.OK"/></returns>
            public static QumaLowResponse Initialize()
            {
                if (!Initialized)
                {
                    Initialized = true;
                    return _Initialize();
                }
                else
                {
                    return QumaLowResponse.OK;
                }
            }

            /// <summary>APIを終了します。ライブラリを使い終わったら呼び出してください。</summary>
            /// <returns>終了に成功するとOKを返します。</returns>
            public static QumaLowResponse Exit() => Is64bit ? Exit64() : Exit86();

            /// <summary>
            /// Quma Driverのバージョン情報を取得します。
            /// </summary>
            /// <returns>バージョン情報</returns>
            public static string GetVersion()
            {
                var result = new StringBuilder(ApiConstants.API_VERSION_STR_MAX_LENGTH);
                GetVersion(result);
                return result.ToString();
            }

            /// <summary>QumaのデバイスIDと呼ばれるバイト列を取得します.</summary>
            /// <param name="qumaHandle">デバイスIDを取得するQumaのハンドル</param>
            /// <returns>デバイスID</returns>
            public static byte[] GetDeviceID(QumaHandle qumaHandle)
            {
                var result = new byte[ApiConstants.DEVICE_ID_LENGTH];

                var resultPtr = Marshal.AllocHGlobal(sizeof(byte) * ApiConstants.DEVICE_ID_LENGTH);
                GetDeviceID(qumaHandle.Handle, resultPtr);

                Marshal.Copy(resultPtr, result, 0, ApiConstants.DEVICE_ID_LENGTH);

                Marshal.FreeHGlobal(resultPtr);
                return result;
            }

            /// <summary>Qumaのデバイス名を取得します.</summary>
            /// <param name="qumaHandle">デバイス名を取得するQumaのハンドル</param>
            /// <returns>デバイス名</returns>
            public static string GetDeviceName(QumaHandle qumaHandle)
            {
                var result = new StringBuilder(ApiConstants.QUMAHANDLE_NAME_SIZE);
                GetDeviceName(qumaHandle.Handle, result);
                return result.ToString();
            }

            /// <summary>
            /// デバッグ出力のオン/オフを設定します。
            /// </summary>
            /// <param name="isEnable">デバッグ出力を行う場合true</param>
            public static void SetDebugWrite(bool isEnable)
            {
                if (Is64bit)
                {
                    SetDebugWrite64(isEnable);
                }
                else
                {
                    SetDebugWrite86(isEnable);
                }
            }

            /// <summary>ライブラリ内で使う座標系を設定します。</summary>
            /// <param name="coordinateSystem">座標系。デフォルトでは左手系が使用されます。</param>
            public static void SetCoordinateSystem(CoordinateSystem coordinateSystem)
            {
                if (Is64bit)
                {
                    SetCoordinateSystem64(coordinateSystem);
                }
                else
                {
                    SetCoordinateSystem86(coordinateSystem);
                }
            }

            /// <summary>回転正方向を設定します。</summary>
            /// <param name="direction">回転方向。デフォルトでは右回転が正です。</param>
            public static void SetRotateDirection(RotateDirection direction)
            {
                if (Is64bit)
                {
                    SetRotateDirection64(direction);
                }
                else
                {
                    SetRotateDirection86(direction);
                }
            }

            /// <summary>
            /// <para>ミリ秒単位でタイムアウトを設定します。デフォルトではタイムアウトは無効化されています。</para>
            /// <para>API仕様上引数に0, 0x7fffffff, 0xffffffffのいずれかを指定すると例外が発生します。</para>
            /// <para>タイムアウトを無効化する場合<see cref="DisableTimeout(QumaHandle)"/>を用いてください。</para> 
            /// </summary>
            /// <param name="qumaHandle">Qumaデバイスのハンドル</param>
            /// <param name="timeout">指定する秒数(ミリ秒単位)</param>
            /// <returns>設定に成功: OK</returns>
            /// <exception cref="ArgumentException" />
            /// <exception cref="QmLowException" />
            public static void SetTimeout(QumaHandle qumaHandle, uint timeout)
            {
                if(timeout == ApiConstants.TIMEOUT_DISABLE_0 ||
                   timeout == ApiConstants.TIMEOUT_DISABLE_1 ||
                   timeout == ApiConstants.TIMEOUT_DISABLE_2)
                {
                    throw new ArgumentException("given timeout value is invalid magic number: use DisableTimeout method instead");
                }

                var res = SetTimeout(qumaHandle.Handle, timeout);
                if (res != QumaLowResponse.OK)
                {
                    throw new QmLowException(res);
                }
            }

            /// <summary>タイムアウトを無効化します。</summary>
            /// <param name="qumaHandle">Qumaデバイスのハンドル</param>
            /// <returns>設定に成功: OK</returns>
            /// <exception cref="QmLowException">関数呼び出しに失敗した場合に発生します。</exception>
            public static void DisableTimeout(QumaHandle qumaHandle)
            {
                var res = SetTimeout(qumaHandle.Handle, ApiConstants.TIMEOUT_DISABLE_0);
                if (res != QumaLowResponse.OK)
                {
                    throw new QmLowException(res);
                }
            }

            #endregion
        }

        /// <summary>デバイスの列挙、取得、破棄API</summary>
        public static class Device
        {
            #region privateなDllImport

            #region EnumerateQumaIDs
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowEnumlateQumaIDs@@YAHPAUt_QmLowQumaID@@PAH@Z")]
            private static extern QumaLowResponse EnumlateQumaIDs86(IntPtr arrayHeadPtr, ref int outIdCount);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowEnumlateQumaIDs@@YAHPEAUt_QmLowQumaID@@PEAH@Z")]
            private static extern QumaLowResponse EnumlateQumaIDs64(IntPtr arrayHeadPtr, ref int outIdCount);
            private static QumaLowResponse EnumlateQumaIDs(IntPtr arrayHeadPtr, ref int outIdCount)
                => Is64bit ? EnumlateQumaIDs64(arrayHeadPtr, ref outIdCount) : EnumlateQumaIDs86(arrayHeadPtr, ref outIdCount);
            #endregion

            #region GetQumaHandle
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetQumaHandle@@YAHUt_QmLowQumaID@@PAPAX@Z")]
            private static extern QumaLowResponse GetQumaHandle86(QumaId qumaId, ref IntPtr resultHandle);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetQumaHandle@@YAHUt_QmLowQumaID@@PEAPEAX@Z")]
            private static extern QumaLowResponse GetQumaHandle64(QumaId qumaId, ref IntPtr resultHandle);
            private static QumaLowResponse GetQumaHandle(QumaId qumaId, ref IntPtr resultHandle)
                => Is64bit ? GetQumaHandle64(qumaId, ref resultHandle) : GetQumaHandle86(qumaId, ref resultHandle);
            #endregion

            //NOTE: 元のC/C++ヘッダによると、コイツの第二引数は普通NULLで良いらしい
            #region ActivateQuma
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowActivateQuma@@YAHPAXPAUt_QmLowActivateInfo@@@Z")]
            private static extern QumaLowResponse ActivateQuma86(IntPtr qumaHandle, IntPtr activeInfo);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowActivateQuma@@YAHPEAXPEAUt_QmLowActivateInfo@@@Z")]
            private static extern QumaLowResponse ActivateQuma64(IntPtr qumaHandle, IntPtr activeInfo);
            private static QumaLowResponse ActivateQuma(IntPtr qumaHandle, IntPtr activeInfo)
                => Is64bit ? ActivateQuma64(qumaHandle, activeInfo) : ActivateQuma86(qumaHandle, activeInfo);
            #endregion

            #region DeleteQumaHandle
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowDeleteQumaHandle@@YAHPAX@Z")]
            private static extern QumaLowResponse DeleteQumaHandle86(IntPtr qumaHandle);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowDeleteQumaHandle@@YAHPEAX@Z")]
            private static extern QumaLowResponse DeleteQumaHandle64(IntPtr qumaHandle);
            private static QumaLowResponse DeleteQumaHandle(IntPtr qumaHandle)
                => Is64bit ? DeleteQumaHandle64(qumaHandle) : DeleteQumaHandle86(qumaHandle);

            #endregion

            #endregion

            #region 公開ラッパー関数

            /// <summary>Qumaのハンドル一覧を取得します。</summary>
            /// <returns>IDとハンドルが組になったQumaデバイスの一覧</returns>
            public static QumaId[] EnumerateQumaIDs()
            {
                //配列相当の領域を確保: 当然解放が必須
                IntPtr qumaIdsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(QumaIds)));
                int idCount = 0;

                var res = EnumlateQumaIDs(qumaIdsPtr, ref idCount);
                if (res == QumaLowResponse.OK)
                {
                    QumaIds testIds = (QumaIds)Marshal.PtrToStructure(qumaIdsPtr, typeof(QumaIds));
                    Marshal.FreeHGlobal(qumaIdsPtr);

                    if(testIds.Ids != null)
                    {
                        return testIds.Ids.Take(idCount).ToArray();
                    }
                    else
                    {
                        return new QumaId[0];
                    }
                }
                else
                {
                    Marshal.FreeHGlobal(qumaIdsPtr);
                    throw new QmLowException(res);
                }
            }

            /// <summary>
            /// IDを指定してデバイスのハンドルを取得します。
            /// </summary>
            /// <param name="qumaId">ハンドルを取得するためのID</param>
            /// <returns>デバイスハンドル</returns>
            /// <exception cref="QmLowException">ハンドルの取得に失敗した場合に発生します。</exception>
            public static QumaHandle GetQumaHandle(QumaId qumaId)
            {
                IntPtr handle = IntPtr.Zero;
                var response = GetQumaHandle(qumaId, ref handle);
                if (response != QumaLowResponse.OK)
                {
                    throw new QmLowException(response);
                }
                return new QumaHandle(handle);
            }

            /// <summary>Qumaを有効化します。</summary>
            /// <param name="qumaHandle">有効化するQumaのハンドル</param>
            /// <returns>成功した場合: OK</returns>
            public static void ActivateQuma(QumaHandle qumaHandle)
            {
                var res = ActivateQuma(qumaHandle.Handle, IntPtr.Zero);
                if (res != QumaLowResponse.OK)
                {
                    throw new QmLowException(res);
                }
            }

            /// <summary>
            /// 指定したハンドルのデバイスを削除します。
            /// この関数を呼び出さなくとも<see cref="BaseOperation.Exit"/>を使っていれば
            /// 適切にリソース解放が行われるため、この関数を必ず呼び出す必要はありません。           
            /// 逆に、解放予定でないデバイスに対してこの関数を呼び出すと想定外の動作をする危険があります。
            /// </summary>
            /// <param name="qumaHandle">対象のデバイス</param>
            public static void DeleteQumaHandle(QumaHandle qumaHandle)
            {
                var res = DeleteQumaHandle(qumaHandle.Handle);
                if(res != QumaLowResponse.OK)
                {
                    throw new QmLowException(res);
                }
            }

            #endregion
        }

        /// <summary>ボーン関連のAPI</summary>
        public static class Bone
        {
            #region privateなDllImport

            #region GetRootBone
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetRootBone@@YAPAXPAX@Z")]
            private static extern IntPtr GetRootBone86(IntPtr qumaHandle);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetRootBone@@YAPEAXPEAX@Z")]
            private static extern IntPtr GetRootBone64(IntPtr qumaHandle);
            private static IntPtr GetRootBone(IntPtr qumaHandle)
                => Is64bit ? GetRootBone64(qumaHandle) : GetRootBone86(qumaHandle);
            #endregion

            #region GetBoneByName
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetBoneByName@@YAPAXPAXPB_W@Z")]
            private static extern IntPtr GetBoneByName86(IntPtr rootBone, [MarshalAs(UnmanagedType.LPWStr)]string name);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetBoneByName@@YAPEAXPEAXPEB_W@Z")]
            private static extern IntPtr GetBoneByName64(IntPtr rootBone, [MarshalAs(UnmanagedType.LPWStr)]string name);
            private static IntPtr GetBoneByName(IntPtr rootBone, string name)
                => Is64bit ? GetBoneByName64(rootBone, name) : GetBoneByName86(rootBone, name);
            #endregion

            #region GetBoneName
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetBoneName@@YAXPAXPA_W@Z")]
            private static extern void GetBoneName86(IntPtr bone, [MarshalAs(UnmanagedType.LPWStr, SizeConst = ApiConstants.QUMABONE_NAME_NAX_SIZE)]StringBuilder result);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetBoneName@@YAXPEAXPEA_W@Z")]
            private static extern void GetBoneName64(IntPtr bone, [MarshalAs(UnmanagedType.LPWStr, SizeConst = ApiConstants.QUMABONE_NAME_NAX_SIZE)]StringBuilder result);
            private static void GetBoneName(IntPtr bone, StringBuilder result)
            {
                if(Is64bit)
                {
                    GetBoneName64(bone, result);
                }
                else
                {
                    GetBoneName86(bone, result);
                }
            }
            #endregion

            #region GetChildCount
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetChildCount@@YAHPAX@Z")]
            private static extern int GetChildCount86(IntPtr bone);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetChildCount@@YAHPEAX@Z")]
            private static extern int GetChildCount64(IntPtr bone);
            private static int GetChildCount(IntPtr bone)
                 => Is64bit ? GetChildCount64(bone) : GetChildCount86(bone);
            #endregion

            #region GetChildBone
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetChildBone@@YAPAXPAXH@Z")]
            private static extern IntPtr GetChildBone86(IntPtr bone, int index);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetChildBone@@YAPEAXPEAXH@Z")]
            private static extern IntPtr GetChildBone64(IntPtr bone, int index);
            private static IntPtr GetChildBone(IntPtr bone, int index)
                => Is64bit ? GetChildBone64(bone, index) : GetChildBone86(bone, index);
            #endregion

            #region GetBonePosition
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetBonePosition@@YAXPAXPAM@Z")]
            private static extern void GetBonePosition86(IntPtr boneHandle, IntPtr result);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetBonePosition@@YAXPEAXPEAM@Z")]
            private static extern void GetBonePosition64(IntPtr boneHandle, IntPtr result);
            private static void GetBonePosition(IntPtr boneHandle, IntPtr result)
            {
                if(Is64bit)
                {
                    GetBonePosition64(boneHandle, result);
                }
                else
                {
                    GetBonePosition86(boneHandle, result);
                }
            }

            #endregion

            #region ComputeBoneMatrix
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowComputeBoneMatrix@@YAHPAX0PAM@Z")]
            private static extern QumaLowResponse ComputeBoneMatrix86(IntPtr qumaHandle, IntPtr bone, IntPtr result);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowComputeBoneMatrix@@YAHPEAX0PEAM@Z")]
            private static extern QumaLowResponse ComputeBoneMatrix64(IntPtr qumaHandle, IntPtr bone, IntPtr result);
            private static QumaLowResponse ComputeBoneMatrix(IntPtr qumaHandle, IntPtr bone, IntPtr result)
                => Is64bit ? ComputeBoneMatrix64(qumaHandle, bone, result) : ComputeBoneMatrix86(qumaHandle, bone, result);
            #endregion

            #endregion

            #region 公開ラッパー関数

            /// <summary>ルート要素となるボーンを取得します。</summary>
            /// <param name="qumaHandle">Qumaのハンドル</param>
            /// <returns>ルートボーンのハンドル</returns>
            public static BoneHandle GetRootBone(QumaHandle qumaHandle)
            {
                IntPtr boneHandlePtr = GetRootBone(qumaHandle.Handle);
                return new BoneHandle(boneHandlePtr);
            }

            /// <summary>
            /// ルートボーンとボーン名を指定して子ボーンを取得します。
            /// </summary>
            /// <param name="rootBone">ルートボーンのハンドル</param>
            /// <param name="name">子ボーンの名前</param>
            /// <param name="result">成功した場合は指定した子ボーン、失敗した場合はnull</param>
            /// <returns>ボーンの取得に成功したか否か</returns>
            public static bool TryGetBoneByName(BoneHandle rootBone, string name, out BoneHandle result)
            {
                IntPtr resultHandle = GetBoneByName(rootBone.Handle, name);
                if (resultHandle != IntPtr.Zero)
                {
                    result = new BoneHandle(resultHandle);
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            /// <summary>
            /// ルートボーンとボーン名を指定して子ボーンを取得します。
            /// </summary>
            /// <param name="rootBone">ルートボーンのハンドル</param>
            /// <param name="name">子ボーンの名前</param>
            /// <returns>指定した子ボーン</returns>
            /// <exception cref="InvalidOperationException">
            /// 指定した子ボーンが見つからない場合に発生。
            /// 例外を起こしたくない場合は<see cref="TryGetBoneByName(BoneHandle, string, out BoneHandle)"/>を使ってください。
            /// </exception>
            public static BoneHandle GetBoneByName(BoneHandle rootBone, string name)
            {
                IntPtr result = GetBoneByName(rootBone.Handle, name);
                if (result == IntPtr.Zero)
                {
                    throw new InvalidOperationException("Bone was not found");
                }
                return new BoneHandle(result);
            }

            /// <summary>
            /// ボーンを指定してボーンの名前を取得します。
            /// </summary>
            /// <param name="boneHandle">対象ボーン</param>
            /// <returns>ボーンの名称</returns>
            public static string GetBoneName(BoneHandle boneHandle)
            {
                var result = new StringBuilder(ApiConstants.QUMABONE_NAME_NAX_SIZE);
                GetBoneName(boneHandle.Handle, result);
                return result.ToString();
            }

            /// <summary>
            /// ボーンを指定して子ボーンの数を取得します。
            /// </summary>
            /// <param name="boneHandle">子ボーンの数を調べたいボーン</param>
            /// <returns>子ボーンの数</returns>
            public static int GetChildCount(BoneHandle boneHandle)
            {
                return GetChildCount(boneHandle.Handle);
            }

            /// <summary>
            /// ボーンの子ボーンをインデクスの指定によって取得します。
            /// </summary>
            /// <param name="boneHandle">対象ボーン</param>
            /// <param name="index">子ボーンのインデックス</param>
            /// <returns>結果を収納したボーン。取得に失敗した場合、IntPtr.Zeroを持ったハンドルになります。</returns>
            public static BoneHandle GetChildBone(BoneHandle boneHandle, int index)
            {
                return new BoneHandle(GetChildBone(boneHandle.Handle, index));
            }

            /// <summary>親ボーンを指定して子ボーンの一覧を取得します。</summary>
            /// <param name="bone">親となるボーン</param>
            /// <returns>子となるボーンの一覧。子が無い場合は要素数0の配列を返します。</returns>
            public static BoneHandle[] GetChildBones(BoneHandle boneHandle)
            {
                int childCount = GetChildCount(boneHandle.Handle);
                var result = new BoneHandle[childCount];
                for (int i = 0; i < childCount; i++)
                {
                    result[i] = new BoneHandle(GetChildBone(boneHandle.Handle, i));
                }
                return result;
            }

            /// <summary>
            /// デバイスとボーンを指定してボーンの位置を取得します。
            /// </summary>
            /// <param name="qumaHandle">対象デバイス</param>
            /// <param name="boneHandle">対象ボーン</param>
            /// <returns>ボーンの位置</returns>
            public static Vector3 GetBonePosition(BoneHandle boneHandle)
            {
                var vectorArray = new float[3];
                var vectorPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * 3);

                GetBonePosition(boneHandle.Handle, vectorPtr);
                Marshal.Copy(vectorPtr, vectorArray, 0, 3);
                Marshal.FreeHGlobal(vectorPtr);

                return new Vector3(vectorArray);

            }

            /// <summary>
            /// デバイスとボーンを指定して、ボーンに関連づけられた行列を取得します。
            /// </summary>
            /// <param name="qumaHandle">デバイスのハンドル</param>
            /// <param name="boneHandle">ボーンのハンドル</param>
            /// <returns>ボーンに関連づけられた行列</returns>
            public static Matrix4 ComputeBoneMatrix(QumaHandle qumaHandle, BoneHandle boneHandle)
            {
                var matrixPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * 16);
                var response = ComputeBoneMatrix(qumaHandle.Handle, boneHandle.Handle, matrixPtr);

                if (response == QumaLowResponse.OK)
                {
                    var matrixArray = new float[16];
                    Marshal.Copy(matrixPtr, matrixArray, 0, 16);
                    Marshal.FreeHGlobal(matrixPtr);

                    return new Matrix4(matrixArray);
                }
                else
                {
                    Marshal.FreeHGlobal(matrixPtr);
                    throw new QmLowException(response);
                }
            }

            #endregion
        }

        /// <summary>各ボーンのセンサーと加速度計のAPI</summary>
        public static class Sensors
        {
            #region privateなDllImport

            #region GetSensorCount
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetSensorCount@@YAHPAX@Z")]
            private static extern int GetSensorCount86(IntPtr bone);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetSensorCount@@YAHPEAX@Z")]
            private static extern int GetSensorCount64(IntPtr bone);
            private static int GetSensorCount(IntPtr bone)
                => Is64bit ? GetSensorCount64(bone) : GetSensorCount86(bone);
            #endregion

            #region GetSensor
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetSensor@@YAPAXPAXH@Z")]
            private static extern IntPtr GetSensor86(IntPtr bone, int index);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetSensor@@YAPEAXPEAXH@Z")]
            private static extern IntPtr GetSensor64(IntPtr bone, int index);
            private static IntPtr GetSensor(IntPtr bone, int index) => Is64bit ? GetSensor64(bone, index) : GetSensor86(bone, index);
            #endregion

            #region GetSensorAxis
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetSensorAxis@@YAXPAXPAM@Z")]
            private static extern void GetSensorAxis86(IntPtr sensor, IntPtr result);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetSensorAxis@@YAXPEAXPEAM@Z")]
            private static extern void GetSensorAxis64(IntPtr sensor, IntPtr result);
            private static void GetSensorAxis(IntPtr sensor, IntPtr result)
            {
                if(Is64bit)
                {
                    GetSensorAxis64(sensor, result);
                }
                else
                {
                    GetSensorAxis86(sensor, result);
                }
            }
            #endregion

            #region GetSensorState
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetSensorState@@YA?AW4QMLOW_SENSOR_STATE@@PAX0@Z")]
            private static extern SensorStates GetSensorState86(IntPtr qumaHandle, IntPtr sensor);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetSensorState@@YA?AW4QMLOW_SENSOR_STATE@@PEAX0@Z")]
            private static extern SensorStates GetSensorState64(IntPtr qumaHandle, IntPtr sensor);
            private static SensorStates GetSensorState(IntPtr qumaHandle, IntPtr sensor)
                => Is64bit ? GetSensorState64(qumaHandle, sensor) : GetSensorState86(qumaHandle, sensor);
            #endregion

            #region ComputeSensorAngle
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowComputeSensorAngle@@YAHPAX0PAM@Z")]
            private static extern QumaLowResponse ComputeSensorAngle86(IntPtr qumaHandle, IntPtr sensor, ref float f);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowComputeSensorAngle@@YAHPEAX0PEAM@Z")]
            private static extern QumaLowResponse ComputeSensorAngle64(IntPtr qumaHandle, IntPtr sensor, ref float f);
            private static QumaLowResponse ComputeSensorAngle(IntPtr qumaHandle, IntPtr sensor, ref float f)
                => Is64bit ? ComputeSensorAngle64(qumaHandle, sensor, ref f) : ComputeSensorAngle86(qumaHandle, sensor, ref f);
            #endregion

            #region GetAccelerometer
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetAccelerometer@@YAHPAXPAM@Z")]
            private static extern QumaLowResponse GetAccelerometer86(IntPtr qumaHandle, IntPtr result);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetAccelerometer@@YAHPEAXPEAM@Z")]
            private static extern QumaLowResponse GetAccelerometer64(IntPtr qumaHandle, IntPtr result);
            private static QumaLowResponse GetAccelerometer(IntPtr qumaHandle, IntPtr result)
                => Is64bit ? GetAccelerometer64(qumaHandle, result) : GetAccelerometer86(qumaHandle, result);
            #endregion

            #region GetAccelerometerPoseMatrix
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetAccelerometerPoseMatrix@@YAHPAXPAM@Z")]
            private static extern QumaLowResponse GetAccelerometerPoseMatrix86(IntPtr qumaHandle, IntPtr result);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetAccelerometerPoseMatrix@@YAHPEAXPEAM@Z")]
            private static extern QumaLowResponse GetAccelerometerPoseMatrix64(IntPtr qumaHandle, IntPtr result);
            private static QumaLowResponse GetAccelerometerPoseMatrix(IntPtr qumaHandle, IntPtr result)
                => Is64bit ? GetAccelerometerPoseMatrix64(qumaHandle, result) : GetAccelerometerPoseMatrix86(qumaHandle, result);
            #endregion

            #endregion

            #region 公開ラッパー関数

            /// <summary>
            /// ボーンを指定してセンサーの個数を取得します。
            /// </summary>
            /// <param name="boneHandle">確認対象のボーン</param>
            /// <returns>ボーンに割り当てられたセンサーの個数</returns>
            public static int GetSensorCount(BoneHandle boneHandle)
            {
                return GetSensorCount(boneHandle.Handle);
            }

            /// <summary>
            /// ボーンとインデックスを指定してセンサーへのハンドルを取得します。
            /// </summary>
            /// <param name="boneHandle">対象のボーン</param>
            /// <param name="index">
            /// 取得するセンサのインデックス。インデックス範囲を調べるには
            /// <see cref="GetSensorCount(BoneHandle)"/>を使います。
            /// </param>
            /// <returns></returns>
            public static SensorHandle GetSensor(BoneHandle boneHandle, int index)
            {
                return new SensorHandle(GetSensor(boneHandle.Handle, index));
            }

            /// <summary>
            /// ボーンを指定してボーンに関連づけられるセンサーの一覧を取得します。
            /// </summary>
            /// <param name="boneHandle">対象のボーン</param>
            /// <returns>ボーンに関連づけられるセンサーの一覧</returns>
            public static SensorHandle[] GetSensors(BoneHandle boneHandle)
            {
                int count = GetSensorCount(boneHandle);
                var result = new SensorHandle[count];
                for (int i = 0; i < count; i++)
                {
                    result[i] = GetSensor(boneHandle, i);
                }
                return result;
            }

            /// <summary>
            /// センサーを指定して3軸の値を読み取ります。
            /// </summary>
            /// <param name="sensorHandle">調べる対象のセンサー</param>
            /// <returns>読みだされた3軸の値</returns>
            public static Vector3 GetSensorAxis(SensorHandle sensorHandle)
            {
                var vectorPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * 3);
                GetSensorAxis(sensorHandle.Handle, vectorPtr);

                var result = new float[3];
                Marshal.Copy(vectorPtr, result, 0, 3);
                Marshal.FreeHGlobal(vectorPtr);

                return new Vector3(result);
            }

            /// <summary>
            /// デバイスとセンサーを指定してセンサーの状態を取得します。
            /// </summary>
            /// <param name="qumaHandle">対象のデバイス</param>
            /// <param name="sensorHandle">対象のセンサー</param>
            /// <returns>センサーの状態</returns>
            public static SensorStates GetSensorState(QumaHandle qumaHandle, SensorHandle sensorHandle)
            {
                return GetSensorState(qumaHandle.Handle, sensorHandle.Handle);
            }

            /// <summary>
            /// デバイスとセンサーを指定して角度を求めます。
            /// </summary>
            /// <param name="qumaHandle">対象のデバイス</param>
            /// <param name="sensorHandle">対象のセンサー</param>
            /// <returns>角度値</returns>
            /// <exception cref="QmLowException">呼び出しが正常な場合以外に発生します。</exception>
            public static float ComputeSensorAngle(QumaHandle qumaHandle, SensorHandle sensorHandle)
            {
                float result = 0.0f;
                var response = ComputeSensorAngle(qumaHandle.Handle, sensorHandle.Handle, ref result);
                if (response != QumaLowResponse.OK)
                {
                    throw new QmLowException(response);
                }
                return result;
            }

            /// <summary>
            /// デバイスとセンサーを指定して角度を求めます。このメソッドは例外を発生しません。
            /// </summary>
            /// <param name="qumaHandle">対象のデバイス</param>
            /// <param name="sensorHandle">対象のセンサー</param>
            /// <param name="result">結果を収納する値。関数の呼び出しに失敗した場合は何も代入されません。</param>
            /// <returns>関数呼び出しの結果。OKが返ってきた場合は正しく値が取得出来ており、他の場合では失敗しています。</returns>
            public static QumaLowResponse TryComputeSensorAngle(QumaHandle qumaHandle, SensorHandle sensorHandle, ref float result)
            {
                return ComputeSensorAngle(qumaHandle.Handle, sensorHandle.Handle, ref result);
            }

            /// <summary>
            /// <para>フィルタ処理をおこなっていない加速度センサの状態を取得します。</para>
            /// <para>この値は、加速度センサの設置状態に対するセンサ値です。</para>
            /// <para>Qumarionから値を取得する場合、加速度センサは、QUMARION本体グリップ内に設置されているため、胸を正面に向けた場合、25度傾いています。</para>
            /// </summary>
            /// <param name="qumaHandle">デバイスのハンドル</param>
            /// <returns>加速度計の読み</returns>
            /// <exception cref="QmLowException" />
            public static Vector3 GetAccelerometer(QumaHandle qumaHandle)
            {
                var vectorPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * 3);
                var response = GetAccelerometer(qumaHandle.Handle, vectorPtr);

                if (response == QumaLowResponse.OK)
                {
                    var vectorArray = new float[3];
                    Marshal.Copy(vectorPtr, vectorArray, 0, 3);
                    Marshal.FreeHGlobal(vectorPtr);

                    return new Vector3(vectorArray);
                }
                else
                {
                    Marshal.FreeHGlobal(vectorPtr);
                    throw new QmLowException(response);
                }
            }

            /// <summary>
            /// <para>フィルタ処理をおこなっていない加速度センサの状態を取得します。</para>
            /// <para>この値は、加速度センサの設置状態に対するセンサ値です。</para>
            /// <para>Qumarionから値を取得する場合、加速度センサは、QUMARION本体グリップ内に設置されているため、胸を正面に向けた場合、25度傾いています。</para>
            /// </summary>
            /// <param name="qumaHandle">デバイスのハンドル</param>
            /// <param name="result">成功した場合結果を受け取るベクトル</param>
            /// <returns>処理が成功した場合OK、失敗の場合はそれ以外</returns>
            public static QumaLowResponse TryGetAccelerometer(QumaHandle qumaHandle, out Vector3 result) 
            {
                var vectorPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * 3);
                var response = GetAccelerometer(qumaHandle.Handle, vectorPtr);

                if (response == QumaLowResponse.OK)
                {
                    var vectorArray = new float[3];
                    Marshal.Copy(vectorPtr, vectorArray, 0, 3);
                    result = new Vector3(vectorArray);
                }
                else
                {
                    result = new Vector3(new float[3]);
                }
                Marshal.FreeHGlobal(vectorPtr);

                return response;
            }

            /// <summary>
            /// <para>デバイスを指定して加速度計から求まる姿勢行列を取得します。</para>
            /// <para>Qumarionから値を取得する場合、加速度センサは、QUMARION本体グリップ内に設置されていっるため、胸を正面に向けた場合、25度傾いています。</para>
            /// <para>この関数は、加速度センサ値にローパスフィルタ処理を施した後、25度の傾きを補正し、胸を正面に向けた状態からの回転角度を行列を取得することができます。</para>
            /// </summary>
            /// <param name="qumaHandle">対象のデバイス</param>
            /// <returns>加速度計に関する姿勢行列</returns>
            /// <exception cref="QmLowException" />
            public static Matrix4 GetAccelerometerPoseMatrix(QumaHandle qumaHandle)
            {
                var matrixPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * 16);
                var response = GetAccelerometerPoseMatrix(qumaHandle.Handle, matrixPtr);

                if (response == QumaLowResponse.OK)
                {
                    var matrixArray = new float[16];
                    Marshal.Copy(matrixPtr, matrixArray, 0, 16);
                    Marshal.FreeHGlobal(matrixPtr);

                    return new Matrix4(matrixArray);
                }
                else
                {
                    Marshal.FreeHGlobal(matrixPtr);
                    throw new QmLowException(response);
                }
            }

            /// <summary>
            /// <para>デバイスを指定して加速度計から求まる姿勢行列を取得します。</para>
            /// <para>Qumarionから値を取得する場合、加速度センサは、QUMARION本体グリップ内に設置されていっるため、胸を正面に向けた場合、25度傾いています。</para>
            /// <para>この関数は、加速度センサ値にローパスフィルタ処理を施した後、25度の傾きを補正し、胸を正面に向けた状態からの回転角度を行列を取得することができます。</para>
            /// </summary>
            /// <param name="qumaHandle">対象のデバイス</param>
            /// <param name="result">成功した場合、姿勢を表す行列が格納される</param>
            /// <returns>処理が成功した場合OK、失敗の場合はそれ以外</returns>
            public static QumaLowResponse TryGetAccelerometerPoseMatrix(QumaHandle qumaHandle, out Matrix4 result)
            {
                var matrixPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * 16);
                var response = GetAccelerometerPoseMatrix(qumaHandle.Handle, matrixPtr);

                if (response == QumaLowResponse.OK)
                {
                    var matrixArray = new float[16];
                    Marshal.Copy(matrixPtr, matrixArray, 0, 16);
                    result = new Matrix4(matrixArray);
                }
                else
                {
                    result = new Matrix4(new float[16]);
                }
                Marshal.FreeHGlobal(matrixPtr);
                return response;
            }

            #endregion
        }

        /// <summary>更新処理のAPI</summary>
        public static class Update
        {
            #region privateなDllImport

            #region UpdateBuffer
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowUpdateBuffer@@YAHPAX@Z")]
            private static extern QumaLowResponse UpdateBuffer86(IntPtr qumaHandle);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowUpdateBuffer@@YAHPEAX@Z")]
            private static extern QumaLowResponse UpdateBuffer64(IntPtr qumaHandle);
            private static QumaLowResponse UpdateBuffer(IntPtr qumaHandle)
                => Is64bit ? UpdateBuffer64(qumaHandle) : UpdateBuffer86(qumaHandle);
            #endregion

            #region UpdateQumaHandle
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowUpdateQumaHandle@@YAHPAX@Z")]
            private static extern QumaLowResponse UpdateQumaHandle86(IntPtr qumaHandle);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowUpdateQumaHandle@@YAHPEAX@Z")]
            private static extern QumaLowResponse UpdateQumaHandle64(IntPtr qumaHandle);
            private static QumaLowResponse UpdateQumaHandle(IntPtr qumaHandle)
                => Is64bit ? UpdateQumaHandle64(qumaHandle) : UpdateQumaHandle86(qumaHandle);
            #endregion

            #endregion

            #region 公開ラッパー関数

            /// <summary>
            /// デバイスのセンサ情報を更新します。このメソッドは例外を投げません。
            /// </summary>
            /// <param name="qumaHandle">対象のデバイス。</param>
            /// <returns>正常な更新: OK, Qumaの情報が更新されUpdateQumaHandleの呼び出しが必要: INFO_CHANGED, 更新失敗: ERR</returns>
            public static QumaLowResponse TryUpdateBuffer(QumaHandle qumaHandle)
            {
                return UpdateBuffer(qumaHandle.Handle);
            }

            /// <summary>
            /// デバイスのセンサ情報を更新します。このメソッドは正常更新以外のケースで例外を投げます。
            /// </summary>
            /// <param name="qumaHandle">対象のデバイス。</param>
            /// <exception cref="QmLowException">正常更新以外のケースで発生</exception>
            public static void UpdateBuffer(QumaHandle qumaHandle)
            {
                var res = UpdateBuffer(qumaHandle.Handle);
                if (res != QumaLowResponse.OK)
                {
                    throw new QmLowException(res);
                }
            }

            /// <summary>
            /// デバイスを最新状態に更新します。普段は用いず、UpdateBuffer関数の戻り値が
            /// <see cref="QumaLowResponse.INFO_CHANGED"/>だった場合に使います。
            /// </summary>
            /// <param name="qumaHandle">Qumaのハンドル</param>
            /// <returns>成功した場合: OK</returns>
            public static QumaLowResponse TryUpdateQumaHandle(QumaHandle qumaHandle)
            {
                return UpdateQumaHandle(qumaHandle.Handle);
            }

            /// <summary>
            /// デバイスを最新状態に更新します。普段は用いず、UpdateBuffer関数の戻り値が
            /// <see cref="QumaLowResponse.INFO_CHANGED"/>だった場合に使います。
            /// このメソッドは正常に呼ばれた場合以外で例外を発生させます。
            /// </summary>
            /// <param name="qumaHandle">Qumaのハンドル</param>
            /// <exception cref="QmLowException">呼び出しに失敗した場合</exception>
            public static void UpdateQumaHandle(QumaHandle qumaHandle)
            {
                var res = UpdateQumaHandle(qumaHandle.Handle);
                if(res != QumaLowResponse.OK)
                {
                    throw new QmLowException(res);
                }
            }

            #endregion
        }

        /// <summary>ボタンのAPI</summary>
        public static class Button
        {
            #region GetButtonState
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetButtonState@@YAHPAXW4QMLOW_BUTTON_TYPE@@PAW4QMLOW_BUTTON_STATE@@@Z")]
            private static extern QumaLowResponse GetButtonState86(IntPtr qumaHandle, ButtonType buttonType, ref ButtonState buttonState);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetButtonState@@YAHPEAXW4QMLOW_BUTTON_TYPE@@PEAW4QMLOW_BUTTON_STATE@@@Z")]
            private static extern QumaLowResponse GetButtonState64(IntPtr qumaHandle, ButtonType buttonType, ref ButtonState buttonState);
            private static QumaLowResponse GetButtonState(IntPtr qumaHandle, ButtonType buttonType, ref ButtonState buttonState)
                => Is64bit ?
                GetButtonState64(qumaHandle, buttonType, ref buttonState) :
                GetButtonState86(qumaHandle, buttonType, ref buttonState);
            #endregion

            /// <summary>ボタンの状態を取得します。</summary>
            /// <param name="qumaHandle">Qumaデバイスのハンドル</param>
            /// <returns>ボタンが押されてるかどうか</returns>
            /// <exception cref="QmLowException">関数呼び出しに失敗した場合発生</exception>
            public static ButtonState GetState(QumaHandle qumaHandle)
            {
                var result = ButtonState.Up;
                var response = GetButtonState(qumaHandle.Handle, ButtonType.MainButton, ref result);
                if (response != QumaLowResponse.OK)
                {
                    throw new QmLowException(response);
                }
                return result;
            }

            /// <summary>ボタンの状態取得を試みます。このメソッドは例外を投げません。</summary>
            /// <param name="qumaHandle">Qumaデバイスのハンドル</param>
            /// <param name="buttonState">
            /// 呼び出しに成功した場合ボタンの状態、失敗した場合は
            /// <see cref="ButtonState.Up"/>を返します。
            /// </param>
            /// <returns>関数呼び出しに成功したかどうか</returns>
            public static QumaLowResponse TryGetState(QumaHandle qumaHandle, out ButtonState buttonState)
            {
                buttonState = ButtonState.Up;
                var result = GetButtonState(qumaHandle.Handle, ButtonType.MainButton, ref buttonState);
                if (result != QumaLowResponse.OK)
                {
                    buttonState = ButtonState.Up;
                }

                return result;
            }

        }

        /// <summary>バッファ履歴API: あんま触る気がないので現状放置中</summary>
        public static class BufferHistory
        {
            #region GetBufferHistory
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetBufferHistory@@YAHPAXPAPAPAX@Z")]
            private static extern QumaLowResponse GetBufferHistory86(IntPtr qumaHandle, IntPtr history);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetBufferHistory@@YAHPEAXPEAPEAPEAX@Z")]
            public static extern QumaLowResponse GetBufferHistory64(IntPtr qumaHandle, IntPtr history);
            public static QumaLowResponse GetBufferHistory(IntPtr qumaHandle, IntPtr history)
                => Is64bit ? GetBufferHistory64(qumaHandle, history) : GetBufferHistory86(qumaHandle, history);
            #endregion

            #region GetBufferHistoryCount
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetBufferHistoryCount@@YAHPAPAX@Z")]
            private static extern int GetBufferHistoryCount86(IntPtr history);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowGetBufferHistoryCount@@YAHPEAPEAX@Z")]
            private static extern int GetBufferHistoryCount64(IntPtr history);
            public static int GetBufferHistoryCount(IntPtr history)
                => Is64bit ? GetBufferHistoryCount64(history) : GetBufferHistoryCount86(history);
            #endregion

            #region DeleteBufferHistory
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowDeleteBufferHistory@@YAHPEAPEAX@Z")]
            private static extern QumaLowResponse DeleteBufferHistory86(IntPtr bufferHistoryHandle);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowDeleteBufferHistory@@YAHPAPAX@Z")]
            private static extern QumaLowResponse DeleteBufferHistory64(IntPtr bufferHistoryHandle);
            public static QumaLowResponse DeleteBufferHistory(IntPtr bufferHistoryHandle)
                => Is64bit ? DeleteBufferHistory64(bufferHistoryHandle) : DeleteBufferHistory86(bufferHistoryHandle);
            #endregion

            #region UpdateBufferHistory
            [DllImport(DllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowUpdateBufferOneStep@@YAHPAXPAPAXH@Z")]
            private static extern QumaLowResponse UpdateBufferOneStep86(IntPtr qumaHandle);
            [DllImport(DllName64, CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?QmLowUpdateBufferOneStep@@YAHPEAXPEAPEAXH@Z")]
            private static extern QumaLowResponse UpdateBufferOneStep64(IntPtr qumaHandle);
            public static QumaLowResponse UpdateBufferOneStep(IntPtr qumaHandle)
                => Is64bit ? UpdateBufferOneStep64(qumaHandle) : UpdateBufferOneStep86(qumaHandle);
            #endregion

        }
    }

}
