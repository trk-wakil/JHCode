using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdApp.Models;

namespace ThirdApp.Helpers
{
    public interface IGameManager
    {
        GameState CreateNewGame(int numOfCards);

        int GetMaxNumOfCards();
    }
}
