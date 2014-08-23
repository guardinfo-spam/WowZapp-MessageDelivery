using System.Diagnostics;

namespace LOLMessageDelivery.Classes
{
    public sealed class SystemSettingsManager
    {
        private static SystemSettingsManager _instance;

        private SystemSettingsManager()
        {
            UserMaxLoginTries = 4;
        }

        public static SystemSettingsManager Instance
        {
            get { return _instance ?? (_instance = new SystemSettingsManager()); }
        }

        public int UserMaxLoginTries { get; set; }
    }
}