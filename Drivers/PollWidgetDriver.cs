using System;
using DarkSky.Poll.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace DarkSky.Poll.Drivers {
	[OrchardFeature("DarkSky.Poll")]
	public class PollWidgetDriver : ContentPartDriver<PollWidgetPart> {
		private readonly IContentManager _contentManager;

		public PollWidgetDriver(IContentManager contentManager) {
			_contentManager = contentManager;
		}

		protected override string Prefix {
			get { return "PollWidget"; }
		}

		protected override DriverResult Display(PollWidgetPart part, string displayType, dynamic shapeHelper) {
			return ContentShape("Parts_PollWidget", () => shapeHelper.Parts_PollWidget(
				Poll: part.Poll,
				PollShape: (Func<dynamic>)(() => _contentManager.BuildDisplay(part.Poll, part.DisplayType))));
		}

		protected override DriverResult Editor(PollWidgetPart part, dynamic shapeHelper) {
			return ContentShape("Parts_PollWidget_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts/PollWidget", Model: part, Prefix: Prefix));
		}

		protected override DriverResult Editor(PollWidgetPart part, IUpdateModel updater, dynamic shapeHelper) {
			updater.TryUpdateModel(part, Prefix, null, null);
			return Editor(part, shapeHelper);
		}
	}
}