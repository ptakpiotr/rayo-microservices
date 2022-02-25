using AutoMapper;

namespace RayoInfo.GraphQL
{
    public class Mutation
    {
        [UseDbContext(typeof(AppDbContext))]
        public ResponseModel DeleteComment(DeleteInput input, [ScopedService] AppDbContext ctx, [Service] ILogger<Mutation> logger)
        {
            try
            {
                CommentModel cm = ctx.Comments.FirstOrDefault(c => c.Id == input.Id);

                if (cm is null)
                {
                    return new ResponseModel { Message = "Resource with given ID does not exist" };
                }

                ctx.Comments.Remove(cm);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

            return new ResponseModel { Message = "Unable to perform action" };

        }

        [UseDbContext(typeof(AppDbContext))]
        public ResponseModel AddNews(NewsCreateDTO dto, [ScopedService] AppDbContext ctx,
            [Service] ILogger<Mutation> logger, [Service] IMapper mapper)
        {
            try
            {
                NewsModel news = mapper.Map<NewsModel>(dto);
                ctx.News.Add(news);
                ctx.SaveChanges();

                return new ResponseModel();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

            return new ResponseModel { Message = "Unable to perform action" };
        }
    }
}
