using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class Audio : AFiles<Audio>
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
        [WLive.WLive("title", true)]
        public string Title { get; set; }
        [WLive.WLive("artist", true)]
        public string Artist { get; set; }
        [WLive.WLive("album", true)]
        public string Album { get; set; }
        [WLive.WLive("album_artist", true)]
        public string AlbumArtist { get; set; }
        [WLive.WLive("genre", true)]
        public string Genre { get; set; }
        [WLive.WLive("duration")]
        public int Duration { get; set; }
        [WLive.WLive("picture")]
        public string Picture { get; set; }
        [WLive.WLive("shared_with")]
        public SharedWith SharedWith { get; set; }
        [WLive.WLive("created_time")]
        public DateTime CreatedTime { get; set; }
        [WLive.WLive("updated_time")]
        public DateTime UpdatedTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns audio picture if picture attribute is not empty otherwise null</returns>
        public Bitmap GetPicture
        {
            get
            {
                if (!string.IsNullOrEmpty(Picture))
                {
                    try
                    {
                        HttpWebRequest wreq = (HttpWebRequest)WebRequest.Create(new Uri(Picture));
                        using (HttpWebResponse wres = (HttpWebResponse)wreq.GetResponse())
                            return new Bitmap(wres.GetResponseStream());
                    }
                    catch
                    {
                        throw;
                    }
                }
                return null;
            }
        }
    }
}
