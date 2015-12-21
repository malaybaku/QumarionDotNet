using System;
using System.Runtime.InteropServices;

namespace Baku.Quma.Low
{
    /// <summary>3次元のベクトルを表します。</summary>
    public struct Vector3
    {
        /// <summary>ベクトルに相当する小数の配列でインスタンスを初期化します。</summary>
        /// <param name="f">[x, y, z]の3要素をもつ小数の配列</param>
        internal Vector3(float[] f)
        {
            if (f.Length != 3) throw new ArgumentException("Range of array is not correct");

            X = f[0];
            Y = f[1];
            Z = f[2];
        }

        public float X { get; }
        public float Y { get; }
        public float Z { get; }
    }

}
