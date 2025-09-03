using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService_SnakeState_NicolasThomas.Data.Models
{
    public class SnakeStateModel
    {
        public int x { get; set; }
        public int y { get; set; }

        public List<(int x, int y)> Snake { get; set; } = new List<(int x, int y)>();
        public string sessionId { get; set; } = "";
        public string route { get; set; } = "";

        public string direction { get; set; } = "";
    }
}
