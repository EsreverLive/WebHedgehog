using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebPayment.Models
{
	public class PaymentContext : DbContext
	{

		public DbSet<Forms> Form { get; set; }
		public DbSet<Pos> Position { get; set; }

		public List<Forms> listF = new List<Forms>();
		public List<Pos> listP = new List<Pos>();

		public static int idF;

		public Forms GetForms(int id)
		{
			return listF.Find(x => x.Id == id);
		}

		public List<Pos> GetPos(int id)
		{
			return listP.FindAll(x => x.IdF == id);
		}
	}
}