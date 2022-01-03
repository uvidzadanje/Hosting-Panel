import {User} from "./user.js";

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
        let successAlert = this.getAlert(message, "alert-success");
        host.prepend(successAlert);
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

    static async setUsernameOnNavbar(navbar)
    {
        let usernameLi = document.createElement("li");
        usernameLi.className = "item";

        let usernameP = document.createElement("p");
        usernameP.className = "nav-fullname";

        let user = await User.getUserFromSession();
        usernameP.innerText = user.FullName;

        usernameLi.appendChild(usernameP);

        navbar.prepend(usernameLi);
    }

    static async setLogoutBtn(navbar)
    {
        let logoutLi = document.createElement("li");
        logoutLi.className = "item";

        let logoutBtn = document.createElement("button");
        logoutBtn.className = "logout-btn";
        logoutBtn.innerText = "Odjavi se";

        logoutBtn.addEventListener("click", async ()=>{
            let error = await User.logoutUser();
            if(error) 
            {
                Helper.drawErrors(body, {error: "Problem with logout"});
                return;
            }
            window.location.replace("/");
        })

        logoutLi.appendChild(logoutBtn);
        navbar.appendChild(logoutLi);
    }
}