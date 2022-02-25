using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace RayoInfo.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class MainHub : Hub
    {
        private readonly AppDbContext _ctx;
        private readonly IMapper _mapper;
        private readonly ILogger<MainHub> _logger;

        public MainHub(IDbContextFactory<AppDbContext> contextFactory,IMapper mapper,ILogger<MainHub> logger)
        {
            _ctx = contextFactory.CreateDbContext();
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddComment(string message)
        {
            try
            {
                CommentCreateDTO dto = JsonSerializer.Deserialize<CommentCreateDTO>(message);
                CommentModel cm = _mapper.Map<CommentModel>(dto);

                _ctx.Comments.Add(cm);
                _ctx.SaveChanges();

                await Clients.All.SendAsync("CommentAdded",message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task ModifyComment(string message)
        {
            try
            {
                CommentModifyDTO dto = JsonSerializer.Deserialize<CommentModifyDTO>(message);
                CommentModel cm = _ctx.Comments.FirstOrDefault(c=>c.Id==dto.Id);
                _mapper.Map(dto,cm);

                _ctx.SaveChanges();

                await Clients.All.SendAsync("CommentModified", message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
