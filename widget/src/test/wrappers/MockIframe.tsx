import React from "react";

export const mockIframeStyles = {
    borderRadius: "9px",
    border: "1px solid black",
    marginTop: "2rem",
    marginBottom: "2rem",
    height: "500px",
    width: "380px",
};

export interface IMockIFrameWrapper {
    children: React.ReactNode;
}

export const MockIFrameWrapper = ({ children }: IMockIFrameWrapper) => {
    return <div style={mockIframeStyles}>{children}</div>;
};
