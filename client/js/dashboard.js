import {User} from "./user.js";
import {Helper} from "./helper.js";

let body = document.querySelector(".body");
let navbar = document.querySelector(".navbar>ul");

await Helper.setUsernameOnNavbar(navbar);
await Helper.setLogoutBtn(navbar);

let user = await User.getUserFromSession();
await user.crtajReports(body);