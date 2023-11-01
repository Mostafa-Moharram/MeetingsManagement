const headerAnchors = document.querySelectorAll("header .container .right ul li:nth-child(2) a");

headerAnchors.forEach(anchor => anchor.addEventListener("click", function (e) {
    e.preventDefault();
    const tmpForm = document.createElement("form");
    tmpForm.action = this.href;
    tmpForm.method = "post";
    document.body.appendChild(tmpForm);
    tmpForm.submit();
}));