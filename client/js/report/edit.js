import {Helper} from "../helper.js";
import {Report} from "../model/report.js";
import { Navbar } from "../navbar.js";

let body = document.querySelector(".body");
let wrapp = document.querySelector(".wrapp");
let navbar = document.querySelector(".navbar>ul");
let form = document.querySelector(".form");
let formDescription = document.querySelector(`textarea[name="description"]`);
let reportTypeSelect = document.querySelector(`select[name="report_type"]`);
let formEdit = document.querySelector(`.form input[type="submit"]`);
let serversSelect = document.querySelector(`select[name="server"]`);

let report = await Report.getById(window.localStorage.getItem("report"));
if(!report) window.location.replace("/");

wrapp.prepend(await Navbar.getNav());

await Helper.setRentedServers(serversSelect);
await Helper.setReportTypes(reportTypeSelect);

fillInputs();

formEdit.addEventListener("click", async (e) => {
    e.preventDefault();

    report.Description = formDescription.value;
    report.Server.ID = serversSelect.value;
    report.ReportType.ID = serversSelect.value;
    let response = await report.update();

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
    serversSelect.value = report.Server.ID;
    reportTypeSelect.value = report.ReportType.ID;
    formDescription.value = report.Description;
}