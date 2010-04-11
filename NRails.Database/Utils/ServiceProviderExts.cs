using System;

namespace Rsdn.Janus
{
    public static class ServiceProviderExts
    {
        public static T GetRequiredService<T>(this IServiceProvider provider)
        {
            return (T) provider.GetService(typeof (T));
        }
    }
}