using System;
using System.Linq;
using System.Web.Http;

using Sitecore.Diagnostics;
using Sitecore.Data;
using Sitecore.Publishing;

namespace Example.DevServices {
  using Sitecore.Foundation.Devops.Security;

  [RoutePrefix("devops/publishing")]
  [ServiceKeyAuth]
  public class PublishingController : ApiController {

    [HttpPost]
    [Route("publishSync")]
    public void PublishSync() {
      using (new Sitecore.SecurityModel.SecurityDisabler())
      {
        var admin = Sitecore.Security.Accounts.User.FromName(@"sitecore\admin", true);
        using (new Sitecore.Security.Accounts.UserSwitcher(admin))
        {
          var master = Database.GetDatabase("master");
          Log.Info("Starting publish master -> web", this);
          var options =
            master.Languages.Select(
              language =>
                new PublishOptions(
                  Database.GetDatabase("master"),
                  Database.GetDatabase("web"),
                  PublishMode.Full,
                  language,
                  DateTime.UtcNow) { Deep = true }).ToArray();
          var handle = PublishManager.Publish(options);
          var status = PublishManager.GetStatus(handle);
          while (!status.IsDone && !status.Expired)
          {
            System.Threading.Thread.Yield();
            status = PublishManager.GetStatus(handle);
          }
        }
      }
    }
  }
}
