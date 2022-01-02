import {User} from "./user.js";

let fullnameInput = document.querySelector("#full_name");
let usernameInput = document.querySelector("#username");
let passwordInput = document.querySelector("#password");
let repeatPasswordRepeat = document.querySelector("#repeat_password");
let registerBtn = document.querySelector('input[type="submit"]');

registerBtn.addEventListener("click", async (e) => {
    e.preventDefault();
    if(passwordInput.value !== passwordInput.value)
    {
        console.log("passwords do not match!");
        return;
    }
    let user = new User(fullnameInput.value, usernameInput.value, passwordInput.value, 0);
    let response = await user.register();
    if(response.token)
    {
        document.cookie = `token=${response.token}`;
        window.location.replace("/");
    }
    else
    {
        console.log(response.error);
    }
})