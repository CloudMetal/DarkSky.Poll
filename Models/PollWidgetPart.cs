using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.Core.Common.Utilities;

namespace DarkSky.Poll.Models {
	public class PollWidgetPart : ContentPart<PollWidgetPartRecord> {
		internal LazyField<PollPart> PollField = new LazyField<PollPart>();

		public PollPart Poll {
			get { return PollField.Value; }
		}

		public string DisplayType {
			get { return Record.DisplayType; }
			set { Record.DisplayType = value; }
		}
	}

	public class PollWidgetPartRecord : ContentPartRecord {
		public virtual string DisplayType { get; set; }
	}
}