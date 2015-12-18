using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace QumarionDataViewer
{
    /// <summary>UDP送信器を表します。VM/M兼任型の実装になっています。</summary>
    public class UdpSenderViewModel : ViewModelBase
    {
        private string _targetIP = "127.0.0.1";
        /// <summary>
        /// 送信先のIPアドレスを取得、設定します。IPアドレスとして使用不可能な文字列は設定しても反映されません。
        /// </summary>
        public string TargetIP
        {
            get { return _targetIP; }
            set
            {
                if (_targetIP == value) return;

                IPAddress address;
                if(IPAddress.TryParse(value, out address))
                {
                    _targetIP = value;
                    RaisePropertyChanged();
                }
            }
        }

        private ushort _targetPort = 13000;
        /// <summary>送信先のポート番号</summary>
        public ushort TargetPort
        {
            get { return _targetPort; }
            set { SetAndRaisePropertyChanged(ref _targetPort, value); }
        }

        private UdpClient _udpClient = new UdpClient();


        /// <summary>角度情報をバイナリ化して送信します。</summary>
        /// <param name="angles">送信したい角度値の一覧</param>
        public void SendArrayDataBinary(float[] angles)
        {
            int size = Marshal.SizeOf(typeof(float));
            var data = new byte[angles.Length * size];
            for (int i = 0; i < angles.Length; i++)
            {
                byte[] f = BitConverter.GetBytes(angles[i]);
                Array.Copy(f, 0, data, i * size, size);
            }

            Send(data);
        }

        /// <summary>角度情報をカンマ区切り文字列として送信します。<see cref="SendArrayDataBinary(float[])"/>よりデータサイズが増えることに注意してください。</summary>
        /// <param name="angles">送信したい角度値の一覧</param>
        public void SendArrayDataString(float[] angles, Encoding encoding)
            => Send(encoding.GetBytes(string.Join(",", angles)));

        /// <summary>
        /// <para>センサー名の一覧をカンマ区切り文字列として送信します。</para>
        /// <para><see cref="SendArrayDataBinary(float[])"/>や<see cref="SendArrayDataString(float[], Encoding)"/>の送信データと</para>
        /// <para>一貫性を持たせるように用いてください。</para>
        /// </summary>
        /// <param name="names">センサー名の一覧</param>
        public void SendSensorNames(string[] names, Encoding encoding)
            => Send(encoding.GetBytes(string.Join(",", names)));

        /// <summary>
        /// <para>センサ名と角度値をJSONオブジェクトライクな文字列({ "name1":0.0, "name2":0.2, .. })として送信します。</para>
        /// <para>この送信は辞書型のデータ構造を用いるため、<see cref="SendSensorNames(string[], Encoding)"/>とセンサ名の順序が同一であることを保障しません。</para>
        /// </summary>
        /// <param name="data">送信したいデータ</param>
        /// <param name="encoding">送信時の文字列のエンコード</param>
        public void SendJsonLikeString(IReadOnlyDictionary<string, float> data, Encoding encoding)
            => Send(encoding.GetBytes("{" + string.Join(",", data.Select(kvp => $"\"{kvp.Key}\":{kvp.Value}")) + "}"));

        private void Send(byte[] msg) 
            => _udpClient.Send(msg, msg.Length, new IPEndPoint(IPAddress.Parse(TargetIP), TargetPort));

    }

}

