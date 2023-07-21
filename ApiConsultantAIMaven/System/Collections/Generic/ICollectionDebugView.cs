using System.Security.Claims;

namespace System.Collections.Generic
{
    internal class ICollectionDebugView<T>
    {
        private IEnumerable<Claim> claims;

        public ICollectionDebugView(IEnumerable<Claim> claims)
        {
            this.claims = claims;
        }
    }
}