using System;
using System.Collections.Generic;
using System.Text;
using Takas.Core.Model.Entities;
using Takas.Core.Repositories.Database;

namespace Takas.Core.Services.Interfaces
{
    public interface IChatService:IRepository<Chat>
    {
    }
}
