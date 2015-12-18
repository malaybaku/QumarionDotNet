using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace QumarionDataViewer
{
    static class DispatcherHelper
    {
        public static Dispatcher UIDispatcher { get; private set; }

        public static void SetFromUIDispatcher()
        {
            UIDispatcher = Dispatcher.CurrentDispatcher;
        }
    }
}
