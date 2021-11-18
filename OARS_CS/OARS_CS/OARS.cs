using System;
using System.IO;
using System.Net.Http;

namespace Oars
{
    public static class Oars
    {
        private const string oarsUrl = "https://apps-nefsc.fisheries.noaa.gov/oars/";

        private static MultipartFormDataContent BuildFormBase(OarsConfiguration config)
        {
            string apiEnv = string.Empty;
            string dbEnv = string.Empty;

            if (config.apiEnv == OarsApiEnv.Development) apiEnv = "DEVELOPMENT";
            if (config.apiEnv == OarsApiEnv.Development) apiEnv = "TEST";
            if (config.apiEnv == OarsApiEnv.Development) apiEnv = "PRODUCTION";

            if (apiEnv == string.Empty)
                throw new Exception("Invalid OARS API Environment");

            if (config.dbEnv == OarsDbEnv.Development) dbEnv = "DEVELOPMENT";
            if (config.dbEnv == OarsDbEnv.Development) dbEnv = "TEST";
            if (config.dbEnv == OarsDbEnv.Development) dbEnv = "PRODUCTION";

            if (dbEnv == string.Empty)
                throw new Exception("Invalid OARS DB Environment");

            MultipartFormDataContent form = new MultipartFormDataContent
            {
                { new StringContent(config.project), "PROJECT" },
                { new StringContent(config.key), "KEY" },
                { new StringContent(apiEnv), "API_ENV" },
                { new StringContent(dbEnv), "DB_ENV" }
            };

            return form;
        }

        public static OarsResult Download(OarsConfiguration config, string filename)
        {
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = BuildFormBase(config);
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

        public static string Upload(OarsConfiguration config, string filename, byte[] buffer)
        {
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = BuildFormBase(config);
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
