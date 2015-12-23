
namespace Baku.Quma.Pdk
{
    /// <summary>ライブラリ中で定義されているTemplateボーンモデルを表します。</summary>
    public class TemplateModel
    {
        private TemplateModel(ModelHandle modelHandle)
        {
            ModelHandle = modelHandle;
            RootBone = Bone.LoadTemplateRootBone(modelHandle);
        }

        /// <summary>テンプレートの作成元になっているモデルのハンドルを取得します。</summary>
        public ModelHandle ModelHandle { get; }

        /// <summary>テンプレートボーンのルートを取得します。他の全てのボーンはここから再帰的に取得可能です。</summary>
        public Bone RootBone { get; }

        /// <summary>モデルのハンドルを指定して対応するテンプレートボーンを取得します。</summary>
        /// <param name="modelHandle">モデル</param>
        /// <returns>テンプレートモデル</returns>
        public static TemplateModel Load(ModelHandle modelHandle) => new TemplateModel(modelHandle);

    }
}
