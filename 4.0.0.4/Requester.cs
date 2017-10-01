using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using WLive.WLiveObjects;

namespace WLive
{
    public static class Requester
    {
        static int bufferSize = 8192;
        public static int BufferSize
        {
            get { return bufferSize; }
            set { bufferSize = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Object which have to be return</typeparam>
        /// <param name="requestObject">Object with request data</param>
        /// <returns>Returns collection of objects gives from JSON</returns>
        public static IEnumerable<T> Request<T>(RequestObject requestObject)
        {
            return Request<T>(requestObject, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type of object which you want return</typeparam>
        /// <param name="requestObject">Object with request data</param>
        /// <param name="handler">Handler for upload request</param>
        /// <returns>Return collection of object which gives from JSON</returns>
        public static IEnumerable<T> Request<T>(RequestObject requestObject, RequestProgressHandler handler)
        {
            if (requestObject == null)
                throw new ArgumentNullException("requestObject");
            List<T> obj = new List<T>();
            bool error = false;
            string responseContentType = "";
            WebException exception = null;
            string result = "";
            using (MemoryStream ms = new MemoryStream())
            {

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestObject.Url);
                    request.Method = requestObject.Method;

                    request.Headers = requestObject.Headers;
                    request.ContentLength = requestObject.ContentLength;
                    request.ContentType = !string.IsNullOrEmpty(requestObject.ContentType) ? requestObject.ContentType : request.ContentType;
                    if (requestObject.ContentLength > 0)
                    {
                        using (Stream requestStream = request.GetRequestStream())
                        {
                            int k = 0;
                            while (k < requestObject.GetData().Length)
                            {
                                if (k + bufferSize < requestObject.ContentLength)
                                {
                                    requestStream.Write(requestObject.GetData(), k, bufferSize);
                                    k += bufferSize;
                                }
                                else
                                {
                                    requestStream.Write(requestObject.GetData(), k, requestObject.ContentLength - k);
                                    k += requestObject.GetData().Length - k;
                                }
                                if (handler != null && handler.ProgressChange != null)
                                {
                                    if (requestObject is BitsRequest)
                                    {
                                        BitsRequest br = (BitsRequest)requestObject;
                                        handler.ProgressChange(((br.Count - 1) * br.MaxLength) + k, (double)(((br.Count - 1) * br.MaxLength) + k) / (br.TotalLength / (double)100));
                                    }
                                    else
                                        handler.ProgressChange(k, (k / (request.ContentLength / 100)));
                                }
                            }
                        }
                    }
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    responseContentType = response.ContentType;
                    if (requestObject is BitsRequest)
                        ((BitsRequest)requestObject).SetSessionId(response.Headers["BITS-Session-Id"]);
                    response.GetResponseStream().CopyTo(ms);
                }
                catch (WebException e)
                {
                    exception = e;
                    error = true;
                    if (e.Response != null)
                    {
                        HttpWebResponse response = (HttpWebResponse)e.Response;
                        responseContentType = response.ContentType;
                        response.GetResponseStream().CopyTo(ms);
                    }
                }
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                    result = sr.ReadToEnd();
            }
            if (error)
            {
                if (responseContentType.Contains(Utils.ContentType.ApplicationJson))
                {
                    WLError wlError = Newtonsoft.Json.JsonConvert.DeserializeObject<WLError>(result, AGetOpConverter<WLError>.Settings);
                    throw new WLiveException(wlError);
                }
                throw new WebException(exception.Message, exception);
            }
            else
            {
                if (responseContentType.Contains(Utils.ContentType.ApplicationJson))
                {
                    object res = Newtonsoft.Json.JsonConvert.DeserializeObject(result, typeof(T), AGetOpConverter<T>.Settings);
                    if (res is T)
                        obj.Add((T)res);
                    else
                        obj.AddRange((IEnumerable<T>)res);
                }
            }
            return obj;
        }
    }

    public class RequestObject
    {
        byte[] data;
        string contentType = "";
        WebHeaderCollection hcol = new WebHeaderCollection();
        
        string method = WebRequestMethods.Http.Get;
       
        public Uri Url
        {
            get; set;
        }

        public string Method
        {
            get { return method; }
            set { method = value; }
        }
        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        public int ContentLength
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns headers for request</returns>
        public virtual WebHeaderCollection Headers
        {
            get { return hcol; }
        }
        public RequestObject()
        {
            ContentLength = 0;
        }
        /// <summary>
        /// Set string data into byte array
        /// </summary>
        /// <param name="stringdata">Data for request stream</param>
        public void SetData(string value)
        {
            data = System.Text.Encoding.UTF8.GetBytes(value);
            ContentLength = data.Length;
        }
        public void SetData(byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            data = value;
            ContentLength = data.Length;
        }
        public byte[] GetData()
        {
            return data;
        }
        /// <summary>
        /// Add header into collection
        /// </summary>
        /// <param name="header">Header name</param>
        /// <param name="value">Header value</param>
        public void AddHeader(string header, string value)
        {
            hcol.Add(header, value);
        }
    }
    public class BitsRequest : RequestObject
    {
        int state;
        int maxLength = -1;
        int count;
        string sessionId;

        public int TotalLength { get; set; }
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        
        
        public int MaxLength {
            get { return maxLength; }
        }

        public BitsRequest():base()
        {
            state = 0;
            Method = WebRequestMethods.Http.Post;
            maxLength = Utils.Settings.GetSettingsInt("OneDrive.UploadChunkedSize", 16777216);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns header based on state of bits request</returns>
        public override WebHeaderCollection Headers
        {
            get
            {
                switch (state)
                {
                    case 0: return GetHeadersSessionStart;
                    case 1: return GetHeadersUpload;
                    case 2: return GetHeaderCloseSession;
                }
                return null;
            }
        }
        /// <summary>
        /// Set session id of upload
        /// </summary>
        /// <param name="session">Upload session id</param>
        public void SetSessionId(string session)
        {
            sessionId = session;
        }
        /// <summary>
        /// Method initiate new session
        /// </summary>
        public void StartSession()
        {
            state = 0;
        }
        /// <summary>
        /// Method for initiate commit session
        /// </summary>
        public void CommitSession()
        {
            state = 2;
        }
        /// <summary>
        /// Method prepare headers for start session
        /// </summary>
        /// <returns>Returns headers collection for start session</returns>
        protected WebHeaderCollection GetHeadersSessionStart
        {
            get
            {
                state = 1;
                WebHeaderCollection hrc = new WebHeaderCollection();
                hrc.Add("X-Http-Method-Override", "BITS_POST");
                hrc.Add("Authorization", "Bearer " + Connection.Token);
                hrc.Add("BITS-Packet-Type", "Create-Session");
                hrc.Add("BITS-Supported-Protocols", "{7df0354d-249b-430f-820d-3d2a9bef4931}");
                return hrc;
            }
        }
        /// <summary>
        /// Method prepare headers for upload fragment
        /// </summary>
        /// <returns>Returns header collection for fragment upload</returns>
        protected WebHeaderCollection GetHeadersUpload
        {
            get
            {
                int cl = ContentLength;
                WebHeaderCollection hrc = new WebHeaderCollection();
                hrc.Add("X-Http-Method-Override", "BITS_POST");
                hrc.Add("Authorization", "Bearer " + Connection.Token);
                hrc.Add("BITS-Packet-Type", "Fragment");
                hrc.Add("BITS-Session-Id", sessionId);
                hrc.Add("Content-Range", "bytes " + (MaxLength * count).ToString(CultureInfo.InvariantCulture) + "-" + ((MaxLength * count) + (cl - 1)).ToString(CultureInfo.InvariantCulture) + "/" + TotalLength);
                count++;
                return hrc;
            }
        }
        /// <summary>
        /// Method prepare headers for close session
        /// </summary>
        /// <returns>Returns header collection for closing session</returns>
        protected WebHeaderCollection GetHeaderCloseSession
        {
            get
            {
                ContentLength = 0;
                WebHeaderCollection hrc = new WebHeaderCollection();
                hrc.Add("X-Http-Method-Override", "BITS_POST");
                hrc.Add("Authorization", "Bearer " + Connection.Token);
                hrc.Add("BITS-Packet-Type", "Close-Session");
                hrc.Add("BITS-Session-Id", sessionId);
                return hrc;
            }
        }
    }
    public delegate void OnProgressChange(int bytes, double percent);
    public class RequestProgressHandler
    {
        public OnProgressChange ProgressChange { get; set; }
    }
}
