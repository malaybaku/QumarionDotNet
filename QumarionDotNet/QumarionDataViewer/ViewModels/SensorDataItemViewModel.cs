using System;

namespace QumarionDataViewer
{
    /// <summary>角度の観測値と観測時刻からのずれを表します。</summary>
    public class SensorDataItemViewModel : ViewModelBase
    {
        public SensorDataItemViewModel(double angle, DateTime originTime, DateTime currentTime)
        {
            OriginTime = originTime;
            Time = -(currentTime - originTime).TotalSeconds;
            Angle = angle;
        }
        public SensorDataItemViewModel(double angle, DateTime originTime)
            : this(angle, originTime, DateTime.Now)
        {
        }

        public DateTime OriginTime { get; }

        private double _time;
        public double Time
        {
            get { return _time; }
            set { SetAndRaisePropertyChanged(ref _time, value); }
        }

        public double Angle { get; }
 
        public void Update(DateTime time)
        {
            Time = -(time - OriginTime).TotalSeconds;
        }
    }
}
