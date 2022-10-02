using Luna.Biz.DataTransferObjects;
using Luna.Biz.Services;
using Luna.Communications;
using Luna.Extensions;
using Luna.GalaxyMap.Testing;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luna.GalaxyMap
{
    class GalaxyMapViewModel : BaseViewModel
    {
        public IReadOnlyCollection<SolarSystem> SolarSystems
        {
            get => solarSystems;
            set => SetProperty(ref solarSystems, value);
        }
        public IReadOnlyCollection<PointOfInterest> Sectors
        {
            get => sectors;
            set => SetProperty(ref sectors, value);
        }
        public SKRect MapBounds
        {
            get => mapBounds;
            set => SetProperty(ref mapBounds, value);
        }
        public PlayerPosition PlayerPosition
        {
            get => playerPosition;
            set => SetProperty(ref playerPosition, value);
        }
        public bool IsContextMenuVisible
        {
            get => isContextMenuVisible;
            set => SetProperty(ref isContextMenuVisible, value);
        }
        public ICommand ExploreSystem { get; }
        public ICommand LeaveSystem { get; }
        public bool IsJumpSelectionVisible
        {
            get => isJumpSelectionVisible;
            set => SetProperty(ref isJumpSelectionVisible, value);
        }
        public IReadOnlyList<TravelOptionViewModel> TravelOptions
        {
            get => travelOptions;
            set => SetProperty(ref travelOptions, value);
        }

        SKRect mapBounds;
        PlayerPosition playerPosition;
        IReadOnlyCollection<SolarSystem> solarSystems;
        IReadOnlyCollection<PointOfInterest> sectors;
        bool isContextMenuVisible = false;
        bool isJumpSelectionVisible = false;
        IReadOnlyList<TravelOptionViewModel> travelOptions;

        readonly SceneService sceneService;
        readonly PlayerService playerService;

        public GalaxyMapViewModel(SceneService sceneService, PlayerService playerService)
        {
            this.sceneService = sceneService;
            this.playerService = playerService;

            ExploreSystem = new Command(HandleExploreSystem);
            LeaveSystem = new Command(HandleLeaveSystem);

            Sectors = new PointOfInterest[] {
                    new PointOfInterest("Core Sector", new SKPoint(0, 0), false),
                    new PointOfInterest("Aquatos Sector", new SKPoint(500, -200), false),
                    new PointOfInterest("Helios Sector", new SKPoint(-400, 300), false) };

            MessagingCenter.Subscribe<PlayerService>(this, "player_started_traveling", (sender) => LoadData());

            LoadData();
        }

        private async Task LoadData()
        {
            var locs = await sceneService.GetLocations(App.PlayerId);

            /*var newLocs = new LocationDTO[100];
            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                if (i < locs.Length)
                    newLocs[i] = locs[i];
                else
                    newLocs[i] = new LocationDTO(new Vector2((float)(random.NextDouble() * 3000), (float)(random.NextDouble() * 3000)), "Name", Guid.NewGuid());
            }*/
            SolarSystems = locs.Select(ls =>
            {
                Random random = new Random(ls.SceneId.GetHashCode());
                var starInstance = StarClass.GenerateStarInstance(random);

                return new SolarSystem(ls.Position.ToSKPoint(), ls.Name, ls.SceneId, starInstance.Scale, starInstance.Tint);
            }
            ).ToArray();

            UpdateMapBounds();

            var playerState = await playerService.GetPlayersState(App.PlayerId);

            var curPlayerScene = SolarSystems.Where(s => s.SceneId == playerState.CurrentSceneId).First();
            var prevPlayerScene = SolarSystems.Where(s => s.SceneId == playerState.PrevSceneId).First();

            PlayerPosition = new PlayerPosition(curPlayerScene, prevPlayerScene, playerState.LockoutEndUTC, playerState.LockoutStartUTC);
        }

        private void UpdateMapBounds()
        {
            if (SolarSystems.Count == 0)
                return;

            SKPoint min = SolarSystems.First().Position;
            SKPoint max = SolarSystems.First().Position;

            foreach (var ss in solarSystems)
            {
                min = SKPointExtensions.Min(min, ss.Position);
                max = SKPointExtensions.Max(max, ss.Position);
            }

            // enlarge bounds
            min -= new SKPoint(10, 10);
            max += new SKPoint(10, 10);

            MapBounds = new SKRect(min.X, min.Y, max.X, max.Y);
        }

        private void HandleExploreSystem()
        {
            IsContextMenuVisible = false;
            App.Current.MainPage.Navigation.PushAsync(new QuestLogPage(PlayerPosition.Position.SceneId));
        }

        private async void HandleLeaveSystem()
        {
            IsContextMenuVisible = false;
            IsJumpSelectionVisible = true;

            var options = await playerService.GetNextLocationOptions(App.PlayerId);
            TravelOptions = options.Select(op => new TravelOptionViewModel(op, HandleTravelHere))
                .ToArray();
        }

        private async void HandleTravelHere(Guid sceneId)
        {
            IsJumpSelectionVisible = false;
            IsContextMenuVisible = false;
            await playerService.LetPlayerTravelTo(App.PlayerId, sceneId);
            await LoadData();
        }
    }
}
