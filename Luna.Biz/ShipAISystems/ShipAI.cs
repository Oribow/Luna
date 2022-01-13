using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.ShipAISystems
{
    public class ShipAI : IShipAI
    {
        IShipAIOutput output;
        Random random = new Random();

        public void BecomesVisible()
        {
            string[] greetings = new string[] {
                "Yo! What's up?",
                "Hi, glad to see you again.",
                "Hey!",
            };
            string g = greetings[random.Next(greetings.Length)];
            output.Say(g);
        }

        public void Interact()
        {
            string[] interactions = new string[]
            {
                "Are you trying to win a petting contest?",
                "You haven't petted any other jellys have you?",
                "Arrrrrrr"
            };
            string g = interactions[random.Next(interactions.Length)];
            output.Say(g);
        }

        public string OnArrive()
        {
            return "I wonder what we will find here...";
        }

        public string OnJump()
        {
            return "Of to a new place!";
        }

        public void SetOutput(IShipAIOutput output)
        {
            this.output = output;
        }
    }
}
