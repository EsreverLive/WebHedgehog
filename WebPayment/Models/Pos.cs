using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebPayment.Models
{
	public class Pos
	{
		public int Id { get; set; }

		public int IdF { get; set; }

		[Required(ErrorMessage = "Пожалуйста, введите заявленную сумму")]
		public decimal Sum { get; set; }

		public string Comment { get; set; }

		public string Files { get; set; }

		public Forms Forms;
	}
}