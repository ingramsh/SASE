using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using SASELibrary;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using MvcSASE.Models;

namespace SYWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private DBContext db = new DBContext();
        private AzureAccountService sase;
        
        private int? uID;

        private string container = "sase-youtube-videos";
        private string queue_in = "sase-youtube-in";
        private string queue_out = "sase-youtube-out";

        private bool completed = false;
        string sytitle;

        public override void Run()
        {
            Trace.TraceInformation("SYWorker entry point called");

            if (uID == null || uID < 0)
                return;

            sase.service.CreateContainer(container);
            sase.service.CreateQueue(queue_in);
            sase.service.CreateQueue(queue_out);

            while (sase.service.QueueMessageCount("sase-youtube-in") > 0)
            {
                Trace.TraceInformation("Working");
                completed = false;

                if (sase.service.QueueMessageCount(queue_in) > 0)
                {
                    Message peek;
                    peek = sase.service.PeekMessage(queue_in);
                    sase.service.DequeueMessage(queue_in);

                    if (Convert.ToInt32(peek.DequeueCount) >= 2)
                        continue;
                    else
                    {
                        string message = string.Empty;
                        string id = string.Empty;

                        message = peek.MessageString;
                        string url = message;

                        Regex YoutubeVideoRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase);
                        Match youtubeMatch = YoutubeVideoRegex.Match(url);

                        if (youtubeMatch.Success)
                        {
                            id = youtubeMatch.Groups[1].Value;
                            url = "http://www.youtube.com/watch?v=" + id;
                            string info_url = "http://www.youtube.com/get_video_info?video_id=" + id;

                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(info_url);
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                            Stream response_stream = response.GetResponseStream();
                            StreamReader read = new StreamReader(response_stream, Encoding.GetEncoding("utf-8"));

                            string vid_info = HttpUtility.HtmlDecode(read.ReadToEnd());

                            if (vid_info.Contains("errorcode"))
                            {
                                sase.service.EnqueueMessage(queue_out, "The following youtube id returned an error (likely copyright): " + id);
                                continue;
                            }

                            NameValueCollection vid_params = HttpUtility.ParseQueryString(vid_info);
                            string[] vid_urls = vid_params["url_encoded_fmt_stream_map"].Split(',');

                            foreach (string param_url in vid_urls)
                            {
                                string s_url = HttpUtility.HtmlDecode(param_url);

                                NameValueCollection url_params = HttpUtility.ParseQueryString(s_url);
                                string v_format = HttpUtility.HtmlDecode(url_params["type"]);

                                s_url = HttpUtility.HtmlDecode(url_params["url"]);
                                s_url += "&signature=" + HttpUtility.HtmlDecode(url_params["sig"]);
                                s_url += "&type=" + v_format;
                                s_url += "&title=" + HttpUtility.HtmlDecode(vid_params["title"]);

                                if (s_url.Contains("mp4"))
                                {
                                    v_format = url_params["quality"] + " - " + v_format.Split(';')[0].Split('/')[1];

                                    url_params = HttpUtility.ParseQueryString(s_url);

                                    string vid_title = url_params["title"];
                                    string vid_t = HttpUtility.HtmlDecode(url_params["type"]);
                                    vid_t = vid_t.Split(';')[0].Split('/')[1];

                                    sytitle = vid_title + "." + vid_t;

                                    WebClient web_client = new WebClient();
                                    web_client.DownloadDataCompleted += DownloadDataCompleted;
                                    web_client.DownloadDataAsync(new Uri(s_url));

                                    while (completed == false)
                                    {
                                        Thread.Sleep(1000);
                                    }

                                    break;
                                }
                            }

                            if (completed == true)
                                sase.service.EnqueueMessage(queue_out, sytitle + "--extraction completed.");
                            else 
                                sase.service.EnqueueMessage(queue_out, "The following youtube id could not be converted to mp4: " + id);

                        }
                    }
                }
                else break;

                Thread.Sleep(10000);
            }
        }

        private void DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                byte[] raw = e.Result;

                Stream stream = new MemoryStream(raw);
                stream.Position = 0;

                sase.service.UploadBlockBlobStream(container, sytitle, stream);
                
                completed = true;
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //  The specific ID on the user entity table that will facilitate the pointer queue "sase-youtube-id"
            int checkID = 2;

            sase = (from i in db.Sase where i.ID == checkID select i).FirstOrDefault();

            while (sase.service.QueueMessageCount("sase-youtube-id") == 0)
                Thread.Sleep(100000);

            uID = -1;
            uID = Convert.ToInt32(sase.service.DequeueMessage("sase-youtube-id"));
            sase = (from i in db.Sase where i.ID == uID select i).FirstOrDefault();
            
            return base.OnStart();
        }
    }
}
