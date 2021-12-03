using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOS;
using API.Entities;
using API.Extentions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IUserRepo _userRepo;
        private readonly IMessageRepo _messageRepo;
        private readonly IMapper _mapper;
        public MessagesController(IUserRepo userRepo, IMessageRepo messageRepo, IMapper mapper)
        {
            _mapper = mapper;
            _messageRepo = messageRepo;
            _userRepo = userRepo;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("cant send msges to urself");

            var sender = await _userRepo.GetUserByUsernameAsync(username);
            var recipient = await _userRepo.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null)
                return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.Username,
                RecipientUsername = recipient.Username,
                Content = createMessageDto.Content
            };

            _messageRepo.AddMessage(message);

            if (await _messageRepo.SaveAllAsync())
            {
                return Ok(_mapper.Map<MessageDto>(message));
            }

            return BadRequest("Failed to send msg");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages([FromQuery] MessageParams messageParams) //returnin IEnumerable gt pername tin PagedList san header
        {
            messageParams.Username = User.GetUsername();

            var messages = await _messageRepo.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);

            return messages;

        }

        [HttpGet("thread/{username}")] //username: the other participant of the conv
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username) //logged in as lisa, {{url}}api/Messages/thread/todd
        {                                                                                               //messages convo between lisa and todd
            var currentUsername = User.GetUsername();

            return Ok(await _messageRepo.GetMessageThread(currentUsername, username));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();

            var message = await _messageRepo.GetMessage(id);

            if (message.Sender.Username != username && message.Recipient.Username != username)
                return Unauthorized();

            if (message.Sender.Username == username)
                message.SenderDeleted = true;

            if (message.Recipient.Username == username)
                message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
                _messageRepo.DeleteMessage(message);

            if (await _messageRepo.SaveAllAsync() == true)
                return Ok();

            return BadRequest("Problem deleting msg");
        }
    }
}