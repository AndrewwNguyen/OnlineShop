using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
namespace WebBanHang.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangEntities1 db = new QuanLyBanHangEntities1();
        public List<ItemGioHang> LayGioHang()
        {
            //Giỏ Hàng đã tồn tại
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                //nếu giỏ seesion chưa tồn tại thì khởi tạo giỏ hàng
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            //kiểm tra sản phẩm có tồn tại trong CSDL không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;

            }
            //Lấy Giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            // Trường hợp 1 nếu sản phẩm đã tồn tại trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                // kiểm tra số lượng trước khi cho khách đặt hàng
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }

            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            lstGioHang.Add(itemGH);
            return Redirect(strURL);

        }
        //Tính toorng số lượng
        public double TinhTongSoLuong()
        {
            //lấy giỏ hàng
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);

        }
        public decimal TinhTongTien()
        {
            //lấy giỏ hàng
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }
        public ActionResult XemGioHang()
        {
            //lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }
        [HttpGet]
        public ActionResult XoaGioHang(int MaSP)
        {
            //kiểm tra session tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //kiểm tra sản phẩm có tồn tại trong CSDL không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //lấy list giỏ hàng từ session 
            List<ItemGioHang> lstGioHang = LayGioHang();
            //kiểm tra xem sản phẩm có tồn tại trong giỏ hàng chưa
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //xoa item gio hang
            lstGioHang.Remove(spCheck);
            return RedirectToAction("XemGioHang");
        }
        //xây đựng chức năng đặt hàng
        public ActionResult DatHang(KhachHang kh)
        {
            //kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang Khang = new KhachHang();
            //kiểm tra khách hàng đã đăng nhập chưa
            if (Session["TaiKhoan"] == null)
            {
                //thêm khách hàng vào bảng khách hàng đối với khách hàng chưa có tài khoản
                Khang = new KhachHang();
                Khang = kh;
                db.KhachHangs.Add(Khang);
                //lưu thông tin khách hàng
                db.SaveChanges();

            }
            else
            {
                //đối với khách hàng là thành viên
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                Khang.TenKH = tv.HoTen;
                Khang.DiaChi = tv.DiaChi;
                Khang.Email = tv.Email;
                db.KhachHangs.Add(Khang);
                db.SaveChanges();
            }
            //thêm đơn đặt hàng
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = Khang.MaKH;
            ddh.NgayDatHang = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            db.DonDatHangs.Add(ddh);
            //save để cập nhâp vào bảng DonDatHang
            db.SaveChanges();
            //Thêm chi tiết đơn đặt hàng
            List<ItemGioHang> lstGH = LayGioHang();
            foreach (var item in lstGH)
            {
                ChiTietDonDatHang ChiTiet = new ChiTietDonDatHang();
                ChiTiet.MaDonDatHang = ddh.MaDonDatHang;
                ChiTiet.MaSP = item.MaSP;
                ChiTiet.TenSP = item.TenSP;
                ChiTiet.SoLuong = item.SoLuong;
                ChiTiet.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ChiTiet);
            }
            //save để cập nhâp vào bảng ChiTietDonDatHang
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }


    }
}