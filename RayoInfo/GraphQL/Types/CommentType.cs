namespace RayoInfo.GraphQL.Types
{
    public class CommentType : ObjectType<CommentModel>
    {
        protected override void Configure(IObjectTypeDescriptor<CommentModel> descriptor)
        {
            descriptor.Field(f => f.News).ResolveWith<Resolvers>(r => r.GetNewsResolver(default!, default!)).UseDbContext<AppDbContext>();

        }
    }
}
