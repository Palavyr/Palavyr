import React from "react";

export const MoreInformation = () => {
    const links = ["www.youtube.com"];
    return (
        <>
            <div>For more information, view our youtube tutorial series.</div>
            {links.map((link: string, index: number) => {
                return <p key={index}>{link}</p>;
            })}
        </>
    );
};
