using System.Linq;
using DarkSky.Poll.Models;
using DarkSky.Poll.Services;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;

namespace DarkSky.Poll.Handlers {
	[OrchardFeature("DarkSky.Poll")]
	public class PollHandler : ContentHandler {
		private readonly IPollService _pollService;

		public PollHandler(IRepository<PollPartRecord> repository, IPollService pollService) {
			_pollService = pollService;
			Filters.Add(StorageFilter.For(repository));
			OnActivated<PollPart>(OnPollActivated);
		}

		private void OnPollActivated(ActivatedContentContext context, PollPart part) {
			part.OptionsField.Loader(() => _pollService.GetOptions(part.Id).ToList());
			part.SubmissionsField.Loader(() => _pollService.GetSubmissions(part.Id).ToList());
		}
	}
}