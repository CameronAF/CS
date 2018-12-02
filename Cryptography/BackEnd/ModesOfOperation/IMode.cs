using BackEnd.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModesOfOperation
{
    /// <summary>
    /// Mode of Operation interface
    /// </summary>
    interface IMode
    {
        /// <summary>
        /// Send blocks to be encrypted 
        /// </summary>
        /// <param name="blocks">Blocks that will be encrypted</param>
        /// <returns>Blocks of encypted text</returns>
        Blocks Send(Blocks blocks);
        /// <summary>
        /// Receive blocks to be decrypted 
        /// </summary>
        /// <param name="blocks">Blocks that will be decrypted</param>
        /// <returns>Blocks of decrypted text</returns>
        Blocks Receive(Blocks blocks);
    }
}
