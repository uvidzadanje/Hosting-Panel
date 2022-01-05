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
}
