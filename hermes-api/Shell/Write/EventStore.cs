using Hermes.Core;
using Hermes.Core.Ports;
using Hermes.Shell.Read;
using Hermes.Shell.Write;

namespace Hermes.Shell
{
    public partial class DomainInterpreter : IEventsRepository
    {
        public void StoreEvent(IDomainEvent @event)
        {
            switch(@event) {
                case ArticleEvent e: ApplyArticleEvent(e); break;
                case ArticleTemplateEvent e: ApplyArticleTemplateEvent(e); break;
                case RoomEvent e: ApplyRoomEvent(e); break;
                case UserEvent e: ApplyUserEvent(e); break;
                case GoogleAccountEvent e: ApplyGoogleAccountEvent(e); break;
                default:
                    throw new DomainException("Invalid Event");
            }
        }
    }
}