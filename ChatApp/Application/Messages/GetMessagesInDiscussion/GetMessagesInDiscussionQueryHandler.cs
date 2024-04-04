using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Messages.GetMessagesInDiscussion;

internal sealed class GetMessagesInDiscussionQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetMessagesInDiscussionQuery, List<MessageResponse>>
{
    private readonly IApplicationDbContext dbContext = dbContext;

    public async Task<Result<List<MessageResponse>>> Handle(GetMessagesInDiscussionQuery request, CancellationToken cancellationToken)
    {
        List<MessageResponse> messages = await dbContext.Messages
            .Where(m => m.DiscussionId == request.DiscussionId)
            .Select(m => new MessageResponse
            {
                Id = m.Id,
                Username = dbContext.Users.Where(u => u.Id == m.UserId).Select(u => u.Username).First(),
                Contents = m.Contents,
                DateSentUtc = m.DateSentUtc,
                IsEdited = m.IsEdited
            })
            // TODO: implement pagination later
            //.OrderByDescending(m => m.DateSentUtc)
            //.ThenBy(m => m.Id)
            //.Where(m => m.DateSentUtc < request.LastDateSentUtc
            //        || (m.DateSentUtc == request.LastDateSentUtc && m.Id > request.LastMessageId))
            //.Take(request.Limit)
            .ToListAsync(cancellationToken);

        return Result.Success(messages);
    }
}

