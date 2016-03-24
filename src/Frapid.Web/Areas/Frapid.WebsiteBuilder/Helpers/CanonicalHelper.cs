using System.Linq;
using System.Web;
using Frapid.Configuration;
using Frapid.Framework;

namespace Frapid.WebsiteBuilder.Helpers
{
    public static class CanonicalHelper
    {
        public static string ToCanonicalUrl(string url = "")
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                url = HttpContext.Current.Request.Url.PathAndQuery;
            }

            string domain = DbConvention.GetDomain();
            var approved = new DomainSerializer("DomainsApproved.json");
            var tenant = approved.Get().FirstOrDefault(x => x.GetSubtenants().Contains(domain.ToLowerInvariant()));

            if (tenant != null)
            {
                string protocol = HttpContext.Current.Request.IsSecureConnection ? "https://" : "http://";
                string domainName = protocol + tenant.DomainName;
                url = UrlHelper.CombineUrl(domainName, url);
                return url;
            }

            return string.Empty;
        }
    }
}