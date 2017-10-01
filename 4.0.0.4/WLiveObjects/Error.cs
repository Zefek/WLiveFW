using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WLive.WLiveObjects
{
    public class WLError
    {
        [WLive("error")]
        public WLError Err { get; set; }
        [WLive("error_description")]
        public string ErrorDescription { get; set; }
        [WLive("code")]
        public string Code { get; set; }
        [WLive("message")]
        public string Message { get; set; }
    }

    [Serializable]
    public class WLiveException : Exception
    {
        public WLiveException()
        { }
        public WLiveException(string message) : base(message)
        { }
        public WLiveException(string message, Exception exception) : base(message, exception)
        { }
        protected WLiveException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
        public string Error { get { return err; } }
        public string ErrorDescription { get { return errdesc; } }
        string err, errdesc;
        public WLiveException(string error, string description) : base(description)
        {
            err = error;
            errdesc = description;
        }
        public WLiveException(WLError liveError) : base(liveError == null ? "" : string.IsNullOrEmpty(liveError.ErrorDescription) ? liveError.Err.Message : liveError.ErrorDescription)
        {
            if (liveError == null)
                throw new ArgumentNullException("liveError");
            err = liveError.Err.Code;
            errdesc = string.IsNullOrEmpty(liveError.ErrorDescription) ? liveError.Err.Message : liveError.ErrorDescription;
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }
}
