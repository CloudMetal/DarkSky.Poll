using System.Linq;
using System.Web.Mvc;
using DarkSky.Poll.Services;
using Orchard;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Admin;
using Orchard.UI.Notify;

namespace DarkSky.Poll.Controllers {
	[OrchardFeature("DarkSky.Poll")]
	[Admin]
	public class OptionAdminController : Controller {
		private readonly IOrchardServices _services;
		private readonly IPollService _pollService;
		private readonly INotifier _notifier;
		public dynamic New { get; set; }
		public Localizer T { get; set; }

		public OptionAdminController(IOrchardServices services, IPollService pollService) {
			_services = services;
			_pollService = pollService;
			_notifier = _services.Notifier;
			New = _services.New;
			T = NullLocalizer.Instance;
		}


		public ActionResult Index(int id) {
			var poll = _pollService.GetPoll(id);
			var options = _pollService.GetOptions(id);
			var model = New.ViewModel(
				Poll: New.Poll(
					Id: id,
					Title: poll.DisplayName,
					Options: options.Select(x => New.PollOption(
						Id: x.Id,
						Text: x.Text,
						Position: x.Position,
						Content: x)).ToList()
				));
			return View(model);
		}

		[HttpPost]
		public ActionResult Delete(int pollId, int id) {
			var option = _pollService.GetOption(id);

			if(option == null)
				_notifier.Error(T("No option with ID {0} could be found", id));

			_pollService.DeleteOption(option);
			_notifier.Information(T("Option \"{0}\" has been removed", option.Text));
			return RedirectToAction("Index", new {id = pollId});
		}
	}
}