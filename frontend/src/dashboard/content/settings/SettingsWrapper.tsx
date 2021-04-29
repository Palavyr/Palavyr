import React from "react";

export interface SettingsWrapperProps {
    children: React.ReactNode;
}
export const SettingsWrapper = ({ children }: SettingsWrapperProps) => {
    return <div style={{ width: "50%", paddingTop: "1rem" }}>{children}</div>;
};
