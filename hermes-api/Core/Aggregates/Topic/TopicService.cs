namespace Hermes.Core
{
    public static class TopicService
    {
        public static void ValidateExistence(Topic topic) {
            if (!topic.Created || topic.Deleted)
                throw new DomainException("The Topic does not exist");
        }
    }
}