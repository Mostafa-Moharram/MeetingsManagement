import { toastMessage } from "./toast.js?2023-11-08 23:19";

const settingNavigationItems =
    document.querySelectorAll(".content .container .holder .settings .sidebar ul li");
const settingForms =
    document.querySelectorAll(".content .container .holder .settings .setting form");
const nicknameElement = document.querySelector(".content h2");

const validatePassword = (password) => {
    if (password.trim() === "")
        return { validation: false, message: "The password can't be empty or contains spaces." };
    const lengthCheck = password.length >= 6;
    const lowercaseCheck = /[a-z]/.test(password);
    const uppercaseCheck = /[A-Z]/.test(password);
    const digitCheck = /\d/.test(password);
    const validation = lengthCheck && lowercaseCheck && uppercaseCheck && digitCheck;
    if (validation) return { validation };
    return { validation, message: "The password isn't strong enough." };
};

const validateNickname = (nickname) => {
    if (nickname.trim() === "")
        return { validation: false, message: "The nickname field can't be empty." };
    return { validation: true };
};

const validatePhoneNumber = (phoneNumber) => {
    if (phoneNumber.trim() === "")
        return { validation: false, message: "The phone number field can't be empty." };
    return { validation: true };
};

const validateNewPassword = (password, form) => {
    if (password.trim() === "")
        return { validation: false, message: "The new password can't be empty." };
    if (password === form[2].value)
        return { validation: false, message: "The new password can't equal the current password." };
    return validatePassword(password);
};

const validateNewPasswordConfirmation = (password, form) => {
    if (password.trim() === "")
        return { validation: false, message: "The new password confirmation can't be empty." };
    if (password === form[0].value)
        return { validation: true };
    return { validation: false, message: "The new password and its confirmation doesn't match." };
};

const validateConfirmationPassword = (password) => {
    const result = validatePassword(password);
    if (result.validation)
        return result;
    return { validation: false, message: "The password is incorrect." };
};

const links = ["Account/Update/Nickname", "Account/Update/PhoneNumber", "Account/Update/Password"];

const validators = [
    [validateNickname, validateConfirmationPassword],
    [validatePhoneNumber, validateConfirmationPassword],
    [validateNewPassword, validateNewPasswordConfirmation, validateConfirmationPassword]
];

const onSuccess = [
    [(response) => {
        nicknameElement.textContent = `Hello, ${response.nickname}`;
        toastMessage("The nickname is updated successfully.");
    }],
    [() => toastMessage("The phone number is updated successfully.")],
    [() => toastMessage("The password is updated successfully.")]
];

const onFailure = [
    [(response) => toastMessage(response.status + '\n' + response.message)],
    [(response) => toastMessage(response.status + '\n' + response.message)],
    [(response) => toastMessage(response.status + '\n' + response.message)]
];

const submitForm = (form, index) => {
    let validation = true;
    validators[index].forEach((validator, i) => {
        form[i].nextElementSibling.textContent = "";
        const result = validator(form[i].value, form);
        if (!result.validation) {
            validation = false;
            form[i].nextElementSibling.textContent = result.message;
        }
    });
    if (!validation)
        return;
    const data = {};
    for (const [key, value] of new FormData(form))
        data[key] = value;
    fetch(links[index], {
        method: "put",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    })
        .then(async response => {
            const responseBody = await response.json();
            if (response.ok) {
                form.reset();
                onSuccess[index].forEach((callback) => callback(responseBody));
            } else {
                onFailure[index].forEach((callback) => callback(responseBody));
            }
        });
}

settingForms.forEach((form, index) => {
    form.addEventListener('submit', (e) => {
        e.preventDefault();
        submitForm(form, index);
    });
});

let activeFormIndex = 0;

const updateActiveForm = (index) => {
    for (let i = 0; settingForms[activeFormIndex][i].type !== "submit"; ++i)
        settingForms[activeFormIndex][i].nextElementSibling.textContent = "";
    settingNavigationItems[activeFormIndex].classList.remove("selected");
    settingForms[activeFormIndex].setAttribute("hidden", "");
    activeFormIndex = index;
    settingForms[activeFormIndex].removeAttribute("hidden");
    settingNavigationItems[activeFormIndex].classList.add("selected");
};

settingNavigationItems.forEach((element, index) => element.addEventListener("click", () => updateActiveForm(index)));