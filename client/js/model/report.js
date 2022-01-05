import {Helper} from "../helper.js";
import { ReportType } from "./reportType.js";
import { Server } from "./server.js";
import { User } from "./user.js";
export class Report
{
    constructor(
        description, 
        id=null, 
        createdAt=null,
        isSolved=null,
        server=null,
        user=null,
        reportType=null
        )
    {
        this.ID          = id;
        this.Description = description;
        this.CreatedAt   = createdAt;
        this.Server      = server;
        this.User        = user;
        this.ReportType  = reportType;
        this.IsSolved    = isSolved;
        this.Container   = null;
    }

    async crtaj(host)
    {
        let report = document.createElement("article");
        report.className = "report";
        
        let title = document.createElement("div");
        title.className = "report_title";
        title.innerHTML = `<h3>${this.ReportType.Name} - ${this.Server.IPAddress}</h3>`
        let btnContainer = document.createElement("div");
        btnContainer.className = "report_btn_container";

        let reportEditBtn = document.createElement("button");
        reportEditBtn.className = "report_edit";
        reportEditBtn.innerText = "Izmeni";
        let reportDeleteBtn = document.createElement("button");
        reportDeleteBtn.className = "report_delete";
        reportDeleteBtn.innerText = "Obriši";
        
        btnContainer.appendChild(reportEditBtn);
        btnContainer.appendChild(reportDeleteBtn);
        
        reportDeleteBtn.addEventListener("click", async () => {
            let response = await this.delete();
            if(response.message)
            {
                Helper.drawSuccess(response.message, host);
                report.parentNode.removeChild(report);
            }
            else
            {
                Helper.drawErrors(host, response);
            }
        });

        reportEditBtn.addEventListener("click", async ()=> {
            window.localStorage.setItem("report", this.ID);
            window.location.replace("/report/edit");
        })

        title.appendChild(btnContainer);
        
        let description = document.createElement("div");
        description.className = "report_description";
        description.innerHTML = `<p>${this.Description}</p>`
        
        let reportInfo = document.createElement("div");
        reportInfo.className = "report_info";
        
        let reportBy = document.createElement("div");
        reportBy.className = "report_by";
        reportBy.innerHTML = `<b>${this.User.FullName}</b>`;
        
        let reportDate = document.createElement("div");
        reportDate.className = "report_date";
        reportDate.innerHTML = `<span>${this.CreatedAt}</span>`;

        let isSolved = document.createElement("p");
        isSolved.innerHTML = `Solved: ${this.IsSolved ? `<span class="report_solved">&#10004;</span>` : `<span class="report_not_solved">&#10006;</span>`}`;

        reportInfo.appendChild(reportBy);
        reportInfo.appendChild(isSolved);
        reportInfo.appendChild(reportDate);
        
        report.appendChild(title);
        report.appendChild(description);
        report.appendChild(reportInfo);

        this.Container = report;
        host.appendChild(report);
    }

    async add()
    {
        try {
            let response = await fetch("/Report",{
                method: "POST",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    description: this.Description,
                    server: {
                        id: this.Server.ID
                    },
                    reportType:{
                        id: this.ReportType.ID
                    }
                })
            })
            let data = await response.json();
            if(data.errors) return data.errors;
            if(response.status !== 200) throw new Error(data.error);
            return {message: data.message, id: data.id};
        } catch (error) {
            return {error: error.message};
        }
    }

    async delete()
    {
        try {
            let response = await fetch("/Report/"+this.ID,{
                method: "DELETE",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })
            let data = await response.json();
            if(data.errors) return data.errors;
            if(response.status !== 200) throw new Error(data.error);
            return {message: data.message};
        } catch (error) {
            return {error: error.message};
        }
    }

    async update()
    {
        try {
            let response = await fetch("/Report/",{
                method: "PUT",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    id: this.ID,
                    description: this.Description,
                    isSolved: this.IsSolved,
                    server: {
                        id: this.Server.ID
                    },
                    reportType: {
                        id: this.ReportType.ID
                    }
                })
            });

            let data = await response.json();
            if(data.errors) return {errors: data.errors};
            if(response.status !== 200) throw new Error(data.error);
            return {message: data.message};
        } catch (error) {
            return {error: error.message};
        }
    }

    static async getById(id)
    {
        try {
            let response = await fetch("/Report/"+id, {
                method: "GET",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            });
            let data = await response.json();
            if(response.status !== 200) throw new Error(response.error);
            let report = data.report;
            report = new Report(
                report.description, 
                report.id,
                report.createdAt, 
                report.isSolved, 
                new Server(report.server.ipAddress, report.server.id),
                new User(null, null, report.user.fullName),
                new ReportType(report.reportType.name, report.reportType.id)
                );
            return report;
        } catch (error) {
            return {error: error.message};
        }
    }
}