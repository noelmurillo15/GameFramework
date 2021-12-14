#if UNITY_EDITOR
/*
 * Packages - 
 * Created by : Allan N. Murillo
 * Last Edited : 9/20/2021
 */

using System.IO;
using System.Net.Http;
using static System.IO.Path;
using System.Threading.Tasks;
using static UnityEngine.Application;

namespace ANM.EditorUtils
{
    public static class Packages
    {
        public static async Task ReplacePackageFromGist(string id1, string id2)
        {
            var url = GetGistUrl(id1, id2);
            var contents = await GetContents(url);
            ReplacePackageFile(contents);
        }

        private static string GetGistUrl(string id1, string id2, string user = "noelmurillo15") =>
            $"https://gist.githubusercontent.com/{user}/{id1}/raw/{id2}/package.json";

        private static async Task<string> GetContents(string url)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        private static void ReplacePackageFile(string contents)
        {
            var existing = Combine(dataPath, "../Packages/manifest.json");
            File.WriteAllText(existing, contents);
            UnityEditor.PackageManager.Client.Resolve();
        }

        public static void InstallUnityPackage(string packageName)
        {
            UnityEditor.PackageManager.Client.Add($"com.unity.{packageName}");
        }
    }
}
#endif