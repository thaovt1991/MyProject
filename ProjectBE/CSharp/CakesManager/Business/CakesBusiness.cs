using CakesManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakesManager.Business
{
    public class CakesBusiness
    {
        public async Task<Cakes> GetOneAsync()
        {
            return  new Cakes() ;
        }

        public async Task<List<Cakes>> GetCakesAsync()
        {
            return new List<Cakes> { new Cakes() };
        }
    }
}
