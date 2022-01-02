import {User} from "./user.js";


let usernameInput = document.querySelector(`#username`);
let passwordInput = document.querySelector(`#password`);
let loginBtn = document.querySelector(`input[type="submit"]`);

loginBtn.addEventListener("click", async (e) => {
    e.preventDefault();
    let user = new User(usernameInput.value, passwordInput.value);
    let response = await user.login();
    if(response.token)
    {
        document.cookie = `token=${response.token}`;
        window.location.replace("/");
    }
    else
    {
        console.log(response);
    }
})