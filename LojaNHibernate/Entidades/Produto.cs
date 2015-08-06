using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LojaNHibernate.Entidades
{
  public class Produto
  {
    public virtual int IdProduto { get; set; }
    public virtual string Nome { get; set; }
    public virtual string Descricao { get; set; }
    public virtual decimal? Preco { get; set; }
    public virtual Categoria Categoria { get; set; }
  }
}
