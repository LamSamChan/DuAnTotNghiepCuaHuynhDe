 
const enableInputCheckbox = document.getElementById('enable-input');
const inpt1 = document.getElementById('inputNameBuss');
const inpt2 = document.getElementById('inputPosition');
const inpt3 = document.getElementById('certi');
const inpt5 = document.getElementById('dateSDN');
const inpt6 = document.getElementById('adSDN');
//const dataInput = document.querySelectorAll('.form-control');
const content = document.querySelector('.content');
//enableInputCheckbox.addEventListener('change', function () {
//    dataInput.disabled = !enableInputCheckbox.checked;
//});
enableInputCheckbox.addEventListener('change', function () {
    inpt1.disabled = !enableInputCheckbox.checked;
    inpt2.disabled = !enableInputCheckbox.checked;
    inpt3.disabled = !enableInputCheckbox.checked;
    inpt5.disabled = !enableInputCheckbox.checked;
    inpt6.disabled = !enableInputCheckbox.checked;
    //if (enableInputCheckbox.checked) {
    //    content.classList.add('disabled');
    //} else {
    //    content.classList.remove('disabled');
    //}  

});
