export class Server
{
    constructor(
        ipaddress, 
        id=null, 
        processor=null, 
        ramcapacity=null, 
        ssdcapacity=null, 
        reports=null, 
        userserver=null,
        datacenter=null
        )
    {
        this.ID          = id;
        this.IPAddress   = ipaddress;
        this.Processor   = processor;
        this.RAMCapacity = ramcapacity;
        this.SSDCapacity = ssdcapacity;
        this.Reports     = reports;
        this.UserServer  = userserver;
        this.Datacenter  = datacenter;
    }
}
