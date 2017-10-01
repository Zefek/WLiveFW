using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class Folder:Operational<Folder>
    {
        [WLive.WLive("from")]
        public From From { get; set; }
        [WLive.WLive("count")]
        public int Count { get; set; }
        [WLive.WLive("link")]
        public string Link { get; set; }
        [WLive.WLive("upload_location")]
        public string UploadLocation { get; set; }
        [WLive.WLive("is_embeddable")]
        public bool IsEmbeddable { get; set; }
        [WLive.WLive("type")]
        public string FType { get; set; }
        [WLive.WLive("created_time")]
        public DateTime CreatedTime { get; set; }
        [WLive.WLive("updated_time")]
        public DateTime UpdatedTime { get; set; }
        [WLive.WLive("client_updated_time")]
        public DateTime ClientUpdatedTime { get; set; }
        [WLive.WLive("shared_with")]
        public SharedWith SharedWith { get; set; }
        [WLive.WLive("sort_by", true)]
        public string SortBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns list of folders of current folder</returns>
        public IEnumerable<Folder> GetFolders()
        {
                try
                {
                    return GetFolders(System.IO.SearchOption.TopDirectoryOnly);
                }
                catch
                { throw; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="option">Option for search in current folder or is subfolders</param>
        /// <returns>Returns list of folders or subfolders of current fulder</returns>
        public IEnumerable<Folder> GetFolders(System.IO.SearchOption searchOption)
        {
            try
            {
                return GetFolders(Id, searchOption);
            }
            catch
            { throw; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderid">FOLDER_ID</param>
        /// <returns>Returns list of folders of folder based od location id</returns>
        public static IEnumerable<Folder> GetFolders(string folderId)
        {
            try
            {
                return GetFolders(folderId, System.IO.SearchOption.TopDirectoryOnly);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderid">FOLDER_ID</param>
        /// <param name="option">Option for search in current folder or is subfolders</param>
        /// <returns>Returns list of folders or subfolders of folder based od location id</returns>
        public static IEnumerable<Folder> GetFolders(string folderId, System.IO.SearchOption searchOption)
        {
            if (string.IsNullOrEmpty(folderId))
                folderId = "me/skydrive";
            Filter f = new Filter(FilterTypes.Folder | FilterTypes.Album);
            QueryParameters qp = new QueryParameters();
            qp.Add("filter", f.GetTypesForQuery);
            try
            {
                if (searchOption == System.IO.SearchOption.TopDirectoryOnly)
                    return Requester.Request<Folder>(new RequestObject { Url = UrlBuilder1.Build(folderId + "/files", qp) }).ToList();
                else
                {
                    List<Folder> ret = new List<Folder>();
                    foreach (Folder folder in Requester.Request<Folder>(new RequestObject { Url = UrlBuilder1.Build(folderId + "/files", qp) }))
                    {
                        ret.Add(folder);
                        ret.AddRange(GetFolders(folder.Id, searchOption));
                    }
                    return ret;
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
        /// <returns>Returns files in current folder</returns>
        public IEnumerable<File> GetFiles()
        {
                try
                {
                    return GetFiles(Id);
                }
                catch
                { throw; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type of file which you want to return. If you want to return more types, use type File</typeparam>
        /// <param name="filter">Types of file which you want to return.</param>
        /// <returns>Return files of current folder</returns>
        public IEnumerable<T> GetFiles<T>(FilterTypes filter)
        {
            try
            {
                return GetFiles<T>(Id, filter);
            }
            catch
            { throw; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderid">FOLDER_ID</param>
        /// <returns>Return files in folder get by folderid</returns>
        public static IEnumerable<File> GetFiles(string folderId)
        {
            try
            {
                if (string.IsNullOrEmpty(folderId))
                    folderId = "me/skydrive";
                return Requester.Request<File>(new RequestObject { Url = UrlBuilder1.Build(folderId + "/files") }).ToList();
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type of file which you want to return. If you want to return more types, use type File</typeparam>
        /// <param name="folderid">FOLDER_ID</param>
        /// <param name="filter">Types of file which you want to return.</param>
        /// <returns>Return files in folder get by folderid</returns>
        public static IEnumerable<T> GetFiles<T>(string folderId, FilterTypes filter)
        {
            QueryParameters qp = new QueryParameters();
            Filter f = new Filter(filter);
            qp.Add("filter", f.GetTypesForQuery);
            try
            {
                if (string.IsNullOrEmpty(folderId))
                    folderId = "me/skydrive";
                return Requester.Request<T>(new RequestObject { Url = UrlBuilder1.Build(folderId + "/files", qp) }).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
