import {User} from "./model/user.js";
import {Helper} from "./helper.js";
import {Navbar} from "./navbar.js";

let wrapp = document.querySelector(".wrapp");
let usernameInput = document.querySelector(`input[name="username"]`);
let passwordInput = document.querySelector(`input[name="password"]`);
let loginBtn = document.querySelector(`input[type="submit"]`);
let body = document.querySelector(".body");
let form = document.querySelector(".form");

wrapp.prepend(await Navbar.getNav());

loginBtn.addEventListener("click", async (e) => {
    e.preventDefault();
    let user = new User(usernameInput.value, passwordInput.value);
    let response = await user.login();
    if(response.token)
    {
        window.location.replace("/dashboard");
    }
    else
    {
        Helper.drawErrors(form, response);
    }
})