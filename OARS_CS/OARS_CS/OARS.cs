using System.IO;
using System.Net.Http;

namespace OARS_CS
{
    public static class OARS
    {
        private const string oarsUrl = "https://apps-nefsc.fisheries.noaa.gov/oars/";

        public static byte[] Download(string project, string key, string filename, string environment = "PRODUCTION")
        {
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent
            {
                { new StringContent(project), "PROJECT" },
                { new StringContent(key), "KEY" },
                //form.Add(new StringContent("JSON"), "FORMAT"); // Don't need this if we are downloading files
                { new StringContent(environment), "API_ENV" },
                { new StringContent(environment), "DB_ENV" },
                { new StringContent("FILE"), "TYPE" },
                { new StringContent(filename), "FILENAME" }
            };
            //form.Add(new ByteArrayContent(file_bytes, 0, file_bytes.Length), "profile_pic", "hello1.jpg"); // Use this if you're uploading a file

            HttpResponseMessage response = httpClient.PostAsync(oarsUrl, form).Result;
            HttpResponseMessage ensured = response.EnsureSuccessStatusCode();

            httpClient.Dispose();

            MemoryStream ms = new MemoryStream();
            response.Content.ReadAsStreamAsync().Result.CopyTo(ms);

            return ms.ToArray();
        }
    }
}
