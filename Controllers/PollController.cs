using System.Web.Mvc;
using DarkSky.Poll.Services;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Themes;
using Orchard.UI.Notify;

namespace DarkSky.Poll.Controllers {
	[OrchardFeature("DarkSky.Poll")]
	[Themed]
	public class PollController : Controller {
		private readonly IPollService _pollService;
		private readonly INotifier _notifier;

		public PollController(IPollService pollService, INotifier notifier) {
			_pollService = pollService;
			_notifier = notifier;
			T = NullLocalizer.Instance;
		}

		public Localizer T { get; set; }

		public ActionResult Vote(int id, int optionId) {
			var ipAddress = Request.UserHostAddress;
			var submission = _pollService.GetSubmissionByIpAddress(ipAddress);

			if (submission != null) {
				_notifier.Error(T("Only one vote per IP address is allowed"));
			}
			else {
				_pollService.SubmitVote(id, optionId, ipAddress);
			}

			return Redirect(Request.UrlReferrer.PathAndQuery);
		}
	}
}