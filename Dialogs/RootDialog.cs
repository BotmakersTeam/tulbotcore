using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;


namespace SimpleEchoBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument as Activity;
            if (message.Type == ActivityTypes.ConversationUpdate)
            {
                if (message.MembersAdded != null && message.MembersAdded.Any())
                {

                    //var bot = message.MembersAdded.First();
                    if (!(message.MembersAdded.First().Name == "Bot"))
                        await context.PostAsync($" Welcome to TULBot! You can type \"info\"" +
                            $" to get info about my abilities.");
                }
            }
            else
            if (message.Type == ActivityTypes.Message)
            {
                string content;
                if (message.Text.ToLower().Contains("help"))
                {
                    context.PostAsync("")
                }
                if (message.Text.ToLower().Contains("wykladowca") || message.Text.ToLower().Contains("wykładowca"))
                {
                    //11 to skip wykladowca + space
                    content = message.Text.Substring(11);
                    //TODO go to db and find record then send it to user
                }
                else if (message.Text.ToLower().Contains("jednostka"))
                {
                    content = message.Text.Substring(9);
                    //go to db and find record, send to user;
                } else await context.PostAsync("Dont know that command sorry"); 

            }
    }
}