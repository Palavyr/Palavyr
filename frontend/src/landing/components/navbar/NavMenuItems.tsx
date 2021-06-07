import * as React from "react";
import HowToRegIcon from "@material-ui/icons/HowToReg";
import LockOpenIcon from "@material-ui/icons/LockOpen";
import HomeIcon from "@material-ui/icons/Home";
import BookIcon from "@material-ui/icons/Book";

type menuItem = {
    name: string;
    link?: string;
    icon?: React.ReactNode;
    onClick?(): void;
};
export const menuItems = (openRegisterDialog: any, openLoginDialog: any): menuItem[] => [
    // {
    //     name: "Home",
    //     link: "/",
    //     icon: <HomeIcon className="text-white" />
    // },
    // {
    //     name: "Blog",
    //     link: "/blog",
    //     icon: <BookIcon className="text-white" />
    // },
    // {
    //     name: "Docs",
    //     link: "/docs",
    //     icon: <BookIcon className="text-white" />
    // },
    {
        name: "Register",
        onClick: openRegisterDialog,
        icon: <HowToRegIcon className="text-white" />,
    },
    {
        name: "Login",
        onClick: openLoginDialog,
        icon: <LockOpenIcon className="text-white" />,
    },
];
