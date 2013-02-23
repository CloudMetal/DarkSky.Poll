using Orchard;
using Orchard.Environment.Extensions;
using Orchard.UI.Navigation;

namespace DarkSky.Poll {
	[OrchardFeature("DarkSky.Poll")]
	public class AdminMenu : Component, INavigationProvider {
		public string MenuName { get { return "admin"; } }

		public void GetNavigation(NavigationBuilder builder) {

			builder
				.AddImageSet("polls")
				.Add(T("Polls"), "3", polls => polls
					.LinkToFirstChild(true)
					.Add(T("Polls"), "2", item => item.Action("Index", "PollAdmin", new { area = "DarkSky.Poll" }))
					.Add(T("Create Poll"), "1", item => item.Action("Create", "Admin", new { area = "Contents", id = "Poll" }).AddClass("create-poll"))
				);
		}
	}
}