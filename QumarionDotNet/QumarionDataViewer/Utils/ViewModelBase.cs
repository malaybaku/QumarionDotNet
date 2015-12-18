using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QumarionDataViewer
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected void SetAndRaisePropertyChanged<T>(ref T target, T value, [CallerMemberName]string pname = "")
            where T : struct, IEquatable<T>
        {
            if (!value.Equals(target))
            {
                target = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pname));
            }
        }

        protected void SetAndRaisePropertyChanged(ref string target, string value, [CallerMemberName]string pname = "")
        {
            if (value != target)
            {
                target = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pname));
            }
        }

        protected void RaisePropertyChanged([CallerMemberName]string pname = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pname));
        
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
