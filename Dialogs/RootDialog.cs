using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using SimpleEchoBot.Models;

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
                        await context.PostAsync($" Cześć! to jest bot, który nada Ci różną informację" +
                            $" o Politechnice Łódzkiej. Napisz \"help\"" +
                            $"dla otrzymania informacji o tym co aktualnie umiem.");
                }
            }
            else
            if (message.Type == ActivityTypes.Message)
            {
                string content;
                if (message.Text.ToLower().Contains("help"))
                {
                    context.PostAsync("");
                }
                if (message.Text.ToLower().Contains("wykladowca") || message.Text.ToLower().Contains("wykładowca"))
                {
                    
                    //11 to skip wykladowca + space
                    content = message.Text.Substring(11);
                    //TODO go to db and find record then send it to user
                    using (var dbcontext = new tulbotdevEntities1())
                    {
                        // Query for all blogs with names starting with B 
                        var teacher = dbcontext.peoples
                            .Where(b => b.Surname == content)
                            .FirstOrDefault();
                        if (teacher != null)
                        {
                            var depart = dbcontext.department.Find(teacher.Department);
                            await context.PostAsync($"{teacher.Title} {teacher.Name} {teacher.Surname}," +
                                $"  jednostka:{depart.Short} , konsultacje: {teacher.Consultation}, telefon: {teacher.Tel}");
                        }
                        else await context.PostAsync("Nie znalazłem nikogo z takim nazwiskiem");
                    }
                }
                else if (message.Text.ToLower().Contains("jednostka"))
                {
                    content = message.Text.Substring(10);
                    //go to db and find record, send to user;
                    using (var dbcontext = new tulbotdevEntities1())
                    {
                        var departm = dbcontext.department
                            .Where(b => b.Short.ToLower() == content.ToLower() ||
                            b.Code.ToLower() == content.ToLower() ||
                            b.Name.ToLower() == content.ToLower())
                            .FirstOrDefault();
                        if (departm != null)
                        {
                            await context.PostAsync($"{departm.Name}, skrót: {departm.Short}, " +
                                $"strona internetowa: {departm.Website} telefon kontaktowy: {departm.Tel}" +
                                $", adresa: {departm.Adress}");
                        }else await context.PostAsync("Nie znalazłem jednostki o takiej nazwie");
                    }
                }
                else await context.PostAsync("Dont know that command sorry");

            }
        }
    }
}