import {User} from "./model/user.js";

export class Navbar
{
    static async getNav(host)
    {
        let user = await User.getUserFromSession();

        let nav = document.createElement("nav");
        
        let menu = document.createElement("ul");
        menu.className = "menu";
        nav.appendChild(menu);

        let logo = document.createElement("li");
        logo.className = "logo";
        logo.innerHTML = "<h1>Hosting<span>Panel</span></h1>";

        let hamburger = document.createElement("div");
        hamburger.className = "hamburger";

        for(let i = 0; i < 3; i++)
        {
            let hamburgerLine = document.createElement("div");
            hamburgerLine.className = "hamburger-line";
            hamburger.appendChild(hamburgerLine);
        }

        logo.appendChild(hamburger);

        menu.appendChild(logo);
        
        let navbar = this.getNavbar(user);
        
        hamburger.addEventListener("click", ()=> {
            if(window.getComputedStyle(navbar).display === "none") navbar.style.display = "flex";
            else navbar.style.display = "none";
        })
        menu.appendChild(navbar);

        return nav;
    }

    static getNavbar(user)
    {
        let navbar = document.createElement("li");
        navbar.className = "navbar";

        if(user.error) navbar.appendChild(this.getGuestNavbar());
        else if(user.Priority == 0) navbar.appendChild(this.getAdminNavbar(user.FullName));
        else navbar.appendChild(this.getUserNavbar(user.FullName));

        return navbar;
    }

    static getGuestNavbar()
    {
        let navbarUl = document.createElement("ul");

        navbarUl.innerHTML = `<li class="item"><a href="/">Početna</a></li>
                              <li class="item"><a href="/register">Registruj se</a></li>
                              <li class="item"><a href="/login">Prijavi se</a></li>`;
        return navbarUl;
    }

    static getUserNavbar(fullname)
    {
        let navbarUl = document.createElement("ul");
        
        let userLi = this.getUserNavItem(fullname);

        let homeLi = document.createElement("li");
        homeLi.className = "item";
        homeLi.innerHTML = `<a href="/">Početna</a>`;

        let settingsLi = document.createElement("li");
        settingsLi.className = "item";
        settingsLi.innerHTML = `<a href="/settings">Podešavanja</a>`;

        let logoutLi = this.getLogoutBtn();

        navbarUl.appendChild(userLi);
        navbarUl.appendChild(homeLi);
        navbarUl.appendChild(settingsLi);
        navbarUl.appendChild(logoutLi);

        return navbarUl;
    }

    static getUserNavItem(fullname)
    {
        let userLi = document.createElement("li");
        userLi.className = "item";

        let userP = document.createElement("p");
        userP.className = "nav-fullname";
        userP.innerText = fullname;

        userLi.appendChild(userP);

        return userLi;
    }

    static getLogoutBtn()
    {
        let logoutLi = document.createElement("li");
        logoutLi.className = "item";

        let logoutBtn = document.createElement("button");
        logoutBtn.className = "logout-btn";
        logoutBtn.innerText = "Odjavi se";

        logoutBtn.addEventListener("click", async ()=>{
            await User.logoutUser();
            window.localStorage.clear();
            window.location.replace("/");
        });

        logoutLi.appendChild(logoutBtn);

        return logoutLi;
    }

    static getAdminNavbar(fullname)
    {
        let baseNavbar = this.getUserNavbar(fullname);

        let serversLi = document.createElement("li");
        serversLi.className = "item";
        serversLi.innerHTML = `<a href="/servers">Serveri</a>`;

        let statLi = document.createElement("li");
        statLi.className = "item";
        statLi.innerHTML = `<a href="/stats">Statistika</a>`;

        baseNavbar.insertBefore(serversLi, baseNavbar.children[2]);
        baseNavbar.insertBefore(statLi, baseNavbar.lastChild);

        return baseNavbar;
    }
}