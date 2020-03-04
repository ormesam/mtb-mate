﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Tracked.Contexts;
using Tracked.Models;
using Xamarin.Forms;

namespace Tracked.Screens.Settings {
    public class ExploreSegmentsScreenViewModel : ViewModelBase {
        public ExploreSegmentsScreenViewModel(MainContext context) : base(context) {
        }

        public ObservableCollection<Segment> Segments => Model.Instance.Segments;

        public override string Title => "Segments";

        public async Task AddSegment(INavigation nav) {
            await Context.UI.GoToCreateSegmentScreenAsync(nav);
        }

        public async Task GoToSegment(INavigation nav, Segment segment) {
            await Context.UI.GoToSegmentScreenAsync(nav, segment);
        }
    }
}