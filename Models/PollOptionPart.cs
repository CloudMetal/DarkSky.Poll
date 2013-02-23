using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.Core.Common.Utilities;
using Orchard.Core.Title.Models;

namespace DarkSky.Poll.Models {
	public class PollOptionPart : ContentPart<PollOptionPartRecord> {
		internal LazyField<PollPart> PollField = new LazyField<PollPart>();

		public PollPart Poll {
			get { return PollField.Value; }
			set { PollField.Value = value; }
		}

		public string Text {
			get { return this.As<TitlePart>().Title; }
			set { this.As<TitlePart>().Title = value; }
		}

		public int Position {
			get { return Record.Position; }
			set { Record.Position = value; }
		}
	}

	public class PollOptionPartRecord : ContentPartRecord {
		public virtual int PollId { get; set; }
		public virtual int Position { get; set; }
	}
}