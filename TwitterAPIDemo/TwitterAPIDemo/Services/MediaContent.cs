using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TwitterAPIDemo.Services
{
    public class MediaContent
    {
        public MediaContent()
        {

        }
        public MultipartFormDataContent MediaUpload(string mediaPath, string mediaContent)
        {
            byte[] imgdata = System.IO.File.ReadAllBytes(mediaPath);

            var imageContent = new ByteArrayContent(imgdata);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(imageContent, mediaContent);
            return multipartContent;
        }
    }
}
