'use strict';
let createForm = document.createElement('button');
createForm.textContent = 'Create new object';
createForm.addEventListener('click', createFormEvent);
createForm.style.margin = '10px';

document.getElementsByTagName('main')[0].appendChild(createForm);

let duration;
let textColor;
let firstColor;
let secondColor;
let field;

function createFormEvent() {
  document.getElementsByTagName('main')[0].removeChild(createForm);
  field = document.createElement('fieldset');
  const legend = document.createElement('legend');
  legend.textContent = 'Create objects';
  field.appendChild(legend);

  const form = document.createElement('form');
  form.setAttribute('method', 'post');

  let labels = [4];
  let ids = ['duration', 'text color', 'first color', 'second color'];
  let paragraphs = [4];
  for (let i = 0; i < 4; i++) {
    labels[i] = document.createElement('label');
    labels[i].setAttribute('for', ids[i]);
    labels[i].textContent = ids[i];
    labels[i].style.marginRight = '10px';
    paragraphs[i] = document.createElement('p');
  }

  duration = document.createElement('input');
  duration.setAttribute('type', 'text');
  duration.setAttribute('min', '0');
  duration.setAttribute('id', ids[0]);
  paragraphs[0].appendChild(labels[0]);
  paragraphs[0].appendChild(duration);

  textColor = document.createElement('input');
  textColor.setAttribute('type', 'color');
  textColor.setAttribute('id', ids[1]);
  paragraphs[1].appendChild(labels[1]);
  paragraphs[1].appendChild(textColor);

  firstColor = document.createElement('input');
  firstColor.setAttribute('type', 'color');
  firstColor.setAttribute('id', ids[2]);
  paragraphs[2].appendChild(labels[2]);
  paragraphs[2].appendChild(firstColor);

  secondColor = document.createElement('input');
  secondColor.setAttribute('type', 'color');
  secondColor.setAttribute('id', ids[3]);
  paragraphs[3].appendChild(labels[3]);
  paragraphs[3].appendChild(secondColor);

  for (let i = 0; i < 4; i++) {
    form.appendChild(paragraphs[i]);
  }

  field.appendChild(form);
  document.getElementsByTagName('main')[0].appendChild(field);

  let buttonParagraph = document.createElement('p');
  let buttonSend = document.createElement('button');
  buttonSend.textContent = 'Create!';
  buttonSend.addEventListener('click', createGlitch);
  buttonParagraph.appendChild(buttonSend);
  form.appendChild(buttonParagraph);
}

function createGlitch() {
  let main = document.getElementsByTagName('main')[0];
  main.removeChild(field);
  main.innerHTML +=
      "<div class='glitch' style='--duration: " +
      duration.value +
      'ms; --first-color: ' +
      firstColor.value +
      '; --second-color: ' +
      secondColor.value +
      '; --font-color: ' +
      textColor.value +
      "'><span class='glitch1'>Glitch text</span>" +
      "<span class='glitch2'>Glitch text</span>  " +
      "<span class='glitch3'>Glitch text</span>  " +
      "<span class='glitch4'>Glitch text</span> </div>";

  main.innerHTML += '<div> </div>';

  document.getElementsByTagName('main')[0].appendChild(createForm);
}
