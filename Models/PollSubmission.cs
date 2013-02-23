using System;
using System.Collections.Generic;

namespace DarkSky.Poll.Models {
	public class PollSubmission {
		public virtual int Id { get; set; }
		public virtual int PollId { get; set; }
		public virtual int? UserId { get; set; }
		public virtual string IpAddress { get; set; }
		public virtual DateTime CreatedUtc { get; set; }
		public virtual IList<PollResponse> Responses { get; set; }

		public PollSubmission() {
// ReSharper disable DoNotCallOverridableMethodsInConstructor
			Responses = new List<PollResponse>();
// ReSharper restore DoNotCallOverridableMethodsInConstructor
		}
	}
}