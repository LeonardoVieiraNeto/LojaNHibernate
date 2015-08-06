using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LojaNHibernate.Entidades
{
  public abstract class Usuario
  {
    public virtual int Id { get; set; }
    public virtual string Nome { get; set; }
  }
}
