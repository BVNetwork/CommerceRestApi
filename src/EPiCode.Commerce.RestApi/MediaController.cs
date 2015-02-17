using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using EPiServer;
using EPiServer.Core;
using EPiServer.Security;
using EPiServer.ServiceLocation;

namespace EPiCode.Commerce.RestService
{
    public class MediaController : SecuredApiController
    {
        [HttpGet]
        public MediaData Get(int contentId)
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            ContentReference reference = new ContentReference(contentId);
            return contentRepository.Get<MediaData>(reference);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int contentId)
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            ContentReference reference = new ContentReference(contentId);
            contentRepository.Delete(reference, true, AccessLevel.NoAccess);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
