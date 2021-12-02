using Takas.Core.Model.Entities;
using Takas.Core.Services.Interfaces;
using Takas.Infrastructure.Data;
using Takas.Infrastructure.Data.Repositories;

namespace Takas.Infrastructure.Services
{
    public class ChatService:Repository<Chat>,IChatService
    {
        public ChatService(TakasDbContext context) : base(context) { }
    }
}
