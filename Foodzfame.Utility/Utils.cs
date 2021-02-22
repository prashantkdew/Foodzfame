using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Foodzfame.Utility
{
    public static class Utils
    {
        public static async System.Threading.Tasks.Task<string> getInstaDetailsAsync()
        {
            const string URL = "https://www.instagram.com/foodzfame/";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            string urlParameters = "?__a=1";
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if(response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return "";
        }
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }
        public static byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }
        public static byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }
        public static string RecipeMetaTags(string[] keywords, string description, List<KeyValuePair<string,string>> additionalAttr=null)
        {

            System.Text.StringBuilder strMetaTag = new System.Text.StringBuilder();
            if (additionalAttr != null)
            {
                foreach (var item in additionalAttr)
                {
                    strMetaTag.AppendFormat(@"<meta name='og:{0}' content='{1}' />",item.Key,item.Value);
                }
            }
            strMetaTag.AppendFormat(@"<meta name='keywords' content='{0}' />", string.Join(" ", keywords)); 
            strMetaTag.AppendFormat(@"<meta name='description' content='{0}' />", description); 
            strMetaTag.AppendFormat(@"<meta name='robots' content='{0}' />", "index, follow");
            return strMetaTag.ToString();

        }
    }
}
