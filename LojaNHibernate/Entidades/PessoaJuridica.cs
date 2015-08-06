using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LojaNHibernate.Entidades
{
  public class PessoaJuridica : Usuario
  {
    public virtual string CNPJ { get; set; }
  }
}
