using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using quanlytrungtam.Models;
using System.IO;
using System.Net;

namespace quanlytrungtam.Controllers
{
    public class HomeClienController : Controller
    {
        QUANLYTRUNGTAMDUHOCEntities db = new QUANLYTRUNGTAMDUHOCEntities();
        // GET: HomeClien
        public ActionResult Index()
        {
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View();
        }
       
        public ActionResult truongdh(int ? MANUOC)
        {
            if (MANUOC == null)
            {
                return View(db.TRUONGDAIHOCs.OrderBy(n => n.MATDH));
            }
            ViewBag.MANUOC = MANUOC;
            return View(db.TRUONGDAIHOCs.Where(n => n.MANUOC == MANUOC).OrderBy(n => n.MATDH));
        }
        public ActionResult truongDhNuoc(int? MANUOC)
        {
            if (MANUOC == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.MANUOC = MANUOC;
            return View(db.TRUONGDAIHOCs.Where(n=>n.MANUOC==MANUOC).OrderBy(n => n.MATDH));
        }
       
        public ActionResult deltailDH(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
           TRUONGDAIHOC tdh = db.TRUONGDAIHOCs.SingleOrDefault(n => n.MATDH == id);

            if (tdh == null) return HttpNotFound();

            return View(tdh);

        }
        [HttpGet]
        public ActionResult datlichtuvan()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult datlichtuvan(LICHHENTUVAN ltv)
        {
           
            db.LICHHENTUVANs.Add(ltv);
            //ltv.TRANGTHAI = "Chưa liên hệ";
            db.SaveChanges();

            TempData["result"] = "Bạn đã đặt lịch hẹn tư vấn thành công!";
            return RedirectToAction("Index", "HomeClien");
        }
        public ActionResult hocbong(int? MANUOC)
        {
            if (MANUOC == null)
            {
                return View(db.HOCBONGs.OrderBy(n => n.MAHB));
            }
            ViewBag.MANUOC = MANUOC;
            return View(db.HOCBONGs.Where(n => n.MANUOC == MANUOC).OrderBy(n => n.MAHB));
        }
        public ActionResult detailHocBong(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            HOCBONG hb = db.HOCBONGs.SingleOrDefault(n => n.MAHB == id);

            if (hb == null) return HttpNotFound();

            return View(hb);

        }
        [HttpGet]
        public ActionResult nophoso()
        {
            ViewBag.MALT = new SelectList(db.LOTRINHDUHOCs.OrderBy(n => n.MALT), "MALT", "TENLT");
            var HoSoKh = new KhachHangHoSo();
            return View(HoSoKh);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult nophoso(KhachHangHoSo khhs, HttpPostedFileBase FILEHOSO)
        {
            if (Session["KHACHHANG"] != null && Session["KHACHHANG"] != "")
            {

                KHACHHANG tk = (KHACHHANG)Session["KHACHHANG"];
                if (FILEHOSO != null)
                {
                    // lấy tên hình ảnh
                    var fileName = Path.GetFileName(FILEHOSO.FileName);
                    //lấy hình ảnh chuyển vào thư mục hình ảnh
                    var path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        khhs.HOSOKHACHHANG.FILEHOSO = fileName;

                    }
                    else
                    {
                        FILEHOSO.SaveAs(path);

                        khhs.HOSOKHACHHANG.FILEHOSO = fileName;

                    }


                }
                ViewBag.MALT = new SelectList(db.LOTRINHDUHOCs.OrderBy(n => n.MALT), "MALT", "TENLT");
                khhs.KHACHHANG.MAKH = tk.MAKH;
                db.KHACHHANGs.Add(khhs.KHACHHANG);

                db.SaveChanges();

                khhs.HOSOKHACHHANG.MAKH = khhs.KHACHHANG.MAKH;
                khhs.HOSOKHACHHANG.NGAYTAOHS = DateTime.Now;
                khhs.HOSOKHACHHANG.TRANGTHAIHS = "Chưa duyệt";
                db.HOSOKHACHHANGs.Add(khhs.HOSOKHACHHANG);
                db.SaveChanges();
                TempData["result"] = "Nộp hồ sơ thành công !";
                return RedirectToAction("Index", "HomeClien");
            }
            return View();
        }
        [HttpGet]
        public ActionResult dangky()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult dangky(KHACHHANG user)
        {
            user.ROLES = 2;
            db.KHACHHANGs.Add(user);
            db.SaveChanges();
            TempData["result"] = "Đăng ký thành công !";
            return RedirectToAction("Index", "HomeClien");

        }
        [HttpGet]
        public ActionResult dangnhap()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult dangnhap(string TAIKHOAN, string MATKHAU)
        {
            var user = db.KHACHHANGs.FirstOrDefault(u => u.TAIKHOAN == TAIKHOAN && u.MATKHAU == MATKHAU);
            var admin = db.NHANVIENs.FirstOrDefault(d => d.TAIKHOAN == TAIKHOAN && d.MATKHAU == MATKHAU);

            if (user != null)
            {
                if (user.ROLES == 2)
                {
                    // Đăng nhập thành công bằng tài khoản User với roles là 1
                    // Lưu thông tin người dùng vào Session
                    Session["KHACHHANG"] = user;
                    return RedirectToAction("Index", "HomeClien");
                }
            }

            if (admin != null)
            {
                if (admin.ROLES == 1)
                {
                    // Đăng nhập thành công bằng tài khoản User với roles là 2
                    // Lưu thông tin người dùng vào Session
                    Session["NHANVIEN"] = admin;
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "HomeClien");
        }

        public ActionResult dangxuat(string TAIKHOAN, string MATKHAU)
        {
            // Xóa giá trị của DocGiaRoles trong Session
            Session["NHANVIEN"] = null;
            Session["KHACHHANG"] = null;
            // Sau đó chuyển hướng đến trang chủ
            return RedirectToAction("Index", "HomeClien");
        }
        [HttpGet]
        public ActionResult tinhtranghskh(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            KHACHHANG tk = db.KHACHHANGs.SingleOrDefault(n => n.MAKH == id);
            return View(tk);
        }
        public ActionResult thanhtoan(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MAKH == id);
            HOSOKHACHHANG hskh = db.HOSOKHACHHANGs.SingleOrDefault(n => n.MAKH == kh.MAKH);
            LOTRINHDUHOC lotrinh = db.LOTRINHDUHOCs.SingleOrDefault(n => n.MALT == hskh.MALT);
            HOADON hd = new HOADON();
            hd.MAKH = kh.MAKH;
            hd.NGAYDAT = DateTime.Now;
            hd.TINHTRANGHOADON = 0;
            hd.DATHANHTOAN = 0;
            db.HOADONs.Add(hd);
            db.SaveChanges();

            CHITIETHOADON cthd = new CHITIETHOADON();
            cthd.MALT = lotrinh.MALT;
            cthd.MAHD = hd.MAHD;
            cthd.TENLT = lotrinh.TENLT;
            cthd.DONGIA = lotrinh.CHIPHI;
            db.CHITIETHOADONs.Add(cthd);
            db.SaveChanges();
            return RedirectToAction("Index", "HomeClien");
        }
    }
}