export class Report
{
    constructor(
        description, 
        id=null, 
        createdAt=null, 
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
        this.Container   = null;
    }

    crtaj(host)
    {
        let report = document.createElement("article");
        report.className = "report";
        
        let title = document.createElement("div");
        title.className = "report_title";
        title.innerHTML = `<h3>${this.ReportType.Name}</h3>`
        
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

        reportInfo.appendChild(reportBy);
        reportInfo.appendChild(reportDate);
        
        report.appendChild(title);
        report.appendChild(description);
        report.appendChild(reportInfo);

        this.Container = report;
        host.appendChild(report);
    }
}