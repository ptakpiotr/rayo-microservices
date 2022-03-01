using AutoMapper;

namespace RayoInfo.Profiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<CommentCreateDTO, CommentModel>().ForMember(dest=>dest.Likes,opts=>opts.MapFrom(src=>0))
                    .ForMember(dest=>dest.Dislikes,opts=>opts.MapFrom(src=>0))
                    .ForMember(dest=>dest.NewsId,opts=>opts.MapFrom(src=>int.Parse(src.NewsId)));
            CreateMap<CommentModifyDTO, CommentModel>().ForMember(dest => dest.Id, opts => opts.Ignore());
            

            CreateMap<NewsCreateDTO, NewsModel>().ForMember(dest=>dest.DateOfCreation,opts=>opts.MapFrom(src=>DateTime.UtcNow));

        }
    }
}
