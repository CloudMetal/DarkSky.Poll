namespace DarkSky.Poll.Models {
	public class PollResponse {
		public virtual int Id { get; set; }
		public virtual PollSubmission Submission { get; set; }
		public virtual int OptionId { get; set; }
	}
}