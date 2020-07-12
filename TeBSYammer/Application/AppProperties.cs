using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TeBSYammer
{
    public class AppProperties
    {

        public async static Task AddOrUpdate(string key, object value)
        {
            if (Application.Current.Properties.ContainsKey(key))
            {
                Application.Current.Properties[key] = value;
                await Application.Current.SavePropertiesAsync();                
            }
            else
            {
                Application.Current.Properties.Add(key, value);
                await Application.Current.SavePropertiesAsync();
            }
           
        }

        public async static void RemoveKey(string key)
        {
            if (Application.Current.Properties.ContainsKey(key))
            {
                Application.Current.Properties.Remove(key);
                await Application.Current.SavePropertiesAsync();
            }
           
        }

        public async static void ClearAll()
        {
            var keyList = Application.Current.Properties.Keys;

            foreach (var key in keyList)
            {
                Application.Current.Properties.Remove(key);
                await Application.Current.SavePropertiesAsync();
            }

           
        }
    }
}
