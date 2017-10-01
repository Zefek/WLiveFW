using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WLive.Utils;

namespace WLive.WLiveObjects
{
    public interface IOperationalFolders
    {
        void Copy(string newLocationId);
        void Move(string newLocationId);
        void Delete();
        void Update();
    }
    
    public interface IOperationalFiles : IOperationalFolders
    {
        void UpdateContent(ref Stream file);
        void UpdateContent(ref Stream file, bool append);
    }
    public abstract class AGetOp<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectId">Object ID - FOLDER_ID/FILE_ID/...</param>
        /// <returns>Return object get by their ID</returns>
        public static T Get(string objectId)
        {
            try
            {
                return Requester.Request<T>(new RequestObject { Url = UrlBuilder1.Build(objectId) }).ElementAt(0);
            }
            catch
            {
                throw;
            }
        }
    }
    public abstract class AOperational<T>:AGetOp<T>
    {
        internal class WLDestination
        {
            [WLive("destination", true)]
            public string Destination { get; set; }
        }

        static int downloadBufferSize = 512;

        [WLive.WLive("id")]
        public string Id { get; set; }
        [WLive.WLive("description", true)]
        public string Description { get; set; }
        [WLive.WLive("name", true)]
        public string Name { get; set; }
        [WLive.WLive("parent_id", false)]
        public string ParentId { get; set; }
        
        public static int DownloadBufferSize
        {
            get { return downloadBufferSize; }
            set { downloadBufferSize = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owoption">Overwrite option</param>
        /// <returns>Return string representation of overwrite option</returns>
        protected static string GetOverwriteOption(OverwriteOption overwriteOption)
        {
            switch (overwriteOption)
            {
                case OverwriteOption.ChooseNewName:return "choosenewname";
                case OverwriteOption.DoNotOverwrite:return "false";
                case OverwriteOption.Overwrite: return "true";
            }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location">FOLDER_ID</param>
        /// <param name="name">Name of object which you want to find</param>
        /// <returns>Return object if object exists in location otherwise null</returns>
        public static T GetByName(string location, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(location))
                location = "me/skydrive";
            name = name.ToUpper(CultureInfo.InvariantCulture);
            string type = Activator.CreateInstance(typeof(T)).GetType().Name.ToUpper(CultureInfo.InvariantCulture);
            foreach (File f in Folder.GetFiles(location))
            {
                if (string.Compare(type, WLType.File, StringComparison.Ordinal) == 0 && WLType.IsFile(f.FType))
                {
                    if (string.Compare(f.Name.ToUpper(CultureInfo.InvariantCulture), name, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return Get(f.Id);
                    }
                }
                else if ((string.Compare(type, WLType.Folder, StringComparison.Ordinal) == 0 || type == WLType.Album) && WLType.IsFolder(f.FType))
                {
                    if (string.Compare(f.Name.ToUpper(CultureInfo.InvariantCulture), name, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return Get(f.Id);
                    }
                }
                else
                    if (string.Compare(type, f.FType.ToUpper(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(f.Name.ToUpper(CultureInfo.InvariantCulture), name, StringComparison.OrdinalIgnoreCase) == 0)
                    return Get(f.Id);
            }
            return default(T);
        }
    }
    public abstract class Operational<T>:AOperational<T>, IOperationalFolders
    {
        /// <summary>
        /// Create new object
        /// </summary>
        /// <param name="locationid">Location id where you want object create</param>
        /// <param name="name">Name of object</param>
        /// <param name="description">Description object</param>
        /// <returns>Created object</returns>
        public static T Create(T value)
        {
            AOperational<T> obj = (value as AOperational<T>);
            if (string.IsNullOrEmpty(obj.ParentId))
                obj.ParentId = "me/skydrive";
            //string data = JSONDataBuilder.Build(obj); //"{\"name\": \"" + name + "\",\r\n \"description\": \"" + description + "\"}";
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj, AGetOpConverter<T>.Settings);
            try
            {
                RequestObject ro = new RequestObject { Url = UrlBuilder1.Build(obj.ParentId), Method = WebRequestMethods.Http.Post, ContentType = ContentType.ApplicationJson };
                ro.SetData(data);
                return Requester.Request<T>(ro).ElementAt(0);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Update object properties - name and description. You can use this method for rename object.
        /// </summary>
        public virtual void Update()
        {
            //string data = JSONDataBuilder.Build(this);
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(this, AGetOpConverter<T>.Settings);
            try
            {
                RequestObject ro = new RequestObject { Url = UrlBuilder1.Build(Id.ToString()), Method = WebRequestMethods.Http.Put, ContentType = ContentType.ApplicationJson };
                ro.SetData(data);
                Requester.Request<T>(ro);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Copy object into new location.
        /// </summary>
        /// <param name="newLocationId">Location id where you want to copy object.</param>
        public virtual void Copy(string newLocationId)
        {
            if (string.IsNullOrEmpty(newLocationId))
                newLocationId = "me/skydrive";
            try
            {
                WLDestination dest = new WLDestination { Destination = newLocationId };
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(dest, AGetOpConverter<T>.Settings);
                RequestObject ro = new RequestObject { Url = UrlBuilder1.Build(Id.ToString()), Method = WebRequestMethods.Http.Copy, ContentType = ContentType.ApplicationJson };
                //ro.SetData("{ \"destination\": \"" + newLocationId + "\"}");
                ro.SetData(data);
                Requester.Request<T>(ro);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// MOve object into new location
        /// </summary>
        /// <param name="newLocationId">Location id where you want to move object</param>
        public virtual void Move(string newLocationId)
        {
            if (string.IsNullOrEmpty(newLocationId))
                newLocationId = "me/skydrive";
            try
            {
                WLDestination dest = new WLDestination { Destination = newLocationId };
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(dest, AGetOpConverter<T>.Settings);
                RequestObject ro = new RequestObject { Url = UrlBuilder1.Build(Id.ToString()), Method = WebRequestMethods.Http.Move, ContentType = ContentType.ApplicationJson };
                ro.SetData(data);
                //ro.SetData("{ \"destination\": \"" + newLocationId + "\"}");
                Requester.Request<T>(ro);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Delete object
        /// </summary>
        public virtual void Delete()
        {
            try
            {
                Requester.Request<T>(new RequestObject { Url = UrlBuilder1.Build(Id.ToString()), Method = WebRequestMethods.Http.Delete });
            }
            catch
            {
                throw;
            }
        }
    }

    public abstract class AFiles<T>:AOperational<T>, IOperationalFiles
    {
        [WLive("source")]
        public string Source { get; set; }

        /// <summary>
        /// Create object with content
        /// </summary>
        /// <param name="locationid">Location id where you want to create object</param>
        /// <param name="name">Name of object</param>
        /// <param name="file">Object content</param>
        /// <param name="owoption">Overwrite option</param>
        /// <returns>Return created object</returns>
        public static T Create(T fileObject, Stream file, OverwriteOption overwriteOption)
        {
            return Create(fileObject, file, overwriteOption, null);
        }
        /// <summary>
        /// Create object with content and progress when upload
        /// </summary>
        /// <param name="locationid">Location id where you want to object create</param>
        /// <param name="name">Object name</param>
        /// <param name="file">Object content</param>
        /// <param name="owoption">Overwrite option</param>
        /// <param name="handler">Progress handler for watch progress when uploading</param>
        /// <returns>Returns created object</returns>
        public static T Create(T fileValue, Stream file, OverwriteOption overwriteOption, RequestProgressHandler handler)
        {
            T obj = default(T);
            AFiles<T> fileobj = (fileValue as AFiles<T>);
            if (string.IsNullOrEmpty((fileobj as AFiles<T>).ParentId))
                fileobj.ParentId = "me/skydrive";
            try
            {
                QueryParameters qp = new QueryParameters();
                qp.Add("overwrite", GetOverwriteOption(overwriteOption));
                RequestObject ro = new RequestObject { Url = UrlBuilder1.Build(fileobj.ParentId + "/files/" + fileobj.Name, qp), Method = WebRequestMethods.Http.Put, ContentType = ContentType.ApplicationJsonODataVerbose };
                ro.SetData(new byte[] { 0 });
                obj = Requester.Request<T>(ro).ElementAt(0);
                fileValue = Get((obj as AOperational<T>).Id);
            }
            catch
            {
                throw;
            }
            string[] lid = fileobj.ParentId.Split('.');
            string usrid = User.Get("").Id;
            try
            {
                UploadContent(file, usrid, fileobj.Name, lid[lid.Length - 1], 0, handler, null, obj);
            }
            catch
            {
                throw;
            }
            return obj;
        }
        /// <summary>
        /// Upload content of object
        /// </summary>
        /// <param name="file">Object content</param>
        /// <param name="usrid">Owner of location. If null or empty logged user is used</param>
        /// <param name="name">Object name</param>
        /// <param name="locationid">Location id where you want to create object</param>
        /// <param name="fposition">Position in stream. It is used when handled exception and continue upload</param>
        /// <param name="handler">Handler which is used for watch upload progress</param>
        /// <param name="br">Bits request object</param>
        /// <param name="resobj">Result object. It is created object given when exception durin uploading process</param>
        static void UploadContent(Stream file, string usrid, string name, string locationid, long fposition, RequestProgressHandler handler, BitsRequest br, object resobj)
        {
            if (br == null)
            {
                Uri url = new Uri("https://cid-" + usrid + ".users.storage.live.com/items/" + locationid + "/" + name);
                br = new BitsRequest { Url = url, TotalLength = (int)file.Length };
                br.StartSession();
                br.Method = WebRequestMethods.Http.Post;
                try
                {
                    Requester.Request<T>(br);
                }
                catch
                { throw; }
            }
            file.Position = fposition;
            while (file.Position < file.Length)
            {
                br.Method = WebRequestMethods.Http.Post;
                long position = file.Position;
                int c = br.Count;
                br.SetData(new byte[file.Length - file.Position > br.MaxLength ? br.MaxLength : file.Length - file.Position]);
                file.Read(br.GetData(), 0, br.ContentLength);
                try
                {
                    if (handler != null)
                        Requester.Request<T>(br, handler);
                    else
                        Requester.Request<T>(br);
                }
                catch (Exception e)
                {
                    br.Count = c;
                    throw new BitsException("", e, file, position, br, Continue, handler, resobj);
                }
            } 
            br.Method = WebRequestMethods.Http.Post;
            br.CommitSession();
            try
            {
                Requester.Request<T>(br);
            }
            catch (Exception e)
            {
                throw new BitsException("", e, file, file.Length, br, Continue, handler, resobj);
            }
        }
        /// <summary>
        /// Can continue upload when exception occured during upload
        /// </summary>
        /// <param name="e">Exception object</param>
        static void Continue(BitsException e)
        {
            try
            {
                UploadContent(e.Content, "", "", "", e.Position, e.Handler, e.Request, e.Result);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Delete current object
        /// </summary>
        public virtual void Delete()
        {
            try
            {
                Requester.Request<T>(new RequestObject { Url = UrlBuilder1.Build(Id.ToString()), Method = WebRequestMethods.Http.Delete });
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Move object into new location
        /// </summary>
        /// <param name="newLocationId">Location id where you want to move object</param>
        public virtual void Move(string newLocationId)
        {
            if (string.IsNullOrEmpty(newLocationId))
                newLocationId = "me/skydrive";
            try
            {
                WLDestination dest = new WLDestination { Destination = newLocationId };
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(dest, AGetOpConverter<T>.Settings);
                RequestObject ro = new RequestObject { Url = UrlBuilder1.Build(Id.ToString()), Method = WebRequestMethods.Http.Move, ContentType = ContentType.ApplicationJson };
                //ro.SetData("{ \"destination\": \"" + newLocationId + "\"}");
                ro.SetData(data);
                Requester.Request<T>(ro);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Copy object into location
        /// </summary>
        /// <param name="newLocationId">Location id where you want to copy object</param>
        public virtual void Copy(string newLocationId)
        {
            if (string.IsNullOrEmpty(newLocationId))
                newLocationId = "me/skydrive";
            try
            {
                WLDestination dest = new WLDestination { Destination = newLocationId };
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(dest, AGetOpConverter<T>.Settings); 
                RequestObject ro = new RequestObject { Url = UrlBuilder1.Build(Id), Method = WebRequestMethods.Http.Copy, ContentType = ContentType.ApplicationJson };
                //ro.SetData("{ \"destination\": \"" + newLocationId + "\"}");
                ro.SetData(data);
                Requester.Request<T>(ro);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Update properties of object. Use this method when you want to rename object
        /// </summary>
        public virtual void Update()
        {
            try
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(this, AGetOpConverter<T>.Settings);
                RequestObject ro = new RequestObject { Url = UrlBuilder1.Build(Id.ToString()), Method = WebRequestMethods.Http.Put, ContentType = ContentType.ApplicationJson };
                ro.SetData(data);
                Requester.Request<T>(ro);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Download content of object
        /// </summary>
        /// <param name="destination">Where downloaded data will be saved</param>
        /// <param name="handler">Handler for progress when downloading</param>
        public void Download(Stream destination, RequestProgressHandler handler)
        {
            if (destination == null)
                throw new ArgumentNullException("destination");
            try
            {
                T obj = Get(Id);
                System.Net.HttpWebRequest wr = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(new Uri((obj as AFiles<T>).Source));
                wr.Timeout = 20000;
                System.Net.HttpWebResponse wres = (System.Net.HttpWebResponse)wr.GetResponse();
                byte[] buffer;
                long read = 0;
                wres.GetResponseStream().CopyTo(destination);
                
                Stream s = wres.GetResponseStream();
                while (read < wres.ContentLength)
                {
                    int bufferlenght = Convert.ToInt32(wres.ContentLength - read > DownloadBufferSize ? DownloadBufferSize : wres.ContentLength - read);
                    buffer = new byte[bufferlenght];
                    
                    s.Read(buffer, 0, bufferlenght);
                    destination.Write(buffer, 0, bufferlenght);
                    read += buffer.Length;
                    if (handler != null)
                        handler.ProgressChange((int)read, read / (wres.ContentLength / (double)100));
                }
                
            }
            catch
            {
                throw;
            }
        }
        public virtual void UpdateContent(ref Stream file)
        {
            UpdateContent(ref file, false);
        }
        /// <summary>
        /// Uploaded content of object
        /// </summary>
        /// <param name="file">Object content</param>
        /// <param name="append">When true download content and append new content at the end of current content, else overwrite current content by new content</param>
        public virtual void UpdateContent(ref Stream file, bool append)
        {
            if (file == null)
                throw new ArgumentNullException("file");
            if (append)
            {
                string f = Path.GetTempFileName();
                using (FileStream fs = new FileStream(f, FileMode.Create))
                {
                    Download(fs, null);
                    file.CopyTo(fs);
                }
                file = new FileStream(f, FileMode.Open);
            }
            string usrid = User.Get("").Id;
            try
            {
                if (string.IsNullOrEmpty(ParentId))
                {
                    T obj = Get(Id);
                    string[] lid = (obj as AFiles<T>).ParentId.Split('.');
                    UploadContent(file, usrid, Name, lid[lid.Length - 1], 0, null, null, null);
                }
                else
                {
                    string[] lid = ParentId.Split('.');
                    UploadContent(file, usrid, Name, lid[lid.Length - 1], 0, null, null, null);
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationid">Location id where you want to search object</param>
        /// <param name="name">Name of object</param>
        /// <returns>Returns true when object exists in location otherwise false</returns>
        public static bool ExistsByName(string locationId, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(locationId))
                locationId = "me/skydrive";
            foreach (File f in Folder.GetFiles(locationId))
            {
                if (!WLType.IsFolder(f.FType))
                {
                    if (string.Compare(f.Name.ToUpper(CultureInfo.InvariantCulture), name.ToUpper(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase) == 0)
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationid">Location id where you want to search</param>
        /// <param name="objectid">Object id which you want to search</param>
        /// <returns>Returns true where object exists in location otherwise false</returns>
        public static bool ExistsById(string locationId, string objectId)
        {
            if (string.IsNullOrEmpty(objectId))
                throw new ArgumentNullException("objectId");
            if (string.IsNullOrEmpty(locationId))
                locationId = "me/skydrive";
            foreach (File f in Folder.GetFiles(locationId))
            {
                if (!WLType.IsFolder(f.FType))
                {
                    if (string.Compare(f.Id.ToUpper(CultureInfo.InvariantCulture), objectId.ToUpper(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase) == 0)
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns list of comments for object</returns>
        public IEnumerable<Comment> Comments
        {
            get
            {
                try
                {
                    return Requester.Request<Comment>(new RequestObject { Url = UrlBuilder1.Build(Id + "/comments") }).ToList();
                }
                catch
                {
                    throw;
                }
            }
        }
    }

    public abstract class AWithTag<T> : AFiles<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns tags of object</returns>
        public virtual IEnumerable<Tag> Tags
        {
            get
            {
                try
                {
                    return Requester.Request<Tag>(new RequestObject { Url = UrlBuilder1.Build(Id + "/tags") }).ToList();
                }
                catch
                {
                    throw;
                }
            }
        }
        public void CreateTag(Tag tag)
        {
            try
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(tag, AGetOpConverter<T>.Settings);
                RequestObject ro = new RequestObject { Url = UrlBuilder1.Build(Id + "/tags"), Method = WebRequestMethods.Http.Post, ContentType = ContentType.ApplicationJson };
                ro.SetData(data);
                Requester.Request<T>(ro);
            }
            catch
            {
                throw;
            }
        }
    }

    public delegate void ContinueHandler(BitsException exception);
    [Serializable]
    public class BitsException : Exception
    {
        public Stream Content { get; set; }
        public long Position { get; set; }
        public BitsRequest Request { get; set; }
        public RequestProgressHandler Handler { get; set; }
        public ContinueHandler Continue { get; set; }
        public object Result { get; set; }

        public BitsException()
        { }
        public BitsException(string message) : base(message)
        { }
        public BitsException(string message, Exception exception) : base(message, exception)
        { }
        protected BitsException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
        public BitsException(string message, Exception exception, Stream file, long position, BitsRequest request, ContinueHandler handler, RequestProgressHandler progressHandler, object result) :base(message, exception)
        {
            this.Position = position;
            this.Request = request;
            this.Content = file;
            this.Continue += handler;
            this.Handler = progressHandler;
            Result = result;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
