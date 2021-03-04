using System;
using Hermes.Core;
using RabbitMQ.Client;

namespace Hermes.Shell
{
    public partial class DomainInterpreter
    {
        readonly SQLConnection _connection;
        readonly ConnectionFactory _rabbitMQConnection;
        public EventRepository<Guid, Article> ArticlesRepository { get; private set; }
        public EventRepository<Guid, ArticleTemplate> ArticleTemplatesRepository { get; private set; }
        public EventRepository<string, Room> RoomsRepository { get; private set; }
        public EventRepository<string, User> UsersRepository { get; private set; }
        public EventRepository<string, GoogleAccount> GoogleAccountsRepository { get; private set; }

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