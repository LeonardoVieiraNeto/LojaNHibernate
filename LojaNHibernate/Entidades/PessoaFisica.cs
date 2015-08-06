using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LojaNHibernate.Entidades
{
  public class PessoaFisica : Usuario
  {
    public virtual string CPF { get; set; }
  }
}
