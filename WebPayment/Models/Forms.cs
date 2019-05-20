using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebPayment.Models
{
	public class Forms
	{
		[Required(ErrorMessage = "Пожалуйста, введите номер заявки")]
		public int Id { get; set; }

		[Required(ErrorMessage = "Пожалуйста, введите дату")]
		public DateTime Date { get; set; }

		[Required(ErrorMessage = "Пожалуйста, введите ФИО")]
		public string FIO { get; set; }

		[Required(ErrorMessage = "Пожалуйста, введите тип заявки")]
		public string Type { get; set; }

		[Required(ErrorMessage = "Пожалуйста, введите общую сумму")]
		public decimal Sum { get; set; }
	}
}