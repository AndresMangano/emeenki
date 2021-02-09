namespace Hermes.Core
{
    public interface IGoogleAccountRepository
    {
        GoogleAccount FetchGoogleAccount(string googleAccountID);
    }
}