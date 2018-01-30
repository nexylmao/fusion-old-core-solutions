using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Fusion
{
    public static class __RESTCLIENT
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string baseUrl = __METADATA.DefaultPath;
        public static async Task<__METADATA_FILE> GetMetadataFile()
        {
            return JsonConvert.DeserializeObject<List<__METADATA_FILE>>(await client.GetStringAsync(baseUrl + "startJSON"))[0];
        }
        public static async Task<string> PostMetadataFile(__METADATA_FILE obj)
        {
            return await(await client.PostAsync(baseUrl + "startJSON", new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"))).Content.ReadAsStringAsync();
        }
        public static async Task<string> PostMessage(__MESSAGE obj)
        {
            return await (await client.PostAsync(baseUrl + "logthis", new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"))).Content.ReadAsStringAsync();
        }
        public static async Task<string> DeleteMetadataFile()
        {
            return await (await client.DeleteAsync(baseUrl + "startJSON")).Content.ReadAsStringAsync();
        }
        public static async Task<List<T>> Get<T>()
        {
            return JsonConvert.DeserializeObject<List<T>>(await client.GetStringAsync(baseUrl + typeof(T).Name));
        }
        public static async Task<List<T>> GetWithObjectID<T>()
        {
            return JsonConvert.DeserializeObject<List<T>>(await client.GetStringAsync(baseUrl + typeof(T).Name + "/WithObjectID"));
        }
        public static async Task<List<T>> GetQueueObjectID<T>(string ObjectID)
        {
            return JsonConvert.DeserializeObject<List<T>>(await client.GetStringAsync(baseUrl + typeof(T).Name + "/QueueObjectID/" + ObjectID));
        }
        public static async Task<List<T>> GetQueueCustomID<T>(string CustomID)
        {
            return JsonConvert.DeserializeObject<List<T>>(await client.GetStringAsync(baseUrl + typeof(T).Name + "/QueueCustomID/" + CustomID));
        }
        public static async Task<List<T>> GetQueueObjectIDWithObjectID<T>(string ObjectID)
        {
            return JsonConvert.DeserializeObject<List<T>>(await client.GetStringAsync(baseUrl + typeof(T).Name + "/QueueObjectID/" + ObjectID + "/WithObjectID"));
        }
        public static async Task<List<T>> GetQueueCustomIDWithObjectID<T>(string CustomID)
        {
            return JsonConvert.DeserializeObject<List<T>>(await client.GetStringAsync(baseUrl + typeof(T).Name + "/QueueCustomID/" + CustomID + "/WithObjectID"));
        }
        public static async Task<string> Post<T>(T obj)
        {
            return await (await client.PostAsync(baseUrl + typeof(T).Name, new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"))).Content.ReadAsStringAsync();
        }
        public static async Task<string> PutQueueCustomID<T>(T obj, string CustomID)
        {
            return await (await client.PutAsync(baseUrl + typeof(T).Name + "/QueueCustomID/" + CustomID, new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"))).Content.ReadAsStringAsync();
        }
        public static async Task<string> PutQueueObjectID<T>(T obj, string ObjectID)
        {
            return await (await client.PutAsync(baseUrl + typeof(T).Name + "/QueueCustomID/" + ObjectID, new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"))).Content.ReadAsStringAsync();
        }
        public static async Task<string> DeleteCollection<T>()
        {
            return await (await client.DeleteAsync(baseUrl + typeof(T).Name)).Content.ReadAsStringAsync();
        }
        public static async Task<string> DeleteQueueCustomID<T>(string CustomID)
        {
            return await (await client.DeleteAsync(baseUrl + typeof(T).Name + "/QueueCustomID/" + CustomID)).Content.ReadAsStringAsync();
        }
        public static async Task<string> DeleteQueueObjectID<T>(string ObjectID)
        {
            return await (await client.DeleteAsync(baseUrl + typeof(T).Name + "/QueueObjectID/" + ObjectID)).Content.ReadAsStringAsync();
        }
    }
}
