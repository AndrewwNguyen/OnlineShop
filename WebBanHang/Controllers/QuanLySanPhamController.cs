using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        QuanLyBanHangEntities1 db = new QuanLyBanHangEntities1();

        //
        // GET: /QuanLySanPham/
        public ActionResult Index()
        {
            return View(db.SanPhams.Where(n => n.DaXoa == 1).OrderByDescending(n => n.MaSP));
        }
        [HttpGet]
        public ActionResult TaoMoi()
        {
            //load dropdownlist nhà cung cấp,mã nhà sản xuất
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            return View();

        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SanPham sp, HttpPostedFileBase HinhAnh)
        { //load dropdownlist nhà cung cấp,mã nhà sản xuất
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            //kiểm tra hình ảnh tồn tại trong csdl chưa
            if (HinhAnh.ContentLength > 0)
            {
                //lấy tên hình ảnh
                var fileName = Path.GetFileName(HinhAnh.FileName);
                //lấy hình ảnh chuyển vào mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/Hinhanh"), fileName);
                //đưa vào thư mục hình ảnh
                HinhAnh.SaveAs(path);
                sp.HinhAnh = fileName;


            }
            db.SanPhams.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
	}
}