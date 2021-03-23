function changeImg(nameImg){
	imgIp.src = nameImg;
}

var KichThuoc = document.getElementsByClassName("slide")[0].clientWidth;
var ChuyenSlide = document.getElementsByClassName("chuyen-slide")[0];
var Img = ChuyenSlide.getElementsByTagName("img");
var Max = KichThuoc * Img.length;
Max -= KichThuoc;
var Chuyen = 0;
function Next(){
	if(Chuyen < Max) Chuyen += KichThuoc;
	else Chuyen = 0;
	ChuyenSlide.style.marginLeft = '-' + Chuyen + 'px';
}

function Back(){
	if(Chuyen == 0) Chuyen = Max;
	else Chuyen -= KichThuoc;
	ChuyenSlide.style.marginLeft = '-' + Chuyen + 'px';
}

setInterval(function(){
	Next();
}, 3000);


function GiaTien(){
	let x = document.getElementById('sele').value;
	let text;
	if(x === '128GB')
		text = "35.999.000đ";
	if(x === '64GB')
		text = "33.999.000đ";
	if(x === '256GB')
		text = "37.999.000đ";
	document.getElementById('gia').innerHTML = text;
}