using System.IO.IsolatedStorage;

namespace WPET
{
    public class Settings<T>
    {
        string tag;
        T defaultValue;

        public Settings(string tag, T defaultValue)
        {
            this.tag = tag;
            this.defaultValue = defaultValue;
        }

        public T Value
        {
            get
            {
                T valueFromIsolatedStorage;

                if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue(tag,out valueFromIsolatedStorage))
                {
                    IsolatedStorageSettings.ApplicationSettings[tag] = defaultValue;
                    return defaultValue;
                }
                else
                    return valueFromIsolatedStorage;
            }

            set
            {
                IsolatedStorageSettings.ApplicationSettings[tag] = value;
            }
        }
    }
}
