using System;

namespace Baku.Quma
{
    /// <summary>
    /// <para>姿勢表現に対応する4次正方行列を表します。</para>
    /// <para>NOTE:あくまでラップが目的なので計算ユーティリティは提供されていません</para>
    /// </summary>
    public struct Matrix4
    {
        /// <summary>APIから受け取った小数配列でインスタンスを初期化します。</summary>
        /// <param name="f">16要素(x11, x12, x13, x14, .., x44)をもつ小数配列</param>
        public Matrix4(float[] f)
        {
            if(f.Length != 16) throw new ArgumentException("Range of array is not correct");

            M11 = f[0];
            M12 = f[1];
            M13 = f[2];
            M14 = f[3];

            M21 = f[4];
            M22 = f[5];
            M23 = f[6];
            M24 = f[7];

            M31 = f[8];
            M32 = f[9];
            M33 = f[10];
            M34 = f[11];

            M41 = f[12];
            M42 = f[13];
            M43 = f[14];
            M44 = f[15];
        }

        public float M11 { get; }
        public float M12 { get; }
        public float M13 { get; }
        public float M14 { get; }

        public float M21 { get; }
        public float M22 { get; }
        public float M23 { get; }
        public float M24 { get; }

        public float M31 { get; }
        public float M32 { get; }
        public float M33 { get; }
        public float M34 { get; }

        public float M41 { get; }
        public float M42 { get; }
        public float M43 { get; }
        public float M44 { get; }

        public float[] Values
        {
            get
            {
                return new float[]
                {
                    M11,
                    M12,
                    M13,
                    M14,

                    M21,
                    M22,
                    M23,
                    M24,

                    M31,
                    M32,
                    M33,
                    M34,

                    M41,
                    M42,
                    M43,
                    M44
                };
            }           
        }
    }
}
