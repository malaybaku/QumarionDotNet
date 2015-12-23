using System.Linq;
using System.Collections.Generic;

using Baku.Quma.Pdk.Api;

namespace Baku.Quma.Pdk
{

    /// <summary>キャラクタのボーンを表します。</summary>
    public class Bone
    {
        /// <summary>
        /// <see cref="CharacterModel"/>用のコンストラクタ</summary>
        /// <param name="modelHandle">モデル</param>
        /// <param name="index">ボーンのインデックス</param>
        private Bone(ModelHandle modelHandle, int index)
        {
            ModelHandle = modelHandle;
            Index = index;
            var nodeNames = QmPdk.Character.GetName(modelHandle, index);
            Name = nodeNames.Name;
            OriginalName = nodeNames.OriginalName;
            LocalMatrix = Matrix4f.Create();
        }

        /// <summary><see cref="TemplateModel"/>用のコンストラクタ</summary>
        /// <param name="modelHandle">モデル</param>
        /// <param name="index">ボーンのインデックス</param>
        /// <param name="parent">親ボーン(ルートボーンを初期化する場合はnullにしておく)</param>
        private Bone(ModelHandle modelHandle, int index, Bone parent)
        {
            ModelHandle = modelHandle;
            Index = index;
            Name = QmPdk.TemplateBone.GetName(modelHandle, index);
            OriginalName = Name;
            LocalMatrix = Matrix4f.Create();

            Parent = parent;
            Root = (parent != null) ? parent.Root : this;
        }

        /// <summary>このボーンが含まれるキャラクタボーンのモデルのハンドルを取得します。</summary>
        public ModelHandle ModelHandle { get; }

        /// <summary>このボーンを表す、同一キャラクタボーン中では重複しない識別番号を取得します。</summary>
        public int Index { get; }

        /// <summary>ボーンの名前を取得します。</summary>
        public string Name { get; }

        /// <summary>
        /// ボーンの名前のうちユーザが指定したものを取得します。この名前はライブラリの内部処理では用いられていません。
        /// <see cref="TemplateModel"/>のボーンではNameと同じ値で初期化されます。
        /// </summary>
        public string OriginalName { get; }

        /// <summary>このボーンが含まれるボーン全体のルートボーンを取得します。</summary>
        public Bone Root { get; private set; }

        /// <summary>親ボーンを取得します。ルートボーンの親はnullです。</summary>
        public Bone Parent { get; private set; }

        /// <summary>子ボーンの一覧を取得します。子ボーンが存在しない場合は配列長0の配列を取得します。</summary>
        public Bone[] Childs { get; private set; }

        //NOTE: 行列は再代入できると参照云々とかまずそうなので要注意

        /// <summary>初期状態(Tポーズ)での回転を取得します。</summary>
        public Matrix4f LocalMatrix { get; }

        /// <summary>
        /// ボーンの回転つまり可動部の動きを取得します。
        /// 取得のたびにAPIアクセスを行うため、必要以上に頻繁に呼ばないよう注意してください。
        /// </summary>
        public Matrix4f RotationMatrix => QmPdk.Character.GetLocalMatrix(ModelHandle, Index);

        /// <summary>このボーン、およびボーンの子孫ボーンを再帰的にすべて取得します。</summary>
        /// <returns>子孫ボーンの一覧</returns>
        public IEnumerable<Bone> GetChildBonesRecursive()
        {
            return new Bone[] { this }.Concat(
                Childs.SelectMany(child => child.GetChildBonesRecursive())
                );
        }


        /// <summary>
        /// モデル、インデックス、親ボーンを指定してテンプレートボーンを生成します。
        /// </summary>
        /// <param name="modelHandle"></param>
        /// <param name="index"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private static Bone LoadTemplateBone(ModelHandle modelHandle, int index, Bone parent)
        {
            var result = new Bone(modelHandle, index, parent);

            result.Childs = QmPdk.TemplateBone.GetChildNodeIndex(modelHandle, index)
                .Select(childIndex => LoadTemplateBone(modelHandle, childIndex, parent))
                .ToArray();

            return result;
        }

        /// <summary>テンプレートのルートボーンを呼び出し、その際に全身のボーンもすべてロードします。</summary>
        /// <param name="modelHandle">取得元のモデル</param>
        /// <returns>子ボーンまで初期化された状態のルートボーン</returns>
        public static Bone LoadTemplateRootBone(ModelHandle modelHandle)
        {
            int rootIndex = QmPdk.TemplateBone.GetRootNodeIndex(modelHandle);
            return LoadTemplateBone(modelHandle, rootIndex, null);
        }

        /// <summary>標準ボーンをボーンの階層構造として生成し、ルートボーンを返します。</summary>
        /// <param name="modelHandle">初期化対象のモデル</param>
        /// <param name="parentIndexes">親ボーンインデックスの一覧</param>
        /// <returns></returns>
        public static Bone LoadCharacterStandardModelPSRootBone(ModelHandle modelHandle, int[] parentIndexes)
        {
            var bones = new Bone[parentIndexes.Length];

            //各ボーンの個別初期化
            for (int i = 0; i < parentIndexes.Length; i++)
            {
                bones[i] = new Bone(modelHandle, i);
                bones[i].LocalMatrix.CopyFrom(QmPdk.Character.GetLocalMatrix(modelHandle, i));
            }

            //ヒューリスティックとしてbones[0]がStandardModelPSではHipsなハズだけど一応一般性を高くしておく
            var rootBone = bones.First(b => b.Name == StandardPSBonesUtil.GetName(StandardPSBones.Hips));

            //各ボーンの子ボーン計算のためにバッファを用意
            var childsDic = new List<int>[parentIndexes.Length];
            for(int i = 0; i < parentIndexes.Length; i++)
            {
                childsDic[i] = new List<int>();
            }

            for (int i = 0; i < parentIndexes.Length; i++)
            {
                //ルートはみんな一緒
                bones[i].Root = rootBone;

                //NOTE: レンジチェックを行ってるのはHipsの親要素がインデックス-1になってるため            
                if(parentIndexes[i] >= 0 && parentIndexes[i] < parentIndexes.Length)
                {
                    //親子関係をメモ
                    childsDic[parentIndexes[i]].Add(i);
                    bones[i].Parent = bones[parentIndexes[i]];
                }
                else
                {
                    bones[i].Parent = null;
                }
            }

            //一周繰り返しして得られたデータを基に子ボーン一覧を指定
            for (int i = 0; i < parentIndexes.Length; i++)
            {
                bones[i].Childs = childsDic[i]
                    .Select(j => bones[j])
                    .ToArray();
            }

            return rootBone;
        }

    }
}
