using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EmonMonitor.DataAccess.Contracts;
using EmonMonitor.Utils;


namespace EmonMonitor.DataAccess
{
    public class ApiJsonClient
    {
        /// <summary>
        /// Return EmonCMS Feeds
        /// </summary>
        /// <param name="api_url"></param>
        /// <param name="api_key"></param>
        /// <returns>List of FeedItem</returns>
        public async Task<List<FeedItem>> GetFeedsListAsync(string api_url, string api_key)
        {
            HttpWebRequest req = HttpWebRequest.CreateHttp(String.Format("{0}/feed/list.json&apikey={1}", api_url, api_key));
            if (req.Headers == null)
            {
                req.Headers = new WebHeaderCollection();
            }
            req.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
            //req.ContentType = "application/json";
            var resp = await DoRequestJsonAsync<List<FeedItem>>(req);
            return resp;
        }

        public async Task<List<FeedValue>> GetFeedAsync(string api_url, string api_key, int feedId, DateTime startTime, DateTime endTime, int dp = 0)
        {
            HttpWebRequest req = HttpWebRequest.CreateHttp(String.Format("{0}/feed/data.json?&id={1}&start={2}&end={3}dp={4}&apikey={5}", api_url,feedId, startTime.Ticks, endTime.Ticks, dp, api_key));
            if (req.Headers == null)
            {
                req.Headers = new WebHeaderCollection();
            }
            req.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
            //req.Accept = "application/json";
            //req.ContentType = "application/json";
            var resp = await DoRequestJsonAsync<List<FeedValue>>(req);
            return resp;
        }


        public async Task<Decimal> GetFeedValueAsync(string api_url, string api_key, int feedId)
        {
            HttpWebRequest req = HttpWebRequest.CreateHttp(String.Format("{0}/feed/value.json?&id={1}&apikey={2}",api_url, feedId, api_key));
            if (req.Headers == null)
            {
                req.Headers = new WebHeaderCollection();
            }
            req.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
            //req.ContentType = "application/json";
            var resp = await DoRequestJsonAsync<Decimal>(req);
            return resp;
        }

        #region Privates
        private async Task<System.IO.TextReader> DoRequestAsync(HttpWebRequest req)
        {
            HttpWebResponse resp = (HttpWebResponse)await req.GetResponseAsync();
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(resp.StatusCode.ToString());
            }
            var stream = resp.GetResponseStream();
            var sr = new System.IO.StreamReader(stream);
            return sr;
        }

        private static T DeserializeObject<T>(string objString)
        {
            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(objString)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                object ob = null;
                try
                {
                    ob = (T)serializer.ReadObject(stream);
                    return (T)ob;
                }
                catch (Exception ex)
                {
                    throw new Exception("Deserialization error.");
                }
            }

        }

        private async Task<T> DoRequestJsonAsync<T>(HttpWebRequest req)
        {
            req.Method = HttpMethod.Get;
            var result = await DoRequestAsync(req);
            var response = await result.ReadToEndAsync();
            // deserialize the response
            return (T)DeserializeObject<T>(response);
        }

        #endregion

    }
}

