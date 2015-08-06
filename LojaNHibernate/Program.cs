using LojaNHibernate.DAO;
using LojaNHibernate.Entidades;
using LojaNHibernate.Infra;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LojaNHibernate
{
  class Program
  {
    static void Main(string[] args)
    {
      NHibernateHelper.GeraSchema();

      try
      {
        ISession session = NHibernateHelper.AbreSession();
        ITransaction transacao = session.BeginTransaction();

        IList<Produto> lst = new List<Produto>();

        Categoria category = new Categoria();
        category.Nome = "Comida";
        category.Descricao = "Descrição da categoria comida";
        category.Produtos = lst;

        Produto product1 = new Produto();
        product1.Nome = "Arroz";
        product1.Preco = 8;
        product1.Descricao = "Descrição do produto arroz";
        product1.Categoria = category;

        lst.Add(product1);

        Produto product2 = new Produto();

        product2.Nome = "Feijão";
        product2.Descricao = "Descrição do produto feijão";
        product2.Preco = 15;
        product2.Categoria = category;

        lst.Add(product2);

        Produto produto3 = new Produto();
        produto3.Nome = "Produto Sem Categoria";
        produto3.Descricao = "Descrição do produto sem categoria";
        //produto3.Preco = 5;
        produto3.Categoria = category;

        lst.Add(produto3);

        session.Save(category);
        transacao.Commit();

        Categoria categoriaDB = session.Get<Categoria>(1);
        IList<Produto> produtos = categoriaDB.Produtos;
        foreach (var p in produtos)
        {
          Console.WriteLine("Nome do produto = " + p.Nome);
          Console.WriteLine("Preço do produto = " + p.Preco.GetValueOrDefault(0));
        }

        #region Consultando todos os produtos no banco

        string hql0 = "from Produto ";
        IQuery query0 = session.CreateQuery(hql0);
        IList<Produto> ListaProdutos0 = query0.List<Produto>();

        foreach (var item in ListaProdutos0)
        {
          Console.WriteLine(item.Nome + " -- " + item.Categoria.Nome);
        }

        #endregion

        #region Consultando produtos no banco com um valor minimo e de uma determinada categoria

        string hql1 = "from Produto p where p.Preco > :minimo and p.Categoria.Nome = :categoria";
        IQuery query1 = session.CreateQuery(hql1);
        query1.SetParameter("minimo", 10);
        query1.SetParameter("categoria", "Comida");
        IList<Produto> ListaProdutos1 = query1.List<Produto>();

        foreach (var item in ListaProdutos1)
        {
          Console.WriteLine(item.Nome);
        }

        #endregion

        #region Usando AliasToBean para mapear objetos

        string hql = "select p.Categoria.Nome as Categoria, count(p) as NumeroDeProdutos " +
             "from Produto p group by p.Categoria";
        IQuery query = session.CreateQuery(hql);
        query.SetResultTransformer(Transformers.AliasToBean<ProdutosPorCategoria>());
        IList<ProdutosPorCategoria> resultado = query.List<ProdutosPorCategoria>();

        #endregion

        #region Usando Criteria para buscar dados mais complexos

        string nome = "Arroz";
        double precoMinimo = 0.0;
        string nomeCategoria = "Comida";

        ICriteria criteria = session.CreateCriteria<Produto>();
        if (nome != null)
        {
          criteria.Add(Restrictions.Eq("Nome", nome));
        }
        if (precoMinimo > 0.0)
        {
          criteria.Add(Restrictions.Ge("Preco", precoMinimo));
        }
        if (nomeCategoria != null)
        {
          ICriteria criteriaCategoria = criteria.CreateCriteria("Categoria");
          criteriaCategoria.Add(Restrictions.Eq("Nome", nomeCategoria));
        }

        IList<Produto> listaProdutosCriteria = criteria.List<Produto>();

        #endregion

        #region Usando Cache

        Console.WriteLine("Testando o uso de cache.");
        Console.WriteLine("");

        ISession session1 = NHibernateHelper.AbreSession();
        ISession session2 = NHibernateHelper.AbreSession();

        Categoria c1 = session1.Get<Categoria>(1);
        Categoria c2 = session2.Get<Categoria>(1);

        ISession session3 = NHibernateHelper.AbreSession();
        IQuery query6 = session.CreateQuery("from Categoria");
        query.SetCacheable(true);
        // IList<Categoria> categorias = query.List<Categoria>();

        #endregion

        #region Usando um relacionamento N:N

        ISession sessionRelacionamentoNN = NHibernateHelper.AbreSession();
        ITransaction transacaoRelacionamentoNN = sessionRelacionamentoNN.BeginTransaction();

        PessoaFisica murilo = new PessoaFisica();
        murilo.Nome = "Murilo";
        murilo.CPF = "123.456.789.00";
        sessionRelacionamentoNN.Save(murilo);

        PessoaJuridica caelum = new PessoaJuridica();
        caelum.Nome = "Caelum";
        caelum.CNPJ = "123.456/0001-09";
        sessionRelacionamentoNN.Save(caelum);

        transacaoRelacionamentoNN.Commit();
        sessionRelacionamentoNN.Close();

        #endregion
      }
      catch (Exception ex)
      {
        throw ex;
      }

      Console.Read();
    }
  }

  public class ProdutosPorCategoria
  {
    public String Categoria { get; set; }
    public long NumeroDeProdutos { get; set; }
  }
}
