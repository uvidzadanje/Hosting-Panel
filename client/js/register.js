import { Helper } from "./helper.js";
import {User} from "./model/user.js";

let fullnameInput = document.querySelector(`input[name="full_name"]`);
let usernameInput = document.querySelector(`input[name="username"]`);
let passwordInput = document.querySelector(`input[name="password"]`);
let repeatPasswordInput = document.querySelector(`input[name="repeat_password"]`);
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
    let checkedRadio = document.querySelector(`input[type="radio"]:checked`);
    if(!checkedRadio)
    {
        Helper.drawErrors(form, {error: "You must choose priority"});
        return;
    }
    let user = new User(usernameInput.value, passwordInput.value, fullnameInput.value, checkedRadio.value);
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