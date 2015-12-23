using System.Linq;
using System.Collections.Generic;

namespace Baku.Quma.Pdk
{
    /// <summary>標準PSボーンと文字列の変換に関するユーティリティ関数を定義します。</summary>
    public static class StandardPSBonesUtil
    {
        static StandardPSBonesUtil()
        {
            _boneEnums = new ReadOnlyDictionary<string, StandardPSBones>(
                _boneNames.ToDictionary(kvp => kvp.Value, kvp => kvp.Key)
                );
        }

        /// <summary>ボーンを指定して名前を取得します。</summary>
        /// <param name="bone">ボーン</param>
        /// <returns>ボーンを文字列表現した場合の名前</returns>
        public static string GetName(StandardPSBones bone) => _boneNames[bone];

        /// <summary>標準PSボーンの名前を指定し、対応する列挙体番号を取得します。</summary>
        /// <param name="name">ボーン名。標準PSボーンに入っていない名前を指定した場合<see cref="KeyNotFoundException"/>がスローされます。</param>
        /// <returns>対応するボーンの列挙体番号</returns>
        /// <exception cref="KeyNotFoundException"/>
        public static StandardPSBones GetStandardPSBone(string name) => _boneEnums[name];

        private static ReadOnlyDictionary<StandardPSBones, string> _boneNames = new ReadOnlyDictionary<StandardPSBones, string>(
            new Dictionary<StandardPSBones, string>()
        {
            #region 列挙体と実際の名前対応の一覧
            { StandardPSBones.Hips, "hips_bb_" },
            { StandardPSBones.HipsEnd, "hips_end_bb_" },
            { StandardPSBones.Spine, "spine_bb_" },
            { StandardPSBones.Spine1, "spine1_bb_" },
            { StandardPSBones.Spine2, "spine2_bb_" },
            { StandardPSBones.Neck, "neck_bb_" },
            { StandardPSBones.Head, "head_bb_" },
            { StandardPSBones.HeadEnd, "head_end_bb_" },
            { StandardPSBones.LeftShoulder, "leftshoulder_bb_" },
            { StandardPSBones.LeftArm, "leftarm_bb_" },
            { StandardPSBones.LeftForeArm, "leftforearm_bb_" },
            { StandardPSBones.LeftHand, "lefthand_bb_" },
            { StandardPSBones.LeftHandMiddle1, "lefthandmiddle1_bb_" },
            { StandardPSBones.LeftHandMiddle2, "lefthandmiddle2_bb_" },
            { StandardPSBones.LeftHandMiddle3, "lefthandmiddle3_bb_" },
            { StandardPSBones.LeftHandMiddle4, "lefthandmiddle4_bb_" },
            { StandardPSBones.LeftHandIndex1, "lefthandindex1_bb_" },
            { StandardPSBones.LeftHandIndex2, "lefthandindex2_bb_" },
            { StandardPSBones.LeftHandIndex3, "lefthandindex3_bb_" },
            { StandardPSBones.LeftHandIndex4, "lefthandindex4_bb_" },
            { StandardPSBones.LeftHandRing1, "lefthandring1_bb_" },
            { StandardPSBones.LeftHandRing2, "lefthandring2_bb_" },
            { StandardPSBones.LeftHandRing3, "lefthandring3_bb_" },
            { StandardPSBones.LeftHandRing4, "lefthandring4_bb_" },
            { StandardPSBones.LeftHandPinky1, "lefthandpinky1_bb_" },
            { StandardPSBones.LeftHandPinky2, "lefthandpinky2_bb_" },
            { StandardPSBones.LeftHandPinky3, "lefthandpinky3_bb_" },
            { StandardPSBones.LeftHandPinky4, "lefthandpinky4_bb_" },
            { StandardPSBones.LeftHandThumb1, "lefthandthumb1_bb_" },
            { StandardPSBones.LeftHandThumb2, "lefthandthumb2_bb_" },
            { StandardPSBones.LeftHandThumb3, "lefthandthumb3_bb_" },
            { StandardPSBones.LeftHandThumb4, "lefthandthumb4_bb_" },
            { StandardPSBones.RightShoulder, "rightshoulder_bb_" },
            { StandardPSBones.RightArm, "rightarm_bb_" },
            { StandardPSBones.RightForeArm, "rightforearm_bb_" },
            { StandardPSBones.RightHand, "righthand_bb_" },
            { StandardPSBones.RightHandMiddle1, "righthandmiddle1_bb_" },
            { StandardPSBones.RightHandMiddle2, "righthandmiddle2_bb_" },
            { StandardPSBones.RightHandMiddle3, "righthandmiddle3_bb_" },
            { StandardPSBones.RightHandMiddle4, "righthandmiddle4_bb_" },
            { StandardPSBones.RightHandIndex1, "righthandindex1_bb_" },
            { StandardPSBones.RightHandIndex2, "righthandindex2_bb_" },
            { StandardPSBones.RightHandIndex3, "righthandindex3_bb_" },
            { StandardPSBones.RightHandIndex4, "righthandindex4_bb_" },
            { StandardPSBones.RightHandRing1, "righthandring1_bb_" },
            { StandardPSBones.RightHandRing2, "righthandring2_bb_" },
            { StandardPSBones.RightHandRing3, "righthandring3_bb_" },
            { StandardPSBones.RightHandRing4, "righthandring4_bb_" },
            { StandardPSBones.RightHandPinky1, "righthandpinky1_bb_" },
            { StandardPSBones.RightHandPinky2, "righthandpinky2_bb_" },
            { StandardPSBones.RightHandPinky3, "righthandpinky3_bb_" },
            { StandardPSBones.RightHandPinky4, "righthandpinky4_bb_" },
            { StandardPSBones.RightHandThumb1, "righthandthumb1_bb_" },
            { StandardPSBones.RightHandThumb2, "righthandthumb2_bb_" },
            { StandardPSBones.RightHandThumb3, "righthandthumb3_bb_" },
            { StandardPSBones.RightHandThumb4, "righthandthumb4_bb_" },
            { StandardPSBones.LeftUpLeg, "leftupleg_bb_" },
            { StandardPSBones.LeftLeg, "leftleg_bb_" },
            { StandardPSBones.LeftFoot, "leftfoot_bb_" },
            { StandardPSBones.LeftToeBase, "lefttoebase_bb_" },
            { StandardPSBones.LeftToeBaseEnd, "lefttoebase_end_bb_" },
            { StandardPSBones.RightUpLeg, "rightupleg_bb_" },
            { StandardPSBones.RightLeg, "rightleg_bb_" },
            { StandardPSBones.RightFoot, "rightfoot_bb_" },
            { StandardPSBones.RightToeBase, "righttoebase_bb_" },
            { StandardPSBones.RightToeBaseEnd, "righttoebase_end_bb_" }
            #endregion
        });

        private static ReadOnlyDictionary<string, StandardPSBones> _boneEnums;
    }

}
