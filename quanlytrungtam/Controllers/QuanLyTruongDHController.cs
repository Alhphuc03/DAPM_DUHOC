using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using quanlytrungtam.Models;
using System.IO;

namespace quanlytrungtam.Controllers
{
    
    public class QuanLyTruongDHController : Controller
    {
        QUANLYTRUNGTAMDUHOCEntities db = new QUANLYTRUNGTAMDUHOCEntities();
        public int pageSize = 10;

        // GET: QuanLyTruongDH
        public ActionResult Index( int? page)
        {
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            if (page > 0)
            {
                page = page;
            }
            else
            {
                page = 1;
            }
            int start = (int)(page - 1) * pageSize;

            ViewBag.pageCurrent = page;
            int totalPage = db.TRUONGDAIHOCs.Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
/*            ViewBag.tdh = db.TRUONGDAIHOCs.OrderByDescending(x => x.MATDH).Skip(start).Take(pageSize);
*/            return View(db.TRUONGDAIHOCs.OrderByDescending(x => x.MATDH).Skip(start).Take(pageSize));

        }
        [HttpGet]
        public ActionResult ThemtruongDH()
        {
            ViewBag.MANUOC = new SelectList(db.NUOCs.OrderBy(n => n.MANUOC), "MANUOC", "TENNUOC");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemtruongDH(TRUONGDAIHOC tdh, HttpPostedFileBase ANH)
        {
            ViewBag.MANUOC = new SelectList(db.NUOCs.OrderBy(n => n.MANUOC), "MANUOC", "TENNUOC");
            if (ANH!=null)
            {
                // lấy tên hình ảnh
                var fileName = Path.GetFileName(ANH.FileName);
                //lấy hình ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                if (System.IO.File.Exists(path))
                {
                    tdh.ANH = fileName;
                  
                
                }
                else
                {
                    ANH.SaveAs(path);
                  
                    tdh.ANH = fileName;

                }

            }
           
            db.TRUONGDAIHOCs.Add(tdh);
            db.SaveChanges();
            TempData["result"] = "Thêm thành công !";
            return RedirectToAction("Index", "QuanLyTruongDH");
        }
        [HttpGet]
        public ActionResult editTruong(int? id)
        {
            ViewBag.MANUOC = new SelectList(db.NUOCs.OrderBy(n => n.MANUOC), "MANUOC", "TENNUOC");
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            
            TRUONGDAIHOC tdh = db.TRUONGDAIHOCs.SingleOrDefault(n => n.MATDH == id);
            if (tdh == null) return HttpNotFound();
            return View(tdh);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult editTruong(TRUONGDAIHOC tdh, HttpPostedFileBase ANH)
        {
            var dh = db.TRUONGDAIHOCs.SingleOrDefault(n => n.MATDH == tdh.MATDH);
            ViewBag.MANUOC = new SelectList(db.NUOCs.OrderBy(n => n.MANUOC), "MANUOC", "TENNUOC");
            if (ANH!=null)
            {
                // lấy tên hình ảnh
                var fileName = Path.GetFileName(ANH.FileName);
                //lấy hình ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                if (System.IO.File.Exists(path))
                {
                    tdh.ANH = fileName;
                    
                }
                else
                {
                    ANH.SaveAs(path);
                    /*Session["TENHINH"] = HINHANH.FileName;*/
                    tdh.ANH = fileName;

                }

            }
            else
            {
                string anh = dh.ANH;
                tdh.ANH = anh;
            }
           
            TRUONGDAIHOC dhupdate = db.TRUONGDAIHOCs.SingleOrDefault(n => n.MATDH == tdh.MATDH);
            dhupdate.ANH = tdh.ANH;
            dhupdate.MATDH = tdh.MATDH;
            dhupdate.MOTANGAN = tdh.MOTANGAN;
            dhupdate.MOTACHITIET = tdh.MOTACHITIET;
            dhupdate.DIACHI = tdh.DIACHI;
            dhupdate.SDT = tdh.SDT;
            dhupdate.Email = tdh.Email;
            dhupdate.TRANG_WEB = tdh.TRANG_WEB;
            dhupdate.MANUOC = tdh.MANUOC;
            db.SaveChanges();
            TempData["result"] = "Chỉnh sửa thành công !";
            return RedirectToAction("Index", "QuanLyTruongDH");
        }
        public ActionResult xoaTruong(int? id)
        {
            ViewBag.MANUOC = new SelectList(db.NUOCs.OrderBy(n => n.MANUOC), "MANUOC", "TENNUOC");
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            TRUONGDAIHOC tdh = db.TRUONGDAIHOCs.SingleOrDefault(n => n.MATDH == id);
            if (tdh == null) return HttpNotFound();
            return View(tdh);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult xoaTruong(int id)
        {
            ViewBag.MANUOC = new SelectList(db.NUOCs.OrderBy(n => n.MANUOC), "MANUOC", "TENNUOC");
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            TRUONGDAIHOC tdh = db.TRUONGDAIHOCs.SingleOrDefault(n => n.MATDH == id);
            if (tdh == null) return HttpNotFound();
            db.TRUONGDAIHOCs.Remove(tdh);
            db.SaveChanges();
            TempData["result"] = "Xóa thành công !";

            return RedirectToAction("Index", "QuanLyTruongDH");
        }
    }
}