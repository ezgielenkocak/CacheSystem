using Newtonsoft.Json;

namespace TryRedis.API.Extensions
{
    public  static class ObjectExtension //Redis’e veriyi yazarken json’a çevirip atacağız bu yüzden ObjectExtension classını yaptık.
    {
        //object value'yu json stringine çevirir.
        public static string ToJsON(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }

    public static class StringExtension //Redisden veri alırken json verisi olarak alacağımız için onu modele çevirmemiz gerektiğinden dolayıStringExtension classını bu yüzden yaptık.
    {
        //stringi belirtilen objeye çevirir
        public static T ToObject<T>(this string value) where T : class
        {
            return string.IsNullOrEmpty(value) ? null : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
