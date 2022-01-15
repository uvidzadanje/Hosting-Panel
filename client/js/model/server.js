import {Helper} from "../helper.js";

export class Server
{
    constructor(
        ipaddress, 
        id=null, 
        processor=null, 
        ramcapacity=null, 
        ssdcapacity=null, 
        reports=null, 
        userserver=null
        )
    {
        this.ID          = id;
        this.IPAddress   = ipaddress;
        this.Processor   = processor;
        this.RAMCapacity = ramcapacity;
        this.SSDCapacity = ssdcapacity;
        this.Reports     = reports;
        this.UserServer  = userserver;
        this.Container   = null;
    }

    static async showServerBySession()
    {
        try {
            let response = await fetch("/Server/User", {
                method: "GET",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })
            if(response.status !== 200) throw new Error("No rented servers!");

            let data = await response.json();
            let servers = [];
            data.servers.forEach(server => {
                servers.push(new Server(server.ipAddress, server.id));
            })

            return servers;
        } catch (error) {
            return {error: error.message};
        }
    }

    async add()
    {
        try {
            let response = await fetch("/Server", {
                method: "POST",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    ipAddress: this.IPAddress,
                    processor: this.Processor,
                    ramCapacity: +this.RAMCapacity,
                    ssdCapacity: +this.SSDCapacity
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

    async update()
    {
        try {
            let response = await fetch("/Server", {
                method: "PUT",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    id: this.ID,
                    ipAddress: this.IPAddress,
                    processor: this.Processor,
                    ramCapacity: +this.RAMCapacity,
                    ssdCapacity: +this.SSDCapacity
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

    async getServerTitle(host)
    {
        let title = document.createElement("div");
        title.className = "report_title";
        title.innerHTML = `<h3>${this.IPAddress}</h3>`
        let btnContainer = document.createElement("div");
        btnContainer.className = "report_btn_container";

        let serverEditBtn = this.getEditBtn();
        let serverDeleteBtn = await this.getDelBtn(host);

        btnContainer.appendChild(serverEditBtn);
        btnContainer.appendChild(serverDeleteBtn);

        title.appendChild(btnContainer);

        this.Container.appendChild(title);
    }

    getEditBtn()
    {
        let serverEditBtn = document.createElement("button");
        serverEditBtn.className = "report_edit";
        serverEditBtn.innerText = "Izmeni";
        serverEditBtn.addEventListener("click", () => {
            window.localStorage.setItem("server", this.ID);
            window.location.href = "/server/edit";
        })

        return serverEditBtn;
    }

    async getDelBtn(host)
    {
        let serverDeleteBtn = document.createElement("button");
        serverDeleteBtn.className = "report_delete";
        serverDeleteBtn.innerText = "Obriši";

        serverDeleteBtn.addEventListener("click", async () => {
            let response = await this.delete();
            if(response.message)
            {
                Helper.drawSuccess(response.message, host);
                this.Container.parentNode.removeChild(this.Container);
            }
            else
            {
                Helper.drawErrors(host, response);
            }
        });

        return serverDeleteBtn;
    }

    getDescription()
    {
        let description = document.createElement("div");
        description.className = "report_description";
        let descriptionEl = [
            `Processor: ${this.Processor}`,
            `RAM: ${this.RAMCapacity} GB`,
            `SSD: ${this.SSDCapacity} GB`
        ]
        descriptionEl.forEach(el => {
            description.innerHTML += `<p>${el}</p>`
        })

        this.Container.appendChild(description);
    }

    async crtaj(host)
    {
        let server = document.createElement("article");
        server.className = "report";

        this.Container = server;

        await this.getServerTitle(host);
        this.getDescription()

        host.appendChild(this.Container);
    }

    async delete()
    {
        try {
            let response = await fetch("/Server/"+this.ID,{
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

    static async showAllServers()
    {
        try {
            let response = await fetch("/Server", {
                method: "GET",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })
            if(response.status !== 200) throw new Error("No servers!");

            let data = await response.json();
            let servers = [];
            data.servers.forEach(server => {
                servers.push(new Server(server.ipAddress, server.id, server.processor, server.ramCapacity, server.ssdCapacity));
            })

            return servers;
        } catch (error) {
            return {error: error.message};
        }
    }

    static async drawServers(host)
    {
        let servers = await Server.showAllServers();
        let isExist = document.querySelector(".reports");
        if(isExist) isExist.parentNode.removeChild(isExist);
        let reportsEl = document.createElement("section");
        reportsEl.className = "reports";

        if(servers.error)
        {
            Helper.drawErrors(host, servers);
            return;
        }
        if(servers.length === 0)
        {
            reportsEl.innerHTML += `<h2>Trenutno nema nijednog reporta!</h2>`;
            return;
        }
        
        servers.forEach(el => {
            el.crtaj(reportsEl);
        });

        host.appendChild(reportsEl);    
    }

    static async getById(id)
    {
        try {
            let response = await fetch("/Server/"+id, {
                method: "GET",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            });
            let data = await response.json();
            if(response.status !== 200) throw new Error(response.error);
            let server = data.server;
            server = new Server(
                server.ipAddress,
                server.id,
                server.processor,
                server.ramCapacity,
                server.ssdCapacity
                );
            return server;
        } catch (error) {
            return {error: error.message};
        }
    }

    static async getStats()
    {
        try {
            let response = await fetch("/Server/Stats", {
                method: "GET",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })

            let data = await response.json();
            if(response.status !== 200) throw new Error(data.error);
            
            return data.servers;
        } catch (error) {
            return {error: error.message};
        }
    }
}
