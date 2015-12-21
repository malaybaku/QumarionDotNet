using System;
using System.Collections.ObjectModel;

using Baku.Quma.Low;
using System.Windows.Input;

namespace QumarionDataViewer
{
    /// <summary>センサの時系列データを受け取って可視化するためのクラスです。</summary>
    public class SensorTimeSeriesViewModel : ViewModelBase
    {
        public SensorTimeSeriesViewModel(Sensor sensorModel, Sensors sensorType)
        {
            _model = sensorModel;
            _sensorType = sensorType;
            SensorName = _sensorType.ToString();
            BonePosInfo = $"Bone Pos:({_model.Bone.Position.X:0.000}, {_model.Bone.Position.Y:0.000}, {_model.Bone.Position.Z:0.000})";
            AxisInfo = $"Axis:({_model.Axis.X:0.000}, {_model.Axis.Y:0.000}, {_model.Axis.Z:0.000})";

            SensorMinimumOutput = AngleLimits.GetLowerLimit(sensorType);
            SensorMaximumOutput = AngleLimits.GetUpperLimit(sensorType);
        }

        private readonly Sensor _model;
        private readonly Sensors _sensorType;

        /// <summary>時系列データを取得します。</summary>
        public ObservableCollection<SensorDataItemViewModel> TimeSeries { get; } = new ObservableCollection<SensorDataItemViewModel>();

        public void Update(DateTime time)
        {
            //NOTE: モデルのUpdateはメイン側VMの責任
            double angle = _model.Angle;
            TimeSeries.Add(new SensorDataItemViewModel(angle, time));
            LatestValue = (float)angle;

            while ((time - TimeSeries[0].OriginTime).TotalSeconds > OldDataLimitSec)
            {
                TimeSeries.RemoveAt(0);
            }
            foreach (var item in TimeSeries)
            {
                item.Update(time);
            }
        }

        private string _sensorName = "";
        public string SensorName
        {
            get { return _sensorName; }
            private set { SetAndRaisePropertyChanged(ref _sensorName, value); }
        }
        public string BonePosInfo { get; }
        public string AxisInfo { get; }

        private float _latestValue;
        public float LatestValue
        {
            get { return _latestValue; }
            private set { SetAndRaisePropertyChanged(ref _latestValue, value); }
        }

        //古いデータを切り捨てるときの閾値: ChartMinimumXより少し大きく取るとグラフの見た目的にキレイ
        public double OldDataLimitSec { get; } = 11.0;

        public double ChartMinimumX { get; } = -10.0;
        public double ChartMaximumX { get; } = 0.0;
        public double ChartIntervalX { get; } = 5.0;

        public double SensorMinimumOutput { get; }
        public double SensorMaximumOutput { get; }
        public double SensorIndicationInterval { get; } = 30.0;


        private ICommand _showChartCommand;
        public ICommand ShowChartCommand
            => _showChartCommand ?? (_showChartCommand = new ActionCommand(ShowChart));

        /// <summary>Qumarionとの接続を解除します。</summary>
        private void ShowChart() => ShowChartRequested?.Invoke(this, EventArgs.Empty);

        private ICommand _hideChartCommand;
        public ICommand HideChartCommand
            => _hideChartCommand ?? (_hideChartCommand = new ActionCommand(HideChart));

        /// <summary>Qumarionとの接続を解除します。</summary>
        private void HideChart() => HideChartRequested?.Invoke(this, EventArgs.Empty);

        public event EventHandler ShowChartRequested;
        public event EventHandler HideChartRequested;


    }

}
