﻿using Api.Interfaces;
using APi.Dtos;
using APi.Entities;
using APi.Extensions;
using APi.Helpers;
using APi.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APi.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMesage(CreateMessageDto createMessageDto)
        {
            var usernme = User.GetUsername();
            if (usernme == createMessageDto.RecipientsUsername.ToLower()) return BadRequest("You cannot sent Message to yourself");
            var sender = await _userRepository.GetUserByUserNameAsync(usernme);
            var respient = await _userRepository.GetUserByUserNameAsync(createMessageDto.RecipientsUsername);

            if (respient == null) return NotFound();
            var message = new Message
            {
                Sender = sender,
                Recipient = respient,
                SenderUserName = sender.UserName,
                RecipientUsername = respient.UserName,
                Content = createMessageDto.Content


            };
            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));
            return BadRequest("failes to send message");

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();
            var messages = await _messageRepository.GetMessageForUser(messageParams);
            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TOtalCount, messages.TotalPages);
            return messages;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var currentUsername = User.GetUsername();
            return Ok(await _messageRepository.GetMessageThread(currentUsername, username));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username= User.GetUsername();
            var message = await _messageRepository.GetMessage(id);
            if(message.Sender.UserName !=username &&message.Recipient.UserName != username)
            {
                return Unauthorized();
            }
            if(message.Sender.UserName ==username) message.SenderDeleted = true;
            if(message.Recipient.UserName==username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted) _messageRepository.DeleteMessage(message);
            if (await _messageRepository.SaveAllAsync()) return Ok();
            return BadRequest("Problem deleting the message"); 


        }
    }
}
