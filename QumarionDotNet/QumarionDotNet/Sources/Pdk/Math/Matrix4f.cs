using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Baku.Quma.Pdk
{

    /// <summary>回転および並進情報を表す4次正方行列を表します。</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4f
    {
        internal static Matrix4f Create()
        {
            return new Matrix4f();
            //配列をメンバにすると値型の良さがつぶれるので廃止です。
            //{
            //    _values = new float[16]
            //};
        }

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        //private float[] _values;

        //順番を入れ替えないように！
        [MarshalAs(UnmanagedType.R4)]
        public float M11;
        [MarshalAs(UnmanagedType.R4)]
        public float M21;
        [MarshalAs(UnmanagedType.R4)]
        public float M31;
        [MarshalAs(UnmanagedType.R4)]
        public float M41;

        [MarshalAs(UnmanagedType.R4)]
        public float M12;
        [MarshalAs(UnmanagedType.R4)]
        public float M22;
        [MarshalAs(UnmanagedType.R4)]
        public float M32;
        [MarshalAs(UnmanagedType.R4)]
        public float M42;

        [MarshalAs(UnmanagedType.R4)]
        public float M13;
        [MarshalAs(UnmanagedType.R4)]
        public float M23;
        [MarshalAs(UnmanagedType.R4)]
        public float M33;
        [MarshalAs(UnmanagedType.R4)]
        public float M43;

        [MarshalAs(UnmanagedType.R4)]
        public float M14;
        [MarshalAs(UnmanagedType.R4)]
        public float M24;
        [MarshalAs(UnmanagedType.R4)]
        public float M34;
        [MarshalAs(UnmanagedType.R4)]
        public float M44;

        /// <summary>
        /// コピーされた成分値の一覧を取得します。
        /// </summary>
        /// <returns>成分値の一覧。0列目の0, 1, 2, 3行目, 2列目の0, 1, 2, 3行目…という列ベースの順序であることに注意</returns>
        public float[] GetValues() => new float[]
        {
            M11, M12, M13, M14,
            M21, M22, M23, M24,
            M31, M32, M33, M34,
            M41, M42, M43, M44
        };

        /// <summary>行列のi行j列目の成分を取得、設定します。</summary>
        /// <param name="i">行番号(0, 1, 2, 3)</param>
        /// <param name="j">列番号(0, 1, 2, 3)</param>
        /// <returns>指定した箇所の成分値</returns>
        public float this[int i, int j]
        {
            get
            {
                if (i < 0 || i > 3 || j < 0 || j > 3)
                {
                    throw new IndexOutOfRangeException();
                }
                if (i == 0)
                {
                    if (j == 0) return M11;
                    if (j == 1) return M12;
                    if (j == 2) return M13;
                    return M14;
                }
                else if (i == 1)
                {
                    if (j == 0) return M21;
                    if (j == 1) return M22;
                    if (j == 2) return M23;
                    return M24;
                }
                else if (i == 2)
                {
                    if (j == 0) return M31;
                    if (j == 1) return M32;
                    if (j == 2) return M33;
                    return M34;
                }
                else //i == 3
                {
                    if (j == 0) return M41;
                    if (j == 1) return M42;
                    if (j == 2) return M43;
                    return M44;
                }
            }
            set
            {
                if (i < 0 || i > 3 || j < 0 || j > 3)
                {
                    throw new IndexOutOfRangeException();
                }
                if (i == 0)
                {
                    if (j == 0)      M11 = value;
                    else if (j == 1) M12 = value;
                    else if (j == 2) M13 = value;
                    else if (j == 3) M14 = value;
                }
                else if (i == 1)
                {
                    if (j == 0)      M21 = value;
                    else if (j == 1) M22 = value;
                    else if (j == 2) M23 = value;
                    else if (j == 3) M24 = value;
                }
                else if (i == 2)
                {
                    if (j == 0)      M31 = value;
                    else if (j == 1) M32 = value;
                    else if (j == 2) M33 = value;
                    else if (j == 3) M34 = value;
                }
                else //i == 3
                {
                    if (j == 0)      M41 = value;
                    else if (j == 1) M42 = value;
                    else if (j == 2) M43 = value;
                    else if (j == 3) M44 = value;
                }
            }
        }

        /// <summary>行列の表す平行移動成分を取得します。</summary>
        public Vector3f Translate => new Vector3f(this[0, 3], this[1, 3], this[2, 3]);

        /// <summary>行列のうち平行移動成分を0としたものを取得します。</summary>
        public Matrix4f Rotate
        {
            get
            {
                var result = Create();

                result.M11 = M11;
                result.M12 = M12;
                result.M13 = M13;

                result.M21 = M21;
                result.M22 = M22;
                result.M23 = M23;

                result.M31 = M31;
                result.M32 = M32;
                result.M33 = M33;

                result.M44 = 1.0f;
                return result;
            }
        }

        /// <summary>平行移動成分を行列形式で表したものを取得します。</summary>
        public Matrix4f TranslateAsMatrix
        {
            get
            {
                var result = Create();
                var t = Translate;
                result.M11 = 1.0f;
                result.M22 = 1.0f;
                result.M33 = 1.0f;
                result.M44 = 1.0f;

                result.M14 = t.X;
                result.M24 = t.Y;
                result.M34 = t.Z;
                return result;
            }
        }

        ///// <summary>別の行列から成分値をコピーします。</summary>
        ///// <param name="m">コピー元の行列</param>
        //public void CopyFrom(Matrix4f m)
        //{
        //    for (int i = 0; i < 4; i++)
        //    {
        //        for (int j = 0; j < 4; j++)
        //        {
        //            this[i, j] = m[i, j];
        //        }
        //    }
        //}

        ///// <summary>別の行列から回転成分(左上の3次正方行列ぶん)をコピーします。</summary>
        ///// <param name="m">コピー元の行列</param>
        //public void CopyRotationFrom(Matrix4f m)
        //{
        //    for (int i = 0; i < 3; i++)
        //    {
        //        for (int j = 0; j < 3; j++)
        //        {
        //            this[i, j] = m[i, j];
        //        }
        //    }
        //}

        /// <summary>行列の積を求めます。</summary>
        /// <param name="m">掛けられる行列</param>
        /// <returns>行列積</returns>
        public Matrix4f Multiply(Matrix4f m)
        {
            var result = Create();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        result[i, j] += this[i, k] * m[k, j];
                    }
                }
            }
            return result;
        }

        /// <summary>転置行列を取得します。</summary>
        /// <returns>転置された行列</returns>
        public Matrix4f Transpose()
        {
            var result = Matrix4f.Create();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[i, j] = this[j, i];
                }
            }
            return result;
        }

        /// <summary>成分一覧を文字列として取得します。</summary>
        /// <returns>行列の成分一覧の文字列</returns>
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("[");

            for (int i = 0; i < 4; i++)
            {
                result.Append("[");
                for (int j = 0; j < 4; j++)
                {
                    result.Append(this[i, j].ToString("0.000") + ", ");
                }
                result.Append("], ");
            }

            result.Append("]");
            return result.ToString();
        }

        /// <summary>単位行列を取得します。</summary>
        public static Matrix4f Unit
        {
            get
            {
                var result = new Matrix4f();
                result.M11 = 1.0f;
                result.M22 = 1.0f;
                result.M33 = 1.0f;
                result.M44 = 1.0f;
                return result;
            }
        }

    }

}
