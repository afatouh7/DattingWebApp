﻿using APi.Dtos;
using APi.Entities;
using APi.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APi.Interfaces
{
    public interface IMessageRepository
    {
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnection(string connectionId);
        Task<Group> GetMessageGroup(string groupName);
        Task<Group> GetGroupForConnection(string connectionId);

        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PagedLIst<MessageDto>> GetMessageForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUSername, string recipientUsername);
        Task<bool> SaveAllAsync();
    }
}
