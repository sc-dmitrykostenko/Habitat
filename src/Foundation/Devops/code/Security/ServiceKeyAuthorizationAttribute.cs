namespace Sitecore.Foundation.Devops.Security
{
  using System.Linq;
  using System.Net.Http;
  using System.Web.Http;
  using System.Web.Http.Controllers;

  using Sitecore.Configuration;
  using Sitecore.Diagnostics;

  public class ServiceKeyAuthAttribute : AuthorizeAttribute
  {
    protected override bool IsAuthorized(HttpActionContext actionContext)
    {
      var queryString = actionContext.Request.GetQueryNameValuePairs().ToDictionary(p => p.Key, p => p.Value);

      string validKey = Factory.GetString("devops/sharedSecret", false);
      if (string.IsNullOrEmpty(validKey))
      {
        Log.Error($"Access denied to {actionContext.Request.RequestUri}: api key is not configured", this);
      }

      string actualKey;
      if (!queryString.TryGetValue("apikey", out actualKey))
      {
        Log.Error($"Access denied to {actionContext.Request.RequestUri}: missing apikey.", this);
        return false;
      }

      if (actualKey == validKey)
      {
        return true;
      }

      Log.Error($"Access denied to {actionContext.Request.RequestUri}: invalid apikey {actualKey}.", this);
      return false;
    }
  }
}