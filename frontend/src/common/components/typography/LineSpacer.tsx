import React from "react";

export interface LineSpacerProps {
    numLines?: number;
}

export const LineSpacer = ({ numLines = 1 }: LineSpacerProps) => {
    if (numLines % 1 !== 0) throw new Error("NumLines in spacer must be integer");
    let breaks: JSX.Element[] = [];
    for (let index = 0; index < numLines; index++) {
        breaks.push(<br></br>);
    }
    return <>{breaks.map((x) => x)}</>;
};
