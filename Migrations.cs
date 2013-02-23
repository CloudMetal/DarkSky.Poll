using System;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace DarkSky.Poll {
	[OrchardFeature("DarkSky.Poll")]
	public class Migrations : DataMigrationImpl {
		public int Create() {
			SchemaBuilder.CreateTable("PollPartRecord", t => t
				.ContentPartRecord()
				.Column<string>("Mode")
				.Column<int>("MaxAnswers", c => c.Nullable()));

			SchemaBuilder.CreateTable("PollOptionPartRecord", t => t
				.ContentPartRecord()
				.Column<int>("PollId")
				.Column<int>("Position", c => c.NotNull()));

			SchemaBuilder.CreateTable("PollSubmission", t => t
				.Column<int>("Id", c => c.Identity().PrimaryKey())
				.Column<int>("PollId")
				.Column<int>("UserId")
				.Column<string>("IpAddress", c => c.WithLength(50))
				.Column<DateTime>("CreatedUtc"));

			SchemaBuilder.CreateTable("PollResponse", t => t
				.Column<int>("Id", c => c.Identity().PrimaryKey())
				.Column<int>("Submission_Id")
				.Column<int>("OptionId"));

			SchemaBuilder.CreateTable("PollWidgetPartRecord", t => t
				.ContentPartRecord()
				.Column<string>("DisplayType"));

			ContentDefinitionManager.AlterPartDefinition("PollPart", p => p.Attachable(false));
			ContentDefinitionManager.AlterTypeDefinition("Poll", t => t
				.WithPart("CommonPart")
				.WithPart("IdentityPart")
				.WithPart("TitlePart")
				.WithPart("AutoroutePart", p => p
					.WithSetting("AutorouteSettings.AllowCustomPattern", "true")
					.WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", "false")
					.WithSetting("AutorouteSettings.PatternDefinitions", "[{Name:'Title', Pattern: 'poll/{Content.Slug}', Description: 'poll/my-poll'}]")
					.WithSetting("AutorouteSettings.DefaultPatternIndex", "0"))
				.WithPart("PollPart")
				.Creatable(false)
				.Draftable(false));

			ContentDefinitionManager.AlterPartDefinition("PollOptionPart", p => p.Attachable(false));
			ContentDefinitionManager.AlterTypeDefinition("PollOption", p => p
				.WithPart("CommonPart")
				.WithPart("IdentityPart")
				.WithPart("TitlePart")
				.WithPart("PollOptionPart")
				.Creatable(false)
				.Draftable(false));

			ContentDefinitionManager.AlterPartDefinition("PollWidgetPart", p => p
				.WithField("Poll", f => f
					.OfType("ContentPickerField")
					.WithSetting("ContentPickerFieldSettings.DisplayedContentTypes", "PollPart")
					.WithSetting("ContentPickerFieldSettings.Required", "True"))
				.Attachable(false));

			ContentDefinitionManager.AlterTypeDefinition("PollWidget", t => t
				.WithPart("CommonPart")
				.WithPart("WidgetPart")
				.WithPart("BodyPart")
				.WithPart("PollWidgetPart")
				.Creatable(false)
				.Draftable(false)
				.WithSetting("Stereotype", "Widget"));

			return 1;
		}
	}
}