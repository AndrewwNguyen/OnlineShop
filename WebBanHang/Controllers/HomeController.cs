using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
namespace WebBanHang.Controllers
{
    public class HomeController : Controller
    {
        QuanLyBanHangEntities1 db = new QuanLyBanHangEntities1();
        public ActionResult Index()
        {
            var ListSanPham = db.SanPhams.Where(n => n.MaNSX == 1).ToList();
            ViewBag.ListSP = ListSanPham;
            return View();
        }
        public ActionResult SanPham1()
        {
            var ListSanPham = db.SanPhams.Where(n => n.MaLoaiSP== 1).ToList();
            ViewBag.ListSP = ListSanPham;
            return View();
        }
        public ActionResult SanPham2()
        {
            var ListSanPham = db.SanPhams.Where(n => n.MaLoaiSP == 2).ToList();
            ViewBag.ListSP = ListSanPham;
            return View();
        }
        public ActionResult SanPham3()
        {
            var ListSanPham = db.SanPhams.Where(n => n.MaLoaiSP == 3).ToList();
            ViewBag.ListSP = ListSanPham;
            return View();
        }
        // Tạo Partial View
        public ActionResult SanPhamPartial1()
        {
            // Lấy dữ liệu nạp vào Model
            //var ListSanPham = db.SanPhams.Where(n => n.MaLoaiSP == 1);

            //return PartialView(ListSanPham);
            return PartialView();
        }
        public ActionResult SanPhamPartial2()
        {
            // Lấy dữ liệu nạp vào Model
            //var ListSanPham = db.SanPhams.Where(n => n.MaLoaiSP == 1);

            //return PartialView(ListSanPham);
            return PartialView();
        }
        public ActionResult SanPhamPartial3()
        {
            // Lấy dữ liệu nạp vào Model
            //var ListSanPham = db.SanPhams.Where(n => n.MaLoaiSP == 1);

            //return PartialView(ListSanPham);
            return PartialView();
        }
        public ActionResult Dangky(ThanhVien tv,FormCollection f)
        {
            db.ThanhViens.Add(tv);
            db.SaveChanges();
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            // Kiểm Tra tên đăng nhập và mật khẩu
            string tk = f["taikhoan"].ToString();
            string mk = f["matkhau"].ToString();
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == tk && n.MatKhau == mk);
            if (tv != null)
            {
                Session["TaiKhoan"] = tv;
                //return Content("<script>window.location.reload();</script>");
            }

            return RedirectToAction("Index");
            //return Content("Tài Khoản Hoặc Mật Khẩu Không Chính Xác.");
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index");
        }
        //chi tiết sản phẩm
        public ActionResult SanPham(long MaSP)
        {
            SanPham product = db.SanPhams.Where(n => n.MaSP == MaSP).SingleOrDefault();
            return View(product);
        }
        public ActionResult LienHe()
        {
            return View();
        }
	}
}