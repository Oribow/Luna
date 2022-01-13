using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.ShipAISystems
{
    interface IShipAI
    {
        void SetOutput(IShipAIOutput output);
        void BecomesVisible();
        void Interact();
        void OnArrive();
        void OnJump();
    }
}
