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
using System.Threading.Tasks;

namespace APi.SignalR
{
    public class MessageHub :Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public MessageHub(IMessageRepository  messageRepository,IMapper mapper, IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext= Context.GetHttpContext();
            var otherUSer= httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(Context.User.GetUsername(), otherUSer);
            await Groups.AddToGroupAsync(Context.ConnectionId,groupName);

            var messages = await _messageRepository.GetMessageThread(Context.User.GetUsername(), otherUSer);

            await Clients.Group(groupName).SendAsync("ReciveMessageThread", messages);
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
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
            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync())
            {
                var group = GetGroupName(sender.UserName, respient.UserName);
                await Clients.Group(group).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }


        }
        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(other, caller) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}
