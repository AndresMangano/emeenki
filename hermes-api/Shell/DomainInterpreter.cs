using System;
using Hermes.Core;
using RabbitMQ.Client;

namespace Hermes.Shell
{
    public partial class DomainInterpreter
    {
        readonly SQLConnection _connection;
        readonly ConnectionFactory _rabbitMQConnection;
        EventRepository<Guid, Article> _articlesRepository;
        EventRepository<Guid, ArticleTemplate> _articleTemplatesRepository;
        EventRepository<string, Room> _roomsRepository;
        EventRepository<string, User> _usersRepository;
        EventRepository<string, GoogleAccount> _googleAccountsRepository;

        public DomainInterpreter(SQLConnection connection, ConnectionFactory rabbitMQConnection)
        {
            _connection = connection;
            _rabbitMQConnection = rabbitMQConnection;
            InitArticlesRepository();
            InitArticleTemplatesRepository();
            InitRoomsRepository();
            InitUsersRepository();
            InitGoogleAccountRepository();
        }
    }
}