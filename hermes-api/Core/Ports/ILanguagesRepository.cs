namespace Hermes.Core.Ports
{
    public interface ILanguagesRepository
    {
        Language FetchLanguage(string languageID);
    }
}