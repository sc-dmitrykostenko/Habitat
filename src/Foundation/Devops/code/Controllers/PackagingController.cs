namespace Sitecore.Foundation.Devops.Controllers {

  using System;
  using System.Linq;
  using System.Net;
  using System.Net.Http;
  using System.Web.Http;

  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.Foundation.Devops.Security;
  using Sitecore.Install;
  using Sitecore.Install.Framework;
  using Sitecore.IO;
  using Sitecore.Publishing;
  using Sitecore.SecurityModel;

  [RoutePrefix("devops/packages")]
  [ServiceKeyAuth]
  public class PackagingController : ApiController {

    [HttpPost]
    [Route("build")]
    public void BuildPackage(string projectPath, string packagePath) {

      try
      {
        string fullProjectPath = FileUtil.MapPath(projectPath);
        string fullPackagePath = FileUtil.MapPath(packagePath);

        using (new SecurityDisabler())
        {
          using (new DatabaseSwitcher(Database.GetDatabase("core")))
          {
            PackageGenerator.GeneratePackage(fullProjectPath, fullPackagePath, new SimpleProcessingContext(new NullOutput()));
          }
        }
      }
      catch (Exception e)
      {
        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(e.ToString())});
      }
    }

  }
}
