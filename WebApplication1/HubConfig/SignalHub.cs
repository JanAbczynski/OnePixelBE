using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace OnePixelBE.HubConfig

{
    public class SignalHub: Hub
    {
        public async Task<bool> Send(string someTextFromClient)
        {

            //  await Clients.Client(this.Context.ConnectionId).SendAsync("askServerResponse", tempString);
            await Clients.All.SendAsync("Send", someTextFromClient);
            return true;
        }
    }
}
