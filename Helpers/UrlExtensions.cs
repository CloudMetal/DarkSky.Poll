using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.Mvc.Html;

namespace DarkSky.Poll.Helpers {
	public static class UrlExtensions {
		public static string ItemEditUrlWithReturnUrl(this UrlHelper url, IContent content, string returnUrl = null) {
			if (returnUrl == null)
				returnUrl = url.RequestContext.HttpContext.Request.Path;
			return url.ItemEditUrl(content) + "?returnUrl=" + returnUrl;
		}
	}
}