using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroService_SnakeState_NicolasThomas.Data.Models;

namespace MicroService_SnakeState_NicolasThomas.Data.Repositories
{
    public interface ISnakeStateRepository
    {
        Task<SnakeStateModel> Subscribe(SnakeStateModel model);
        Task<SnakeStateModel> Unsubscribe(SnakeStateModel model);
        Task<SnakeStateModel> EnvoieListPosition(SnakeStateModel model);
    }
}
