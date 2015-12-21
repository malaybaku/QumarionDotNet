
namespace Baku.Quma.Low
{
    /// <summary>
    /// 実機のQumarionで確認したセンサーの一覧です。
    /// X,Y,Zは左、上、後方向きの回転軸を指し、MX, MY, MZはそれぞれ-X,-Y,-Z方向の回転軸を表します。
    /// </summary>
    public enum Sensors
    {
        /// <summary>腰下側のひねり(Y軸)</summary>
        WaistV_MY,
        /// <summary>腰下側のひねり(Z軸)</summary>
        WaistV_MZ,

        /// <summary>左脚付け根の前後</summary>
        L_Thigh_X,
        /// <summary>左脚付け根の開脚</summary>
        L_Thigh_MZ,
        /// <summary>左脚付け根のひねり</summary>
        L_Thigh_MY,
        /// <summary>左ひざの曲げ</summary>
        L_Leg_MX,
        /// <summary>左足首のひねり</summary>
        L_Foot_MZ,
        /// <summary>左足首の曲げ</summary>
        L_Foot_X,

        /// <summary>右脚付け根の前後</summary>
        R_Thigh_MX,
        /// <summary>右脚付け根の開脚</summary>
        R_Thigh_MZ,
        /// <summary>右脚付け根のひねり</summary>
        R_Thigh_MY,
        /// <summary>右ひざの曲げ</summary>
        R_Leg_X,
        /// <summary>右足首のひねり</summary>
        R_Foot_MZ,
        /// <summary>右足首の曲げ</summary>
        R_Foot_MX,

        /// <summary>腰上側の前後(X軸)</summary>
        WaistH_MX,
        /// <summary>腰上側のひねり(Z軸)</summary>
        WaistH_Z,
        /// <summary>首ひねり(Z軸)</summary>
        Neck_MZ,
        /// <summary>首ひねり(X軸)</summary>
        Neck_MX,
        /// <summary>頭のひねり</summary>
        Head_X,
        /// <summary>頭の左右</summary>
        Head_Y,

        /// <summary>左腕全体の上下(NOTE:ここは実際の軸は(0.99,0.1,0)程度で少し上を向いている)</summary>
        L_Shoulder_X1,
        /// <summary>左腕のワキの開き</summary>
        L_Shoulder_MZ,
        /// <summary>左腕のひねり</summary>
        L_Shoulder_X2,

        /// <summary>左ひじの曲げ</summary>
        L_Elbow_MY,
        /// <summary>左手のひねり</summary>
        L_Hand_X,
        /// <summary>左手の前後</summary>
        L_Hand_MZ,

        /// <summary>右腕全体の上下(NOTE:ここは実際の軸は(-0.99,0.1,0)程度で少し上を向いている)</summary>
        R_Shoulder_MX1,
        /// <summary>右腕のワキの開き</summary>
        R_Shoulder_MZ,
        /// <summary>右腕のひねり</summary>
        R_Shoulder_MX2,

        /// <summary>右ひじの曲げ</summary>
        R_Elbow_MY,
        /// <summary>左手のひねり</summary>
        R_Hand_MX,
        /// <summary>左手の前後</summary>
        R_Hand_MZ

    }
}
