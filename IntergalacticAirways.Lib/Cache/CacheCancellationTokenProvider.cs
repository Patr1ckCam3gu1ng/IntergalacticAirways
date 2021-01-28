using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace IntergalacticAirways.Lib.Cache
{
    public class CacheCancellationTokenProvider<T> : ICacheCancellationTokenProvider<T>
    {
        private CancellationChangeToken _changeToken;

        private CancellationTokenSource _tokenSource;

        public CacheCancellationTokenProvider()
        {
            Initialize();
            InstanceID = Guid.NewGuid();
            TypeName = typeof(T).FullName;
        }

        private Guid InstanceID { get; }

        private string TypeName { get; }

        public IChangeToken Token => _changeToken;

        public Task Cancel()
        {
            _tokenSource.Cancel();
            Initialize();

            return Task.CompletedTask;
        }

        private void Initialize()
        {
            if (_tokenSource != null)
            {
                _tokenSource.Dispose();
                _tokenSource = null;
            }

            _tokenSource = new CancellationTokenSource();

            _changeToken = new CancellationChangeToken(_tokenSource.Token);
        }
    }

    // public class CacheCancellationTokenProvider : ICacheCancellationTokenProvider
    // {
    //     private readonly IServiceProvider _serviceProvider;
    //
    //     public CacheCancellationTokenProvider(IServiceProvider serviceProvider)
    //     {
    //         _serviceProvider = serviceProvider;
    //     }
    //
    //
    //     public IChangeToken GetToken<T>()
    //     {
    //         var tokenProvider = _serviceProvider.GetService<ICacheCancellationTokenProvider<T>>();
    //
    //         return tokenProvider.Token;
    //     }
    //
    //     public Task Cancel<T>()
    //     {
    //         var tokenProvider = _serviceProvider.GetService<ICacheCancellationTokenProvider<T>>();
    //
    //         return tokenProvider.Cancel();
    //     }
    // }
}