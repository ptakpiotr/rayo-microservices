namespace RayoInfo.GraphQL.Types
{
    public class Resolvers
    {
        [UseDbContext(typeof(AppDbContext))]
        public NewsModel GetNewsResolver([Parent] CommentModel cm, [ScopedService] AppDbContext ctx)
        {
            NewsModel news = ctx.News.FirstOrDefault(n => n.Id == cm.NewsId);

            return news;
        }

        [UseDbContext(typeof(AppDbContext))]
        public List<CommentModel> GetCommentsResolver([Parent] NewsModel nm, [ScopedService] AppDbContext ctx)
        {
            List<CommentModel> comms = ctx.Comments.Where(c => c.NewsId == nm.Id).ToList();

            return comms;
        }
    }
}
