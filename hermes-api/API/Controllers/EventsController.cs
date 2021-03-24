using System;
using System.Collections.Generic;
using System.Linq;
using Hermes.Shell;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Hermes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        readonly DomainInterpreter _interpreter;

        public EventsController(DomainInterpreter interpreter)
        {
            _interpreter = interpreter;
        }

        public record EventDTO<TKey>(
            string RoutingKey,
            string Payload
        );

        [HttpGet("article/{index}")]
        public IEnumerable<EventDTO<Guid>> GetArticleEvents(int index) =>
            _interpreter.ArticlesRepository.GetMissingEvents(index)
                .Select(e => new EventDTO<Guid>(
                    e.Message.Metadata.EventName,
                    _interpreter.ParseEvent(
                        e.Index,
                        e.Message.Metadata,
                        e.Message.Payload,
                        obj => obj.Add("ID", e.Message.Metadata.ID))
                        .ToString()
                ));

        [HttpGet("articleTemplate/{index}")]
        public IEnumerable<EventDTO<Guid>> GetArticleTemplateEvents(int index) =>
            _interpreter.ArticleTemplatesRepository.GetMissingEvents(index)
                .Select(e => new EventDTO<Guid>(
                    e.Message.Metadata.EventName,
                    _interpreter.ParseEvent(
                        e.Index,
                        e.Message.Metadata,
                        e.Message.Payload,
                        obj => obj.Add("ID", e.Message.Metadata.ID))
                        .ToString()
                ));

        [HttpGet("room/{index}")]
        public IEnumerable<EventDTO<string>> GetRoomEvents(int index) =>
            _interpreter.RoomsRepository.GetMissingEvents(index)
                .Select(e => new EventDTO<string>(
                    e.Message.Metadata.EventName,
                    _interpreter.ParseEvent(
                        e.Index,
                        e.Message.Metadata,
                        e.Message.Payload,
                        obj => obj.Add("ID", e.Message.Metadata.ID))
                        .ToString()
                ));

        [HttpGet("user/{index}")]
        public IEnumerable<EventDTO<string>> GetUserEvents(int index) =>
            _interpreter.UsersRepository.GetMissingEvents(index)
                .Select(e => new EventDTO<string>(
                    e.Message.Metadata.EventName,
                    _interpreter.ParseEvent(
                        e.Index,
                        e.Message.Metadata,
                        e.Message.Payload,
                        obj => obj.Add("ID", e.Message.Metadata.ID))
                        .ToString()
                ));

        [HttpGet("forumPost/{index}")]
        public IEnumerable<EventDTO<string>> GetForumPostEvents(int index) =>
            _interpreter.ForumPostsRepository.GetMissingEvents(index)
                .Select(e => new EventDTO<string>(
                    e.Message.Metadata.EventName,
                    _interpreter.ParseEvent(
                        e.Index,
                        e.Message.Metadata,
                        e.Message.Payload,
                        obj => obj.Add("ID", e.Message.Metadata.ID))
                        .ToString()
                ));
    }
}