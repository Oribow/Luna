using Luna.Biz.DataTransferObjects;
using Luna.Biz.Services;
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

        SKRect mapBounds;
        PlayerPosition playerPosition;
        IReadOnlyCollection<SolarSystem> solarSystems;
        IReadOnlyCollection<PointOfInterest> sectors;

        readonly SceneService sceneService;
        readonly PlayerService playerService;

        public GalaxyMapViewModel(SceneService sceneService, PlayerService playerService)
        {
            this.sceneService = sceneService;
            this.playerService = playerService;

            Sectors = new PointOfInterest[] {
                    new PointOfInterest("Core Sector", new SKPoint(0, 0), false),
                    new PointOfInterest("Aquatos Sector", new SKPoint(500, -200), false),
                    new PointOfInterest("Helios Sector", new SKPoint(-400, 300), false) };

            MessagingCenter.Subscribe<PlayerService>(this, "player_started_traveling", (sender) => LoadPlayerData());

            LoadLocationData();
        }

        private async Task LoadLocationData()
        {
            var locs = await sceneService.GetLocations(App.PlayerId);
            SolarSystems = locs.Select(ls => new SolarSystem(ls.Position.ToSKPoint(), ls.Name, ls.HasBeenVisited, ls.SceneId)).ToArray();

            UpdateMapBounds();
            await LoadPlayerData();
        }

        private async Task LoadPlayerData()
        {
            var playerState = await playerService.GetPlayersState(App.PlayerId);

            var curPlayerScene = SolarSystems.Where(s => s.SceneId == playerState.CurrentSceneId).First();
            var prevPlayerScene = SolarSystems.Where(s => s.SceneId == playerState.PrevSceneId).First();

            PlayerPosition = new PlayerPosition(curPlayerScene, prevPlayerScene, playerState.LockoutEndUTC, playerState.LockoutStartUTC);

            if (PlayerPosition.IsTraveling)
            {
                Device.StartTimer(PlayerPosition.ArrivalUTC - DateTime.UtcNow, () => { OnPlayerArrives(); return false; });
            }
            else if (!PlayerPosition.Position.HasBeenVisited)
            {
                OnPlayerArrives();
            }
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

        private async Task OnPlayerArrives()
        {
            if (PlayerPosition.Position.HasBeenVisited)
                return;

            await sceneService.ArriveAtScene(App.PlayerId, PlayerPosition.Position.SceneId);
            await LoadLocationData();
        }
    }
}
