using Songza_WP8.Resources;

namespace Songza_WP8
{
    /// <summary>
    /// Provides access to string resources.
    /// </summary>
    public class LocalizedStrings
    {
        private static AppResources _localizedResources = new AppResources();

        public static AppResources LocalizedResources { get { return _localizedResources; } }
    }
}