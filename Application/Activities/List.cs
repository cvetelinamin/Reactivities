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
        public class Query : IRequest<Result<List<ActivityDto>>> {}

        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
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
            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activities = await this.context.Activities.ProjectTo<ActivityDto>(this.mapper.ConfigurationProvider, new {currentUsername = this.userAccessor.GetUsername()}).ToListAsync();

                var activitiesToReturn = this.mapper.Map<List<ActivityDto>>(activities);
                return Result<List<ActivityDto>>.Success(activitiesToReturn);
            }
        }
    }
}