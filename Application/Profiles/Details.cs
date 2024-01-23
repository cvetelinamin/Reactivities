using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class Details
    {
        public class Query : IRequest<Result<Profile>>
        {
            public string Username { get; set; }

        }

        public class Handler : IRequestHandler<Query, Result<Profile>>
        {
            private readonly IMapper mapper;
            private readonly DataContext context;
            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;                
            }
            public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await this.context.Users
                            .ProjectTo<Profile>(this.mapper.ConfigurationProvider)
                            .SingleOrDefaultAsync(x => x.Username == request.Username);

                return Result<Profile>.Success(user);
            }
        }
    }
}