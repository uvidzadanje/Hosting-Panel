import {User} from "./model/user.js";
import {Server} from "./model/server.js";
import {ReportType} from "./model/reportType.js";

export class Helper
{
    static sanitize(text)
    {
        return text.replace(/&/g, '&amp;')
                .replace(/</g, '&lt;')
                .replace(/"/g, '&quot;');
    }

    static getFromCookie(name)
    {
        let match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
        if (match) return match[2];
    }

    static drawErrors(host, response)
    {
        if(host.firstChild.className && host.firstChild.className === "alert-container") host.removeChild(host.firstChild);
        let alertContainer = document.createElement("div");
        alertContainer.className = "alert-container";
        alertContainer.style.display = "none";
        alertContainer.innerHTML = "";

        if(response.errors)
        {
            for(let e in response.errors)
            {
                this.drawError(response.errors[e][0], alertContainer);
            }
        }
        else this.drawError(response.error, alertContainer);
        alertContainer.style.display = "block";

        host.prepend(alertContainer);
    }

    static drawError(error, host)
    {
        let errorAlert = this.getAlert(error, "alert-error");
        host.appendChild(errorAlert);
    }

    static drawSuccess(message, host)
    {
        if(host.firstChild.className && host.firstChild.className === "alert-container") host.removeChild(host.firstChild);
        let alertContainer = document.createElement("div");
        alertContainer.className = "alert-container";
        alertContainer.innerHTML = "";
        
        let successAlert = this.getAlert(message, "alert-success");
        alertContainer.prepend(successAlert);

        host.prepend(alertContainer);
    }

    static getAlert(text, className)
    {
        let alert = document.createElement("div");
        alert.className = className;
        alert.innerText = text;
        alert.style.display = "block";
        alert.innerHTML += `<span class="closebtn" onclick="this.parentElement.style.display='none';">&times;</span>`;

        return alert;
    }
    
    static async setRentedServers(host)
    {
        let servers = await Server.showServerBySession();
        servers.forEach(server => {
            host.innerHTML += `<option value="${server.ID}">${server.IPAddress}</option>`;
        });
    }
    
    static async setReportTypes(host)
    {
        let reportTypes = await ReportType.getReportTypes();
        reportTypes.forEach(reportType=>{
            host.innerHTML += `<option value="${reportType.ID}">${reportType.Name}</option>`;
        });
    }

    static async setAllServers(host)
    {
        let servers = await Server.showAllServers();
        servers.forEach(server => {
            host.innerHTML += `<option value="${server.ID}">${server.IPAddress}</option>`;
        });
    }
}