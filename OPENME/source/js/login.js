//--------------Common---------------
let passwords = document.querySelectorAll("input[type=\"password\"]");
let show = document.getElementById("show");

function showPassword() {
    if (show.checked) {
        passwords.forEach(password => {
            password.type = "text";
        });
    }
    else {
        passwords.forEach(password => {
            password.type = "password";
        });
    }
}

//-------------Login--------------
function onSubmitLogin() {
    let form = document.Login;
    let warningText = "Неправильный пароль или логин";
    let warning = document.getElementById("warning");

    let xnr = new XMLHttpRequest();
    xnr.open("GET", "/accounts/login?email=" + 
                    form.email.value + "&password=" + 
                    form.password.value + "&remember=" +
                    (form.remember.checked ? "on" : "off"), false);
    xnr.send();

    if (eval(xnr.responseText)) {
        window.location.href = "/";
    }
    else {
        warning.style.display = "block";
        warning.innerHTML = warningText;
    }

    return false;
}

//--------------Sign Up----------------
function validateName() {
    return validateInput(document.signup.name, /^\w+$/, 1, 50);
}
function validateEmail() {
    return validateInput(document.signup.email, /^[\w.]+@[\w.]+\.[a-z]{2,4}$/, 6, 200);
}
function validatePassword() {
    let pass = document.signup.password;
    let val1 = pass.value;
    let accept = document.signup.acceptpassword;
    let val2 = accept.value;
    var result = val1 === val2 && /[a-z]/.test(val1) && /[A-Z]/.test(val1) && /[0-9]/.test(val1) && val1.length >= 8 && val1.length <= 50;
    if (result) {
        pass.style.backgroundColor = null;
        accept.style.backgroundColor = null;
    }
    else {
        pass.style.backgroundColor = "var(--warning-color-light)";
        accept.style.backgroundColor = "var(--warning-color-light)";
    }
    return result;
}
function validateInput(input, pattern, min, max) {
    let val = input.value;
    let result = pattern.test(val);
    if (result && val.length >= min && val.length <= max) {
        input.style.backgroundColor = null;
        return true;
    }
    else {
        input.style.backgroundColor = "var(--warning-color-light)";
        return false;
    }
}
function onSubmitSignup() {

    let warning = document.getElementById("warning");
    let form = document.signup;

    if (validateName() & validateEmail() & validatePassword()) {
        let xnr = new XMLHttpRequest();
        xnr.open("POST", "/accounts/signup", false);
        xnr.send("email=" + form.email.value + 
        "&password=" + form.password.value + 
        "&name=" + form.name.value + 
        "&gender=" + form.gender.value + 
        "&acceptpassword=" + form.acceptpassword.value);

        if (eval(xnr.responseText)) {
            window.location.href = "/html/signin";
        }
        else {
            warning.style.display = "block";
            warning.innerHTML = "Пользователь с таким email уже существует или данные не корректны"
        }
    }
    else {
        warning.style.display = "block";
        warning.innerHTML = "Введенные данные не корректны"
    }

    return false;
}

//-----------------------Profile----------------------
function toEditMode() {
    let submit = document.querySelector("input[type=\"submit\"]");

    submit.addEventListener("click", apply);
    submit.value = "Применить";

    let disabled = document.querySelectorAll("*[disabled]");

    disabled.forEach(element => {
        element.removeAttribute("disabled");
    });

    let hided = document.querySelectorAll("*[style=\"display: none;\"]")

    hided.forEach(element => {
        element.style.display = null;
    });

    document.getElementById("title").innerHTML = "Изменение профиля";
}

function apply() {

    let warning = document.getElementById("warning");
    let form = document.signup;

    if (validateName() & validateEmail() & validatePassword()) {
        let xnr = new XMLHttpRequest();
        xnr.open("POST", "/accounts/update", false);
        xnr.send("email=" + form.email.value + 
        "&password=" + form.password.value + 
        "&name=" + form.name.value + 
        "&gender=" + form.gender.value + 
        "&acceptpassword=" + form.acceptpassword.value);

        if (eval(xnr.responseText)) {
            window.location.reload();
        }
        else {
            warning.style.display = "block";
            warning.innerHTML = "Пользователь с таким email уже существует или данные не корректны"
        }
    }
    else {
        warning.style.display = "block";
        warning.innerHTML = "Введенные данные не корректны"
    }
}