using System;

namespace Baku.Quma
{
    /// <summary>Dllのロード先を定義します。</summary>
    public static class DllImportSetting
    {
        /// <summary>現在実行中のプロセスが64bitプロセスかどうかを取得します。</summary>
        internal static readonly bool Is64bit = (IntPtr.Size == 8);

        //NOTE1: 理由がよく分からないがdebugのQmPdkDll.dllは呼べないらしい
        //NOTE2: Unity用にビルドするときはUNITYシンボルをプロジェクトプロパティで定義してAny CPU構成を取ること
        //NOTE3: Unity上にソース自体を持っていくのは無理: なぜなら本プロジェクトがC# 6.0準拠で書かれているため。

        /// <summary>QUMARION SDKのライブラリ(32bit版)のインポート先です。</summary>
#if UNITY
        public const string DllName86 = "QmPdkDll";
#else
        public const string DllName86 = @"dll\release\x86\QmPdkDll.dll";
#endif

        /// <summary>QUMARION SDKのライブラリ(64bit版)のインポート先です。</summary>
#if UNITY
        public const string DllName64 = "QmPdkDll";
#else
        public const string DllName64 = @"dll\release\x64\QmPdkDll.dll";
#endif



    }
}
