import React from "react";

export const MoreInformation = () => {
    const links = ["www.youtube.com"];
    return (
        <>
            <div>For more information, view our youtube tutorial series.</div>
            {links.map((link: string) => {
                return <p>{link}</p>;
            })}
        </>
    );
};
