namespace RayoInfo.GraphQL.Types
{
    public class NewsType : ObjectType<NewsModel>
    {
        protected override void Configure(IObjectTypeDescriptor<NewsModel> descriptor)
        {
            descriptor.Field(f => f.Comments).ResolveWith<Resolvers>(r => r.GetCommentsResolver(default!, default!)).UseDbContext<AppDbContext>();
            descriptor.Field(f => f.Comments).Authorize("Admin","User");
        }
    }
}
