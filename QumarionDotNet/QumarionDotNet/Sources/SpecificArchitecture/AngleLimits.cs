using System.Collections.Generic;

namespace Baku.Quma
{

    /// <summary>
    /// 角度の上限と下限を定義します。
    /// (NOTE: ここで定義する限界値は「コレを超える可能性はほぼない」という目安の値であって正確な上下限値ではない)
    /// </summary>
    public static class AngleLimits
    {
        /// <summary>センサの種類を指定して角度の下限値を取得します。</summary>
        /// <param name="sensor">対象のセンサ</param>
        /// <returns>そのセンサが示す角度値の下限</returns>
        public static float GetLowerLimit(Sensors sensor)
        {
            return LowerLimits[sensor];
        }

        /// <summary>センサの種類を指定して角度の上限値を取得します。</summary>
        /// <param name="sensor">対象のセンサ</param>
        /// <returns>そのセンサが示す角度値の上限</returns>
        public static float GetUpperLimit(Sensors sensor)
        {
            return UpperLimits[sensor];
        }

        #region 下限の一覧
        private static IReadOnlyDictionary<Sensors, float> LowerLimits =
            new Dictionary<Sensors, float>()
            {
                { Sensors.WaistV_MY, -45.0f },
                { Sensors.WaistV_MZ, -45.0f },

                { Sensors.L_Thigh_X, -60.0f },
                { Sensors.L_Thigh_MZ, -135.0f },
                { Sensors.L_Thigh_MY, -90.0f },
                { Sensors.L_Leg_MX, -30.0f },
                { Sensors.L_Foot_MZ, -90.0f },
                { Sensors.L_Foot_X, -60.0f },

                { Sensors.R_Thigh_MX, -135.0f },
                { Sensors.R_Thigh_MZ, -30.0f },
                { Sensors.R_Thigh_MY, -90.0f },
                { Sensors.R_Leg_X, -150.0f },
                { Sensors.R_Foot_MZ, -45.0f },
                { Sensors.R_Foot_MX, -90.0f },

                { Sensors.WaistH_MX, -45.0f },
                { Sensors.WaistH_Z, -90.0f },

                { Sensors.Neck_MZ, -45.0f },
                { Sensors.Neck_MX, -45.0f },
                { Sensors.Head_X, -45.0f },
                { Sensors.Head_Y, -180.0f },

                { Sensors.L_Shoulder_X1, -60.0f },
                { Sensors.L_Shoulder_MZ, -60.0f },
                { Sensors.L_Shoulder_X2, -135.0f },
                { Sensors.L_Elbow_MY, -150.0f },
                { Sensors.L_Hand_X, -150.0f },
                { Sensors.L_Hand_MZ, -90.0f },

                { Sensors.R_Shoulder_MX1, -225.0f },
                { Sensors.R_Shoulder_MZ, -135.0f },
                { Sensors.R_Shoulder_MX2, -135.0f },
                { Sensors.R_Elbow_MY, -30.0f },
                { Sensors.R_Hand_MX, -150.0f },
                { Sensors.R_Hand_MZ, -90.0f }
            };
        #endregion

        #region 下限の一覧
        private static IReadOnlyDictionary<Sensors, float> UpperLimits =
            new Dictionary<Sensors, float>()
            {
                { Sensors.WaistV_MY, 45.0f },
                { Sensors.WaistV_MZ, 45.0f },

                { Sensors.L_Thigh_X, 135.0f },
                { Sensors.L_Thigh_MZ, 30.0f },
                { Sensors.L_Thigh_MY, 90.0f },
                { Sensors.L_Leg_MX, 150.0f },
                { Sensors.L_Foot_MZ, 45.0f },
                { Sensors.L_Foot_X, 90.0f },

                { Sensors.R_Thigh_MX, 60.0f },
                { Sensors.R_Thigh_MZ, 135.0f },
                { Sensors.R_Thigh_MY, 90.0f },
                { Sensors.R_Leg_X, 30.0f },
                { Sensors.R_Foot_MZ, 90.0f },
                { Sensors.R_Foot_MX, 60.0f },

                { Sensors.WaistH_MX, 45.0f },
                { Sensors.WaistH_Z, 30.0f },

                { Sensors.Neck_MZ, 45.0f },
                { Sensors.Neck_MX, 0.0f },
                { Sensors.Head_X, 45.0f },
                { Sensors.Head_Y, 180.0f },

                { Sensors.L_Shoulder_X1, 225.0f },
                { Sensors.L_Shoulder_MZ, 135.0f },
                { Sensors.L_Shoulder_X2, -135.0f },
                { Sensors.L_Elbow_MY, 30.0f },
                { Sensors.L_Hand_X, -150.0f },
                { Sensors.L_Hand_MZ, -90.0f },

                { Sensors.R_Shoulder_MX1, 60.0f },
                { Sensors.R_Shoulder_MZ, 60.0f },
                { Sensors.R_Shoulder_MX2, 135.0f },
                { Sensors.R_Elbow_MY, 150.0f },
                { Sensors.R_Hand_MX, 150.0f },
                { Sensors.R_Hand_MZ, 90.0f }
            };
        #endregion

    }
}
