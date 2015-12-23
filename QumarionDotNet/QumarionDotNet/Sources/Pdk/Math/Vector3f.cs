
namespace Baku.Quma.Pdk
{
    /// <summary>3次元ベクトルを表します。</summary>
    public struct Vector3f
    {
        public Vector3f(float x, float y, float z) : this()
        {
            X = x;
            Y = y;
            Z = z;     
        }

        public float X;
        public float Y;
        public float Z;

        /// <summary>別のベクトルから値をコピーします。</summary>
        /// <param name="v">コピー元のベクトル</param>
        public void CopyFrom(Vector3f v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }
    }
}
