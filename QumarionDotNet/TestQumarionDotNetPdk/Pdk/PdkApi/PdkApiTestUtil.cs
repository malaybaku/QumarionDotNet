using System;

using Baku.Quma.Pdk;
using Baku.Quma.Pdk.Api;

namespace TestQumarionDotNet.Pdk
{
    public static class PdkApiTestUtil
    {
        public static QumaHandle LoadQumaHandle()
        {
            QmPdk.BaseOperation.Initialize();
            if (QmPdk.Quma.GetDeviceCount() == 0)
            {
                throw new InvalidOperationException("Qumarion is not connected to this machine");
            }

            return QmPdk.Quma.GetHandle(0);
        }
    }
}
