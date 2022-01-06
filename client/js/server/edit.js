import {Helper} from "../helper.js";
import {Server} from "../model/server.js";
import { Navbar } from "../navbar.js";

// if(!window.localStorage.getItem("report")) window.location.replace("/");

let body = document.querySelector(".body");
let wrapp = document.querySelector(".wrapp");
let form = document.querySelector(".form");
let ipaddressInput = document.querySelector(`input[name="ipaddress"]`);
let processorInput = document.querySelector(`input[name="processor"]`);
let ramInput = document.querySelector(`input[name="ram"]`);
let ssdInput = document.querySelector(`input[name="ssd"]`);
let formEdit = document.querySelector(`.form input[type="submit"]`);

let server = await Server.getById(window.localStorage.getItem("server"));

if(!server || server.error || server.errors) window.location.replace("/");
fillInputs();
// window.localStorage.removeItem("report");

wrapp.prepend(await Navbar.getNav());

formEdit.addEventListener("click", async (e) => {
    e.preventDefault();

    server.IPAddress = ipaddressInput.value;
    server.Processor = processorInput.value;
    server.RAMCapacity = ramInput.value;
    server.SSDCapacity = ssdInput.value;

    let response = await server.update();

    if(response.message)
    {
        Helper.drawSuccess("Uspešno izmenjen report!", body);
    }
    else
    {
        Helper.drawErrors(body, response);
    }
});

function fillInputs()
{
    ipaddressInput.value = server.IPAddress;
    processorInput.value = server.Processor;
    ramInput.value = server.RAMCapacity;
    ssdInput.value = server.SSDCapacity;
}