using System.IO;
using System.Net.Http;

namespace Oars
{
    public static class Oars
    {
        private const string oarsUrl = "https://apps-nefsc.fisheries.noaa.gov/oars/";

        private static MultipartFormDataContent BuildFormBase(string project, string key, string api_env, string db_env)
        {
            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new StringContent(project), "PROJECT");
            form.Add(new StringContent(key), "KEY");
            form.Add(new StringContent(api_env), "API_ENV");
            form.Add(new StringContent(db_env), "DB_ENV");

            return form;
        }

        public static OarsResult Download(string project, string key, string filename, string environment = "PRODUCTION")
        {
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = BuildFormBase(project, key, environment, environment);
            form.Add(new StringContent("file"), "TYPE");
            form.Add(new StringContent(filename), "FILENAME");

            HttpResponseMessage response = httpClient.PostAsync(oarsUrl, form).Result;

            httpClient.Dispose();

            MemoryStream ms = new MemoryStream();
            response.Content.ReadAsStreamAsync().Result.CopyTo(ms);

            OarsResult result;
            result.data = ms.ToArray();
            result.contentType = response.Content.Headers.ContentType.MediaType;

            return result;
        }

        public static string Upload(string project, string key, string filename, byte[] buffer, string environment = "PRODUCTION")
        {
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = BuildFormBase(project, key, environment, environment);
            form.Add(new StringContent(filename), "FORMAT");

            //form.Add(new StringContent("JSON"), "FORMAT"); // Don't need this if we are downloading files
            //form.Add(new ByteArrayContent(file_bytes, 0, file_bytes.Length), "profile_pic", "hello1.jpg"); // Use this if you're uploading a file

            HttpResponseMessage response = httpClient.PostAsync(oarsUrl, form).Result;

            httpClient.Dispose();

            MemoryStream ms = new MemoryStream();
            response.Content.ReadAsStreamAsync().Result.CopyTo(ms);

            return ms.ToString();
        }
    }
}
