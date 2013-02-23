using System.Linq;
using System.Web.Mvc;
using DarkSky.Poll.Services;
using Orchard;
using Orchard.Core.Contents.Controllers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;

namespace DarkSky.Poll.Controllers {
	[OrchardFeature("DarkSky.Poll")]
	[Admin]
	public class PollAdminController : Controller {
		private readonly IOrchardServices _services;
		private readonly IPollService _pollService;
		private readonly INotifier _notifier;
		public dynamic New { get; set; }
		public Localizer T { get; set; }

		public PollAdminController(IOrchardServices services, IPollService pollService) {
			_services = services;
			_pollService = pollService;
			_notifier = _services.Notifier;
			New = _services.New;
			T = NullLocalizer.Instance;
		}
		
		public ActionResult Index(PagerParameters pagerParameters, string q) {
			var pager = new Pager(_services.WorkContext.CurrentSite, pagerParameters);
			var polls = _pollService.GetPolls(pager.GetStartIndex(), pager.PageSize, q).ToList();
			var count = _pollService.CountPolls(q);
			var model = New.ViewModel(
				Polls: polls.Select(x => New.Poll(
					Content: x,
					Id: x.Id,
					Title: x.DisplayName,
					Submissions: x.Submissions.Count,
					Created: x.Created)).ToList(),
				SearchTerm: q,
				Pager: New.Pager(pager).TotalItemCount(count)
			);
			return View(model);
		}

		[FormValueRequired("submit.Search"), HttpPost, ActionName("Index")]
		public ActionResult Search(PagerParameters pagerParameters, string q) {
			return RedirectToAction("Index", new { page = pagerParameters.Page, pageSize = pagerParameters.PageSize, q });
		}

		[HttpPost]
		public ActionResult Delete(int id) {
			var poll = _pollService.GetPoll(id);

			if (poll == null)
				_notifier.Error(T("No poll with ID {0} could be found", id));

			_pollService.DeletePoll(poll);
			_notifier.Information(T("Poll \"{0}\" has been removed", poll.DisplayName));
			return RedirectToAction("Index");
		}
	}
}