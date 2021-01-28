using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace IntergalacticAirways.Lib.Cache
{
    public interface ICacheCancellationTokenProvider<T>
    {
        IChangeToken Token { get; }

        Task Cancel();
    }
    //
    // public interface ICacheCancellationTokenProvider
    // {
    //     IChangeToken GetToken<T>();
    //
    //     Task Cancel<T>();
    // }
}