using System;
using System.Windows.Input;

using Baku.Quma.Low;

namespace QumarionDataViewer
{
    /// <summary>
    /// デバイスのロード処理のビューモデルを表します。
    /// </summary>
    public class DeviceLoaderViewModel
    {
        public DeviceLoaderViewModel(QumaId qumaId)
        {
            _qumaId = qumaId;
            DeviceInfo = $"{_qumaId.Id}({GetStringFromQumaType(_qumaId.QumaType)})";
        }
        private readonly QumaId _qumaId;
    
        private ICommand _loadDeviceCommand;
        public ICommand LoadDeviceCommand
            => _loadDeviceCommand ?? (_loadDeviceCommand = new ActionCommand(LoadDevice));

        public void LoadDevice()
        {
            var device = QumarionManager.GetDeviceById(_qumaId);
            DeviceLoaded?.Invoke(this, new QumarionDeviceLoadedEventArgs(device));
        }
        
        public string DeviceInfo { get; }

        private string GetStringFromQumaType(QumaTypes qumaType)
        {
            switch(qumaType)
            {
                case QumaTypes.Software:
                    return "シミュレータ";
                case QumaTypes.HardwareFirst:
                    return "試作機1";
                case QumaTypes.HardwarePrototype:
                    return "試作機2";
                case QumaTypes.HardwareCustom:
                    return "試作機3";
                case QumaTypes.HardwareAsai:
                    return "実機";
                default:
                    throw new InvalidOperationException("unknown quma type were given");
            }
        }

        public event EventHandler<QumarionDeviceLoadedEventArgs> DeviceLoaded;
    }

    public class QumarionDeviceLoadedEventArgs : EventArgs
    {
        public QumarionDeviceLoadedEventArgs(Qumarion device)
        {
            LoadedDevice = device;
        }
        public Qumarion LoadedDevice { get; }
    }
}
