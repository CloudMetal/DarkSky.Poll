using System;
using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.Core.Common.Models;
using Orchard.Core.Common.Utilities;

namespace DarkSky.Poll.Models {
	public class PollPart : ContentPart<PollPartRecord> {
		internal LazyField<IList<PollOptionPart>> OptionsField = new LazyField<IList<PollOptionPart>>();
		internal LazyField<IList<PollSubmission>> SubmissionsField = new LazyField<IList<PollSubmission>>();
		
		public PollMode Mode {
			get { return Record.Mode; }
			set { Record.Mode = value; }
		}

		public int? MaxAnswers {
			get { return Record.MaxAnswers; }
			set { Record.MaxAnswers = value; }
		}

		public IList<PollOptionPart> Options {
			get { return OptionsField.Value; }
		}

		public IList<PollSubmission> Submissions {
			get { return SubmissionsField.Value; }
		}

		public string DisplayName {
			get { return ContentItem.ContentManager.GetItemMetadata(this).DisplayText; }
		}

		public DateTime Created {
			get { return this.As<CommonPart>().CreatedUtc.Value; }
		}
	}

	public class PollPartRecord : ContentPartRecord {
		public virtual PollMode Mode { get; set; }
		public virtual int? MaxAnswers { get; set; }
	}
}