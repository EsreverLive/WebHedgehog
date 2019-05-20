using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPayment.Models;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Services.Client;
using System.IO;
using System.Diagnostics;

namespace WebPayment.Controllers
{
	public class HomeController : Controller
	{
		PaymentContext pm = new PaymentContext();

		protected override void Dispose(bool disposing)
		{
			pm.Dispose();
			base.Dispose(disposing);
		}
		
		public ViewResult Index()
		{
			int hour = DateTime.Now.Hour;
			ViewBag.Greeting = hour < 12 ? "Доброе утро" : "Добрый день";
			return View();
		}
		
		public ViewResult PaymentForm()
		{
			ViewBag.id = pm.Form.Max(x => x.Id) + 1;
			return View();

		}

		[HttpPost]
		public ActionResult PaymentForm([Bind(Exclude = "Id")] Forms f)
		{
			if (!ModelState.IsValid)
				return View();

			pm.Form.Add(f);
			pm.SaveChanges();

			var id = pm.Form.Max(x => x.Id);
			return View("ListNP", pm.GetPos(id).ToList());
		}
		
		public ActionResult ListNP()
		{
			pm.Position.Load();
			var id = pm.Form.Max(x => x.Id);
			var pos = (from m in pm.Position where m.IdF == id select m);
			pm.listP = pos.ToList();
			return View(pm.listP);
		}

		public ActionResult AddPos()
		{
			ViewBag.idF = pm.Form.Max(x => x.Id);
			return View();
		}

		[HttpPost]
		public ActionResult AddPos([Bind(Exclude = "Id")] Pos f, HttpPostedFileBase uploads)
		{
			if (!ModelState.IsValid)
				return View();

			if (uploads != null)
			{
				string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files\\" + Path.GetFileNameWithoutExtension(uploads.FileName));
			
				uploads.SaveAs(FilePath);
				
				f.Files = Path.GetFileNameWithoutExtension(uploads.FileName);
			}
			pm.Position.Add(f);
			pm.SaveChanges();
			pm.listP.Add(f);

			return RedirectToAction("ListNP", pm.listP);
		}

		public ActionResult ListF(DateTime? Date, string Type)
		{
			var f = pm.Form.ToList();
			if (Date != null)
				f = f.FindAll(x => x.Date == Date);
			if (Type != null && Type != "")
				f = f.FindAll(x => x.Type == Type);
			return View(f);
		}	

		public ActionResult Details(int id)
		{
			var pos = (from m in pm.Position where m.IdF == id select m);
			PaymentContext.idF = id;
			ViewBag.id = id;
			pm.listP = pos.ToList();
			return View(pos.ToList());
		}

		public ActionResult DeleteNP(int id)
		{
			var del = (from m in pm.Position where m.Id == id select m).First();
			return View(del);
		}

		[HttpPost]
		public ActionResult DeleteNP(Pos del)
		{
			var check_edit = (from m in pm.Position where m.Id == del.Id select m).First();

			pm.Position.Remove(check_edit);
			pm.SaveChanges();
			pm.listP.Remove(check_edit);
			return RedirectToAction("ListNP", pm.listP);
		}

		public ActionResult DetailsP(int id)
		{
			var pos = (from m in pm.Position where m.Id == id select m).First();
			return View(pos);
		}

		public ActionResult Edit(int id)
		{
			var edit = (from m in pm.Form where m.Id == id select m).First();
			return View(edit);
		}

		[HttpPost]
		public ActionResult Edit(Forms edit)
		{
			if (!ModelState.IsValid)
			{
				var check_edit = (from m in pm.Form where m.Id == edit.Id select m).First();
				return View(check_edit);
			}
	
			pm.Form.Attach(edit);
			pm.Entry(edit).State = EntityState.Modified;
			pm.SaveChanges();
	
			return RedirectToAction("ListF");
		}

		public ActionResult Create()
		{
			ViewBag.idF = PaymentContext.idF;
			return View();
		}

		[HttpPost]
		public ActionResult Create([Bind(Exclude = "Id")] Pos f, HttpPostedFileBase uploads)
		{
			if (!ModelState.IsValid)
				return View();

			if (uploads != null)
			{
				Path.GetFileName(uploads.FileName);
				string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files\\" + Path.GetFileNameWithoutExtension(uploads.FileName));

				uploads.SaveAs(FilePath);

				f.Files = Path.GetFileNameWithoutExtension(uploads.FileName);
			}

			pm.Position.Add(f);
			pm.SaveChanges();
			pm.listP.Add(f);
			return RedirectToAction("ListP", pm.listP);
		}

		public ActionResult ListP()
		{
			ViewBag.id = PaymentContext.idF;
			pm.Position.Load();
			var pos = (from m in pm.Position where m.IdF == PaymentContext.idF select m);
			pm.listP = pos.ToList();
			return View(pm.listP);
		}

		public ActionResult Delete(int id)
		{
			var del = (from m in pm.Position where m.Id == id select m).First();
			return View(del);
		}

		[HttpPost]
		public ActionResult Delete(Pos del)
		{
			var check_edit = (from m in pm.Position where m.Id == del.Id select m).First();

			pm.Position.Remove(check_edit);
			pm.SaveChanges();
			pm.listP.Remove(check_edit);

			return RedirectToAction("ListP", pm.listP);
		}

		public ActionResult Open(string file, int id)
		{
			string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files\\" + file);
			Process.Start(FilePath);
			var pos = (from m in pm.Position where m.Id == id select m).First();
			return View(pos);
		}
	}
}