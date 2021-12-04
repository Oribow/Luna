using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.Models
{
    public class VisitedScene
    {
        public int Id { get; set; }
        public Guid LocationId { get; set; }
        public Player Player { get; set; }
        public int PlayerId { get; set; }
        public List<AvailableQuest> AvailableQuests { get; set; } = new List<AvailableQuest>();

        private VisitedScene() { }
        public VisitedScene(Guid locationId)
        {
            LocationId = locationId;
        }
    }
}
