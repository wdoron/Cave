using System.Reflection;

namespace Dweiss.Network
{
    public static class Utils
    {
        private const string UrlFirstParamFormat = "?{0}={1}";
        private const string UrlOtherParamFormat = "&{0}={1}";
        public static string GetUrlParams(params System.Tuple<string, string>[] input)
        {
            var ret = new System.Text.StringBuilder();
            if (input.Length == 0) return ret.ToString();
            ret.AppendFormat(UrlFirstParamFormat, input[0], input[0]);

            for (int i = 1; i < input.Length; ++i)
            {
                ret.AppendFormat(UrlOtherParamFormat, input[i], input[i]);
            }
            return ret.ToString();
            //Should use System.Uri.EscapeUriString(ret) ?

        }
    }

    public static class ObjectExtensions
    {
        public static string ToUrlParams<T>(this T instance, bool useQPrefix = true)
        {
            var urlBuilder = new System.Text.StringBuilder();

            var properties = instance.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length > 0)
            {
                
                urlBuilder.AppendFormat(useQPrefix ? "?{0}={1}" : "{0}={1}", properties[0].Name, properties[0].GetValue(instance));

                for (int i = 1; i < properties.Length; i++)
                {
                    urlBuilder.AppendFormat("&{0}={1}", properties[i].Name, properties[i].GetValue(instance));
                }
            } else
            {
                UnityEngine.Debug.LogError(instance.GetType() + " prop length " + properties.Length);
            }
            return urlBuilder.ToString();
        }
    }
}