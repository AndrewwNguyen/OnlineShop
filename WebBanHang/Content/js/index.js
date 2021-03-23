$("#dangky").click(function () {
    var loi = 0;
    if ($("#HoTen").val() == "") {
        $("#tb_loi").text("Họ Tên Không Được Trống");
        return false;
    }
    else {
        loi++;
        $("#tb_loi").text("");
    }
});
