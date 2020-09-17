import * as React from 'react';
import HowToRegIcon from '@material-ui/icons/HowToReg';
import LockOpenIcon from '@material-ui/icons/LockOpen';
import HomeIcon from '@material-ui/icons/Home';

export const menuItems = (openRegisterDialog: any, openLoginDialog: any) =>  ([
    {
        name: "Home",
        link: "/",
        icon: <HomeIcon className="text-white" />
    },
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
        icon: <HowToRegIcon className="text-white" />
    },
    {
        name: "Login",
        onClick: openLoginDialog,
        icon: <LockOpenIcon className="text-white" />
    }
]);
