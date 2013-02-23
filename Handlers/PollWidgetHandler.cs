using System.Collections.Generic;
using System.Linq;
using DarkSky.Poll.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;

namespace DarkSky.Poll.Handlers {
	[OrchardFeature("DarkSky.Poll")]
	public class PollWidgetHandler : ContentHandler {
		
		public PollWidgetHandler(IRepository<PollWidgetPartRecord> repository) {
			Filters.Add(StorageFilter.For(repository));
			OnActivated<PollWidgetPart>(OnPollWidgetActivated);
		}

		private void OnPollWidgetActivated(ActivatedContentContext context, PollWidgetPart part) {
			part.PollField.Loader(() => {
				var pollField = ((dynamic) part.Fields.Single(x => x.Name == "Poll"));
				var poll = ((IEnumerable<ContentItem>) pollField.ContentItems).FirstOrDefault();
				return poll != null ? poll.As<PollPart>() : default(PollPart);
			});
		}
	}
}