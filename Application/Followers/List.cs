using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Profiles;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers
{
    public class List
    {
        public class Query : IRequest<Result<List<Profiles.Profile>>>
        {
            public string Predicate { get; set; }
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<Profiles.Profile>>>
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

            public async Task<Result<List<Profiles.Profile>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profiles = new List<Profiles.Profile>();

                switch(request.Predicate)
                {
                    case "followers":
                        profiles = await this.context.UserFollowings.Where(x => x.Target.UserName == request.Username)
                                    .Select(u => u.Observer)
                                    .ProjectTo<Profiles.Profile>(this.mapper.ConfigurationProvider, new {currentUsername = this.userAccessor.GetUsername()})
                                    .ToListAsync();
                        break;
                    case "following":
                        profiles = await this.context.UserFollowings.Where(x => x.Observer.UserName == request.Username)
                                    .Select(u => u.Target)
                                    .ProjectTo<Profiles.Profile>(this.mapper.ConfigurationProvider, new {currentUsername = this.userAccessor.GetUsername()})
                                    .ToListAsync();
                        break;
                }

                return Result<List<Profiles.Profile>>.Success(profiles);
            }
        }
    }
}