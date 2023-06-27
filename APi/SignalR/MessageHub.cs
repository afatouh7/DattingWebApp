using Api.Data;
using Api.Interfaces;
using APi.Data;
using APi.Dtos;
using APi.Entities;
using APi.Extensions;
using APi.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace APi.SignalR
{
    public class MessageHub :Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<PresencHub> _presenceHub;
        private readonly PresenceTracker _tracker;

        public MessageHub(IMessageRepository  messageRepository,IMapper mapper, IUserRepository userRepository,
            IHubContext<PresencHub> presenceHub,PresenceTracker tracker)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _presenceHub = presenceHub;
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext= Context.GetHttpContext();
            var otherUSer= httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(Context.User.GetUsername(), otherUSer);
            await Groups.AddToGroupAsync(Context.ConnectionId,groupName);
            await AddtoGroup(Context,groupName);

            var messages = await _messageRepository.GetMessageThread(Context.User.GetUsername(), otherUSer);

            await Clients.Group(groupName).SendAsync("ReciveMessageThread", messages);
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await ReomveFromMessageGroup(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessage(CreateMessageDto  createMessageDto)
        {

            var usernme =Context.User.GetUsername();
            if (usernme == createMessageDto.RecipientsUsername.ToLower()) 
                 throw new HubException("You cannot sent Message to yourself");
            var sender = await _userRepository.GetUserByUserNameAsync(usernme);
            var respient = await _userRepository.GetUserByUserNameAsync(createMessageDto.RecipientsUsername);

            if (respient == null) throw new HubException("not found user");
            var message = new Message
            {
                Sender = sender,
                Recipient = respient,
                SenderUserName = sender.UserName,
                RecipientUsername = respient.UserName,
                Content = createMessageDto.Content


            };
            var groupName = GetGroupName(sender.UserName, respient.UserName);
            var group = await _messageRepository.GetMessageGroup(groupName);
            if(group.Connection.Any(x=>x.Username==respient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connections= await _tracker.GetConnectionForUser(usernme);
                if(createMessageDto != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                        new { usernme = sender.UserName, knownAs = sender.KnownAs });
                }
            }


            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync())
            {
               
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }


        }

        private async Task<bool> AddtoGroup(HubCallerContext context,string groupName)
        {
            var group = await _messageRepository.GetMessageGroup(groupName);
            var connection = new Connection(context.ConnectionId, context.User.GetUsername());
            if(group == null)
            {
                group = new Group (groupName );
                _messageRepository.AddGroup(group);
            }
            group.Connection.Add(connection);
            return await _messageRepository.SaveAllAsync();
        }


        private async Task ReomveFromMessageGroup(string connectionId)
        {
            var connection = await _messageRepository.GetConnection(connectionId);
            _messageRepository.RemoveConnection(connection);
            await _messageRepository.SaveAllAsync();
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(other, caller) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}
