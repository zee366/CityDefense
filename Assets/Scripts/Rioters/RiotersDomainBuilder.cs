using FluidHTN;
using FluidHTN.Factory;

namespace Rioters {
    public class RiotersDomainBuilder : BaseDomainBuilder<RiotersDomainBuilder, RioterHTNContext> {

        public RiotersDomainBuilder(string domainName, IFactory factory) : base(domainName, factory) { }

    }
}