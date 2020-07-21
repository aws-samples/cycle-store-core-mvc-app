using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventureWorksMVCCore.Web
{
    public static class ServicesConfiguration
    {
        public static async Task<Dictionary<string, string>> GetSqlCredential(this IServiceCollection services, string secretId)
        {

            var credential = new Dictionary<string, string>();

            using (var secretsManager = new AmazonSecretsManagerClient(Amazon.RegionEndpoint.USEast1))
            using (var cache = new SecretsManagerCache(secretsManager))
            {


                var sec = await cache.GetSecretString(secretId);
                var jo = Newtonsoft.Json.Linq.JObject.Parse(sec);

                credential["username"] = jo["username"].ToObject<string>();
                credential["password"] = jo["password"].ToObject<string>();
            }

            return credential;
        }
    }
}
