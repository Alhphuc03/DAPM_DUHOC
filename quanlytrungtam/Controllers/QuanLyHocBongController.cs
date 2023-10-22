using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using quanlytrungtam.Models;
using System.IO;


namespace quanlytrungtam.Controllers
{
    public class QuanLyHocBongController : Controller
    {
        QUANLYTRUNGTAMDUHOCEntities db = new QUANLYTRUNGTAMDUHOCEntities();
        // GET: QuanLyHocBong
        public ActionResult Index()
        {
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(db.HOCBONGs.OrderByDescending(n=>n.MAHB));
        }
        [HttpGet]
        public ActionResult ThemHocbong()
        {
            ViewBag.MANUOC = new SelectList(db.NUOCs.OrderBy(n => n.MANUOC), "MANUOC", "TENNUOC");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemHocbong(HOCBONG hb, HttpPostedFileBase ANH)
        {

            if (ANH!=null)
            {
                // lấy tên hình ảnh
                var fileName = Path.GetFileName(ANH.FileName);
                //lấy hình ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                if (System.IO.File.Exists(path))
                {
                    hb.ANH = fileName;
                 
                }
                else
                {
                    ANH.SaveAs(path);
                    hb.ANH = fileName;

                }


            }
            ViewBag.MANUOC = new SelectList(db.NUOCs.OrderBy(n => n.MANUOC), "MANUOC", "TENNUOC");
            db.HOCBONGs.Add(hb);
            db.SaveChanges();
            TempData["result"] = "Thêm thành công !";
            return RedirectToAction("Index", "QuanLyHocBong");
        }
        public ActionResult editHocbong(int? id)
        {
            ViewBag.MANUOC = new SelectList(db.NUOCs.OrderBy(n => n.MANUOC), "MANUOC", "TENNUOC");
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            HOCBONG hb = db.HOCBONGs.SingleOrDefault(n => n.MAHB == id);
            if (hb== null) return HttpNotFound();
            return View(hb);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult editHocbong(HOCBONG hb, HttpPostedFileBase ANH)
        {
            var hocbong = db.HOCBONGs.SingleOrDefault(n => n.MAHB == hb.MAHB);
            ViewBag.MANUOC = new SelectList(db.NUOCs.OrderBy(n => n.MANUOC), "MANUOC", "TENNUOC");
            if (ANH != null)
            {
                // lấy tên hình ảnh
                var fileName = Path.GetFileName(ANH.FileName);
                //lấy hình ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                if (System.IO.File.Exists(path))
                {
                    hb.ANH = fileName;

                }
                else
                {
                    ANH.SaveAs(path);
                    /*Session["TENHINH"] = HINHANH.FileName;*/
                    hb.ANH = fileName;

                }

            }
            else
            {
                string anh = hocbong.ANH;
                hb.ANH = anh;
            }

            HOCBONG hbupdate = db.HOCBONGs.SingleOrDefault(n => n.MAHB == hb.MAHB);
            hbupdate.ANH = hb.ANH;
            hbupdate.TENHB = hb.TENHB;
            hbupdate.NGAYHETHAN = hb.NGAYHETHAN;
            hbupdate.SOTIEN = hb.SOTIEN;
            hbupdate.MOTANGAN = hb.MOTANGAN;
            hbupdate.MANUOC = hb.MANUOC;
            db.SaveChanges();
            TempData["result"] = "Chỉnh sửa thành công !";
            return RedirectToAction("Index", "QuanLyHocBong");
        }
        public ActionResult xoaHocbong(int? id)
        {
            ViewBag.MANUOC = new SelectList(db.NUOCs.OrderBy(n => n.MANUOC), "MANUOC", "TENNUOC");
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            HOCBONG hb = db.HOCBONGs.SingleOrDefault(n => n.MAHB == id);
            if (hb == null) return HttpNotFound();
            return View(hb);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult xoaHocbong(int id)
        {
            ViewBag.MANUOC = new SelectList(db.NUOCs.OrderBy(n => n.MANUOC), "MANUOC", "TENNUOC");
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            HOCBONG hb = db.HOCBONGs.SingleOrDefault(n => n.MAHB == id);
            if (hb == null) return HttpNotFound();
            db.HOCBONGs.Remove(hb);
            db.SaveChanges();
            TempData["result"] = "Xóa thành công !";

            return RedirectToAction("Index", "QuanLyHocBong");
        }
    }
}