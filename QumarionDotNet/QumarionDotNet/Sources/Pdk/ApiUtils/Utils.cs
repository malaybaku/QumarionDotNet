
namespace Baku.Quma.Pdk
{

    /// <summary>モデルのハンドルとボーンのインデックス対応がペアになったものを表します。</summary>
    public class IndexedModelHandle
    {
        internal IndexedModelHandle(ModelHandle modelHandle, int[] indexes)
        {
            ModelHandle = modelHandle;
            Indexes = indexes;
        }

        /// <summary>各ボーンインデックスに対し、そのボーンの親のインデックスを対応づけた配列を取得します。</summary>
        public int[] Indexes { get; }

        /// <summary>取得されたモデルを表します。</summary>
        public ModelHandle ModelHandle { get; }
    }

    /// <summary>ボーンのライブラリ内使用名とユーザ定義名がペアになったものを表します。</summary>
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

    /// <summary>Qumarionデバイスのセンサー状態に関する情報を表します。</summary>
    public class SensorsState
    {
        internal SensorsState(bool isOk, string message)
        {
            IsOk = isOk;
            Message = message;
        }

        /// <summary>異常なセンサーが無いかどうかを表します。</summary>
        public bool IsOk { get; }

        /// <summary>異常にかんする情報を取得します。異常なセンサーが存在しない場合は空文字列です。</summary>
        public string Message { get; }
    }

}
