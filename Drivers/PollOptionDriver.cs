using System.Linq;
using System.Xml;
using DarkSky.Poll.Models;
using DarkSky.Poll.Services;
using DarkSky.Poll.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Common.Models;
using Orchard.Environment.Extensions;

namespace DarkSky.Poll.Drivers {
	[OrchardFeature("DarkSky.Poll")]
	public class PollOptionDriver : ContentPartDriver<PollOptionPart> {
		private readonly IWorkContextAccessor _wca;
		private readonly IPollService _pollService;

		public PollOptionDriver(IWorkContextAccessor wca, IPollService pollService) {
			_wca = wca;
			_pollService = pollService;
		}

		protected override string Prefix {
			get { return "PollOption"; }
		}

		protected override DriverResult Editor(PollOptionPart part, dynamic shapeHelper) {
			return ContentShape("Parts_PollOption_Edit", () => {
				var pollId = part.Id != 0 ? part.Poll.Id : int.Parse(_wca.GetContext().HttpContext.Request.QueryString["pollId"]);
				var viewModel = new PollOptionViewModel {
					PollId = pollId,
					Position = part.Id == 0 ? _pollService.GetNextOptionPosition(pollId) : part.Position
				};
				return shapeHelper.EditorTemplate(TemplateName: "Parts/PollOption", Model: viewModel, Prefix: Prefix);
			});
		}

		protected override DriverResult Editor(PollOptionPart part, IUpdateModel updater, dynamic shapeHelper) {
			var viewModel = new PollOptionViewModel();
			if (updater.TryUpdateModel(viewModel, Prefix, null, null)) {
				part.Poll = _pollService.GetPoll(viewModel.PollId);
				part.Position = viewModel.Position;
			}
			return Editor(part, shapeHelper);
		}

		protected override void Exporting(PollOptionPart part, ExportContentContext context) {
			context.Element(part.PartDefinition.Name).SetAttributeValue("Poll", context.ContentManager.GetItemMetadata(part.Poll).Identity.ToString());
			context.Element(part.PartDefinition.Name).SetAttributeValue("Position", part.Position);
		}

		protected override void Importing(PollOptionPart part, ImportContentContext context) {
			context.ImportAttribute(part.PartDefinition.Name, "Position", s => part.Position = !string.IsNullOrWhiteSpace(s) ? XmlConvert.ToInt32(s) : 0);

		}

		protected override void Imported(PollOptionPart part, ImportContentContext context) {
			context.ImportAttribute(part.PartDefinition.Name, "Poll", s => {
				part.Poll = context.GetItemFromSession(s).As<PollPart>();
			});
		}
	}
}