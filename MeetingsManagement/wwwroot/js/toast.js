export function toastMessage(message) {
    const toasterContent = document.createElement("div");
    toasterContent.classList.add("toaster-content");
    toasterContent.textContent = message;
    const toasterBox = document.createElement("div");
    toasterBox.classList.add("toaster-box");
    toasterBox.appendChild(toasterContent);
    const toasterOkButton = document.createElement("button");
    toasterOkButton.textContent = "Ok";
    toasterBox.appendChild(toasterOkButton);
    const toaster = document.createElement("div");
    toaster.setAttribute("id", "toaster");
    toaster.appendChild(toasterBox);
    document.body.appendChild(toaster);
    toasterOkButton.addEventListener("click", () => toaster.remove());
    toasterOkButton.focus();
};
