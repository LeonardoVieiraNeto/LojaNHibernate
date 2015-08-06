using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LojaNHibernate.Entidades
{
  public class Categoria
  {
    public virtual int IdCategoria { get; set; }
    public virtual string Nome { get; set; }
    public virtual string Descricao { get; set; }
    public virtual IList<Produto> Produtos { get; set; }

  }
}
