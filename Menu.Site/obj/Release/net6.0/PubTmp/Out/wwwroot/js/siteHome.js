const hoImg = document.getElementsByClassName("hoImg");
const hoBody = document.getElementsByClassName("hoBody");
for (let j = 0; j < hoBody.length; j++) {
    hoBody[j].tabIndex = j;

}



document.getElementById("body").onscroll = function () { myFunction() };


const navbar = document.getElementsByClassName("category");


let l = navbar.length;
l--;

function myFunction() {
    for (let index = 0; index < navbar.length; index++) {
        let x = index;
        x++;
        if (index != l) {
            if (window.pageYOffset > navbar[index].offsetTop && window.pageYOffset < navbar[x].offsetTop) {
                hoImg[index].classList.add("avciveImg");
                hoBody[index].classList.add("avciveBody");


                for (let i = 0; i < navbar.length; i++) {
                    if (index != i) {
                        hoImg[i].classList.remove("avciveImg");
                        hoBody[i].classList.remove("avciveBody");
                    }


                }
                break;
            }
            else {


            }

        }
        else {
            x = document.getElementById("body").scrollHeight;
            if (window.pageYOffset > navbar[index].offsetTop && window.pageYOffset < x) {
                hoImg[index].classList.add("avciveImg");
                hoBody[index].classList.add("avciveBody");
                let p = index;
                p--;
                hoImg[p].classList.remove("avciveImg");
                hoBody[p].classList.remove("avciveBody");
                break;
            }

        }

    }
}


const sc = document.getElementsByClassName("category");



function btnCategory(para) {
    let o = para.tabIndex;

    document.documentElement.scrollTop = document.body.scrollTop = sc[o].offsetTop + 2;
}






var modal = document.getElementById("myModal");

var btn = document.getElementById("myBtn");

var span = document.getElementsByClassName("close")[0];

btn.onclick = function () {
    modal.style.display = "block";
}

span.onclick = function () {
    modal.style.display = "none";
}

window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}


var price = document.getElementsByClassName("price");

for (let r = 0; r < price.length; r++) {
    price[r].innerHTML = nu(price[r].innerHTML);

}


function nu(str) {
    let output = [];
    let sNumber = str.toString();

    for (var i = 0, len = sNumber.length; i < len; i += 1) {
        output.push(+sNumber.charAt(i));
    }

    let txt = "";
    let num = 3;
    for (var i = sNumber.length - 1; i >= 0; i--) {
        if (num == 0) {
            num = 3;
            txt += "," + sNumber[i];
        }
        else {
            txt += sNumber[i];
        }
        num--;
    }

    return reverseString(txt);
}



function reverseString(str) {

    let newString = "";
    for (let i = str.length - 1; i >= 0; i--) {
        newString += str[i];
    }
    return newString;
}
