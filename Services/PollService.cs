using System;
using System.Collections.Generic;
using System.Linq;
using DarkSky.Poll.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Services;

namespace DarkSky.Poll.Services {
	public interface IPollService : IDependency {
		IEnumerable<PollOptionPart> GetOptions(int pollId);
		IEnumerable<PollSubmission> GetSubmissions(int pollId);
		IEnumerable<PollPart> GetPolls(int startIndex, int pageSize, string q);
		int CountPolls(string q);
		PollPart GetPoll(int id);
		int GetNextOptionPosition(int pollId);
		PollOptionPart GetOption(int id);
		void DeleteOption(PollOptionPart option);
		void DeletePoll(PollPart poll);
		PollSubmission GetSubmissionByIpAddress(string ipAddress);
		PollSubmission SubmitVote(int pollId, int optionId, string ipAddress);
	}

	[OrchardFeature("DarkSky.Poll")]
	public class PollService : IPollService {
		private readonly IContentManager _contentManager;
		private readonly IRepository<PollSubmission> _submissionRepository;
		private readonly ISessionLocator _sessionLocator;
		private readonly IClock _clock;
		private readonly IRepository<PollResponse> _responseRepository;

		public PollService(IContentManager contentManager, IRepository<PollSubmission> submissionRepository, ISessionLocator sessionLocator, IClock clock, IRepository<PollResponse> responseRepository) {
			_contentManager = contentManager;
			_submissionRepository = submissionRepository;
			_sessionLocator = sessionLocator;
			_clock = clock;
			_responseRepository = responseRepository;
		}

		public IEnumerable<PollOptionPart> GetOptions(int pollId) {
			return _contentManager.Query<PollOptionPart, PollOptionPartRecord>().Where(x => x.PollId == pollId).List();
		}

		public IEnumerable<PollSubmission> GetSubmissions(int pollId) {
			return _submissionRepository.Fetch(x => x.PollId == pollId);
		}

		public IEnumerable<PollPart> GetPolls(int startIndex, int pageSize, string q) {
			return _contentManager.Query<PollPart>("Poll").Slice(startIndex, pageSize);
		}

		public int CountPolls(string q) {
			return _contentManager.Query<PollPart>("Poll").Count();
		}

		public PollPart GetPoll(int id) {
			return _contentManager.Get<PollPart>(id);
		}

		public int GetNextOptionPosition(int pollId) {
			var lastOption = _contentManager.Query<PollOptionPart, PollOptionPartRecord>().Where(x => x.PollId == pollId).OrderByDescending(x => x.Position).Slice(0, 1).FirstOrDefault();
			return lastOption == null ? 0 : lastOption.Position + 1;
		}

		public PollOptionPart GetOption(int id) {
			return _contentManager.Get<PollOptionPart>(id);
		}

		public void DeleteOption(PollOptionPart option) {
			_contentManager.Remove(option.ContentItem);
		}

		public void DeletePoll(PollPart poll) {
			_contentManager.Remove(poll.ContentItem);
		}

		public PollSubmission GetSubmissionByIpAddress(string ipAddress) {
			return _submissionRepository.Get(x => x.IpAddress == ipAddress);
		}

		public PollSubmission SubmitVote(int pollId, int optionId, string ipAddress) {
			var submission = new PollSubmission {
				PollId = pollId,
				CreatedUtc = _clock.UtcNow,
				IpAddress = ipAddress
			};

			_submissionRepository.Create(submission);
			var response = new PollResponse {
				Submission = submission,
				OptionId = optionId
			};

			_responseRepository.Create(response);
			return submission;
		}

		public IEnumerable<PollResponse> GetResponses(int pollId) {
			var session = _sessionLocator.For(typeof (PollSubmission));
			var query = session.CreateQuery("select s from PollSubmission s, PollResponse r Where r.SubmissionId = s.Id and s.PollId = :pollId");
			query.SetInt32("pollId", pollId);

			return query.Enumerable<PollResponse>();
		}
	}
}