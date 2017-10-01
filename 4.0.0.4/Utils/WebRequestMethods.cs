using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WLive.Utils
{
    public static class WebRequestMethods
    {
        public static class Http
        {
            public const string Connect = System.Net.WebRequestMethods.Http.Connect;
            public const string Get = System.Net.WebRequestMethods.Http.Get;
            public const string Head = System.Net.WebRequestMethods.Http.Head;
            public const string MKCol = System.Net.WebRequestMethods.Http.MkCol;
            public const string Post = System.Net.WebRequestMethods.Http.Post;
            public const string Put = System.Net.WebRequestMethods.Http.Put;
            public const string Delete = "DELETE";
            public const string Copy = "COPY";
            public const string Move = "MOVE";
        }
    }
}
