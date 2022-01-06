import { Report } from "./report.js";
import { Server } from "./server.js";
import { ReportType } from "./reportType.js";
import { Helper } from "../helper.js";

export class User
{
    constructor(username=null, 
        password=null, 
        fullname=null, 
        priority=null, 
        reports=null, 
        servers=null, 
        id=null)
    {
        this.ID       = id;
        this.Username = username;
        this.FullName = fullname;
        this.Password = password;
        this.Priority = priority;
        this.Reports  = reports;
        this.Servers  = servers;
    }

    async login()
    {
        try {
            let res = await fetch("/User/Login", 
            {
                method: "POST",
                body: JSON.stringify({
                    Username: this.Username,
                    Password: this.Password
                }),
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            });
            let data = await res.json();
            if(res.status !== 200) throw new Error(data.error);
            if(!data && !data.token) throw new Error("Something goes wrong. Please try again.");
            return { token: data.token };
        } catch (error) {
            return {error: error.message};
        }
    }

    async register()
    {
        try {
            let res = await fetch("/User/", 
            {
                method: "POST",
                body: JSON.stringify({
                    Username: this.Username,
                    Password: this.Password,
                    FullName: this.FullName,
                    Priority: this.Priority
                }),
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            });
            let data = await res.json();
            if(res.status !== 200)
            {
                if(data.errors) return {errors: data.errors};
                throw new Error(data.error);
            }
            if(!data && !data.token) throw new Error("Something goes wrong. Please try again.");
            return { token: data.token };
        } catch (error) {
            return {error: error.message};
        }
    }

    async crtajReports(host)
    {
        let isSet = await this.getReportsFromUser();
        let isExist = document.querySelector(".reports");
        if(isExist) isExist.parentNode.removeChild(isExist);
        let reportsEl = document.createElement("section");
        reportsEl.className = "reports";

        if(!isSet)
        {
            reportsEl.innerHTML += `<h2>Trenutno nema nijednog reporta!</h2>`;
            return;
        }

        this.Reports.forEach(el => {
            el.crtaj(reportsEl, this.Priority);
        });

        host.appendChild(reportsEl);
    }

    async getReportsFromUser()
    {
        try {
            let response = await fetch("/Report/User/", {
                method: "GET",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })
    
            let data = await response.json();

            if(data.reports.length === 0) throw new Error("Empty data");

            this.Reports = [];
            data.reports.forEach(r=> {
                let reportInstance = new Report(
                    r.description,
                    r.id,
                    new Date(r.createdAt).toDateString(),
                    r.isSolved,
                    new Server(r.server.ipAddress, r.server.id),
                    new User(null, null, r.user.fullName, null, null, null, r.user.id),
                    new ReportType(r.reportType.name, r.reportType.id)
                );

                this.Reports.push(reportInstance);
            });

            return true;
        } catch (error) {
            return false;
        }
    }

    async updateUser()
    {
        try {
            let response = await fetch("/User/", {
                method: "PUT",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    password: this.Password,
                    fullname: this.FullName
                })
            })
            let data = await response.json();

            if(response.status !== 200)
            {
                if(data.errors) return {errors: data.errors};
                throw new Exception(data.error);
            } 

            return {message : data.message};
        } catch (error) {
            return {error: error.message};
        }
    }

    async deleteUser()
    {
        try {
            let response = await fetch(`/User/`, {
                method: "DELETE",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })
            let data = await response.json();

            if(response.status !== 200) throw new Exception(data.error);

            return {message : data.message};
        } catch (error) {
            return {error: error.message};
        }
    }
    async rentServer(server)
    {
        try {
            let response = await fetch("/User/Server", {
                method: "POST",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    id: server.ID
                })
            })
            let data = await response.json();

            if(data.errors) return {errors: data.errors};
            if(response.status !== 200) throw new Error(data.error);

            return {message : data.message};
        } catch (error) {
            return {error: error.message};
        }
    }

    static async getUserFromSession()
    {
        try {
            let response = await fetch(`/User/`, {
                method: "GET",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })
            let data = await response.json();

            if(response.status !== 200) throw new Exception(data.error);
            let user = data.user;

            return new User(user.username, null, user.fullName, user.priority, null, null, user.id);
        } catch (error) {
            return {error: error.message};
        }
    }

    static async logoutUser()
    {
        try {
            let response = await fetch(`/User/Logout`, {
                method: "POST",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })
            let data = await response.json();

            if(response.status !== 200) throw new Exception(data.error);
        } catch (error) {
            return {error: error.message};
        }
    }
    
}