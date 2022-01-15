import {Navbar} from "./navbar.js";
import {Helper} from "./helper.js";
import {ReportType} from "./model/reportType.js";
import {Server} from "./model/server.js";

let wrapp = document.querySelector(".wrapp");
let body = document.querySelector(".body");
let stats = document.querySelectorAll(".stat");
let statSelect = document.querySelector(`select[name="stats"]`);
let statBtn = document.querySelector(`input[type="submit"]`);

wrapp.prepend(await Navbar.getNav());

statBtn.addEventListener("click", async (e) => {
    e.preventDefault();
    let response = [];
    if(statSelect.value == 0)
    {
        response = await ReportType.getStats();
    }
    else
    {
        response = await Server.getStats();
    }
    
    if(!response.error)
    {
        if(response.length == 0)
        {
            Helper.drawErrors(body, {error: "Nema podataka!"});
        }
        Helper.drawGraph(stats[1], response, Object.keys(response[0])[0], Object.keys(response[0])[1]);
    }
    else
    {
        Helper.drawErrors(body, response);
    }
})


