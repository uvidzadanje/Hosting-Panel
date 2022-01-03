import {User} from "./user.js";
import {Helper} from "./helper.js";

let usernameInput = document.querySelector(`#username`);
let passwordInput = document.querySelector(`#password`);
let loginBtn = document.querySelector(`input[type="submit"]`);
let body = document.querySelector(".body");
let form = document.querySelector(".form");

loginBtn.addEventListener("click", async (e) => {
    e.preventDefault();
    let user = new User(usernameInput.value, passwordInput.value);
    let response = await user.login();
    if(response.token)
    {
        // document.cookie = `token=${response.token}`;
        window.location.replace("/dashboard");
    }
    else
    {
        Helper.drawErrors(form, response);
    }
})