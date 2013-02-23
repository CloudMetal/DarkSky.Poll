using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DarkSky.Poll.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;

namespace DarkSky.Poll.Drivers {
	[OrchardFeature("DarkSky.Poll")]
	public class PollDriver : ContentPartDriver<PollPart> {
		private readonly IContentManager _contentManager;

		public PollDriver(IContentManager contentManager) {
			_contentManager = contentManager;
		}

		protected override DriverResult Display(PollPart part, string displayType, dynamic shapeHelper) {
			return ContentShape("Parts_Poll", () => {
				var optionShapes = (Func<IEnumerable<dynamic>>)(() => part.Options.OrderBy(x => x.Position).Select(x => _contentManager.BuildDisplay(x)).ToList());
				return shapeHelper.Parts_Poll(
					Options: part.Options,
					OptionShapes: optionShapes);
			});
		}

		protected override void Exporting(PollPart part, ExportContentContext context) {
			context.Element(part.PartDefinition.Name).SetAttributeValue("Mode", part.Mode);
			context.Element(part.PartDefinition.Name).SetAttributeValue("MaxAnswers", part.MaxAnswers);
		}

		protected override void Importing(PollPart part, ImportContentContext context) {
			context.ImportAttribute(part.PartDefinition.Name, "Mode", s => part.Mode = !string.IsNullOrWhiteSpace(s) ? (PollMode)Enum.Parse(typeof(PollMode), s) : PollMode.SingleChoice);
			context.ImportAttribute(part.PartDefinition.Name, "MaxAnswers", s => part.MaxAnswers = !string.IsNullOrWhiteSpace(s) ? XmlConvert.ToInt32(s) : default(int?));
		}
	}
}