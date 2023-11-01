const settingNavigationItems =
    document.querySelectorAll(".content .container .holder .settings .sidebar ul li");
const settingForms =
    document.querySelectorAll(".content .container .holder .settings .setting form");

let currentSelectedIndex = 0;

const updateSelected = (index) => {
    settingNavigationItems[currentSelectedIndex].classList.remove("selected");
    settingForms[currentSelectedIndex].setAttribute("hidden", "");
    currentSelectedIndex = index;
    settingNavigationItems[currentSelectedIndex].classList.add("selected");
    settingForms[currentSelectedIndex].removeAttribute("hidden");
};

settingNavigationItems.forEach((navItem, index) => {
    navItem.onclick = () => updateSelected(index);
});

const isValidPassword = (password) => {
    var lengthCheck = password.length >= 8;
    var lowercaseCheck = /[a-z]/.test(password);
    var uppercaseCheck = /[A-Z]/.test(password);
    var digitCheck = /\d/.test(password);
    return lengthCheck && lowercaseCheck && uppercaseCheck && digitCheck;
};

const validatePasswordFormData = (formData) => {
    valid = true;
    document.getElementById("CurrentPassword").nextElementSibling.textContent = "";
    if (!isValidPassword(formData.get("CurrentPassword"))) {
        valid = false;
        document.getElementById("CurrentPassword").nextElementSibling.textContent = "Password is not strong enough.";
    }
    document.getElementById("NewPassword").nextElementSibling.textContent = "";
    if (!isValidPassword(formData.get("NewPassword"))) {
        valid = false;
        document.getElementById("NewPassword").nextElementSibling.textContent = "Password is not strong enough.";
    }
    document.getElementById("NewPasswordConfirmation").nextElementSibling.textContent = "";
    if (formData.get("NewPassword") != formData.get("NewPasswordConfirmation")) {
        valid = false;
        document.getElementById("NewPasswordConfirmation").nextElementSibling.textContent = "`Confirm Password` field doesn't match `New Password`.";
    }
    return valid;
};

settingForms[0].addEventListener("submit", function (e) {
    e.preventDefault();
    if (!this.reportValidity())
        return false;
    const data = new FormData(this);
    if (!validatePasswordFormData(data))
        return;
    fetch("/Account/ChangePassword", {
        method: "post",
        headers: {
            'Content-Type': "application/json"
        },
        body: JSON.stringify({
            CurrentPassword: data.get("CurrentPassword"),
            NewPassword: data.get("NewPassword"),
            NewPasswordConfirmation: data.get("NewPasswordConfirmation")
        })
    })
        .then(response => response.json())
        .then(response => {
            if (response.status == "Success")
            console.log(response);
        })
});

settingForms[1].addEventListener("submit", function (e) {
    e.preventDefault();
    if (!this.reportValidity())
        return;
    const data = new FormData(this);
    
});