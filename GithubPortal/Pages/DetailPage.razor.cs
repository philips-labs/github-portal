using System.Collections.Generic;
using Domain;
using Microsoft.AspNetCore.Components;

namespace GithubPortal.Pages
{
    public partial class DetailPage
    {
        [Parameter]
        public bool IsModalOpen { get; set; }
        [Parameter]
        public CrawlerResult SelectedRepo { get; set; }

        public List<string> TopicList = new List<string>
        {
            "innersource",
            "philips",
            "xrDLS"
        };

        private void CloseModal()
        {
            IsModalOpen = false;
        }

        private string GetActivityLogoPath(int activityScore)
        {
            if (activityScore > 2500)
            {
                return "/static/activity/level-5.png";
            }
            if (activityScore > 1000)
            {
                return "/static/activity/level-4.png";
            }
            if (activityScore > 300)
            {
                return "/static/activity/level-3.png";
            }
            if (activityScore > 150)
            {
                return "/static/activity/level-2.png";
            }
            if (activityScore > 50)
            {
                return "/static/activity/level-1.png";
            }

            return "/static/activity/level-0.png";
        }
    }
}
