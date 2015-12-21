using System;

namespace Baku.Quma.Low
{
    /// <summary>
    /// <para>QumarionDeviceの計算で得た一般的なボーン構造に対し、</para> 
    /// <para>実機のボーン/センサ構成の事前知識を利用して各デバイスにアクセスする方法を提供します。</para> 
    /// </summary>
    public static class QumarionDeviceMap
    {
        /// <summary>
        /// デバイスから指定したボーンを取得します。
        /// </summary>
        /// <param name="device">対象デバイス</param>
        /// <param name="bone">取得対象ボーン</param>
        /// <returns>指定したボーン</returns>
        public static Bone GetBone(GeneralizedQumarion device, Bones bone)
        {
            switch(bone)
            {
                case Bones.Root:
                    return device.RootBone;

                case Bones.Waist_V:
                    return device.RootBone.ChildBones[0];
                case Bones.Waist_H:
                    return device.RootBone.ChildBones[1];
                case Bones.Chest:
                    return GetBone(device, Bones.Waist_H).ChildBones[0];
                case Bones.Neck:
                    return GetBone(device, Bones.Chest).ChildBones[0];
                case Bones.Head:
                    return GetBone(device, Bones.Neck).ChildBones[0];

                case Bones.L_Thigh:
                    return GetBone(device, Bones.Waist_V).ChildBones[0];
                case Bones.L_Leg:
                    return GetBone(device, Bones.L_Thigh).ChildBones[0];
                case Bones.L_Foot:
                    return GetBone(device, Bones.L_Leg).ChildBones[0];
                case Bones.L_Toe:
                    return GetBone(device, Bones.L_Foot).ChildBones[0];

                case Bones.R_Thigh:
                    return GetBone(device, Bones.Waist_V).ChildBones[1];
                case Bones.R_Leg:
                    return GetBone(device, Bones.R_Thigh).ChildBones[0];
                case Bones.R_Foot:
                    return GetBone(device, Bones.R_Leg).ChildBones[0];
                case Bones.R_Toe:
                    return GetBone(device, Bones.R_Foot).ChildBones[0];

                case Bones.L_Shoulder:
                    return GetBone(device, Bones.Chest).ChildBones[1];
                case Bones.L_Elbow:
                    return GetBone(device, Bones.L_Shoulder).ChildBones[0];
                case Bones.L_Hand:
                    return GetBone(device, Bones.L_Elbow).ChildBones[0];

                case Bones.R_Shoulder:
                    return GetBone(device, Bones.Chest).ChildBones[2];
                case Bones.R_Elbow:
                    return GetBone(device, Bones.R_Shoulder).ChildBones[0];
                case Bones.R_Hand:
                    return GetBone(device, Bones.R_Elbow).ChildBones[0];

                default:
                    throw new InvalidOperationException("Unknown bone parameter was given");
            }
        }

        /// <summary>
        /// デバイスから指定したセンサーを取得します。
        /// </summary>
        /// <param name="device">対象デバイス</param>
        /// <param name="sensor">対象センサー</param>
        /// <returns>指定したセンサー</returns>
        public static Sensor GetSensor(GeneralizedQumarion device, Sensors sensor)
        {
            switch(sensor)
            {
                case Sensors.WaistV_MY:
                    return GetBone(device, Bones.Waist_V).Sensors[0];
                case Sensors.WaistV_MZ:
                    return GetBone(device, Bones.Waist_V).Sensors[1];

                case Sensors.L_Thigh_X:
                    return GetBone(device, Bones.L_Thigh).Sensors[0];
                case Sensors.L_Thigh_MZ:
                    return GetBone(device, Bones.L_Thigh).Sensors[1];
                case Sensors.L_Thigh_MY:
                    return GetBone(device, Bones.L_Thigh).Sensors[2];
                case Sensors.L_Leg_MX:
                    return GetBone(device, Bones.L_Leg).Sensors[0];
                case Sensors.L_Foot_MZ:
                    return GetBone(device, Bones.L_Foot).Sensors[0];
                case Sensors.L_Foot_X:
                    return GetBone(device, Bones.L_Foot).Sensors[1];
                case Sensors.R_Thigh_MX:
                    return GetBone(device, Bones.R_Thigh).Sensors[0];
                case Sensors.R_Thigh_MZ:
                    return GetBone(device, Bones.R_Thigh).Sensors[1];
                case Sensors.R_Thigh_MY:
                    return GetBone(device, Bones.R_Thigh).Sensors[2];
                case Sensors.R_Leg_X:
                    return GetBone(device, Bones.R_Leg).Sensors[0];
                case Sensors.R_Foot_MZ:
                    return GetBone(device, Bones.R_Foot).Sensors[0];
                case Sensors.R_Foot_MX:
                    return GetBone(device, Bones.R_Foot).Sensors[1];

                case Sensors.WaistH_MX:
                    return GetBone(device, Bones.Waist_H).Sensors[0];
                case Sensors.WaistH_Z:
                    return GetBone(device, Bones.Waist_H).Sensors[1];
                case Sensors.Neck_MZ:
                    return GetBone(device, Bones.Neck).Sensors[0];
                case Sensors.Neck_MX:
                    return GetBone(device, Bones.Neck).Sensors[1];
                case Sensors.Head_X:
                    return GetBone(device, Bones.Head).Sensors[0];
                case Sensors.Head_Y:
                    return GetBone(device, Bones.Head).Sensors[1];

                case Sensors.L_Shoulder_X1:
                    return GetBone(device, Bones.L_Shoulder).Sensors[0];
                case Sensors.L_Shoulder_MZ:
                    return GetBone(device, Bones.L_Shoulder).Sensors[1];
                case Sensors.L_Shoulder_X2:
                    return GetBone(device, Bones.L_Shoulder).Sensors[2];
                case Sensors.L_Elbow_MY:
                    return GetBone(device, Bones.L_Elbow).Sensors[0];
                case Sensors.L_Hand_X:
                    return GetBone(device, Bones.L_Hand).Sensors[0];
                case Sensors.L_Hand_MZ:
                    return GetBone(device, Bones.L_Hand).Sensors[1];

                case Sensors.R_Shoulder_MX1:
                    return GetBone(device, Bones.R_Shoulder).Sensors[0];
                case Sensors.R_Shoulder_MZ:
                    return GetBone(device, Bones.R_Shoulder).Sensors[1];
                case Sensors.R_Shoulder_MX2:
                    return GetBone(device, Bones.R_Shoulder).Sensors[2];
                case Sensors.R_Elbow_MY:
                    return GetBone(device, Bones.R_Elbow).Sensors[0];
                case Sensors.R_Hand_MX:
                    return GetBone(device, Bones.R_Hand).Sensors[0];
                case Sensors.R_Hand_MZ:
                    return GetBone(device, Bones.R_Hand).Sensors[1];


                default:
                    throw new InvalidOperationException("unknown sensor was given");
            }
        }
    }
}
