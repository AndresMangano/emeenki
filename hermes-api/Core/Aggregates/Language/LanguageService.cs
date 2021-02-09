namespace Hermes.Core
{
    public static class LanguageService
    {
        public static void ValidateExistence(Language language) {
            if (!language.Created || language.Deleted)
                throw new DomainException("The Language does not exist");
        }

        public static void ValidateInexistence(Language language) {
            if (language.Created && language.Deleted)
                throw new DomainException("The Language was deleted");
            else if (language.Created)
                throw new DomainException("The Language does not exist");
        }
    }
}