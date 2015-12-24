using System.Linq;

using Baku.Quma.Pdk.Api;

namespace Baku.Quma.Pdk
{
    public sealed class StandardCharacterModel : CharacterModel
    {
        private StandardCharacterModel(ModelHandle modelHandle, Bone root)
            : base(modelHandle, root)
        {
            //木構造状になってるボーン構成をディクショナリに整形
            Bones = new ReadOnlyDictionary<StandardPSBones, Bone>(
                root.GetChildBonesRecursive().ToDictionary(
                    b => StandardPSBonesUtil.GetStandardPSBone(b.Name),
                    b => b
                    )
                );
        }

        /// <summary>標準ボーンの一覧を取得します</summary>
        public ReadOnlyDictionary<StandardPSBones, Bone> Bones { get; }

        /// <summary>あらかじめ定義されている標準人型ボーンをロードします。</summary>
        /// <returns>ライブラリで定義された標準人型ボーン</returns>
        internal static StandardCharacterModel CreateStandardModelPS()
        {
            var imh = QmPdk.Character.CreateStandardModelPS();
            var rootBone = Bone.LoadCharacterStandardModelPSRootBone(imh.ModelHandle, imh.Indexes);
            return new StandardCharacterModel(imh.ModelHandle, rootBone);
        }
    }
}
