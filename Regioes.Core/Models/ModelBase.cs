using Regioes.Shared.Extensions;

namespace Regioes.Core.Models
{
    public class ModelBase
    {
        public bool Ok()
        {
            return !this.PossuiErros();
        }

        public string ErrrsString()
        {
            return this.ObterErros();
        }
    }
}
