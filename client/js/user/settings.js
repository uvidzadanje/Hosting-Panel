import {Helper} from "../helper.js";
import {User} from "../model/user.js";
import {Navbar} from "../navbar.js";

let body = document.querySelector(".body");
let wrapp = document.querySelector(".wrapp");
let form = document.querySelector(".form");

let fullNameInput = document.querySelector(`input[name="full_name"]`);
let passwordInput = document.querySelector(`input[name="password"]`);
let repeatPasswordInput = document.querySelector(`input[name="repeat_password"]`);

let editBtn = document.querySelector(".edit_btn");
let deleteBtn = document.querySelector(".delete_btn");
let user = await User.getUserFromSession();

wrapp.prepend(await Navbar.getNav());
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

function fillInputs()
{
    fullNameInput.value = user.FullName;
}