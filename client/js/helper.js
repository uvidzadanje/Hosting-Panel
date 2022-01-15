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

    static drawGraph(host,resource, xName, yName)
    {
        if(document.querySelector(".stat canvas")) document.querySelector(".stat canvas").parentNode.removeChild(document.querySelector(".stat canvas")); 
        let canvas = document.createElement("canvas");
        let context = canvas.getContext("2d");
        host.appendChild(canvas);

        canvas.height = canvas.width* 0.7;

        context.beginPath();
        context.moveTo(20,0)
        context.lineTo(20, canvas.height);
        context.stroke();

        context.beginPath();
        context.moveTo(0,canvas.height - 20);
        context.lineTo(canvas.width, canvas.height - 20);
        context.stroke();

        let itemWidth = (canvas.width - 20) / resource.length;
        let maxValue = Math.max(...resource.map(item => item[yName]));
        let podeoci = Math.min(maxValue, 5);
        
        context.font = "Arial 12px";
        context.textAlign = "right";
        
        [1,2,3,4,5].forEach(item => {
            context.fillText((item * maxValue)/podeoci, 18, canvas.height - 20 - item*(canvas.height-20-10)/podeoci);
        });

        context.textAlign = "center";
        resource.forEach((item,index) => {
            if(item[yName] != 0)
            {
                context.fillStyle = "#ff0000";
                let rectX = 20 + index*itemWidth + 10;
                let rectY = (1-(item[yName] / maxValue)) * canvas.height - 5;
                let rectWidth = itemWidth-20;
                let rectHeight = canvas.height*(item[yName]/maxValue)-15;
                context.fillRect(rectX, rectY, rectWidth, rectHeight);
                
                context.fillStyle = "#000000";
                context.fillText(item[yName], 20 + index*itemWidth+itemWidth/2, rectY + (rectHeight/2));
            }
            context.fillText(item[xName], 20 + index*itemWidth+itemWidth/2, canvas.height - 5);
        });
    }
}