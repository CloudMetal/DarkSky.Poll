using System.Linq;
using DarkSky.Poll.Models;
using DarkSky.Poll.Services;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;

namespace DarkSky.Poll.Handlers {
	[OrchardFeature("DarkSky.Poll")]
	public class PollOptionHandler : ContentHandler {
		private readonly IPollService _pollService;

		public PollOptionHandler(IRepository<PollOptionPartRecord> repository, IPollService pollService) {
			_pollService = pollService;
			Filters.Add(StorageFilter.For(repository));
			OnActivated<PollOptionPart>(OnPollOptionActivated);
		}

		private void OnPollOptionActivated(ActivatedContentContext context, PollOptionPart part) {
			part.PollField.Loader(() => _pollService.GetPoll(part.Record.PollId));
			part.PollField.Setter(x => {
				part.Record.PollId = x.Id; 
				return x;
			});
		}
	}
}