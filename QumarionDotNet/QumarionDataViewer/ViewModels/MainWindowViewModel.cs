using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using System.Windows.Threading;

using Baku.Quma;
using System.Text;
using System.IO;
using System.Windows;
using System.ComponentModel;

namespace QumarionDataViewer
{
    /// <summary>ウィンドウのビューモデルを表します。</summary>
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        /// <summary>QUMARIONから姿勢/加速度のデータ拾ってくる周期</summary>
        public readonly double TimeSpanAngleSec = 0.1;

        /// <summary>QUMARIONからボタンのデータ拾う周期</summary>
        public readonly double TimeSpanButtonSec = 0.02;

        /// <summary>UDP送信の最大速度</summary>
        public readonly double TimeSpanUdpSec = 0.025f;


        public MainWindowViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }

            _timerUpdateAngle = Observable
                .Timer(TimeSpan.FromSeconds(TimeSpanAngleSec), TimeSpan.FromSeconds(TimeSpanAngleSec))
                .Where(_ => _device != null && !_updateAngleBusy)
                .Subscribe(OnTimerAngle);

            _timerUpdateButton = Observable
                .Timer(TimeSpan.FromSeconds(TimeSpanButtonSec), TimeSpan.FromSeconds(TimeSpanButtonSec))
                .Where(_ => _device != null && !_updateButtonBusy)
                .Subscribe(OnTimerButton);

            _timerUdp = Observable
                .Timer(TimeSpan.FromSeconds(TimeSpanUdpSec), TimeSpan.FromSeconds(TimeSpanUdpSec))
                .Where(_ => _device != null && !_udpBusy)
                .Where(_ => 
                    (_udpWhenToSend == UdpWhenToSend.Always) || 
                    (_udpWhenToSend == UdpWhenToSend.DuringButtonDown && ButtonPressed)
                    )
                .Subscribe(OnTimerUdp);

            if (!File.Exists(Baku.Quma.Low.QmLow.DllName))
            {
                MessageBox.Show(
                    $"Error: Qumarion SDKのDLLファイル({Baku.Quma.Low.QmLow.DllName})が見つかりませんでした。アプリケーションを終了します。",
                    "QumarionDataViewer - エラー",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
                Application.Current.Shutdown(1);
                return;
            }

            QumarionManager.Initialize();

            foreach (var qumaId in QumarionManager.GetQumaIds())
            {
                var deviceLoaderViewModel = new DeviceLoaderViewModel(qumaId);
                deviceLoaderViewModel.DeviceLoaded += OnDeviceLoaded;
                DeviceLoaders.Add(deviceLoaderViewModel);
            }
        }

        private void OnDeviceLoaded(object sender, QumarionDeviceLoadedEventArgs e)
        {
            _device = QumarionManager.GetDefaultDevice();
            Sensors.Clear();
            var sensors = _device
                .Sensors
                .Select(s => new SensorTimeSeriesViewModel(s.Value, s.Key));

            foreach (var sensor in sensors)
            {
                Sensors.Add(sensor);
                //各センサを将来的にグラフに出すかもしれない
                sensor.ShowChartRequested += OnShowChartRequested;
                sensor.HideChartRequested += OnHideChartRequested;
            }
        }

        private void OnShowChartRequested(object sender, EventArgs e)
        {
            SensorTimeSeriesViewModel vm = sender as SensorTimeSeriesViewModel;
            if (vm == null) return;

            if(!SensorsShownByChart.Contains(vm))
            {
                SensorsShownByChart.Add(vm);
            }            
        }

        private void OnHideChartRequested(object sender, EventArgs e)
        {
            SensorTimeSeriesViewModel vm = sender as SensorTimeSeriesViewModel;
            if (vm == null) return;

            int index = SensorsShownByChart.IndexOf(vm);
            if(index >= 0 && index < SensorsShownByChart.Count)
            {
                SensorsShownByChart.RemoveAt(index);
            }
        }

        private void OnTimerAngle(long n)
        {
            _updateAngleBusy = true;

            var time = DateTime.Now;
            if (_device.TryUpdateSensors() == QumaLowResponse.OK)
            {
                DispatcherHelper.UIDispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() =>
                    {
                        foreach (var sensor in Sensors)
                        {
                            sensor.Update(time);
                        }
                        _updateAngleBusy = false;
                    })
                );
            }
            else
            {
                _updateAngleBusy = false;
            }
        }
        private bool _updateAngleBusy = false;

        private void OnTimerButton(long n)
        {
            _updateButtonBusy = true;
            if (_device.TryUpdateButton() == QumaLowResponse.OK)
            {
                ButtonPressed = (_device.ButtonState == ButtonState.Down);
            }
            _updateButtonBusy = false;
        }
        private bool _updateButtonBusy = false;

        private void OnTimerUdp(long n)
        {
            _udpBusy = true;
            SendUdpData();
            _udpBusy = false;
        }
        private bool _udpBusy = false;


        public ObservableCollection<SensorTimeSeriesViewModel> Sensors { get; } 
            = new ObservableCollection<SensorTimeSeriesViewModel>();

        public ObservableCollection<SensorTimeSeriesViewModel> SensorsShownByChart { get; }
            = new ObservableCollection<SensorTimeSeriesViewModel>();

        public ObservableCollection<DeviceLoaderViewModel> DeviceLoaders { get; }
            = new ObservableCollection<DeviceLoaderViewModel>();

        /// <summary>UDPの送信機を取得します。</summary>
        public UdpSenderViewModel UdpSender { get; } = new UdpSenderViewModel();

        private bool _windowClosed;
        public bool WindowClosed
        {
            get { return _windowClosed; }
            set { SetAndRaisePropertyChanged(ref _windowClosed, value); }
        }

        private bool _buttonPressed;
        public bool ButtonPressed
        {
            get { return _buttonPressed; }
            set
            {
                if(!ButtonPressed && value && _udpWhenToSend == UdpWhenToSend.WhenButtonDown)
                {
                    SendUdpData();
                }

                SetAndRaisePropertyChanged(ref _buttonPressed, value);
            }
        }

        #region UDPの追加設定

        //送信のタイミング
        private UdpWhenToSend _udpWhenToSend = UdpWhenToSend.None;
        public bool UdpNotSend
        {
            set
            {
                if (value)
                {
                    _udpWhenToSend = UdpWhenToSend.None;
                }
            }
        }
        public bool UdpSendWhenButtonDown
        {
            set
            {
                if (value)
                {
                    _udpWhenToSend = UdpWhenToSend.WhenButtonDown;
                }
            }
        }
        public bool UdpSendDuringButtonDown
        {
            set
            {
                if (value)
                {
                    _udpWhenToSend = UdpWhenToSend.DuringButtonDown;
                }
            }
        }
        public bool UdpSendAlways
        {
            set
            {
                if (value)
                {
                    _udpWhenToSend = UdpWhenToSend.Always;
                }
            }
        }

        //送信する中身
        private UdpContent _udpContentType = UdpContent.ArrayBinary;
        public bool UdpUseArrayBinary
        {
            set
            {
                if(value)
                {
                    _udpContentType = UdpContent.ArrayBinary;
                }
            }
        }
        public bool UdpUseArrayString
        {
            set
            {
                if (value)
                {
                    _udpContentType = UdpContent.ArrayString;
                }
            }
        }
        public bool UdpUseSensorNames
        {
            set
            {
                if (value)
                {
                    _udpContentType = UdpContent.SensorNames;
                }
            }
        }
        public bool UdpUseJsonLikeString
        {
            set
            {
                if (value)
                {
                    _udpContentType = UdpContent.JsonLikeString;
                }
            }
        }

        #endregion

        #region コマンド

        private ICommand _disconnectCommand;
        public ICommand DisconnectCommand
            => _disconnectCommand ?? (_disconnectCommand = new ActionCommand(Disconnect));

        /// <summary>Qumarionとの接続を解除します。</summary>
        private void Disconnect()
        {
            _device = null;
            Sensors.Clear();
            SensorsShownByChart.Clear();
            ButtonPressed = false;
        }

        private ICommand _closeCommand;
        public ICommand CloseCommand
            => _closeCommand ?? (_closeCommand = new ActionCommand(Close));

        private void Close()
        {
            WindowClosed = true;
        }

        private ICommand _closingCommand;
        public ICommand ClosingCommand
            => _closingCommand ?? (_closingCommand = new ActionCommand(OnClosing));

        private void OnClosing()
        {
            Dispose();
        }

        #endregion

        private Qumarion _device;
        private readonly IDisposable _timerUpdateAngle;
        private readonly IDisposable _timerUpdateButton;
        private readonly IDisposable _timerUdp;

        public void Dispose()
        {
            _timerUpdateAngle.Dispose();
            _timerUpdateButton.Dispose();
            _timerUdp.Dispose();
            QumarionManager.Exit();          
        }


        private readonly Encoding _udpTextEncoding = Encoding.UTF8;
        private void SendUdpData()
        {
            if (Sensors.Count == 0) return;

            switch(_udpContentType)
            {
                case UdpContent.ArrayBinary:
                    UdpSender.SendArrayDataBinary(Sensors.Select(s => s.LatestValue).ToArray());
                    return;
                case UdpContent.ArrayString:
                    UdpSender.SendArrayDataString(Sensors.Select(s => s.LatestValue).ToArray(), _udpTextEncoding);
                    return;
                case UdpContent.SensorNames:
                    UdpSender.SendSensorNames(Sensors.Select(s => s.SensorName).ToArray(), _udpTextEncoding);
                    return;
                case UdpContent.JsonLikeString:
                    UdpSender.SendJsonLikeString(
                        Sensors.ToDictionary(s => s.SensorName, s => s.LatestValue),
                        _udpTextEncoding
                        );
                    return;
                default:
                    return;
            }
        }

        enum UdpWhenToSend
        {
            None,
            WhenButtonDown,
            DuringButtonDown,
            Always
        }
        enum UdpContent
        {
            ArrayBinary,
            ArrayString,
            SensorNames,
            JsonLikeString
        }

    }
}
