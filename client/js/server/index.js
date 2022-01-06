import {Navbar} from "../navbar.js";
import {Server} from "../model/server.js";
import {Helper} from "../helper.js";

let wrapp = document.querySelector(".wrapp");
let body = document.querySelector(".body");
let addBtn = document.querySelector(".add_report_btn");
let form = document.querySelector(".form");
let formAdd = document.querySelector(`input[type="submit"]`);
let ipaddressInput = document.querySelector(`input[name="ipaddress"]`);
let processorInput = document.querySelector(`input[name="processor"]`);
let ramInput = document.querySelector(`input[name="ram"]`);
let ssdInput = document.querySelector(`input[name="ssd"]`);
let nav = await Navbar.getNav();

wrapp.prepend(nav);

form.style.display= "none";
await Server.drawServers(body);

addBtn.addEventListener("click", () => {
    if(form.style.display === "none") form.style.display = "flex";
    else form.style.display = "none";
});

formAdd.addEventListener("click", async (e) => {
    e.preventDefault();
    let server = new Server(ipaddressInput.value, null, processorInput.value, ramInput.value, ssdInput.value);
    let response = await server.add();

    if(response.message)
    {
        Helper.drawSuccess(response.message, body);
        await Server.drawServers(body);
        form.style.display = "none";
    }
    else
    {
        Helper.drawErrors(body, response);
    }
})

