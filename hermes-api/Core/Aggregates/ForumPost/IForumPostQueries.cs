using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hermes.Core
{
    public interface IForumPostQueries
    {
        Task<ForumPostDTO> Get(Guid ID);
        Task<IEnumerable<ForumPostDTO>> Query();
        Task<IEnumerable<ForumPostCommentDTO>> GetComments(Guid ID);
    }
}