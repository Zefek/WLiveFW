using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class File:AFiles<File>
    {
        [WLive.WLive("from")]
        public From From { get; set; }
        [WLive.WLive("size")]
        public int Size { get; set; }
        [WLive.WLive("upload_location")]
        public string UploadLocation { get; set; }
        [WLive.WLive("comments_count")]
        public int CommentsCount { get; set; }
        [WLive.WLive("comments_enabled")]
        public bool CommentsEnabled { get; set; }
        [WLive.WLive("is_embeddable")]
        public bool IsEmbeddable { get; set; }
        [WLive.WLive("link")]
        public string Link { get; set; }
        [WLive.WLive("type")]
        public string FType { get; set; }
        [WLive.WLive("shared_with")]
        public SharedWith SharedWith { get; set; }
        [WLive.WLive("created_time")]
        public DateTime CreatedTime { get; set; }
        [WLive.WLive("updated_time")]
        public DateTime UpdatedTime { get; set; }
        [WLive.WLive("client_updated_time")]
        public DateTime ClientUpdatedTime { get; set; }
        [WLive.WLive("sort_by", true)]
        public string SortBy { get; set; }
    }
}
