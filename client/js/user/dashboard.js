import {User} from "../model/user.js";
import {Report} from "../model/report.js";
import {Server} from "../model/server.js";
import {Helper} from "../helper.js";
import { ReportType } from "../model/reportType.js";
import {Navbar} from "../navbar.js";

let body = document.querySelector(".body");
let wrapp = document.querySelector(".wrapp");
let addBtn = document.querySelector(".add_report_btn");
let form = document.querySelector(".form");
let formSection = document.querySelector(".form-section");
let rentSelectServer = document.querySelector(`select[name="rent_server"]`);
let rentSubmit = document.querySelector(`input[name="add_rent_server"]`);
let formDescription = document.querySelector(`textarea[name="description"]`);
let reportTypeSelect = document.querySelector(`select[name="report_type"]`);
let formAdd = document.querySelector(`.form input[type="submit"]`);
let serversSelect = document.querySelector(`select[name="server"]`);

let user = await User.getUserFromSession();

wrapp.prepend(await Navbar.getNav());

if(user.Priority==1)
{
    form.style.display= "none";
    addBtn.addEventListener("click", () => {

        if(serversSelect.options.length === 0)
        {
            Helper.drawErrors(body, {error: "Ne postoji nijedan zakupljen server"});
            return;
        }

        if(form.style.display === "none") form.style.display = "flex";
        else form.style.display = "none";
    });

    formAdd.addEventListener("click", async (e) => {
        e.preventDefault();
        let report = new Report(formDescription.value, null, null, null, new Server(null, serversSelect.value), null, new ReportType(null, reportTypeSelect.value));
        let response = await report.add();
        if(response.message)
        {
            Helper.drawSuccess("Uspešno dodat report", body);
            await user.crtajReports(body);
            form.style.display = "none";
        }
        else
        {
            Helper.drawErrors(body, response);
        }
    });

    await Helper.setRentedServers(serversSelect);
    await Helper.setReportTypes(reportTypeSelect);
    await Helper.setAllServers(rentSelectServer);

    if(rentSelectServer.options.length===0)
    {
        document.querySelectorAll("form")[0].style.display = "none";
        Helper.drawErrors(body, {error: "Ne postoji nijedan server!"});
    }

    rentSubmit.addEventListener("click", async (e) => {
        e.preventDefault();
        let server = new Server(null, rentSelectServer.value);
        let response = await user.rentServer(server);
        if(response.message)
        {
            Helper.drawSuccess("Uspešno dodat zakup", body);
            form.style.display = "none";
            setTimeout(() => {
                window.location.reload();
            }, 1000);
        }
        else
        {
            Helper.drawErrors(body, response);
        }
    })
}
else formSection.parentNode.removeChild(formSection);

await user.crtajReports(body);

