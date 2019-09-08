using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoApis.Models
{
    public interface ICryptoRestApi
    {
        string Exchange { get; }
        List<string> GetAllSymbols();
        string GetSymbol(string symbolId);
        Task<XTicker> GetTicker(string symbolId);
        Task<XBalanceMap> GetBalances();

    } // end of interface ICryptoApi

} // end of namespace
