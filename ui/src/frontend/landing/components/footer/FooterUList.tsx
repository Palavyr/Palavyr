import React from "react";

export interface IFooterList {
    children: React.ReactNode;
}

export const FooterUList = ({ children }: IFooterList) => {
    return <ul style={{ textAlign: "center" }}>{children}</ul>;
};
