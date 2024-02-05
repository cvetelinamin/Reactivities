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

namespace Application.Comments
{
    public class List
    {
        public class Query : IRequest<Result<List<CommentDto>>>
        {
            public Guid ActivityId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<CommentDto>>>
        {
        private readonly DataContext context;
        private readonly IMapper mapper;
            public Handler(DataContext context, IMapper mapper) 
            {
                this.mapper = mapper;
                this.context = context;
            }
            public async Task<Result<List<CommentDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var comments = await this.context.Comments
                            .Where(x => x.Activity.Id == request.ActivityId)
                            .OrderBy(x => x.CreatedAt)
                            .ProjectTo<CommentDto>(this.mapper.ConfigurationProvider)
                            .ToListAsync();

                return Result<List<CommentDto>>.Success(comments);
            }
        }
    }
}