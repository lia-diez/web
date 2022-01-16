'use strict';
document.body.onload = receiveJson;
let main = document.getElementsByTagName('main')[0];

async function receiveJson() {
    await fetch("/api/pull").then(function (response) {
        response.json().then(function (json) {
            for (const jsonKey of json) {
                main.innerHTML +=
                    "<div class='glitch' style='--duration: " +
                    jsonKey.duration +
                    'ms; --first-color: ' +
                    jsonKey.firstColor +
                    '; --second-color: ' +
                    jsonKey.secondColor +
                    '; --font-color: ' +
                    jsonKey.textColor +
                    "'><span class='glitch1'>Glitch text</span>" +
                    "<span class='glitch2'>Glitch text</span>  " +
                    "<span class='glitch3'>Glitch text</span>  " +
                    "<span class='glitch4'>Glitch text</span> </div>";

                main.innerHTML += '<div> </div><div> </div>';
            }
        });
    });
}