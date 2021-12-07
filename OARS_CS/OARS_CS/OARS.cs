using System;
using System.IO;
using System.Net.Http;

namespace OARS
{
    public static class Oars
    {
        private const string oarsUrl = "https://apps-nefsc.fisheries.noaa.gov/oars/";

        private static MultipartFormDataContent BuildFormBase(OarsConfiguration config)
        {
            string apiEnv = string.Empty;
            string dbEnv = string.Empty;

            if(config.apiEnv == OarsApiEnv.Development) apiEnv = "DEVELOPMENT";
            if(config.apiEnv == OarsApiEnv.Testing) apiEnv = "TEST";
            if(config.apiEnv == OarsApiEnv.Production) apiEnv = "PRODUCTION";

            if(apiEnv == string.Empty)
                throw new Exception("Invalid OARS API Environment");

            if(config.dbEnv == OarsDbEnv.Development) dbEnv = "DEVELOPMENT";
            if(config.dbEnv == OarsDbEnv.Testing) dbEnv = "TEST";
            if(config.dbEnv == OarsDbEnv.Production) dbEnv = "PRODUCTION";

            if (dbEnv == string.Empty)
                throw new Exception("Invalid OARS DB Environment");

            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(config.project), "PROJECT");
            form.Add(new StringContent(config.key), "KEY");
            form.Add(new StringContent(apiEnv), "API_ENV");
            form.Add(new StringContent(dbEnv), "DB_ENV");

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

        public static OarsResult Upload(OarsConfiguration config, string filename, byte[] buffer)
        {
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = BuildFormBase(config);
            form.Add(new StringContent(filename), "FILENAME");
            form.Add(new StringContent("store"), "TYPE");
            form.Add(new ByteArrayContent(buffer, 0, buffer.Length), "FILE", filename);

            HttpResponseMessage response = httpClient.PostAsync(oarsUrl, form).Result;

            httpClient.Dispose();

            MemoryStream ms = new MemoryStream();
            response.Content.ReadAsStreamAsync().Result.CopyTo(ms);

            OarsResult result;
            result.data = ms.ToArray();
            result.contentType = response.Content.Headers.ContentType.MediaType;

            return result;
        }

        public static OarsResult UploadJson(OarsConfiguration config, string filename, byte[] buffer)
        {
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = BuildFormBase(config);
            form.Add(new StringContent(filename), "FILENAME");
            form.Add(new ByteArrayContent(buffer, 0, buffer.Length), "FILE", filename);

            HttpResponseMessage response = httpClient.PostAsync(oarsUrl, form).Result;

            httpClient.Dispose();

            MemoryStream ms = new MemoryStream();
            response.Content.ReadAsStreamAsync().Result.CopyTo(ms);

            OarsResult result;
            result.data = ms.ToArray();
            result.contentType = response.Content.Headers.ContentType.MediaType;

            return result;
        }
    }
}
