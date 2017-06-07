using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;

namespace BotApplicationTest
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                //await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());

                SaveActivityDataToDB(activity);
                // Call NumberGuesserDialog
                // await Conversation.SendAsync(activity, () => new NumberGuesserDialog());
                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        public static void SaveActivityDataToDB(Activity activity)
        {
            // *************************
            // Log to Database
            // *************************
            // Instantiate the BotData dbContext
            Models.BotDataEntities DB = new Models.BotDataEntities();
            // Create a new UserLog object
            Models.UserLog NewUserLog = new Models.UserLog();
            // Set the properties on the UserLog object
            NewUserLog.Channel = activity.ChannelId;
            NewUserLog.UserID = activity.From.Id;
            NewUserLog.UserName = activity.From.Name;
            NewUserLog.created = DateTime.UtcNow;
            NewUserLog.Message = activity.Text.Truncate(500);
            // Add the UserLog object to UserLogs
            DB.UserLogs.Add(NewUserLog);
            // Save the changes to the database
            DB.SaveChanges();
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}