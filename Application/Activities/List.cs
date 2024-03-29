using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Interfaces;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<PagedList<ActivityDto>>> 
        {
            public ActivityParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;
            private readonly IUserAccessor userAccessor;
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                this.userAccessor = userAccessor;
                this.mapper = mapper;
                this.context = context;
            }
            public async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = this.context.Activities
                                .Where(d => d.Date >= request.Params.StartDate)
                                .OrderBy(d => d.Date)
                                .ProjectTo<ActivityDto>(this.mapper.ConfigurationProvider, new {currentUsername = this.userAccessor.GetUsername()})
                                .AsQueryable();

                if(request.Params.IsGoing && !request.Params.IsHost) 
                {
                    query = query.Where(x => x.Attendees.Any(a => a.Username == this.userAccessor.GetUsername()));
                }

                if(request.Params.IsHost && !request.Params.IsGoing)
                {
                    query = query.Where(x => x.HostName == this.userAccessor.GetUsername());
                }

   //             var activitiesToReturn = this.mapper.Map<PagedList<ActivityDto>>(query);
                return Result<PagedList<ActivityDto>>.Success(
                                await PagedList<ActivityDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize));
            }
        }
    }
}