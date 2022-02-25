using HotChocolate.AspNetCore.Authorization;

namespace RayoInfo.GraphQL
{
    public class Query
    {
        [Authorize(Roles = new string[] {"Admin","User"})]
        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<CommentModel> GetComments([ScopedService] AppDbContext ctx)
        {
            IQueryable<CommentModel> comments = ctx.Comments;

            return comments;
        }

        [Authorize(Roles = new string[] { "Admin", "User" })]
        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<CommentModel> GetNewsComments(int id,[ScopedService] AppDbContext ctx)
        {
            IQueryable<CommentModel> comment = ctx.Comments.Where(c => c.NewsId == id);
            return comment;
        }

        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<NewsModel> GetNews([ScopedService] AppDbContext ctx)
        {
            IQueryable<NewsModel> news = ctx.News;

            return news;
        }

        [UseDbContext(typeof(AppDbContext))]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<StandingsModel> GetStandings([ScopedService] AppDbContext ctx)
        {
            IQueryable<StandingsModel> standings = ctx.Standings;

            return standings;
        } 
    }
}
