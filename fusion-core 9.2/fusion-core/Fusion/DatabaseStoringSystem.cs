using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.IO;
using System;

namespace Fusion.StoringSystems
{
    public enum Database { RESTAPI, LOCALFILES };

    public class DatabaseOutputHandler<Type>
    {
        #region Fields
        string Path;
        Database Output;
        IOutputClient<Type> Client;
        #endregion
        #region Property
        public string OutputPath
        {
            get
            {
                return Path;
            }
        }
        public Database OutputType
        {
            get
            {
                return Output;
            }
        }
        #endregion
        #region Constructor
        public DatabaseOutputHandler(string Path = "", bool LoadNow = false)
        {
            this.Path = Path;
            if((Path != null) && (Path.Contains("http") || Path.Contains("localhost")))
            {
                Output = Database.RESTAPI;
                Client = new RestClient<Type>(Path);
            }
            else
            {
                Output = Database.LOCALFILES;
                Client = new LocalFileManager<Type>();
            }
        }
        #endregion
        #region Methods
        public IEnumerable<Type> Load()
        {
            return Client.GetFullDatabase();
        }
        public string Save(IEnumerable<Type> Data)
        {
            return Client.PostFullDatabase(Data);
        }
        public string Delete()
        {
            return Client.DeleteFullDatabase();
        }
        #endregion
    }

    public interface IOutputClient<Type>
    {
        #region Methods
        IEnumerable<Type> GetFullDatabase();
        string PostFullDatabase(IEnumerable<Type> Data);
        string DeleteFullDatabase();
        #endregion
    }

    public class RestClient<Type> : IOutputClient<Type>
    {
        #region Fields
        protected readonly HttpClient client = new HttpClient();
        protected Uri baseUri;
        #endregion
        #region Properties
        public string CollectionName
        {
            get
            {
                return typeof(Type).Name;
            }
        }
        public string Uri
        {
            get
            {
                return baseUri.AbsolutePath;
            }
            set
            {
                baseUri = new Uri(value);
            }
        }
        #endregion
        #region Constructor
        public RestClient(string newUri = "")
        {
            if(newUri != "")
            {
                Uri = newUri;
            }
        }
        #endregion
        #region AsyncTasks
        public static async Task<List<Type>> Get(string Path, string Collection)
        {
            HttpClient client = new HttpClient();
            return JsonConvert.DeserializeObject<List<Type>>(await client.GetStringAsync(Path + "/" + Collection));
        }
        public static async Task<List<Type>> Get(string Path, string Collection, string Name)
        {
            HttpClient client = new HttpClient();
            return JsonConvert.DeserializeObject<List<Type>>(await client.GetStringAsync(Path + "/" + Collection + "/" + Name));
        }
        public async Task<List<Type>> Get()
        {
            return JsonConvert.DeserializeObject<List<Type>>(await client.GetStringAsync(baseUri + typeof(Type).Name));
        }
        public async Task<Type> Get(string Name)
        {
            return JsonConvert.DeserializeObject<Type>(await client.GetStringAsync(baseUri + typeof(Type).Name + "/" + Name));
        }
        public async Task<List<Type>> GetWithObjectID()
        {
            return JsonConvert.DeserializeObject<List<Type>>(await client.GetStringAsync(baseUri + typeof(Type).Name + "/WithObjectID"));
        }
        public async Task<List<Type>> GetQueueObjectID(string ObjectID)
        {
            return JsonConvert.DeserializeObject<List<Type>>(await client.GetStringAsync(baseUri + typeof(Type).Name + "/QueueObjectID/" + ObjectID));
        }
        public async Task<List<Type>> GetQueueCustomID(string CustomID)
        {
            return JsonConvert.DeserializeObject<List<Type>>(await client.GetStringAsync(baseUri + typeof(Type).Name + "/QueueCustomID/" + CustomID));
        }
        public async Task<List<Type>> GetQueueObjectIDWithObjectID(string ObjectID)
        {
            return JsonConvert.DeserializeObject<List<Type>>(await client.GetStringAsync(baseUri + typeof(Type).Name + "/QueueObjectID/" + ObjectID + "/WithObjectID"));
        }
        public async Task<List<Type>> GetQueueCustomIDWithObjectID(string CustomID)
        {
            return JsonConvert.DeserializeObject<List<Type>>(await client.GetStringAsync(baseUri + typeof(Type).Name + "/QueueCustomID/" + CustomID + "/WithObjectID"));
        }
        public async Task<string> Post(IEnumerable<Type> obj)
        {
            return await (await client.PostAsync(baseUri + typeof(Type).Name, new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"))).Content.ReadAsStringAsync();
        }
        public static async Task<string> Post(IEnumerable<Type> obj, string Path, string Collection)
        {
            HttpClient client = new HttpClient();
            return await (await client.PostAsync(Path + "/" + Collection, new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"))).Content.ReadAsStringAsync();
        }
        public async Task<string> PutQueueCustomID(IEnumerable<Type> obj, string CustomID)
        {
            return await (await client.PutAsync(baseUri + typeof(Type).Name + "/QueueCustomID/" + CustomID, new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"))).Content.ReadAsStringAsync();
        }
        public async Task<string> PutQueueObjectID(IEnumerable<Type> obj, string ObjectID)
        {
            return await (await client.PutAsync(baseUri + typeof(Type).Name + "/QueueCustomID/" + ObjectID, new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"))).Content.ReadAsStringAsync();
        }
        public static async Task<string> DeleteCollection(string Path, string Collection)
        {
            HttpClient client = new HttpClient();
            return await (await client.DeleteAsync(Path + "/" + Collection)).Content.ReadAsStringAsync();
        }
        public async Task<string> DeleteCollection()
        {
            return await (await client.DeleteAsync(baseUri + typeof(Type).Name)).Content.ReadAsStringAsync();
        }
        public async Task<string> DeleteQueueCustomID(string CustomID)
        {
            return await (await client.DeleteAsync(baseUri + typeof(Type).Name + "/QueueCustomID/" + CustomID)).Content.ReadAsStringAsync();
        }
        public async Task<string> DeleteQueueObjectID(string ObjectID)
        {
            return await (await client.DeleteAsync(baseUri + typeof(Type).Name + "/QueueObjectID/" + ObjectID)).Content.ReadAsStringAsync();
        }
        #endregion
        #region InterfaceMethods
        public IEnumerable<Type> GetFullDatabase()
        {
            return Get().Result;
        }
        public string PostFullDatabase(IEnumerable<Type> Data)
        {
            return Post(Data).Result;
        }
        public string DeleteFullDatabase()
        {
            return DeleteCollection().Result;
        }
        #endregion
    }

    public class LocalFileManager<Type> : IOutputClient<Type>
    {
        #region InterfaceMethods
        public IEnumerable<Type> GetFullDatabase()
        {
            throw new NotImplementedException();
        }
        public string PostFullDatabase(IEnumerable<Type> Data)
        {
            throw new NotImplementedException();
        }
        public string DeleteFullDatabase()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
