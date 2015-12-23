using System;
using System.Runtime.InteropServices;

namespace Baku.Quma.Pdk
{

    /// <summary>回転および並進情報を表す4次正方行列を表します。</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4f
    {
        internal static Matrix4f Create()
        {
            return new Matrix4f()
            {
                _values = new float[16]
            };
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private float[] _values;


        /// <summary>
        /// コピーされた成分値の一覧を取得します。
        /// </summary>
        /// <returns>成分値の一覧。0列目の0, 1, 2, 3行目, 2列目の0, 1, 2, 3行目…という列ベースの順序であることに注意</returns>
        public float[] GetValues()
        {
            var result = new float[16];
            Array.Copy(_values, result, 16);
            return result;
        }

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
                return _values[j * 4 + i];
            }
            set
            {
                if (i < 0 || i > 3 || j < 0 || j > 3)
                {
                    throw new IndexOutOfRangeException();
                }
                _values[j * 4 + i] = value;
            }
        }

        /// <summary>行列の表す平行移動成分成分を取得します。</summary>
        /// <returns>平行移動成分</returns>
        public Vector3f GetTranslate()
        {
            return new Vector3f(this[3, 0], this[3, 1], this[3, 2]);
        }

        /// <summary>別の行列から成分値をコピーします。</summary>
        /// <param name="m">コピー元の行列</param>
        public void CopyFrom(Matrix4f m)
        {
            Array.Copy(m._values, _values, 16);
        }

        /// <summary>別の行列から回転成分(左上の3次正方行列ぶん)をコピーします。</summary>
        /// <param name="m">コピー元の行列</param>
        public void CopyRotationFrom(Matrix4f m)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    this[i, j] = m[i, j];
                }
            }
        }


    }

}
