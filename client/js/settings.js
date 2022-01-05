import {Helper} from "./helper.js";
import {User} from "./model/user.js";

let body = document.querySelector(".body");
let form = document.querySelector(".form");
let navbar = document.querySelector(".navbar>ul");

let fullNameInput = document.querySelector(`input[name="full_name"]`);
let passwordInput = document.querySelector(`input[name="password"]`);
let repeatPasswordInput = document.querySelector(`input[name="repeat_password"]`);

let editBtn = document.querySelector(".edit_btn");
let deleteBtn = document.querySelector(".delete_btn");

await Helper.setUsernameOnNavbar(navbar);
await Helper.setLogoutBtn(navbar);
await fillInputs();

editBtn.addEventListener("click", async (e) => {
    e.preventDefault();
    if(passwordInput.value != repeatPasswordInput.value)
    {
         Helper.drawErrors(form, {error: "Passwords do not match"});
         return;
    }
    let user = new User(null, passwordInput.value, fullNameInput.value);

    let response = await user.updateUser();
    if(response.message)
    {
        Helper.drawSuccess("Uspešne izmene!", form);
    }
    else
    {
        Helper.drawErrors(form, response);
    }
});

deleteBtn.addEventListener("click", async(e) => {
    e.preventDefault();
    let user = new User();

    let response = await user.deleteUser();

    if(response.message)
    {
        Helper.drawSuccess("Uspešno izbrisan korisnik!", form);
        window.location.replace("/");
    }
    else
    {
        Helper.drawErrors(form, response);
    }
})

async function fillInputs()
{
    let user = await User.getUserFromSession();

    fullNameInput.value = user.FullName;
}