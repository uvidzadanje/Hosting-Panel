import { Navbar } from "./navbar.js";

let wrapp = document.querySelector(".wrapp");

wrapp.prepend(await Navbar.getNav());