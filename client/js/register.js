import { Helper } from "./helper.js";
import {User} from "./user.js";

let fullnameInput = document.querySelector("#full_name");
let usernameInput = document.querySelector("#username");
let passwordInput = document.querySelector("#password");
let repeatPasswordInput = document.querySelector("#repeat_password");
let registerBtn = document.querySelector('input[type="submit"]');
let body = document.querySelector(".body");
let form = document.querySelector(".form");

registerBtn.addEventListener("click", async (e) => {
    e.preventDefault();
    if(passwordInput.value !== repeatPasswordInput.value)
    {
        Helper.drawErrors(form, {error: "Passwords do not match!"});
        return;
    }
    let user = new User(usernameInput.value, passwordInput.value, fullnameInput.value, 0);
    let response = await user.register();
    if(response.token)
    {
        document.cookie = `token=${response.token}`;
        window.location.replace("/");
    }
    else
    {
        Helper.drawErrors(form, response);
    }
})