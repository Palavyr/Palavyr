import { Divider } from "@material-ui/core";
import React from "react";

export const MoreInformation = () => {
    const links = ["www.youtube.com"];
    return (
        <>
            <Divider />
            <p>For more information, view our youtube tutorial series.</p>
            {links.map((link: string, index: number) => {
                return <p key={index}>{link}</p>;
            })}
        </>
    );
};
