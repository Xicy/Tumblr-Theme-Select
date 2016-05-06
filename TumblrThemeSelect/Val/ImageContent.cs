using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TumblrThemeSelect.Val
{
    [DataContract]
    public class Meta
    {
        [DataMember(Name = "status")]
        public int Status { get; set; }
        [DataMember(Name = "msg")]
        public string Msg { get; set; }
    }

    [DataContract]
    public class Response
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "original_filename")]
        public string OriginalFilename { get; set; }
        [DataMember(Name = "rotation")]
        public bool Rotation { get; set; }
        [DataMember(Name = "upload_id")]
        public bool UploadId { get; set; }
    }

    [DataContract]
    public class ImageContent
    {
        [DataMember(Name = "meta")]
        public Meta Meta { get; set; }
        [DataMember(Name = "response")]
        public IList<Response> Response { get; set; }
    }
}